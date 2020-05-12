using System.Collections.Generic;
using System.Linq;
using Academy;
using UnityEditor;
using UnityEngine;


public class Environment : MonoBehaviour {
    // [SerializeField] private Transform ball;
    // [SerializeField] protected MLAgent agent;
    // [SerializeField] private int maxEpisodes;
    // [SerializeField] private int maxSamples;
    // [SerializeField] private bool heuristic;
    //
    // private List<Sample> episode;
    //
    // public class State {
    //     public static int[] Shape = {4};
    //
    //     public Vector3 ballPosition;
    //     public float agentRotation;
    //
    //     public State(Vector3 ballPosition, float agentRotation) {
    //         this.ballPosition = ballPosition;
    //         this.agentRotation = agentRotation;
    //     }
    //
    //     public float[] ToVector() => new[] {ballPosition.x, ballPosition.y, ballPosition.z, agentRotation};
    // }
    //
    // public readonly struct Sample {
    //     public readonly State state;
    //     public readonly Agent.Action action;
    //     public readonly float reward;
    //
    //     public Sample(State state, Agent.Action action, float reward) {
    //         this.state = state;
    //         this.action = action;
    //         this.reward = reward;
    //     }
    //
    //     public void Deconstruct(out State state, out Agent.Action action, out float reward) {
    //         state = this.state;
    //         action = this.action;
    //         reward = this.reward;
    //     }
    // }
    //
    // private State CurrentState => new State(ball.localPosition, Mathf.Deg2Rad * agent.transform.localEulerAngles.z);
    //
    // // private float CurrentReward => -Math.Abs(ball.localPosition.z);
    // private float CurrentReward => 1;
    //
    // private bool IsTerminal => ball.localPosition.y <= -0.5f ||
    //                            agent.transform.localRotation.eulerAngles.z >= 45 && agent.transform.localRotation.eulerAngles.z <= 360 - 45;
    //
    // private int episodesCompleted = 0;
    //
    // private void Start() {
    //     ResetEnvironment();
    // }
    //
    // private void ResetEnvironment() {
    //     ball.transform.localPosition = new Vector3(0f, 1f, 0f);
    //
    //     var rigidbody = ball.GetComponent<Rigidbody>();
    //     rigidbody.velocity = new Vector3(0f, 0f, 0f);
    //     rigidbody.angularVelocity = new Vector3(0f, 0f, 0f);
    //     ball.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
    //
    //     agent.ResetAgent();
    //
    //     episode = new List<Sample>();
    //     t = 0;
    // }
    //
    // private List<float> scores = new List<float>();
    //
    // private void EndEpisode() {
    //     agent.UpdatePolicy(episode);
    //     if (episodesCompleted >= maxEpisodes) EditorApplication.isPlaying = false;
    //     episodesCompleted += 1;
    //     var cumulativeReward = episode.Select(e => e.reward).Sum();
    //     scores.Add(cumulativeReward);
    //     // Debug.Log($"Episode {episodesCompleted}: {cumulativeReward}");
    //     if (episodesCompleted % 100 == 0)
    //         Debug.Log($"Completed {episodesCompleted} episodes. Average score: {scores.Average()}");
    //     ResetEnvironment();
    // }
    //
    // private int t = 0;
    //
    // private void FixedUpdate() {
    //     var state = CurrentState;
    //
    //     var action = heuristic ? agent.GetHeuristic(state) : agent.GetAction(state);
    //
    //     // if (t == 5) {
    //         if (IsTerminal || episode.Count >= maxSamples) {
    //             EndEpisode();
    //             return;
    //         }
    //
    //         var reward = CurrentReward;
    //         episode.Add(new Sample(state, action, reward));
    //         // t = -1;
    //     // }
    //
    //     agent.ApplyAction(action);
    //     // t++;
    // }
}