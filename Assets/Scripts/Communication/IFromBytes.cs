namespace Communication {
    public interface IFromBytes {
        void FromBytes(byte[] bytes, int offset, out int newOffset);
    }
}