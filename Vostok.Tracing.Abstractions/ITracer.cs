using JetBrains.Annotations;

namespace Vostok.Tracing.Abstractions
{
    [PublicAPI]
    public interface ITracer
    {
        /// <summary>
        /// <para>Gets or sets context used for <see cref="BeginSpan"/> calls. A <c>null</c> value means no context is currently defined.</para>
        /// <para>For most uses cases, it's sufficient to just let <see cref="BeginSpan"/> manage this context.</para>
        /// <para>However, one may need to manually set it in order to restore a serialized context (continue a trace initiated on another machine).</para>
        /// <para>Note that this context is ambient: it's generally not tied to a specific <see cref="ITracer"/> instance.</para>
        /// </summary>
        [CanBeNull] TraceContext CurrentContext { get; set; }

        /// <summary>
        /// <para>Begins construction of a new span and returns a builder responsible for it (see <see cref="ISpanBuilder"/> for details).</para>
        /// <para>This call also manipulates <see cref="CurrentContext"/> and defines core identifiers of the created span.</para>
        /// <para>If <see cref="CurrentContext"/> is not defined (has a <c>null</c> value):</para>
        /// <list type="number">
        ///     <item><description><see cref="CurrentContext"/> is initialized with new random identifiers (a new trace is started). <br/></description></item>
        ///     <item><description>Constructed span <see cref="ISpan.TraceId"/> and <see cref="ISpan.SpanId"/> are taken from <see cref="CurrentContext"/>. <br/><br/></description></item>
        ///     <item><description>Constructed span <see cref="ISpan.ParentSpanId"/> is set to <c>null</c>: this span will be a root in this trace tree. <br/><br/></description></item>
        /// </list>
        /// <para>If <see cref="CurrentContext"/> is defined (has a non-<c>null</c> value):</para>
        /// <list type="number">
        ///     <item><description>Constructed span <see cref="ISpan.ParentSpanId"/> is set to <see cref="TraceContext.SpanId"/> from <see cref="CurrentContext"/>. <br/><br/></description></item>
        ///     <item><description><see cref="CurrentContext"/> is updated with same <see cref="TraceContext.TraceId"/> and a new random <see cref="TraceContext.SpanId"/> (existing trace is continued).<br/><br/></description></item>
        ///     <item><description>Constructed span <see cref="ISpan.TraceId"/> and <see cref="ISpan.SpanId"/> are taken from <see cref="CurrentContext"/>. <br/><br/></description></item>
        /// </list>
        /// </summary>
        [NotNull] ISpanBuilder BeginSpan();
    }
}