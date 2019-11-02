using System.Collections.Generic;
using Serilog.Core;
using Serilog.Events;

namespace SerliogTTransformer.Tests.Fixtures
{
    class SinkFixture : ILogEventSink
    {
        public static List<LogEvent> LogEvents { get; } = new List<LogEvent>();

        public void Emit(LogEvent logEvent)
        {
            LogEvents.Add(logEvent);
        }
    }
}
