using System.Linq;
using SerliogTTransformer.Property;
using SerliogTTransformer.Tests.Fixtures;
using Xunit;

namespace SerliogTTransformer.Tests.Property
{
    public class PropertyFinderTests
    {
        [Fact]
        public void GetAll_FixtureClass1_ContainsNestedProperty()
        {
            var properties = PropertyFinder.GetAll(typeof(FixtureClass1)).ToArray();

            Assert.Contains(properties, p => p.Name == nameof(FixtureClass1.Class2));
        }

        [Fact]
        public void GetAll_FixtureClass1_ContainsProperty()
        {
            var properties = PropertyFinder.GetAll(typeof(FixtureClass1)).ToArray();

            Assert.Contains(properties, p => p.Name == nameof(FixtureClass1.Class2));
        }

        [Fact]
        public void FromExpression_FixtureClass1_Num2Property()
        {
            var property = PropertyFinder.FromExpression<FixtureClass1>(f => f.Num2);
            Assert.Equal(nameof(FixtureClass1.Num2), property.Name);
        }
    }
}
