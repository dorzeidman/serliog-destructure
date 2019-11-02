using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using SerliogTTransformer.Converter;
using SerliogTTransformer.Property;
using SerliogTTransformer.Transformer;

namespace SerliogTTransformer.Builder
{
    public class TypeTransformerBuilder<T> : ITypeTransformerBuilder<T>
        where T : class
    {
        private bool _ignoreAllNulls;
        private readonly Dictionary<PropertyInfo, PropertyTransformerBuilder> _propertyTransformers;

        public TypeTransformerBuilder()
        {
            _propertyTransformers = PropertyFinder.GetPropertiesRecursive(typeof(T))
                .ToDictionary(x => x, x => new PropertyTransformerBuilder());
        }

        public ITypeTransformerBuilder<T> IgnoreAllIfNull()
        {
            _ignoreAllNulls = true;
            return this;
        }

        public ITypeTransformerBuilder<T> Ignore(Expression<Func<T, object>> expression)
        {
            var property = GetPropertyInfo(expression);
            _propertyTransformers.Remove(property);

            return this;
        }

        public ITypeTransformerBuilder<T> Ignore(string propertyName)
        {
            var property = GetPropertyInfo(propertyName);
            _propertyTransformers.Remove(property);

            return this;
        }

        public ITypeTransformerBuilder<T> Ignore(Func<PropertyInfo, bool> propertyFunc)
        {
            var properties = GetPropertyInfos(propertyFunc).ToArray();
            foreach (var property in properties)
            {
                _propertyTransformers.Remove(property);
            }

            return this;
        }

        public ITypeTransformerBuilder<T> IgnoreIfNull(Expression<Func<T, object>> expression)
        {
            UpdateProperty(expression, t => t.IgnoreFunc = (o, p) => p == null);
            return this;
        }

        public ITypeTransformerBuilder<T> IgnoreIfNull(string propertyName)
        {
            UpdateProperty(propertyName, t => t.IgnoreFunc = (o, p) => p == null);
            return this;
        }

        public ITypeTransformerBuilder<T> IgnoreIfNull(Func<PropertyInfo, bool> propertyFunc)
        {
            UpdateProperties(propertyFunc, t => t.IgnoreFunc = (o, p) => p == null);
            return this;
        }

        public ITypeTransformerBuilder<T> IgnoreIf(Expression<Func<T, object>> expression, Func<T,object,bool> func)
        {
            UpdateProperty(expression, t => t.IgnoreFunc = (o, p) => func((T)o,p));
            return this;
        }

        public ITypeTransformerBuilder<T> IgnoreIf(string propertyName, Func<T,object,bool> func)
        {
            UpdateProperty(propertyName, t => t.IgnoreFunc = (o, p) => func((T)o, p));
            return this;
        }

        public ITypeTransformerBuilder<T> IgnoreIf(Func<PropertyInfo, bool> propertyFunc, Func<T, object, bool> func)
        {
            UpdateProperties(propertyFunc, t => t.IgnoreFunc = (o, p) => func((T)o, p));
            return this;
        }

        public ITypeTransformerBuilder<T> Mask(Expression<Func<T, object>> expression, char mask = '*')
        {
            UpdateProperty(expression, t => t.ValueConverter = new SimpleMaskValueConverter(mask));
            return this;
        }

        public ITypeTransformerBuilder<T> Mask(string propertyName, char mask = '*')
        {
            UpdateProperty(propertyName, t => t.ValueConverter = new SimpleMaskValueConverter(mask));
            return this;
        }

        public ITypeTransformerBuilder<T> Mask(Func<PropertyInfo, bool> propertyFunc, char mask = '*')
        {
            UpdateProperties(propertyFunc, t => t.ValueConverter = new SimpleMaskValueConverter(mask));
            return this;
        }

        public ITypeTransformerBuilder<T> Mask(Expression<Func<T, object>> expression, int showFirst, int showLast
            , char mask = '*')
        {
            UpdateProperty(expression, t => t.ValueConverter 
                = new ComplexMaskValueConverter(mask, showFirst, showLast));
            return this;
        }

        public ITypeTransformerBuilder<T> Mask(string propertyName, int showFirst, int showLast, char mask = '*')
        {
            UpdateProperty(propertyName, t => t.ValueConverter
                = new ComplexMaskValueConverter(mask, showFirst, showLast));
            return this;
        }

        public ITypeTransformerBuilder<T> Mask(Func<PropertyInfo, bool> propertyFunc, int showFirst, int showLast, char mask = '*')
        {
            UpdateProperties(propertyFunc, t => t.ValueConverter
                = new ComplexMaskValueConverter(mask, showFirst, showLast));
            return this;
        }

        public ITypeTransformerBuilder<T> Rename(Expression<Func<T, object>> expression, string newName)
        {
            UpdateProperty(expression, t => t.ConvertedName = newName);
            return this;
        }

        public ITypeTransformerBuilder<T> Rename(string propertyName, string newName)
        {
            UpdateProperty(propertyName, t => t.ConvertedName = newName);
            return this;
        }

        public ITypeTransformerBuilder<T> Convert(Expression<Func<T, object>> expression, 
            IPropertyValueConverter converter)
        {
            UpdateProperty(expression, t => t.ValueConverter = converter);
            return this;
        }

        public ITypeTransformerBuilder<T> Convert(string propertyName, IPropertyValueConverter converter)
        {
            UpdateProperty(propertyName, t => t.ValueConverter = converter);
            return this;
        }

        public ITypeTransformerBuilder<T> Convert(Func<PropertyInfo, bool> propertyFunc, IPropertyValueConverter converter)
        {
            UpdateProperties(propertyFunc, t => t.ValueConverter = converter);
            return this;
        }

        public ITypeTransformerBuilder<T> Transform(Expression<Func<T, object>> expression, IPropertyTransformer transformer)
        {
            var property = GetPropertyInfo(expression);
            if (_propertyTransformers.ContainsKey(property))
                _propertyTransformers[property].Custom = transformer;

            return this;
        }

        public ITypeTransformerBuilder<T> Transform(string propertyName, IPropertyTransformer transformer)
        {
            var property = GetPropertyInfo(propertyName);
            if (_propertyTransformers.ContainsKey(property))
                _propertyTransformers[property].Custom = transformer;

            return this;
        }

        public ITypeTransformerBuilder<T> Transform(Func<PropertyInfo, bool> propertyFunc, IPropertyTransformer transformer)
        {
            var properties = GetPropertyInfos(propertyFunc).ToArray();
            foreach (var property in properties)
            {
                _propertyTransformers[property].Custom = transformer;
            }

            return this;
        }

        public TypeTransformer Build()
        {
            var transformerDicTemp =_propertyTransformers
                .ToDictionary(x => x.Key, x => x.Value.Final);

            return new TypeTransformer(typeof(T), _ignoreAllNulls,
                transformerDicTemp);
        }

        private PropertyInfo GetPropertyInfo(Expression<Func<T, object>> expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            var property = PropertyFinder.FromExpression(expression);
            return property ?? throw new ArgumentException("Invalid Property expression", nameof(expression));
        }

        private IEnumerable<PropertyInfo> GetPropertyInfos(Func<PropertyInfo, bool> propertyFunc)
        {
            return _propertyTransformers.Keys.Where(propertyFunc);
        }
        
        private PropertyInfo GetPropertyInfo(string propertyName)
        {
            if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));
            var property = typeof(T).GetProperty(propertyName);
            return property ?? throw new ArgumentException($"Invalid PropertyName:{propertyName}", nameof(property));
        }

        private void UpdateProperty(Expression<Func<T, object>> expression, Action<PropertyTransformer> action)
        {
            var property = GetPropertyInfo(expression);
            if(_propertyTransformers.TryGetValue(property, out var transformerBuilder))
            {
                action?.Invoke(transformerBuilder.Base);
                transformerBuilder.Base.NeedDestructure = false;
            }
        }

        private void UpdateProperty(string propertyName, Action<PropertyTransformer> action)
        {
            var property = GetPropertyInfo(propertyName);
            if (_propertyTransformers.TryGetValue(property, out var transformerBuilder))
            {
                action?.Invoke(transformerBuilder.Base);
                transformerBuilder.Base.NeedDestructure = false;
            }
        }

        private void UpdateProperties(Func<PropertyInfo, bool> propertyFunc, Action<PropertyTransformer> action)
        {
            foreach (var property in GetPropertyInfos(propertyFunc))
            {
                if (_propertyTransformers.TryGetValue(property, out var transformerBuilder))
                {
                    action?.Invoke(transformerBuilder.Base);
                    transformerBuilder.Base.NeedDestructure = false;
                }
            }
        }
    }
}
