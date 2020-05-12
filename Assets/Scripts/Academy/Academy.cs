using System.Collections.Generic;
using System.Linq;
using Num;
using UnityEditor;
using UnityEngine;

namespace Academy {
    /// <summary>
    /// Controller class for training.
    ///
    /// Decides when to reset environment and agents.
    /// Collects training samples, forms batches and sends them to the agent to train.
    /// </summary>
    public class Academy : MonoBehaviour {
        [SerializeField] private Environment environment;
        [SerializeField] protected Agent agent;

        [SerializeField] private int maxSamplesInEpisode = 100;
        [SerializeField] private int episodesInBatch = 10;
        [SerializeField] private int maximumEpochs = 10;

        private List<Episode> episodes;
        protected Episode currentEpisode;

        private int epoch = 1;

        public void Start() {
            episodes = new List<Episode>(episodesInBatch);
            currentEpisode = new Episode();

            agent.ActionTaken += OnActionTaken;

            NextEpisode();
        }

        protected virtual void NextEpisode() {
            if (environment != null) environment.OnNextEpisode();
            agent.OnNextEpisode();
        }

        protected virtual void OnActionTaken(Vector obs, Vector action, float reward, bool isDone) {
            currentEpisode.AddSample(obs, action, reward);

            if (isDone || currentEpisode.Length >= maxSamplesInEpisode) EpisodeEnded();
        }

        private void EpisodeEnded() {
            episodes.Add(currentEpisode);
            currentEpisode = new Episode();

            // print($"Length {episodes.Count}");
            
            if (episodes.Count >= episodesInBatch) {
                Debug.Log($"Epoch {epoch} completed. Average reward: {episodes.Average(e => e.rewards.Sum())}");
                StartCoroutine(agent.Train(episodes));
                episodes = new List<Episode>(episodesInBatch);
                epoch++;
            }

            if (epoch >= maximumEpochs) {
                Debug.Log("Training completed.");
                EditorApplication.isPaused = true;
            } else {
                NextEpisode();
            }
        }
    }
}