namespace Vostok.Tracing.Abstractions
{
    public interface ITracer
    {
        TraceContext CurrentContext { get; set; }

        ISpanBuilder BeginSpan();
    }
}