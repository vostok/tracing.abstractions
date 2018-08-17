namespace Vostok.Tracing.Abstractions
{
    public interface ITraceReporter
    {
        void SendSpan(ISpan span);
    }
}