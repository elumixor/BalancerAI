using System.Collections.Generic;

namespace Basic {
    public struct Sample {
        public readonly List<State> states;
        public readonly List<int> actions;
        public readonly List<float> rewards;

        public Sample(List<State> states, List<int> actions, List<float> rewards) {
            this.states = states;
            this.actions = actions;
            this.rewards = rewards;
        }

        public void Deconstruct(out List<State> states, out List<int> actions, out List<float> rewards) {
            states = this.states;
            actions = this.actions;
            rewards = this.rewards;
        }
    }
}