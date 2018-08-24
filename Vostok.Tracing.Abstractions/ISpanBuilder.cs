using System;
using JetBrains.Annotations;

namespace Vostok.Tracing.Abstractions
{
    [PublicAPI]
    public interface ISpanBuilder : IDisposable
    {
        bool IsEndless { get; set; }

        void SetAnnotation([NotNull]string key, [CanBeNull]string value, bool allowOverwrite = true);
        void SetBeginTimestamp(DateTimeOffset timestamp);
        void SetEndTimestamp(DateTimeOffset timestamp);
    }
}