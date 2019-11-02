using System.Collections.Generic;

namespace SerliogTTransformer.Transformer
{  
    public class TransformedObject
    {
        public string TypeTag { get; }
        public List<DestructedProperty> Properties { get; }

        public TransformedObject(string typeTag, int propertyCapacity)
        {
            TypeTag = typeTag;
            Properties = new List<DestructedProperty>(propertyCapacity);
        }
    }
}
