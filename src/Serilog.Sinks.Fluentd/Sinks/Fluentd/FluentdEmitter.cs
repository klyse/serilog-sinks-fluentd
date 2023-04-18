using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MessagePack;
using MessagePack.Resolvers;

namespace Serilog.Sinks.Fluentd
{
    internal class FluentdEmitter
    {
        private readonly Stream _output;

        public FluentdEmitter(Stream stream)
        {
            _output = stream;

            var resolver = CompositeResolver.Create(
                // enable extension packages first
                CustomResolver.Instance,
                // finally use standard (default) resolver
                StandardResolver.Instance
            );

            _options = MessagePackSerializerOptions
                .Standard
                .WithResolver(resolver);
        }

        private readonly MessagePackSerializerOptions _options;

        public async Task EmitAsync(DateTimeOffset timestamp, string tag, IDictionary<string, object> data)
        {
            await MessagePackSerializer.SerializeAsync(
                _output,
                new FluentdMessage
                {
                    Tag = tag,
                    Time = new FluentdMessage.EventTime
                    {
                        Nanoseconds = timestamp.Millisecond * 1_000_000,
                        Seconds = (int)timestamp.ToUnixTimeSeconds()
                    },
                    Record = data,
                }, _options);
        }
    }
}