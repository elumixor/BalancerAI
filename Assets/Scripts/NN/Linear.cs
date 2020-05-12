using Communication;
using Num;

namespace NN {
    public class Linear : Module {
        private readonly int inFeatures;
        private readonly int outFeatures;

        public Matrix w;
        public Vector b;

        private Matrix dw;
        private Vector db;

        public Linear(int inFeatures, int outFeatures) {
            this.inFeatures = inFeatures;
            this.outFeatures = outFeatures;

            w = Matrix.Random((outFeatures, inFeatures), -1f, 1f);
            b = Vector.Random(outFeatures, -1f, 1f);

            db = Vector.Zeros(outFeatures);
            dw = Matrix.Zeros((outFeatures, inFeatures));
        }

        public override Vector Forward(Vector x) {
            return w.Dot(x) + b;
        }

        protected override Vector Backward(Vector dy, Vector x) {
            db += dy;
            dw += dy.Outer(x);
            return w.T.Dot(dy);
        }

        public override Vector[] Backward(Vector[] dy) {
            db = Vector.Zeros(outFeatures);
            dw = Matrix.Zeros((outFeatures, inFeatures));
            return base.Backward(dy);
        }

        public override void FromBytes(byte[] bytes, int offset, out int newOffset) {
            w = bytes.ToMatrix(offset, out newOffset);
            b = bytes.ToVector(newOffset, out newOffset);
        }
    }
}