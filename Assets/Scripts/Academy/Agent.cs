﻿using System;
using System.Collections;
using System.Collections.Generic;
using Num;
using UnityEngine;

namespace Academy {
    public abstract class Agent : MonoBehaviour {
        [SerializeField] private float reactionTime;
        [SerializeField] public bool useHeuristic;
        public event Action<Vector, Vector, float, bool> ActionTaken;

        private float lastReactionTime;

        private void FixedUpdate() {
            if (useHeuristic) {
                ApplyAction(Heuristic());
                return;
            }

            // if (Time.fixedTime - lastReactionTime <= reactionTime) return;
            // lastReactionTime += reactionTime;
            PerformStep();
        }

        private void PerformStep() {
            var (obs, reward, isDone) = Observe();
            var action = GetAction(obs);
            ActionTaken?.Invoke(obs, action, reward, isDone);
            ApplyAction(action);
        }

        // Resets agent (at the start of new episode)
        public abstract void OnNextEpisode();

        // Define how does agent observe state
        protected abstract (Vector observation, float reward, bool isDone) Observe();

        // Define how does agent decide which action to take
        protected abstract Vector GetAction(Vector state);

        // ...or use heuristic
        protected abstract Vector Heuristic();

        // Define how does the action take effect
        protected abstract void ApplyAction(Vector action);

        // Train agent
        public abstract IEnumerator Train(List<Episode> batch);
    }
}