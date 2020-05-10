using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VVisualizer : MonoBehaviour {
    [SerializeField] public int bins;

    [SerializeField] private GameObject barPrefab;

    private List<RectTransform> bars;

    private void Start() {
        bars = new List<RectTransform>(bins);
        var width = 1f / bins;
        Debug.Log(width);
        for (var i = 0; i < bins; i++) {
            var instance = Instantiate(barPrefab, transform);
            instance.transform.localPosition = new Vector3(.5f - width / 2 - width * i, 0f, 0f);
            instance.transform.localScale = new Vector3(width, 1f, 1f);
            bars.Add(instance.GetComponent<RectTransform>());
        }
    }

    private RectTransform GetBar(float x) {
        var i = Mathf.FloorToInt(x * bins);
        return bars[i];
    }

    public void Set(float state, float v) {
        var bar = GetBar(state);
        var scale = bar.localScale;
        scale.y = v;
        bar.localScale = scale;
    }
}