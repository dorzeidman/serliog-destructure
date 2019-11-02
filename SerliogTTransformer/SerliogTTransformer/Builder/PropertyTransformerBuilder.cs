using SerliogTTransformer.Property;

namespace SerliogTTransformer.Builder
{
    public class PropertyTransformerBuilder
    {
        public PropertyTransformer Base { get; }
        public IPropertyTransformer Custom { get; set; }
        public IPropertyTransformer Final => Custom ?? Base;

        public PropertyTransformerBuilder()
        {
            Base = new PropertyTransformer();
        }

    }
}
