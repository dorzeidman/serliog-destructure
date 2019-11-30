using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SerliogTTransformer.Property
{
    internal static class PropertyFinder
    {
        internal static IEnumerable<PropertyInfo> GetAll(Type type)
        {
            var seenNames = new HashSet<string>();

            var currentTypeInfo = type.GetTypeInfo();

            while (currentTypeInfo.AsType() != typeof(object))
            {
                var unseenProperties = currentTypeInfo.DeclaredProperties.Where(p => p.CanRead &&
                                         p.GetMethod.IsPublic &&
                                         !p.GetMethod.IsStatic &&
                                         (p.Name != "Item" || p.GetIndexParameters().Length == 0) &&
                                         !seenNames.Contains(p.Name));

                foreach (var propertyInfo in unseenProperties)
                {
                    seenNames.Add(propertyInfo.Name);
                    yield return propertyInfo;
                }

                currentTypeInfo = currentTypeInfo.BaseType.GetTypeInfo();
            }
        }
        internal static PropertyInfo FromExpression<T>(Expression<Func<T, object>> expression)
        {
            MemberExpression memberExp;

            if (expression.Body is UnaryExpression unExp)
            {
                if (unExp.Operand is MemberExpression operand)
                {
                    memberExp = operand;
                }
                else
                    throw new ArgumentException("Can't get property info from expression", nameof(expression));
            }
            else if (expression.Body is MemberExpression memberExpression)
            {
                memberExp = memberExpression;
            }
            else
            {
                throw new ArgumentException("Can't get property info from expression", nameof(expression));
            }

            return (PropertyInfo)memberExp.Member;
        }
    }
}
