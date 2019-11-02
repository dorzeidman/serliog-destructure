namespace SerliogTTransformer.Property
{
    public interface IPropertyTransformer
    {
        bool Ignore(object tObject, object propertyValue);
        string ConvertName(string name);
        object ConvertValue(object value);
    }
}
