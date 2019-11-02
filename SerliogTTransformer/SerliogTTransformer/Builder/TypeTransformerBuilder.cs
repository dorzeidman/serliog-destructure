using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using SerliogTTransformer.Property;
using SerliogTTransformer.Transformer;

namespace SerliogTTransformer.Builder
{
    public class TypeTransformerBuilder<T> : ITypeTransformerBuilder<T>
        where T : class
    {
        private readonly Dictionary<string, IPropertyValueConverter> _convProperties;
        private readonly HashSet<string> _ignoreProperties;
        private readonly Dictionary<string, Func<object, object, bool>> _ignoreFuncProperties;
        private readonly Dictionary<string, string> _propertyNames;
        private bool _ignoreAllNulls;

        public TypeTransformerBuilder()
        {
            _convProperties = new Dictionary<string, IPropertyValueConverter>();
            _ignoreProperties = new HashSet<string>();
            _ignoreFuncProperties = new Dictionary<string, Func<object, object, bool>>();
            _propertyNames = new Dictionary<string, string>();
        }

        public ITypeTransformerBuilder<T> IgnoreAllIfNull()
        {
            _ignoreAllNulls = true;
            return this;
        }

        public ITypeTransformerBuilder<T> Ignore(Expression<Func<T, object>> expression)
        {
            var propertyInfo = GetPropertyInfo(expression);
            if (!_ignoreProperties.Contains(propertyInfo.Name))
                _ignoreProperties.Add(propertyInfo.Name);
            return this;
        }

        public ITypeTransformerBuilder<T> Ignore(string propertyName)
        {
            var propertyInfo = GetPropertyInfo(propertyName); 
            if (!_ignoreProperties.Contains(propertyInfo.Name))
                _ignoreProperties.Add(propertyInfo.Name);
            return this;
        }

        public ITypeTransformerBuilder<T> IgnoreIfNull(Expression<Func<T, object>> expression)
        {
            var propertyInfo = GetPropertyInfo(expression);
            _ignoreFuncProperties[propertyInfo.Name] = (o, p) => p == null;

            return this;
        }

        public ITypeTransformerBuilder<T> IgnoreIfNull(string propertyName)
        {
            var propertyInfo = GetPropertyInfo(propertyName);
            _ignoreFuncProperties[propertyInfo.Name] = (o, p) => p == null;

            return this;
        }

        public ITypeTransformerBuilder<T> IgnoreIf(Expression<Func<T, object>> expression, Func<T, bool> func)
        {
            var propertyInfo = GetPropertyInfo(expression);
            _ignoreFuncProperties[propertyInfo.Name] = (o,p) => func((T)o);

            return this;
        }

        public ITypeTransformerBuilder<T> IgnoreIf(string propertyName, Func<T, bool> func)
        {
            var propertyInfo = GetPropertyInfo(propertyName);
            _ignoreFuncProperties[propertyInfo.Name] = (o, p) => func((T)o);

            return this;
        }

        public ITypeTransformerBuilder<T> Mask(Expression<Func<T, object>> expression, char mask = '*')
        {
            var propertyInfo = GetPropertyInfo(expression);
            _convProperties[propertyInfo.Name] = new SimpleMaskValueConverter(mask);
            return this;
        }

        public ITypeTransformerBuilder<T> Mask(string propertyName, char mask = '*')
        {
            var propertyInfo = GetPropertyInfo(propertyName);
            _convProperties[propertyInfo.Name] = new SimpleMaskValueConverter(mask);
            return this;
        }

        public ITypeTransformerBuilder<T> Mask(Expression<Func<T, object>> expression, int showFirst, int showLast
            , char mask = '*')
        {
            var propertyInfo = GetPropertyInfo(expression);
            _convProperties[propertyInfo.Name] = new ComplexMaskValueConverter(mask, showFirst, showLast);
            return this;
        }

        public ITypeTransformerBuilder<T> Mask(string propertyName, int showFirst, int showLast, char mask = '*')
        {
            var propertyInfo = GetPropertyInfo(propertyName);
            _convProperties[propertyInfo.Name] = new ComplexMaskValueConverter(mask, showFirst, showLast);
            return this;
        }

        public ITypeTransformerBuilder<T> Rename(Expression<Func<T, object>> expression, string newName)
        {
            var propertyInfo = GetPropertyInfo(expression);
            _propertyNames[propertyInfo.Name] = newName;

            return this;
        }

        public ITypeTransformerBuilder<T> Rename(string propertyName, string newName)
        {
            var propertyInfo = GetPropertyInfo(propertyName);
            _propertyNames[propertyInfo.Name] = newName;

            return this;
        }

        public ITypeTransformerBuilder<T> Convert(Expression<Func<T, object>> expression, IPropertyValueConverter converter)
        {
            var propertyInfo = GetPropertyInfo(expression);
            _convProperties[propertyInfo.Name] = converter;
            return this;
        }

        public ITypeTransformerBuilder<T> Convert(string propertyName, IPropertyValueConverter converter)
        {
            var propertyInfo = GetPropertyInfo(propertyName);
            _convProperties[propertyInfo.Name] = converter;
            return this;
        }

        public TypeTransformer Build()
        {
            var writeProperties = PropertyFinder.GetPropertiesRecursive(typeof(T))
                .Where(x => !_ignoreProperties.Contains(x.Name))
                .ToArray();

            return new TypeTransformer(typeof(T), writeProperties, 
                _convProperties, _ignoreFuncProperties, _propertyNames, _ignoreAllNulls);
        }

        private PropertyInfo GetPropertyInfo(Expression<Func<T, object>> expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            var property = PropertyFinder.FromExpression(expression);
            return property ?? throw new ArgumentException("Invalid Property expression", nameof(expression));
        }
        
        private PropertyInfo GetPropertyInfo(string propertyName)
        {
            if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));
            var property = typeof(T).GetProperty(propertyName);
            return property ?? throw new ArgumentException($"Invalid PropertyName:{propertyName}", nameof(property));
        }

        
    }
}
