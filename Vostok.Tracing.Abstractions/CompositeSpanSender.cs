using System;
using JetBrains.Annotations;

namespace Vostok.Tracing.Abstractions
{
    /// <summary>
    /// A span sender that passes spans on to all of the base logs.
    /// </summary>
    [PublicAPI]
    public class CompositeSpanSender : ISpanSender
    {
        private readonly ISpanSender[] baseSpanSenders;

        public CompositeSpanSender(params ISpanSender[] baseSpanSenders)
        {
            this.baseSpanSenders = baseSpanSenders ?? throw new ArgumentNullException(nameof(baseSpanSenders));
        }

        public void Send(ISpan span)
        {
            foreach (var baseSpanSender in baseSpanSenders)
                baseSpanSender.Send(span);
        }
    }
}