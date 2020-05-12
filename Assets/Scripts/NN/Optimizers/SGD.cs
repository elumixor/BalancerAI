namespace NN.Optimizers {
    public class SGD : Optimizer {
        public float momentum;
        public float learningRate;
        
        public override void Step() {
            net.Step(learningRate);
        }
    }
}