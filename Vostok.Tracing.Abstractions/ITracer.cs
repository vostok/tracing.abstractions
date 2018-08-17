namespace Vostok.Tracing.Abstractions
{
    public interface ITracer
    {
        ITraceContext CurrentContext { get; set; }

        ISpanBuilder BeginSpan();
    }
}