using System;

namespace Num {
    public class DimensionException : Exception {
        public DimensionException(string message) : base(message) { }
    }
}