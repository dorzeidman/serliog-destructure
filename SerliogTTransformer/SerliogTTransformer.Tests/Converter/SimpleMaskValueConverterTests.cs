using SerliogTTransformer.Converter;
using Xunit;

namespace SerliogTTransformer.Tests.Converter
{
    public class SimpleMaskValueConverterTests
    {
        [Fact]
        public void Convert_AnyValue_Return3MaskChars()
        {
            var propMask = new SimpleMaskValueConverter('*');
            var maskValue = propMask.Convert("sadasfdsfs");

            Assert.Equal("***", maskValue);
        }

    }
}
