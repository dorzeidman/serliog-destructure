using SerliogTTransformer.Property;
using Xunit;

namespace SerliogDestructure.Tests.Property
{
    public class PropertyMaskValueTests
    {
        [Fact]
        public void GetValue_ShowFirstAndLast0_ReturnMaskByStringLength()
        {
            var propMask = new ComplexMaskValueConverter('*', 0, 0);
            var propValue = "sadasfdsfs";

            var maskValue = propMask.Convert(propValue);

            Assert.Equal(propValue.Length, maskValue.ToString().Length);
            Assert.Contains('*', maskValue.ToString());
        }

        [Fact]
        public void GetValue_ShowFirstAndLastTooLong_ReturnEntireString()
        {
            var propMask = new ComplexMaskValueConverter('*', 5, 6);
            var propValue = "sadasfdsfs";

            var maskValue = propMask.Convert(propValue);

            Assert.Equal(propValue, maskValue);
        }

        [Fact]
        public void GetValue_ShowFirstOnly_ReturnFirst3CharsAndMaskAfter()
        {
            var propMask = new ComplexMaskValueConverter('*', 3, 0);
            var propValue = "abcdef";

            var maskValue = propMask.Convert(propValue);

            Assert.Equal("abc***", maskValue);
        }

        [Fact]
        public void GetValue_ShowLastOnly_ReturnFirstMaskThan3Chars()
        {
            var propMask = new ComplexMaskValueConverter('*', 0, 3);
            var propValue = "abcdef";

            var maskValue = propMask.Convert(propValue);

            Assert.Equal("***def", maskValue);
        }
        [Fact]
        public void GetValue_ShowFirstAndLast_ReturnFirst3CharsThanMaskThanLast3Chars()
        {
            var propMask = new ComplexMaskValueConverter('*', 3, 3);
            var propValue = "abcdefgh";

            var maskValue = propMask.Convert(propValue);

            Assert.Equal("abc**fgh", maskValue);
        }
    }
}
