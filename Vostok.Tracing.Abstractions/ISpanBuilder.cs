using System;

namespace Vostok.Tracing.Abstractions
{
    public interface ISpanBuilder : IDisposable
    {
        bool IsEndless { get; set; }

        void SetAnnotation(string key, string value, bool allowOverwrite = true);
        void SetBeginTimestamp(DateTimeOffset timestamp);
        void SetEndTimestamp(DateTimeOffset timestamp);
    }
}