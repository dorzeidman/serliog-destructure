using System.Collections.Generic;
using Serilog.Core;
using Serilog.Events;
using SerliogTTransformer.Transformer;

namespace SerliogTTransformer.Policy
{
    public class TypeTransformerDestructuringPolicy : IDestructuringPolicy
    {
        public bool TryDestructure(object value, ILogEventPropertyValueFactory propertyValueFactory, out LogEventPropertyValue result)
        {
            if (value is TransformedObject destObj)
            {
                result = Destruct(destObj, propertyValueFactory);
                return true;
            }

            result = null;
            return false;
        }

        private LogEventPropertyValue Destruct(TransformedObject destObj, ILogEventPropertyValueFactory propertyValueFactory)
        {
            var logEvents = new List<LogEventProperty>(destObj.Properties.Count);

            foreach (var property in destObj.Properties)
            {
                logEvents.Add(GetLogProperty(property, propertyValueFactory));
            }

            var result = new StructureValue(logEvents, destObj.TypeTag);
            return result;
        }

        private LogEventProperty GetLogProperty(DestructedProperty destProperty,
            ILogEventPropertyValueFactory propertyValueFactory)
        {
            var value = propertyValueFactory.CreatePropertyValue(destProperty.Value, destProperty.NeedsDestruct);
            return new LogEventProperty(destProperty.Name, value);
        }
    }
}
