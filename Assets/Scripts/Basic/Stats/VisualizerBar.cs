using TMPro;
using UnityEngine;

namespace Basic.Stats {
    public class VisualizerBar : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private RectTransform bar;

        public void SetValue(float probability) {
            text.text = $"{probability * 100:#.##}%";
            bar.localScale = new Vector3(1, probability, 1);
        }
    }
}
