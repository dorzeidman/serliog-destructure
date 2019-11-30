using SerliogTTransformer.Property;

namespace SerliogTTransformer.Tests.Fixtures
{
    public class PropertyTransformerFixture : IPropertyTransformer
    {
        public bool NeedDestructure { get; set; }
        public bool ShouldIgnore { set; get; }
        public string FixtureName { get; set; }
        public object FixtureValue { get; set; }

        public bool Ignore(object tObject, object propertyValue)
        {
            return ShouldIgnore;
        }

        public string ConvertName(string name)
        {
            return FixtureName ?? name;
        }

        public object ConvertValue(object value)
        {
            return FixtureValue ?? value;
        }
    }
}
