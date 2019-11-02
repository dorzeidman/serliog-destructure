using SerliogTTransformer.Converter;
using SerliogTTransformer.Property;
using Xunit;

namespace SerliogDestructure.Tests.Property
{
    public class SimplePropertyMaskValueTests
    {
        [Fact]
        public void GetValue_AnyValue_Return3MaskChars()
        {
            var propMask = new SimpleMaskValueConverter('*');
            var maskValue = propMask.Convert("sadasfdsfs");

            Assert.Equal("***", maskValue);
        }

    }
}
