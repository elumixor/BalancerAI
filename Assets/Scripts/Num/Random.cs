namespace Num {
    public static class Random {
        private static System.Random random = new System.Random();
        public static float Value => (float) random.NextDouble();
        public static float Range(float min = 0f, float max = 1f) => Value * (max - min) + min;

        public static T Choice<T>(T a, T b, float aProbability) => Value < aProbability ? a : b;
    }
}