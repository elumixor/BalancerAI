using System.Collections.Generic;
using Num;
using UnityEngine;
using static F;

namespace Basic {
    public class Agent : MonoBehaviour {
        private float discount = 0.99f;
        private float learningRate = 0.002f;
        private float regularization = 0.1f;
        private float b;
        private float w;

        public float P0(State state) {
            return (w * state.position + b).Sigmoid();
        }

        public int SampleAction(State state) {
            return new System.Random().NextDouble() < P0(state) ? 0 : 1;
        }

        private float[] ToDiscounted(IReadOnlyList<float> rewards) {
            var cumulativeRewards = 0f;
            var discounted = new float[rewards.Count];

            for (var i = rewards.Count - 1; i >= 0; i--)
                discounted[i] = cumulativeRewards = cumulativeRewards * discount + rewards[i];

            return discounted;
        }

        private void Learn(State state, int action, float reward, int total) {
            // y = w*x + b
            var dy = b.Sigmoid() * (1 - b.Sigmoid());
            if (action == 1) dy = -dy;

            var db = dy;
            var dw = dy * state.position;

            b += db * learningRate * reward / total;
            w += dw * learningRate * reward / total;
        }

        public void Learn(List<Sample> episodes) {
            print(w + " " + b);
            foreach (var episode in episodes) {
                var (states, actions, rewards) = episode;
                var r = ToDiscounted(rewards);
                for (var i = 0; i < r.Length; i++) Learn(states[i], actions[i], r[i], rewards.Count);

                // L2 regularization?
                w -= 2 * w * learningRate * regularization;
            }
            print(w + " " + b);
            print("\n");
        }

        public void ResetBrain() {
            b = (float) (new System.Random().NextDouble() * 2f - 1f);
            w = (float) (new System.Random().NextDouble() * 2f - 1f);
        }
    }
}