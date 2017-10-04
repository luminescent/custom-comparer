namespace CustomComparer.Tests
{
    class ClassForTesting<TValue>
    {
        public ClassForTesting(TValue value)
        {
            Value = value;
        }
        public TValue Value { get; set; }
    }
}