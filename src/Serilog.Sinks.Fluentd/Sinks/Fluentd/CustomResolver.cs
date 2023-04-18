using System;
using System.Collections.Generic;
using MessagePack;
using MessagePack.Formatters;

namespace Serilog.Sinks.Fluentd
{
    public class CustomResolver : IFormatterResolver
    {
        // Resolver should be singleton.
        public static readonly IFormatterResolver Instance = new CustomResolver();

        private CustomResolver()
        {
        }

        // GetFormatter<T>'s get cost should be minimized so use type cache.
        public IMessagePackFormatter<T> GetFormatter<T>()
        {
            return FormatterCache<T>.Formatter;
        }

        private static class FormatterCache<T>
        {
            public static readonly IMessagePackFormatter<T> Formatter;

            // generic's static constructor should be minimized for reduce type generation size!
            // use outer helper method.
            static FormatterCache()
            {
                Formatter = (IMessagePackFormatter<T>)GetFormatter(typeof(T));
            }
        }

        private static readonly Dictionary<Type, object> FormatterMap = new Dictionary<Type, object>()
        {
            { typeof(FluentdMessage.EventTime), new FileInfoFormatter() }
            // add more your own custom serializers.
        };

        private static object GetFormatter(Type t)
        {
            if (FormatterMap.TryGetValue(t, out var formatter))
            {
                return formatter;
            }

            // If type can not get, must return null for fallback mechanism.
            return null;
        }
    }
}