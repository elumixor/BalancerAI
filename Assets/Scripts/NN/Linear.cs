using System.Collections.Generic;
using Num;
using UnityEngine;

namespace NN {
    public class Linear : Module {
        private readonly int inFeatures;
        private readonly int outFeatures;

        private Matrix w;
        private Vector b;

        private Vector x;

        private Matrix dw;
        private Vector db;

        public Linear(int inFeatures, int outFeatures) {
            this.inFeatures = inFeatures;
            this.outFeatures = outFeatures;

            w = Matrix.Random((outFeatures, inFeatures), -1f, 1f);
            b = Vector.Random(outFeatures, -1f, 1f);
        }

        public override Vector Forward(Vector x) {
            this.x = x;
            return w.Dot(x) + b;
        }

        public override Vector Backward(Vector dy) {
            db = dy;
            dw = dy.Outer(x);
            return w.T.Dot(dy);
        }
        
        protected List<Parameter> Parameters => new List<Parameter> { w, b };
    }
}