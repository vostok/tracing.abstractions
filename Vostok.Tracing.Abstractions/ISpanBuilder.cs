using System;
using JetBrains.Annotations;

namespace Vostok.Tracing.Abstractions
{
    /// <summary>
    /// <para><see cref="ISpanBuilder"/> purpose is to facilitate easy construction of <see cref="ISpan"/>s in applications.</para>
    /// <para>It's lifetime (from instantiation to <see cref="ISpanBuilder.Dispose"/>) is closely tied to the constructed span properties:</para>
    /// <list type="bullet">
    ///     <item><description>Instantiation moment is a default <see cref="ISpan.BeginTimestamp"/> for constructed span. <br/><br/></description></item>
    ///     <item><description>Disposal moment is a default <see cref="ISpan.EndTimestamp"/> for constructed span. <br/><br/></description></item>
    ///     <item><description><see cref="ISpanBuilder.Dispose"/> makes constructed span immutable (subsequent <see cref="SetAnnotation"/> calls have no effect). <br/><br/></description></item>
    ///     <item><description><see cref="ISpanBuilder.Dispose"/> also initiates an operation to send the span to an external storage.<br/><br/></description></item>
    /// </list>
    /// <para>Typical usage of <see cref="ISpanBuilder"/> boils down to this:</para>
    /// <list type="number">
    ///     <item><description>Obtain <see cref="ISpanBuilder"/> instance (typically with a <see cref="ITracer.BeginSpan"/> call). <br/><br/></description></item>
    ///     <item><description>Execute traced operation and fill the span with <see cref="SetAnnotation"/> calls on the way. <br/><br/></description></item>
    ///     <item><description><see cref="ISpanBuilder.Dispose"/> of the builder. <br/><br/></description></item>
    /// </list>
    /// <code>
    ///     using (var builder = tracer.BeginSpan())
    ///     {
    ///         builder.SetAnnotation(..);
    ///         .. execute something
    ///         builder.SetAnnotation(..);
    ///     }
    /// </code><br/>
    /// <para><see cref="SetBeginTimestamp"/> and <see cref="SetEndTimestamp"/> are optional and should only be used to override default timestamps.</para>
    /// <para>Implementations of this interface are generally <b>not expected to be thread-safe!</b> Don't use an <see cref="ISpanBuilder"/> instance concurrently from multiple threads.</para>
    /// </summary>
    [PublicAPI]
    public interface ISpanBuilder : IDisposable
    {
        /// <summary>
        /// Returns current state of the constructed span.
        /// </summary>
        ISpan CurrentSpan { get; }

        /// <summary>
        /// <para>Set the annotation with given <paramref name="key"/> and <paramref name="value"/> in constructed span's <see cref="ISpan.Annotations"/>.</para>
        /// <para>By default, existing annotations are overwritten with provided values. This behaviour can be changed with <paramref name="allowOverwrite"/> parameter.</para>
        /// <para>Has no effect when called after <see cref="ISpanBuilder.Dispose"/>.</para>
        /// </summary>
        void SetAnnotation([NotNull] string key, [CanBeNull] object value, bool allowOverwrite = true);

        /// <summary>
        /// <para>Overrides constructed span's <see cref="ISpan.BeginTimestamp"/>.</para>
        /// <para>Only use this if default value (<see cref="ISpanBuilder"/> creation moment) is not sufficient for your needs.</para>
        /// <para>Has no effect when called after <see cref="ISpanBuilder.Dispose"/>.</para>
        /// </summary>
        void SetBeginTimestamp(DateTimeOffset timestamp);

        /// <summary>
        /// <para>Overrides constructed span's <see cref="ISpan.EndTimestamp"/>.</para>
        /// <para>Only use this if default value (<see cref="ISpanBuilder.Dispose"/> call moment) is not sufficient for your needs.</para>
        /// <para>If called with a <c>null</c> argument, resulting span will not have <see cref="ISpan.EndTimestamp"/>.</para>
        /// <para>Has no effect when called after <see cref="ISpanBuilder.Dispose"/>.</para>
        /// </summary>
        void SetEndTimestamp(DateTimeOffset? timestamp);
    }
}