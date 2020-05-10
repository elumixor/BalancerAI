using System.Collections.Generic;
using System.Linq;
using Num;
using UnityEngine;
using static F;

namespace Basic {
    public class Agent : MonoBehaviour {
        private const float discount = 0.99f;
        private const float learningRate = 0.01f;
        private const float regularization = 0.01f;
        private const float momentum = 0.9f;

        private float b;
        private float w;

        private float db;
        private float dw;

        private float bv;
        private float wv;
        
        public object BrainString => $"{w} {b}";

        public float P0(State state) {
            return (w * state.position + b).Sigmoid();
        }

        public float V(float x) {
            return wv * x + bv;
        }

        public int SampleAction(State state) {
            var r = new System.Random();
            return r.NextDouble() < P0(state) ? 0 : 1;
        }

        private float[] ToDiscounted(IReadOnlyList<float> rewards) {
            var cumulativeRewards = 0f;
            var discounted = new float[rewards.Count];

            for (var i = rewards.Count - 1; i >= 0; i--)
                discounted[i] = cumulativeRewards = cumulativeRewards * discount + rewards[i];

            return discounted;
        }

        private (float db, float dw) Learn(float state, int action, float discounted) {
            // y = w*x + b
            var x = state;
            var y = (w * x + b).Sigmoid();

            var dy = (action == 0 ? 1 - y : -y) * discounted;

            var db = dy;
            var dw = dy * x;

            return (db, dw);
        }

        private void LearnValueFunction(float state, int action, float G) {
            var previous = V(state);
            
        }
        
        public void Learn(List<Sample> episodes) {
            var dbc = 0f;
            var dwc = 0f;

            var size = episodes.SelectMany(e => e.actions).Count();

            var states = new float[size];
            var actions = new int[size];
            var rs = new float[size];

            var off = 0;
            foreach (var (s, a, r) in episodes) {
                var discounted = ToDiscounted(r);
                for (var i = 0; i < s.Count; i++) {
                    states[off + i] = s[i].position;
                    actions[off + i] = a[i];
                    rs[off + i] = discounted[i];
                }

                off += s.Count;
            }

            var rewards = ((Vector) rs).Normalize();

            for (var i = 0; i < states.Length; i++) {
                var x = states[i];
                var a = actions[i];

                var (dbi, dwi) = Learn(states[i], actions[i], rewards[i]);
                dbc += dbi;
                dwc += dwi;

                LearnValueFunction(x, a, rewards[i]);
            }

            dbc *= learningRate;
            dwc *= learningRate;

            db = momentum * db + (1 - momentum) * dbc;
            dw = momentum * dw + (1 - momentum) * dwc;

            b += db;
            w += dw;
        }

        public void ResetBrain() {
            b = (float) (new System.Random().NextDouble() * 2f - 1f);
            w = (float) (new System.Random().NextDouble() * 2f - 1f);
            
            bv = (float) (new System.Random().NextDouble() * 2f - 1f);
            wv = (float) (new System.Random().NextDouble() * 2f - 1f);

            db = dw = 0f;
        }
    }
}