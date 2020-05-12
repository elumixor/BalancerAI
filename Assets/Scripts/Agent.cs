// using System.Collections.Generic;
// using UnityEngine;
// using Random = UnityEngine.Random;
//
// public class Agent : MonoBehaviour {
//     [SerializeField] private float speed;
//     private Policy policy;
//
//     private void Awake() {
//         policy = new Policy(0.002f, 0.99f);
//     }
//
//     public class Action {
//         private static readonly System.Random random = new System.Random();
//
//         public readonly int value;
//         public readonly float direction;
//
//         public Action(int value) {
//             this.value = value;
//             direction = value * 2f - 1f;
//         }
//
//         public static Action Sample(float[] probabilities) {
//             // Debug.Log(probabilities.Str());
//             return new Action(random.NextDouble() < probabilities[0] ? 0 : 1);
//         }
//
//         public static Action Max(float[] probabilities) {
//             return new Action(probabilities[0] > probabilities[1] ? 0 : 1);
//         }
//
//         public static Action Random => new Action(random.NextDouble() < .5f ? 0 : 1);
//     }
//
//     public void ResetAgent() {
//         transform.rotation = Quaternion.identity;
//         var rigidbody = GetComponent<Rigidbody>();
//         rigidbody.velocity = new Vector3(0f,0f,0f); 
//         rigidbody.angularVelocity = new Vector3(0f,0f,0f);
//         transform.localRotation = Quaternion.Euler(new Vector3(0f,0f,0f));
//     }
//
//
//     public Action GetHeuristic(Environment.State state) {
//         Debug.Log("heuristic");
//         return Input.GetKey(KeyCode.D) ? new Action(1) : new Action(0);
//     }
//
//     private float epsilon = .8f;
//     public Action GetAction(Environment.State state) {
//         // var a = Action.Random;
//         // Debug.Log(a.direction);
//         // return a;
//         return Random.value < epsilon ? Action.Random : Action.Sample(policy.Act(state.ToVector()));
//         // return Action.Max(policy.Act(state.ToVector()));
//     }
//
//     public void ApplyAction(Action action) {
//         var direction = action.direction;
//         var angle = direction * Time.deltaTime * speed;
//         transform.Rotate(0, 0, angle, Space.Self);
//     }
//
//     private int updates = 0;
//
//     public void UpdatePolicy(List<Environment.Sample> episode) {
//         // updates++;
//         policy.Update(episode);
//         // if (updates == 25) {
//             // updates = 0;
//             // policy.learningRate *= .9f;
//             // epsilon *= .9f;
//         // }
//     }
// }