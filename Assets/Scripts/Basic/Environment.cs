using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Basic {
    public class Environment : MonoBehaviour {
        [SerializeField] private Agent agent;
        [SerializeField] private ProbabilityVisualizer probabilityVisualizer;
        [SerializeField] private VVisualizer valueVisualizer;

        [SerializeField] private float travelDistance;

        [SerializeField] private float bigReward;
        [SerializeField] private float smallReward;
        [SerializeField] private float stepReward;

        [SerializeField] private bool automatic;
        [SerializeField] private int maxEpisodes;
        [SerializeField] private int maxSamples;
        [SerializeField] private int episodesBatchSize;

        [SerializeField] private TextMeshProUGUI scoreText;

        [SerializeField, Range(0, 100)] private float timeScale;

        private int episodesTotal;

        private void Start() {
            ResetEnvironment();
        }

        private List<State> states;
        private List<int> actions;
        private List<float> rewards;

        private State CurrentState => new State(agent.transform.position.x);

        private void ApplyAction(int action) {
            if (action == 0) agent.transform.Translate(-travelDistance, 0f, 0f);
            else agent.transform.Translate(travelDistance, 0f, 0f);
        }

        public void Step() {
            var state = CurrentState;
            states.Add(state);

            var action = agent.SampleAction(state);
            actions.Add(action);

            ApplyAction(action);

            var x = agent.transform.localPosition.x;
            var terminalState = Mathf.Abs(x) > 4.5f;

            var reward = terminalState ? x > 4.5f ? bigReward : smallReward : stepReward;
            rewards.Add(reward);

            if (terminalState || rewards.Count >= maxSamples) {
                rs.Add(rewards.Sum());
                episode.Add(new Sample(states, actions, rewards));
                NewEpisode();
            }
        }

        private void UpdateStats() {
            UpdateProbability();
            UpdateValueStats();
        }

        private void UpdateProbability() {
            var p0 = agent.P0(CurrentState);
            probabilityVisualizer.SetValue(p0);
        }

        private void UpdateValueStats() {
            var bins = valueVisualizer.bins;
            var step = 1f / bins;

            for (var i = 0; i < bins; i++) {
                var x = step * (.5f + i);
                valueVisualizer.Set(x, agent.V(x));
            }
        }

        private List<float> rs = new List<float>();
        private List<Sample> episode = new List<Sample>();

        private void NewEpisode() {
            agent.transform.localPosition = new Vector3(Random.Range(-4f, 4f), 0, 0);
            states = new List<State>();
            actions = new List<int>();
            rewards = new List<float>();

            UpdateStats();

            if (episodesTotal % episodesBatchSize == episodesBatchSize - 1) {
                agent.Learn(episode);
                episode = new List<Sample>();
            }

            if (episodesTotal % 100 == 99) {
                print($"Episode {episodesTotal}. Average reward: {rs.Average()}");
                print(agent.BrainString);
                rs = new List<float>();
            }

            episodesTotal += 1;


            // if (episodes >= maxEpisodes) {
            //     ResetEnvironment();
            //     return;
            // }
        }

        public void ResetEnvironment() {
            episodesTotal = 0;
            agent.ResetBrain();
            print(agent.BrainString);
            probabilityVisualizer.SetValue(agent.P0(CurrentState));
            NewEpisode();
        }

        private int t = 0;

        private void FixedUpdate() {
            if (!automatic) {
                if (Input.GetKey(KeyCode.A)) ApplyAction(0);
                else if (Input.GetKey(KeyCode.D)) ApplyAction(1);
            } else {
                Step();
            }

            UpdateStats();
            scoreText.text = rewards.Sum().ToString("#.##");
        }

        private void OnValidate() {
            Time.timeScale = timeScale;
        }
    }
}