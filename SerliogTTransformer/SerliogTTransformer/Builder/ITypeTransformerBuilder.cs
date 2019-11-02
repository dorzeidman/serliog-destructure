using System;
using System.Linq.Expressions;
using SerliogTTransformer.Property;

namespace SerliogTTransformer.Builder
{
    public interface ITypeTransformerBuilder<T> where T: class
    {
        ITypeTransformerBuilder<T> IgnoreAllIfNull();


        ITypeTransformerBuilder<T> Ignore(Expression<Func<T, object>> expression);
        ITypeTransformerBuilder<T> Ignore(string propertyName);
        
        ITypeTransformerBuilder<T> IgnoreIfNull(Expression<Func<T, object>> expression);
        ITypeTransformerBuilder<T> IgnoreIfNull(string propertyName);

        ITypeTransformerBuilder<T> IgnoreIf(Expression<Func<T, object>> expression, Func<T,bool> func);
        ITypeTransformerBuilder<T> IgnoreIf(string propertyName, Func<T, bool> func);

        ITypeTransformerBuilder<T> Mask(Expression<Func<T, object>> expression, char mask = '*');

        ITypeTransformerBuilder<T> Mask(string propertyName, char mask = '*');

        ITypeTransformerBuilder<T> Mask(Expression<Func<T, object>> expression, int showFirst,
            int showLast, char mask = '*');

        ITypeTransformerBuilder<T> Mask(string propertyName, int showFirst, int showLast, char mask = '*');

        ITypeTransformerBuilder<T> Rename(Expression<Func<T, object>> expression, string newName);

        ITypeTransformerBuilder<T> Rename(string propertyName, string newName);

        ITypeTransformerBuilder<T> Convert(Expression<Func<T, object>> expression, IPropertyValueConverter converter);
        ITypeTransformerBuilder<T> Convert(string propertyName, IPropertyValueConverter converter);
    }
}
