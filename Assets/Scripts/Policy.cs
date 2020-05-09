using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Policy {
    public float learningRate;
    private readonly float discountFactor;

    private float[] weights;

    private static float[] RandomVector(int size) {
        var res = new float[size];
        for (var i = 0; i < size; i++) res[i] = Random.Range(-1f, 1f);
        return res;
    }

    public Policy(float learningRate, float discountFactor) {
        this.learningRate = learningRate;
        this.discountFactor = discountFactor;

        weights = RandomVector(Environment.State.Shape[0]);
    }

    private float[] GradLogProb(Environment.Sample sample) {
        var (state, action, _) = sample;
        var x = state.ToVector();
        var y = x.dot(weights);

        return action.value == 0 ? x.sub(x.mul(y.Sigmoid())) : x.neg().mul(y.Sigmoid());
    }

    private float[] DiscountRewards(IReadOnlyList<Environment.Sample> samples) {
        var cumulativeRewards = 0f;
        var discounted = new float[samples.Count];

        for (var i = samples.Count - 1; i >= 0; i--)
            discounted[i] = cumulativeRewards = cumulativeRewards * discountFactor + samples[i].reward;

        return discounted;
    }

    public void Update(List<Environment.Sample> samples) {
        // calculate gradients for each action over all observations
        var gradLogP = samples.Select(GradLogProb).ToArray().ToRectangular();

        // var (h, w) = gradLogP.Shape();
        // if (h != samples.Count || w != weights.Length) throw new Exception($"Were not  equal! {h} {w} agains {samples.Count} {weights.Length}");
        // assert grad_log_p.shape == (len(obs), 4)

        // calculate temporaly adjusted, discounted rewards
        var discountedRewards = DiscountRewards(samples);

        // gradients times rewards
        // Debug.Log(gradLogP.Shape());
        // Debug.Log(gradLogP.Transpose().Str());
        var dot = gradLogP.Transpose().dot(discountedRewards);

        // Debug.Log(dot.Length);
        // Debug.Log(weights.Length);

        // gradient ascent on parameters
        // Debug.Log(weights.Str());
        for (var i = 0; i < weights.Length; i++) {
            // Debug.Log(learningRate);
            // Debug.Log(weights[i] + " += " + learningRate * dot[i]);
            weights[i] += learningRate * dot[i];
        }
    }

    public float[] Act(float[] state) {
        var y = state.dot(weights);
        var prob0 = y.Sigmoid();
        // Debug.Log(prob0);
        return new[] {prob0, 1f - prob0};
    }
}