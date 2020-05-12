using Num;

namespace NN {
    public class ReLU : Module {
        private Vector x;

        public override Vector Forward(Vector x) {
            return x.Max(0);
        }

        protected override Vector Backward(Vector dy, Vector x) {
            return x.Map(el => el > 0 ? 1 : 0) * dy;
        }

        public override void FromBytes(byte[] bytes, int offset, out int newOffset) {
            throw new System.NotImplementedException();
        }
    }
}