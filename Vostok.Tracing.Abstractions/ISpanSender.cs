using JetBrains.Annotations;

namespace Vostok.Tracing.Abstractions
{
    /// <summary>
    /// <see cref="ISpanSender"/>'s purpose is to send spans built by <see cref="ISpanBuilder"/> to external storage for further processing.
    /// </summary>
    [PublicAPI]
    public interface ISpanSender
    {
        /// <summary>
        /// <para>Schedules given <paramref name="span"/> to be sent to an external storage in a fire-and-forget manner.</para>
        /// <para>Error handling, retrying and buffering are delegated to implementations.</para>
        /// </summary>
        void Send([NotNull] ISpan span);
    }
}