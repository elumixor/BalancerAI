using Num;
using UnityEngine;
using static F;

namespace Basic {
    public class Agent : MonoBehaviour {
        private float b;
        public float P0 => b.Sigmoid();
        public int SampleAction => new System.Random().NextDouble() < P0 ? 0 : 1;

        public void GetReward(int action, float reward) {
            var db = b.Sigmoid() * (1 - b.Sigmoid());
            if (action == 1) db = -db;
            Debug.Log(db);
            b += db * 0.01f * reward;
        }

        public void ResetBrain() {
            b = (float) (new System.Random().NextDouble() * 2f - 1f);
        }
    }
}