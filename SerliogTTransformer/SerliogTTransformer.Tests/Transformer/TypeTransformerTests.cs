using System.Collections.Generic;
using System.Reflection;
using SerliogTTransformer.Property;
using SerliogTTransformer.Tests.Fixtures;
using SerliogTTransformer.Transformer;
using Xunit;

namespace SerliogTTransformer.Tests.Transformer
{
    public class TypeTransformerTests
    {
        [Fact]
        public void Transform_IgnoreAllNulls_ResultWithEmptyProperties()
        {
            var transformer = new TypeTransformer(typeof(NullClassFixture), true, null);

            var tranObj = transformer.Transform(new NullClassFixture());

            Assert.Equal(typeof(NullClassFixture).Name, tranObj.TypeTag);
            Assert.Empty(tranObj.Properties);
        }

        [Fact]
        public void Transform_CheckIgnore_PropertyNotInResult()
        {
            var numProp = PropertyFinder.FromExpression<NullClassFixture>(f => f.Num);
            var tranDic = new Dictionary<PropertyInfo, IPropertyTransformer>
            {
                [numProp] = new PropertyTransformerFixture
                {
                    ShouldIgnore = true
                }
            };

            var transformer = new TypeTransformer(typeof(NullClassFixture), false, tranDic);

            var tranObj = transformer.Transform(new NullClassFixture());

            Assert.DoesNotContain(tranObj.Properties, x => x.Name == nameof(NullClassFixture.Num));
        }

        [Fact]
        public void Transform_CheckPropertyTransformer()
        {
            var numProp = PropertyFinder.FromExpression<NullClassFixture>(f => f.Num);
            var tranDic = new Dictionary<PropertyInfo, IPropertyTransformer>
            {
                [numProp] = new PropertyTransformerFixture
                {
                    ShouldIgnore = false,
                    FixtureName = "AAA",
                    FixtureValue = "BBB",
                    NeedDestructure = true
                }
            };

            var transformer = new TypeTransformer(typeof(NullClassFixture), false, tranDic);

            var tranObj = transformer.Transform(new NullClassFixture());

            Assert.Equal(typeof(NullClassFixture).Name, tranObj.TypeTag);
            Assert.Contains(tranObj.Properties, 
                x => x.Name == "AAA" 
                     && x.Value.Equals("BBB") 
                     && x.NeedsDestruct);
        }
    }
}
