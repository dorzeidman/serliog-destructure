using System;
using System.Linq.Expressions;
using System.Reflection;
using SerliogTTransformer.Converter;
using SerliogTTransformer.Property;

namespace SerliogTTransformer.Builder
{
    public interface ITypeTransformerBuilder<T> where T: class
    {
        ITypeTransformerBuilder<T> IgnoreAllIfNull();


        ITypeTransformerBuilder<T> Ignore(Expression<Func<T, object>> expression);
        ITypeTransformerBuilder<T> Ignore(string propertyName);
        ITypeTransformerBuilder<T> Ignore(Func<PropertyInfo, bool> propertyFunc);

        ITypeTransformerBuilder<T> IgnoreIfNull(Expression<Func<T, object>> expression);
        ITypeTransformerBuilder<T> IgnoreIfNull(string propertyName);
        ITypeTransformerBuilder<T> IgnoreIfNull(Func<PropertyInfo, bool> propertyFunc);

        ITypeTransformerBuilder<T> IgnoreIf(Expression<Func<T, object>> expression, Func<T,object,bool> func);
        ITypeTransformerBuilder<T> IgnoreIf(string propertyName, Func<T,object,bool> func);
        ITypeTransformerBuilder<T> IgnoreIf(Func<PropertyInfo, bool> propertyFunc, Func<T, object, bool> func);

        ITypeTransformerBuilder<T> Mask(Expression<Func<T, object>> expression, char mask = '*');
        ITypeTransformerBuilder<T> Mask(string propertyName, char mask = '*');
        ITypeTransformerBuilder<T> Mask(Func<PropertyInfo, bool> propertyFunc, char mask = '*');


        ITypeTransformerBuilder<T> Mask(Expression<Func<T, object>> expression, int showFirst,
            int showLast, char mask = '*');
        ITypeTransformerBuilder<T> Mask(string propertyName, int showFirst, int showLast, char mask = '*');
        ITypeTransformerBuilder<T> Mask(Func<PropertyInfo, bool> propertyFunc, int showFirst, int showLast, char mask = '*');

        ITypeTransformerBuilder<T> Convert(Expression<Func<T, object>> expression, IPropertyValueConverter converter);
        ITypeTransformerBuilder<T> Convert(string propertyName, IPropertyValueConverter converter);
        ITypeTransformerBuilder<T> Convert(Func<PropertyInfo, bool> propertyFunc, IPropertyValueConverter converter);


        ITypeTransformerBuilder<T> Transform(Expression<Func<T, object>> expression, IPropertyTransformer transformer);
        ITypeTransformerBuilder<T> Transform(string propertyName, IPropertyTransformer transformer);
        ITypeTransformerBuilder<T> Transform(Func<PropertyInfo, bool> propertyFunc, IPropertyTransformer transformer);
    }
}
