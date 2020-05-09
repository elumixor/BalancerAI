using System;
using UnityEngine;

namespace Basic {
    public class Environment : MonoBehaviour {
        [SerializeField] private Agent agent;
        [SerializeField] private ProbabilityVisualizer visualizer;

        [SerializeField] private float bigReward;
        [SerializeField] private float smallReward;

        [SerializeField] private bool automatic;
        [SerializeField] private int maxEpisodes;

        private int episodes;

        private void Start() {
            ResetEnvironment();
        }

        public void Step() {
            var p0 = agent.P0;
            visualizer.SetValue(p0);
            var action = agent.SampleAction;
            var reward = action == 0 ? smallReward : bigReward;
            agent.GetReward(action, reward);
            Debug.Log(action);

            episodes += 1;
            if (episodes >= maxEpisodes) {
                ResetEnvironment();
            }
        }

        public void ResetEnvironment() {
            episodes = 0;
            agent.ResetBrain();
            visualizer.SetValue(agent.P0);
        }

        private int t = 0;

        private void FixedUpdate() {
            if (!automatic) return;

            Step();
        }
    }
}