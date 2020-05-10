using Num;

namespace NN {
    public abstract class Module {
        // Forward pass: takes a vector and produces a vector
        public abstract Vector Forward(Vector x);
        
        // Backward pass: takes dy, computes and stores inner gradients, returns dx
        public abstract Vector Backward(Vector dy);

        // Updates the weights, learning rate
        public void Step(float stepSize) {
            
        }
    }
}