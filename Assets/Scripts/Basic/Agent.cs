using System.Collections.Generic;
using Num;
using UnityEngine;
using static F;

namespace Basic {
    public class Agent : MonoBehaviour {
        private float discount = 0.99f;
        private float learningRate = 0.01f;
        private float b;
        public float P0 => b.Sigmoid();
        public int SampleAction => new System.Random().NextDouble() < P0 ? 0 : 1;

        private float[] ToDiscounted(IReadOnlyList<float> rewards) {
            var cumulativeRewards = 0f;
            var discounted = new float[rewards.Count];

            for (var i = rewards.Count - 1; i >= 0; i--)
                discounted[i] = cumulativeRewards = cumulativeRewards * discount + rewards[i];

            return discounted;
        }

        private void Learn(int action, float reward) {
            var db = b.Sigmoid() * (1 - b.Sigmoid());
            if (action == 1) db = -db;
            b += db * learningRate * reward;
        }

        public void Learn(List<int> actions, List<float> rewards) {
            var r = ToDiscounted(rewards);
            for (var i = 0; i < r.Length; i++) Learn(actions[i], rewards[i]);
        }

        public void ResetBrain() {
            b = (float) (new System.Random().NextDouble() * 2f - 1f);
        }
    }
}