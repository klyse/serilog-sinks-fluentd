using System.Collections.Generic;
using System.Threading.Tasks;
using Serilog.Events;
using Serilog.Sinks.PeriodicBatching;

namespace Serilog.Sinks.Fluentd
{
    public class FluentdSink : IBatchedLogEventSink
    {
        private readonly FluentdSinkClient _fluentdClient;

        public FluentdSink(FluentdSinkOptions options)
        {
            _fluentdClient = new FluentdSinkClient(options);
        }

        public Task OnEmptyBatchAsync()
        {
            return Task.CompletedTask;
        }

        public async Task EmitBatchAsync(IEnumerable<LogEvent> batch)
        {
            foreach (var logEvent in batch)
            {
                await _fluentdClient.SendAsync(logEvent);
            }
        }
    }
}