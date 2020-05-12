using System.Collections.Generic;
using Communication;
using NN;
using Num;

namespace Academy {
    public class Episode : IToBytes {
        public readonly List<Vector> states;
        public readonly List<Vector> actions;
        public readonly List<float> rewards;

        public Episode() {
            states = new List<Vector>();
            actions = new List<Vector>();
            rewards = new List<float>();
        }

        public Episode(List<Vector> states, List<Vector> actions, List<float> rewards) {
            this.states = states;
            this.actions = actions;
            this.rewards = rewards;
        }

        public int Length => states.Count;


        public void Deconstruct(out List<Vector> states, out List<Vector> actions, out List<float> rewards) {
            states = this.states;
            actions = this.actions;
            rewards = this.rewards;
        }

        public void AddSample(Vector state, Vector action, float reward) {
            states.Add(state);
            actions.Add(action);
            rewards.Add(reward);
        }

        public byte[] ToBytes() {
            return ByteTransformer.Concat(states.ToBytes(), actions.ToBytes(), rewards.ToBytes());
        }
    }
}