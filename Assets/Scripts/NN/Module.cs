using System.Linq;
using Communication;
using Num;

namespace NN {
    public abstract class Module : IFromBytes {
        private Vector[] input;

        // Forward pass: takes a vector and produces a vector
        public abstract Vector Forward(Vector x);

        // Backward pass: takes dy, computes and stores inner gradients, returns dx
        protected abstract Vector Backward(Vector dy, Vector x);

        //  Automatic array implementation
        public virtual Vector[] Forward(Vector[] x) {
            input = x;
            return x.Select(Forward).ToArray();
        }

        //  Automatic array implementation
        public virtual Vector[] Backward(Vector[] dy) {
            return dy.Select((dy1, i) => Backward(dy1, input[i])).ToArray();
        }

        // Updates the weights, learning rate
        public void Step(float stepSize) { }
        public abstract void FromBytes(byte[] bytes, int offset, out int newOffset);
    }
}