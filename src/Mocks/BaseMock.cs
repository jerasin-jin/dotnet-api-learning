namespace RestApiSample.Mocks
{
    public abstract class BaseMock<T>
    {
        public T _value { get; set; } = default(T)!;
    }
}