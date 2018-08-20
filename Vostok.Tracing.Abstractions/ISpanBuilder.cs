using System;

namespace Vostok.Tracing.Abstractions
{
    public interface ISpanBuilder : IDisposable
    {
        bool IsEndless { get; set; }

        void SetAnnotation<TValue>(string key, TValue value, bool copyToChild = false);
        void SetBeginTimestamp(DateTimeOffset timestamp);
        void SetEndTimestamp(DateTimeOffset timestamp);
    }
}