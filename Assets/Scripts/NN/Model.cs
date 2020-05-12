using System.Collections.Generic;
using System.Linq;
using Num;

namespace NN {
    public class Model {
        private Module net;
        private Loss loss;
        private Optimizer optimizer;

        public Model(Module net, Loss loss, Optimizer optimizer) {
            this.net = net;
            this.loss = loss;
            this.optimizer = optimizer;
        }

        public Vector Predict(Vector x) {
            return net.Forward(x);
        }

        public void Fit(IEnumerable<(Vector data, Vector label)> trainingData) {
            var d = trainingData.Select(t => t.data).ToArray();
            var l = trainingData.Select(t => t.label).ToArray();
            var prediction = net.Forward(d);
            // var dy = loss.Grad(prediction, l);
            // net.Backward((Vector) dy);
            optimizer.Step();
        }
    }
}