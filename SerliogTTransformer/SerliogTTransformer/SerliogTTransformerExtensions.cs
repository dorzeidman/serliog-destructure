using System;
using Serilog;
using Serilog.Configuration;
using SerliogTTransformer.Builder;
using SerliogTTransformer.Policy;

namespace SerliogTTransformer
{
    public static class SerliogTTransformerExtensions
    {
        /// <summary>
        /// Adds the basic TTransformer destructor. should be called once.
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static LoggerConfiguration AddTTransformer(this LoggerConfiguration configuration)
        {
            return configuration.Destructure.With<TypeTransformerDestructuringPolicy>();
        }

        public static LoggerConfiguration Transform<T>(this LoggerDestructuringConfiguration configuration,
            Action<ITypeTransformerBuilder<T>> buildAction)
            where T : class
        {
            var builder = new TypeTransformerBuilder<T>();
            buildAction.Invoke(builder);

            var typeDest = builder.Build();

            return configuration.ByTransforming<T>(v => typeDest.Transform(v));
        }

        public static LoggerConfiguration Transform<T>(this LoggerDestructuringConfiguration configuration,
            ITypeTransformerBinder<T> binder)
            where T : class
        {
            var builder = new TypeTransformerBuilder<T>();
            binder.Bind(builder);

            var typeDest = builder.Build();

            return configuration.ByTransforming<T>(v => typeDest.Transform(v));
        }
    }
}
