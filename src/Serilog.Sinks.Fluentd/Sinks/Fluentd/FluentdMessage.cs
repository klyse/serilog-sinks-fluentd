using System.Collections.Generic;
using MessagePack;

namespace Serilog.Sinks.Fluentd
{
    [MessagePackObject]
    public class FluentdMessage
    {
        [MessagePackObject]
        public class EventTime
        {
            [Key(0)] public int Seconds { get; set; }
            [Key(1)] public int Nanoseconds { get; set; }
        }

        [Key(0)] public string Tag { get; set; }

        [Key(1)] public EventTime Time { get; set; }

        [Key(2)] public IDictionary<string, object> Record { get; set; }
    }
}