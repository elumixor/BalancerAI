namespace NN {
    public abstract class Optimizer {
        public Module net;

        public abstract void Step();
    }
}