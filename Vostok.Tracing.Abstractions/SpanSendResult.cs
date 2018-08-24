﻿using JetBrains.Annotations;

namespace Vostok.Tracing.Abstractions
{
    [PublicAPI]
    public enum SpanSendResult
    {
        Sent,
        Queued,
        Failed
    }
}