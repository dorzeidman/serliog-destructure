namespace SerliogTTransformer.Property
{
    class SimpleMaskValueConverter : IPropertyValueConverter
    {
        private const int DefaultLength = 3;
        public char Mask { get; }

        public SimpleMaskValueConverter(char mask)
        {
            Mask = mask;
        }

        public object Convert(object propertyValue)
        {
            return new string(Mask, DefaultLength);
        }
    }
}
