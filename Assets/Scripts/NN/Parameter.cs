using Num;

namespace NN {
    public class Parameter {
        public static implicit operator Parameter(Vector v) => new Parameter();
        public static implicit operator Parameter(Matrix mat) => new Parameter();
    }
}