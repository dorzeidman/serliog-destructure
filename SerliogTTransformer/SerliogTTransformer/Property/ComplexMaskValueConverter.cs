using System;

namespace SerliogTTransformer.Property
{
    public class ComplexMaskValueConverter : IPropertyValueConverter
    {
        public char Mask { get; }
        public int ShowFirst { get; }
        public int ShowLast { get; }

        public ComplexMaskValueConverter(char mask, int showFirst, int showLast)
        {
            Mask = mask;
            ShowFirst = showFirst;
            ShowLast = showLast;
        }

        public object Convert(object propertyValue)
        {
            if(propertyValue is string stringValue)
            {
                if(ShowFirst == 0 && ShowLast == 0)
                    return new string(Mask, stringValue.Length);

                if (ShowFirst + ShowLast >= stringValue.Length)
                    return stringValue;

                var startOfStr = "";
                var endOfString = "";
                if (ShowFirst > 0)
                    startOfStr = stringValue.Substring(0, ShowFirst);
                if (ShowLast > 0) 
                    endOfString = stringValue.Substring(stringValue.Length - ShowLast, ShowLast);

                var maskLeft = stringValue.Length - (ShowFirst + ShowLast);

                return startOfStr + new string(Mask, maskLeft) + endOfString;
            }

            throw new ArgumentException("Must be of string type for mask", nameof(propertyValue));
        }
    }
}
