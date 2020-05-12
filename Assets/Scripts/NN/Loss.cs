using Num;

namespace NN {
    public abstract class Loss {
        public abstract float Grad(Vector prediction, Vector target);
        public abstract float Value(Vector prediction, Vector target);
    }
}