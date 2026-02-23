using AutoMapper;
using System.Reflection;

namespace STEMotion.Application.Extensions
{
    public static class AutoMapperExtension
    {
        // Skip fields that do not exist in the source type
        public static IMappingExpression<TSource, TDestination> IgnoreAllNonExisting<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression)
        {
            var flags = BindingFlags.Public | BindingFlags.Instance;
            var sourceType = typeof(TSource);
            var destinationType = typeof(TDestination).GetProperties(flags);

            foreach (var property in destinationType)
            {
                if (sourceType.GetProperty(property.Name, flags) == null)
                {
                    expression.ForMember(property.Name, opt => opt.Ignore());
                }
            }
            return expression;
        }
    }
}
