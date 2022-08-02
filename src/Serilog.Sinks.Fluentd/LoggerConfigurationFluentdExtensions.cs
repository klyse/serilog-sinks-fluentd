using System;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Sinks.Fluentd;
using Serilog.Sinks.PeriodicBatching;

namespace Serilog
{
    public static class LoggerConfigurationFluentdExtensions
    {
        private const string Host = "localhost";
        private const int Port = 24224;
        private const string Tag = "Tag";

        private static readonly PeriodicBatchingSinkOptions DefaultBatchingOptions = new PeriodicBatchingSinkOptions
        {
            BatchSizeLimit = 50
        };
        
        public static LoggerConfiguration Fluentd(
            this LoggerSinkConfiguration loggerSinkConfiguration,
            FluentdSinkOptions option = null,
            PeriodicBatchingSinkOptions batchingOptions = null,
            LogEventLevel restrictedToMinimumLevel = LogEventLevel.Information)
        {
            var sink = new FluentdSink(option ?? new FluentdSinkOptions(Host, Port, Tag));
            batchingOptions = batchingOptions ?? DefaultBatchingOptions;

            var batchingSink = new PeriodicBatchingSink(sink, batchingOptions);
            return loggerSinkConfiguration.Sink(batchingSink, restrictedToMinimumLevel);
        }

        public static LoggerConfiguration Fluentd(
           this LoggerSinkConfiguration loggerSinkConfiguration,
           string host,
           int port,
           string tag = Tag,
           LogEventLevel restrictedToMinimumLevel = LogEventLevel.Information)
        {
            var sink = new FluentdSink(new FluentdSinkOptions(host, port, tag));

            var batchingSink = new PeriodicBatchingSink(sink, DefaultBatchingOptions);
            return loggerSinkConfiguration.Sink(batchingSink, restrictedToMinimumLevel);
        }

        public static LoggerConfiguration Fluentd(
           this LoggerSinkConfiguration loggerSinkConfiguration,
           string udsSocketFilePath,
           LogEventLevel restrictedToMinimumLevel = LogEventLevel.Information)
        {
            var sink = new FluentdSink(new FluentdSinkOptions(udsSocketFilePath));

            var batchingSink = new PeriodicBatchingSink(sink, DefaultBatchingOptions);
            return loggerSinkConfiguration.Sink(batchingSink, restrictedToMinimumLevel);
        }
    }
}
