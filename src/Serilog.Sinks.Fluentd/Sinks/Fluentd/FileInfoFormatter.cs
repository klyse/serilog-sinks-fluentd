using System;
using MessagePack;
using MessagePack.Formatters;

namespace Serilog.Sinks.Fluentd
{
    public class FileInfoFormatter : IMessagePackFormatter<FluentdMessage.EventTime>
    {
        public void Serialize(
            ref MessagePackWriter writer, FluentdMessage.EventTime value, MessagePackSerializerOptions options)
        {
            /*
                https://github.com/fluent/fluentd/wiki/Forward-Protocol-Specification-v1#eventtime-ext-format
                +-------+----+----+----+----+----+----+----+----+----+
                |     1 |  2 |  3 |  4 |  5 |  6 |  7 |  8 |  9 | 10 |
                +-------+----+----+----+----+----+----+----+----+----+
                |    D7 | 00 | second from epoch |     nanosecond    |
                +-------+----+----+----+----+----+----+----+----+----+
                |fixext8|type| 32bits integer BE | 32bits integer BE |
                +-------+----+----+----+----+----+----+----+----+----+
                 */

            var span = writer.GetSpan(10);
            span[0] = MessagePackCode.FixExt8;
            span[1] = 0;

            span[2] = (byte)(value.Seconds >> 24);
            span[3] = (byte)(value.Seconds >> 16);
            span[4] = (byte)(value.Seconds >> 8);
            span[5] = (byte)value.Seconds;
            span[6] = (byte)(value.Nanoseconds >> 24);
            span[7] = (byte)(value.Nanoseconds >> 16);
            span[8] = (byte)(value.Nanoseconds >> 8);
            span[9] = (byte)value.Nanoseconds;

            writer.Advance(10);
        }

        public FluentdMessage.EventTime Deserialize(
            ref MessagePackReader reader, MessagePackSerializerOptions options)
        {
            throw new Exception("only serialization supported");
        }
    }
}