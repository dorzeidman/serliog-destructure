using System;
using System.Collections.Generic;
using System.Reflection;
using SerliogTTransformer.Property;

namespace SerliogTTransformer.Transformer
{
    public class TypeTransformer : ITypeTransformer
    {
        private readonly PropertyInfo[] _writeProperties;
        private readonly IDictionary<string, IPropertyValueConverter> _destProperties;
        private readonly Type _type;
        private readonly IDictionary<string, Func<object, object, bool>> _ignoreFuncProperties;
        private readonly IDictionary<string, string> _propertyNames;
        private readonly bool _ignoreAllNulls;

        public TypeTransformer(Type type,
            PropertyInfo[] writeProperties,
            IDictionary<string, IPropertyValueConverter> destProperties, 
            IDictionary<string, Func<object, object, bool>> ignoreFuncProperties, 
            IDictionary<string, string> propertyNames,
            bool ignoreAllNulls)
        {
            _type = type;
            _destProperties = destProperties;
            _ignoreFuncProperties = ignoreFuncProperties;
            _propertyNames = propertyNames;
            _ignoreAllNulls = ignoreAllNulls;
            _writeProperties = writeProperties;
        }

        public TransformedObject Transform(object value)
        {
            var destObj = new TransformedObject(_type.Name, _writeProperties.Length);

            foreach (var property in _writeProperties)
            {
                var propValue = property.GetValue(value);
                var propName = property.Name;

                if (_ignoreAllNulls && propValue == null)
                    continue;

                //Check Ignore Func
                if (_ignoreFuncProperties.TryGetValue(property.Name, out var func))
                {
                    if (func(value, propValue))
                        continue;
                }
                
                //Check rename
                if (_propertyNames.TryGetValue(property.Name, out var newPropName))
                {
                    propName = newPropName;
                }

                
                if (propValue  != null &&
                    _destProperties.TryGetValue(property.Name, out var destructure))
                {
                    destObj.Properties.Add(new DestructedProperty(propName, destructure.Convert(propValue), false));
                }
                else
                {
                    destObj.Properties.Add(new DestructedProperty(propName, propValue, true));
                }
            }

            return destObj;
        }
    }
}
