using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MessagePack;

namespace Serilog.Sinks.Fluentd
{
    internal class FluentdEmitter
    {
        private readonly Stream _output;

        public FluentdEmitter(Stream stream)
        {
            this._output = stream;
        }

        public async Task EmitAsync(DateTimeOffset timestamp, string tag, IDictionary<string, object> data)
        {
            await MessagePackSerializer.SerializeAsync(
                this._output,
                new FluentdMessage
                {
                    Tag = tag,
                    Timestamp = timestamp.ToUnixTimeMilliseconds() / 1000d,
                    Data = data,
                });
        }
    }
}