namespace SerliogTTransformer.Builder
{
    public interface ITypeTransformerBinder<T> where T: class
    {
        void Bind(ITypeTransformerBuilder<T> builder);
    }
}
