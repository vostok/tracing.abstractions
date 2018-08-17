using System;

namespace Vostok.Tracing.Abstractions.Tests
{
    public class Example
    {
        private ITracer tracer;

        public void Main()
        {
            using (var span = tracer.BeginSpan())
            {
                span.SetAnnotation("service", "my cool service");
            }
        }
    }
}