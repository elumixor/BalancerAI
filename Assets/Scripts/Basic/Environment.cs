using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Basic {
    public class Environment : MonoBehaviour {
        [SerializeField] private Agent agent;
        [SerializeField] private ProbabilityVisualizer visualizer;

        [SerializeField] private float bigReward;
        [SerializeField] private float smallReward;

        [SerializeField] private bool automatic;
        [SerializeField] private int maxEpisodes;
        [SerializeField] private int maxSamples;

        private int episodes;

        private void Start() {
            ResetEnvironment();
        }

        private List<int> actions;
        private List<float> rewards;

        public void Step() {
            var p0 = agent.P0;
            visualizer.SetValue(p0);

            var action = agent.SampleAction;
            print(action);
            actions.Add(action);

            if (action == 0) agent.transform.Translate(-1f, 0f, 0f);
            else agent.transform.Translate(1f, 0f, 0f);

            var x = agent.transform.localPosition.x;
            var terminalState = Mathf.Abs(x) > 4.5f;

            var reward = (float) (terminalState ? (x > 4.5f ? bigReward : smallReward) : -0.1);
            rewards.Add(reward);

            agent.Learn(actions, rewards);

            if (terminalState || rewards.Count >= maxSamples) {
                print($"Episode {episodes}. Total Reward: {rewards.Sum()}");
                NewEpisode();
            }
        }

        private void NewEpisode() {
            agent.transform.localPosition = Vector3.zero;
            actions = new List<int>();
            rewards = new List<float>();

            episodes += 1;
            // if (episodes >= maxEpisodes) {
            //     ResetEnvironment();
            //     return;
            // }
        }

        public void ResetEnvironment() {
            episodes = 0;
            agent.ResetBrain();
            visualizer.SetValue(agent.P0);
            NewEpisode();
        }

        private int t = 0;

        private void FixedUpdate() {
            if (!automatic) return;

            Step();
        }
    }
}