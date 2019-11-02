using System;

namespace SerliogTTransformer.Property
{
    public class BasePropertyTransformer : IPropertyTransformer
    {
        public string ConvertedName { get; set; }
        public Func<object, object, bool> IgnoreFunc { get; set; } = (o, p) => false;
        public IPropertyValueConverter ValueConverter { get; set; }

        public bool Ignore(object tObject, object propertyValue)
        {
            return IgnoreFunc(tObject, propertyValue);
        }

        public string ConvertName(string name)
        {
            return ConvertedName ?? name;
        }

        public object ConvertValue(object value)
        {
            return ValueConverter != null
                ? ValueConverter.Convert(value)
                : value;
        }
    }
}
