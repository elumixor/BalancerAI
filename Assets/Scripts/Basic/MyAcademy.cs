using System;
using System.Collections.Generic;
using System.Linq;
using Academy;
using Basic.Stats;
using NN;
using Num;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Basic {
    public class MyAcademy : Academy.Academy {
        [SerializeField] private ProbabilityVisualizer probabilityVisualizer;
        [SerializeField] private VVisualizer valueVisualizer;
        [SerializeField] private TextMeshProUGUI scoreText;

        [SerializeField] private bool useHeuristic;

        [SerializeField, Range(0, 100)] private float timeScale;

        private int episodesTotal;

        private void UpdateStats() {
            UpdateProbability();
            // UpdateValueStats();
            scoreText.text = currentEpisode.rewards.Sum().ToString("#.##");
        }

        private void UpdateProbability() {
            var p0 = ((BasicAgent) agent).net.Forward((Vector) agent.transform.position.x)[0].Sigmoid();
            probabilityVisualizer.SetValue(p0);
        }

        private void Update() {
            UpdateStats();
        }

        // private void UpdateValueStats() {
        //     var bins = valueVisualizer.bins;
        //     var step = 1f / bins;
        //
        //     for (var i = 0; i < bins; i++) {
        //         var x = step * (.5f + i);
        //         valueVisualizer.Set(x, agent.V(x));
        //     }
        // }

        protected override void NextEpisode() {
            base.NextEpisode();
            UpdateStats();
        }

        protected override void OnActionTaken(Vector obs, Vector action, float reward, bool isDone) {
            base.OnActionTaken(obs, action, reward, isDone);
            UpdateStats();
        }

        private void OnValidate() {
            Time.timeScale = timeScale;
            agent.useHeuristic = useHeuristic;
        }
    }
}