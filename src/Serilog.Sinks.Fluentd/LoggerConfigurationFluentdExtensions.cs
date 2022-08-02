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
        private const int QueueLimit = 100000;
        private static readonly TimeSpan DefaultPeriod = TimeSpan.FromSeconds(2.0);
        
        
        private static readonly PeriodicBatchingSinkOptions DefaultBatchingOptions = new PeriodicBatchingSinkOptions
        {
            BatchSizeLimit = 50,
            QueueLimit = QueueLimit,
            Period = DefaultPeriod
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
           int port = Port,
           string tag = Tag,
           int queueLimit = QueueLimit,
           TimeSpan? batchPeriod = null,
           LogEventLevel restrictedToMinimumLevel = LogEventLevel.Information)
        {
            var sink = new FluentdSink(new FluentdSinkOptions(host, port, tag));

            DefaultBatchingOptions.Period = batchPeriod ?? DefaultPeriod;
            DefaultBatchingOptions.QueueLimit = queueLimit;

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
