using UnityEngine;

namespace Basic.Stats {
    public class ProbabilityVisualizer : MonoBehaviour {
        [SerializeField] private VisualizerBar p0;
        [SerializeField] private VisualizerBar p1;

        public void SetValue(float p0Probability) {
            p0.SetValue(p0Probability);
            p1.SetValue(1 - p0Probability);
        }
    }
}