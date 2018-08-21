namespace Vostok.Tracing.Abstractions
{
    public interface ITraceReporter
    {
        SpanSendResult SendSpan(ISpan span);
    }
}