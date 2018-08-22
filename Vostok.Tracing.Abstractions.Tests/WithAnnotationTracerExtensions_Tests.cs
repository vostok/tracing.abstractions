using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;

namespace Vostok.Tracing.Abstractions.Tests
{
    [TestFixture]
    public class WithAnnotationTracerExtensions_Tests
    {
        private ITracer baseTracer;
        private ITracer enrichTracer;
        private ISpanBuilder spanBuilder;

        [SetUp]
        public void TestSetup()
        {
            baseTracer = Substitute.For<ITracer>();
            spanBuilder = Substitute.For<ISpanBuilder>();

            baseTracer.BeginSpan().Returns(spanBuilder);
        }

        [Test]
        public void WithAnnotation_should_return_a_tracer_that_setted_given_annotation_when_value_is_not_null()
        {
            enrichTracer = baseTracer.WithAnnotation("key1", "value1");

            enrichTracer.BeginSpan();

            spanBuilder.Received().SetAnnotation("key1", "value1", false);
        }

        [Test]
        public void WithAnnotation_should_return_a_tracer_that_setted_given_annotation_when_value_is_null_and_allowNull()
        {
            enrichTracer = baseTracer.WithAnnotation("key1", () => null as string, false, true);

            enrichTracer.BeginSpan();

            spanBuilder.Received().SetAnnotation("key1", null, false);
        }

        [Test]
        public void WithAnnotation_should_return_a_tracer_that_not_setted_given_annotation_when_value_is_null_and_not_allowNull()
        {
            enrichTracer = baseTracer.WithAnnotation("key1", () => null as string);

            enrichTracer.BeginSpan();

            spanBuilder.DidNotReceive().SetAnnotation("key1", null, false);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void WithAnnotation_should_return_a_tracer_that_pass_allowOverwrite_to_spanbuilder(bool allowOverwrite)
        {
            enrichTracer = baseTracer.WithAnnotation("key1", () => "value1", allowOverwrite);

            enrichTracer.BeginSpan();

            spanBuilder.Received().SetAnnotation("key1", "value1", allowOverwrite);
        }

        [Test]
        public void WithAnnotations_should_return_a_tracer_that_setted_given_annotations()
        {
            enrichTracer = baseTracer.WithAnnotations(
                new Dictionary<string, string>
                {
                    {"key1", "value1"},
                    {"key2", "value2"}
                });

            enrichTracer.BeginSpan();

            spanBuilder.Received().SetAnnotation("key1", "value1", false);
            spanBuilder.Received().SetAnnotation("key2", "value2", false);
        }

        [Test]
        public void WithAnnotations_should_filter_out_null_values_when_asked_to()
        {
            enrichTracer = baseTracer.WithAnnotations(
                new Dictionary<string, string>
                {
                    {"key1", "value1"},
                    {"key2", null}
                });

            enrichTracer.BeginSpan();

            spanBuilder.Received().SetAnnotation("key1", "value1", false);
            spanBuilder.DidNotReceive().SetAnnotation("key2", null, false);
        }

        [Test]
        public void WithAnnotations_should_pass_null_values_when_asked_to()
        {
            enrichTracer = baseTracer.WithAnnotations(
                new Dictionary<string, string>
                {
                    {"key1", "value1"},
                    {"key2", null}
                },
                false,
                true);

            enrichTracer.BeginSpan();

            spanBuilder.Received().SetAnnotation("key1", "value1", false);
            spanBuilder.Received().SetAnnotation("key2", null, false);
        }

        [Test]
        public void WithAnnotations_with_dynamic_provider_should_handle_null_object_returned()
        {
            enrichTracer = baseTracer.WithAnnotations(() => null);

            enrichTracer.BeginSpan();

            spanBuilder.DidNotReceive().SetAnnotation(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>());
        }

        [Test]
        public void WithAnnotations_with_dynamic_provider_should_call_the_delegate_for_each_event()
        {
            var counter = 0;

            enrichTracer = baseTracer.WithAnnotations(
                () => new[]
                {
                    ("key1", (++counter).ToString()),
                    ("key2", (++counter).ToString()),
                });

            enrichTracer.BeginSpan();

            spanBuilder.Received().SetAnnotation("key1", "1", false);
            spanBuilder.Received().SetAnnotation("key2", "2", false);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void WithAnnotations_should_return_a_tracer_that_pass_allowOverwrite_to_spanbuilder(bool allowOverwrite)
        {
            enrichTracer = baseTracer.WithAnnotations(
                new Dictionary<string, string>
                {
                    {"key1", "value1"},
                    {"key2", "value2"}
                },
                allowOverwrite);

            enrichTracer.BeginSpan();

            spanBuilder.Received().SetAnnotation("key1", "value1", allowOverwrite);
            spanBuilder.Received().SetAnnotation("key2", "value2", allowOverwrite);
        }
    }
}