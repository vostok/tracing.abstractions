using JetBrains.Annotations;

namespace Vostok.Tracing.Abstractions
{
    /// <summary>
    /// A trivial <see cref="ISpanSender"/> implementation that simply drops all incoming spans.
    /// </summary>
    [PublicAPI]
    public class DevNullSpanSender : ISpanSender
    {
        public void Send(ISpan span)
        {
        }
    }
}