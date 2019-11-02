namespace SerliogTTransformer.Transformer
{   
    public class DestructedProperty
    {
        public string Name { get; }
        public object Value { get; }
        public bool NeedsDestruct { get; }

        public DestructedProperty(string name, object value, bool needsDestruct)
        {
            Name = name;
            Value = value;
            NeedsDestruct = needsDestruct;
        }
    }

}
