using System;
using System.Collections.Generic;
using System.Reflection;
using SerliogTTransformer.Property;

namespace SerliogTTransformer.Transformer
{
    public class TypeTransformer : ITypeTransformer
    {
        private readonly Type _type;
        private readonly bool _ignoreAllNulls;
        private readonly IDictionary<PropertyInfo, IPropertyTransformer> _propertyTransformers;

        public TypeTransformer(Type type,
            bool ignoreAllNulls, 
            IDictionary<PropertyInfo, IPropertyTransformer> propertyTransformers)
        {
            _type = type;
            _ignoreAllNulls = ignoreAllNulls;
            _propertyTransformers = propertyTransformers ?? new Dictionary<PropertyInfo, IPropertyTransformer>();
        }

        public TransformedObject Transform(object value)
        {
            var destObj = new TransformedObject(_type.Name, _propertyTransformers.Count);

            foreach (var item in _propertyTransformers)
            {
                var propValue = item.Key.GetValue(value);
                
                if (_ignoreAllNulls && propValue == null)
                    continue;

                if(item.Value.Ignore(value, propValue))
                    continue;

                var propName = item.Value.ConvertName(item.Key.Name);
                propValue = item.Value.ConvertValue(propValue);

                destObj.Properties.Add(new DestructedProperty(propName,
                    propValue, item.Value.NeedDestructure));
            }

            return destObj;
        }
    }
}
