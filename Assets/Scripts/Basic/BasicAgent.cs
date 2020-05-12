using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Academy;
using Communication;
using NN;
using Num;
using UnityEngine;
using R = Num.Random;

namespace Basic {
    public class BasicAgent : Agent {
        // private class Net : Module {
        //     private Linear l1 = new Linear(1, 1);
        //
        //     public override Vector Forward(Vector x) {
        //         return l1.Forward(x);
        //     }
        //
        //     public override Vector Backward(Vector dy) {
        //         return l1.Backward(dy);
        //     }
        // }

        public Module net = new Linear(1, 1);

        // private const float discount = 0.99f;
        // private const float learningRate = 0.01f;
        // private const float regularization = 0.01f;
        // private const float momentum = 0.9f;
        //
        // private float b;
        // private float w;
        //
        // private float db;
        // private float dw;
        //
        // private float bv;
        // private float wv;

        // public object BrainString => $"{w} {b}";

        // public float P0(State state) {
        // return (w * state.position + b).Sigmoid();
        // }

        // public float V(float x) {
        //     return wv * x + bv;
        // }
        //
        // public int SampleAction(State state) {
        //     var r = new System.Random();
        //     return r.NextDouble() < P0(state) ? 0 : 1;
        // }
        //

        //
        // private (float db, float dw) Learn(float state, int action, float discounted) {
        //     // y = w*x + b
        //     var x = state;
        //     var y = (w * x + b).Sigmoid();
        //
        //     var dy = (action == 0 ? 1 - y : -y) * discounted;
        //
        //     var db = dy;
        //     var dw = dy * x;
        //
        //     return (db, dw);
        // }
        //
        // private void LearnValueFunction(float state, int action, float G) {
        //     var previous = V(state);
        // }
        //
        // public void Learn(List<Episode> episodes) {
        //     var dbc = 0f;
        //     var dwc = 0f;
        //
        //     var size = episodes.SelectMany(e => e.actions).Count();
        //
        //     var states = new float[size];
        //     var actions = new int[size];
        //     var rs = new float[size];
        //
        //     var off = 0;
        //     foreach (var (s, a, r) in episodes) {
        //         var discounted = ToDiscounted(r);
        //         for (var i = 0; i < s.Count; i++) {
        //             states[off + i] = s[i].position;
        //             actions[off + i] = a[i];
        //             rs[off + i] = discounted[i];
        //         }
        //
        //         off += s.Count;
        //     }
        //
        //     var rewards = ((Vector) rs).Normalize();
        //
        //     for (var i = 0; i < states.Length; i++) {
        //         var x = states[i];
        //         var a = actions[i];
        //
        //         var (dbi, dwi) = Learn(states[i], actions[i], rewards[i]);
        //         dbc += dbi;
        //         dwc += dwi;
        //
        //         LearnValueFunction(x, a, rewards[i]);
        //     }
        //
        //     dbc *= learningRate;
        //     dwc *= learningRate;
        //
        //     db = momentum * db + (1 - momentum) * dbc;
        //     dw = momentum * dw + (1 - momentum) * dwc;
        //
        //     b += db;
        //     w += dw;
        // }
        //
        // public void ResetBrain() {
        //     b = (float) (new System.Random().NextDouble() * 2f - 1f);
        //     w = (float) (new System.Random().NextDouble() * 2f - 1f);
        //
        //     bv = (float) (new System.Random().NextDouble() * 2f - 1f);
        //     wv = (float) (new System.Random().NextDouble() * 2f - 1f);
        //
        //     db = dw = 0f;
        // }


        [SerializeField] private float travelDistance;

        [SerializeField] private float bigReward;
        [SerializeField] private float smallReward;
        [SerializeField] private float stepReward;

        private TorchCommunicator communicator;

        private void Awake() {
            communicator = new TorchCommunicator(11000);
            Debug.Log(((Linear) net).w + " " + ((Linear) net).b);

        }

        public override void OnNextEpisode() {
            transform.localPosition = new Vector3(R.Range(-4f, 4f), 0, 0);
        }

        protected override (Vector observation, float reward, bool isDone) Observe() {
            var x = transform.localPosition.x;
            var isDone = Mathf.Abs(x) >= 4.5f;
            var reward = isDone ? x >= 4.5f ? bigReward : smallReward : stepReward;
            return ((Vector) x, reward, isDone);
        }

        protected override Vector GetAction(Vector state) {
            var p0 = net.Forward(state).Sigmoid()[0];
            return (Vector) R.Choice(0f, 1f, p0);
        }

        protected override Vector Heuristic() {
            if (Input.GetKey(KeyCode.A)) return (Vector) 0f;
            return (Vector) 1f;
        }

        protected override void ApplyAction(Vector action) {
            if (action[0] < 0.5f) transform.Translate(-travelDistance, 0f, 0f);
            else transform.Translate(travelDistance, 0f, 0f);
        }

        private float[] ToDiscounted(IReadOnlyList<float> rewards, float discount = 0.99f) {
            var last = 0f;
            var discounted = new float[rewards.Count];

            for (var i = rewards.Count - 1; i >= 0; i--)
                discounted[i] = last = last * discount + rewards[i];

            return discounted;
        }

        public override IEnumerator Train(List<Episode> batch) {
            var bytes = batch.ToBytes();
            // Debug.Log("Sent");

            var response = communicator.Send(bytes);


            while (!response.IsCompleted)
                yield return null;

            // Debug.Log("Received");
            net.FromBytes(response.Result, 0, out _);
            Debug.Log(((Linear) net).w + " " + ((Linear) net).b);
        }
    }
}