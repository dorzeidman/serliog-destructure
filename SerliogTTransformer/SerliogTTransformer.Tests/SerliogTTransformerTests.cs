using System.Diagnostics;
using System.Linq;
using Serilog;
using Serilog.Events;
using SerliogTTransformer.Tests.Fixtures;
using Xunit;

namespace SerliogTTransformer.Tests
{
    public class SerliogTTransformerTests
    {
        [Fact]
        public void MainTest()
        {
            var logger = new LoggerConfiguration()
                .AddTTransformer()
                .Destructure.Transform<FixtureClass1>(b => b.Ignore(i => i.String2)
                    .Mask(p => p.String1)
                    .IgnoreIfNull(x => x.Num2)
                    .Rename(x => x.Num, "NumNew"))
                .Destructure.Transform<FixtureClass2>(b => b
                    .Ignore(x => x.Double)
                    .Mask(p => p.Phone, 3, 3)
                    .IgnoreIf(nameof(FixtureClass2.Test1), (y,p) => !y.Test1))
                .Destructure.AsScalar<FixtureClass3>()
                .WriteTo.Sink<SinkFixture>()
                .CreateLogger();

            var fixClass = new FixtureClass1
            {
                Num = 1,
                String1 = "123",
                String2 = "456",
                Class2 = new FixtureClass2
                {
                    Double = 1,
                    Num2 = 34,
                    Test1 = false,
                    Phone = "05032412333"
                },
                Class3 = new FixtureClass3()
            };

            logger.Information("Test Message: {@FixClass}", fixClass);

            var logEvent = SinkFixture.LogEvents.First();
            var propValue = (StructureValue)logEvent.Properties.Values.First();

            //FixtureClass1
            Assert.Equal(typeof(FixtureClass1).Name, propValue.TypeTag);
            Assert.DoesNotContain(propValue.Properties, p => p.Name == nameof(FixtureClass1.String2));
            Assert.Contains(propValue.Properties, p => p.Name == nameof(FixtureClass1.String1));
            Assert.Equal(new ScalarValue("***"), propValue.Properties
                .First(x => x.Name == nameof(FixtureClass1.String1)).Value);
            Assert.Contains(propValue.Properties, p => p.Name == "NumNew");
            Assert.DoesNotContain(propValue.Properties, p => p.Name == nameof(FixtureClass1.Num2));

            //FixtureClass2
            var class2PropValue = (StructureValue) propValue.Properties.First(x => x.Name == "Class2").Value;
            Assert.DoesNotContain(class2PropValue.Properties, p => p.Name == "Double2");
            Assert.DoesNotContain(class2PropValue.Properties, p => p.Name == nameof(FixtureClass2.Double));
            Assert.Contains(class2PropValue.Properties, p => p.Name == nameof(FixtureClass2.Num2));
            Assert.DoesNotContain(class2PropValue.Properties, p => p.Name == nameof(FixtureClass2.Test1));
            Assert.Equal(new ScalarValue("050*****333"), class2PropValue.Properties
                .First(x => x.Name == nameof(FixtureClass2.Phone)).Value);

            //FixtureClass3
            Assert.Equal(typeof(FixtureClass3).FullName, 
                propValue.Properties.First(x => x.Name == nameof(FixtureClass1.Class3)).Value.ToString());

        }

        [Fact]
        public void LoadTest()
        {
            var simpleLogger = new LoggerConfiguration()
                .WriteTo.Sink<SinkFixture>()
                .CreateLogger();

            var transformLogger = new LoggerConfiguration()
                .AddTTransformer()
                .Destructure.Transform<FixtureClass1>(b => b.Ignore(i => i.String2)
                    .Mask(p => p.String1)
                    .IgnoreIfNull(x => x.Num2))
                .Destructure.Transform<FixtureClass2>(b => b
                    .Ignore(x => x.Double)
                    .Mask(p => p.Phone, 3, 3)
                    .IgnoreIf(nameof(FixtureClass2.Test1), (y,p) => !y.Test1))
                .WriteTo.Sink<SinkFixture>()
                .CreateLogger();

            var fixClass = new FixtureClass1
            {
                Num = 1,
                String1 = "123",
                String2 = "456",
                Class2 = new FixtureClass2
                {
                    Double = 1,
                    Num2 = 34,
                    Test1 = false,
                    Phone = "05032412333"
                },
                Class3 = new FixtureClass3()
            };

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < 100; i++)
            {
                simpleLogger.Information("Test1:{@FixClass1}", fixClass);
            }

            stopwatch.Stop();
            var elapsedTime1 = stopwatch.Elapsed;

            stopwatch.Reset();
            stopwatch.Start();
            for (int i = 0; i < 100; i++)
            {
                transformLogger.Information("Test1:{@FixClass1}", fixClass);
            }

            stopwatch.Stop();
            var elapsedTime2 = stopwatch.Elapsed;


            Assert.True(elapsedTime1 >= elapsedTime2);
        }
    }
}
