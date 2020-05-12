using Num;

namespace NN {
    public class Sequential : Module {
        private Module[] layers;

        public Sequential(params Module[] layers) {
            this.layers = layers;
        }

        // public override Vector Forward(Vector x) {
        //     foreach (var layer in layers) x = layer.Forward(x);
        //     return x;
        // }
        //
        // protected override Vector Backward(Vector dy, Vector x) {
        //     for (var i = layers.Length - 1; i >= 0; i--) dy = layers[i].Backward(dy);
        //     return dy;
        // }
        public override Vector Forward(Vector x) {
            throw new System.NotImplementedException();
        }

        protected override Vector Backward(Vector dy, Vector x) {
            throw new System.NotImplementedException();
        }

        public override void FromBytes(byte[] bytes, int offset, out int newOffset) {
            throw new System.NotImplementedException();
        }
    }
}