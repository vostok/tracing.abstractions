using System;
using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace Vostok.Tracing.Abstractions.Tests
{
    [TestFixture]
    public class WithAnnotationTracerExtensions_Tests
    {
        private ITracer baseTracer;
        private ITracer enrichedTracer;
        private ISpanBuilder spanBuilder;

        [SetUp]
        public void TestSetup()
        {
            spanBuilder = Substitute.For<ISpanBuilder>();

            baseTracer = Substitute.For<ITracer>();
            baseTracer.BeginSpan().Returns(spanBuilder);
            baseTracer.CurrentContext.Returns(new TraceContext(Guid.Empty, Guid.Empty));
        }

        [Test]
        public void WithAnnotation_should_return_a_tracer_that_adds_given_annotation_when_value_is_not_null()
        {
            enrichedTracer = baseTracer.WithAnnotation("key", "value");

            enrichedTracer.BeginSpan();

            spanBuilder.Received().SetAnnotation("key", "value", Arg.Any<bool>());
        }

        [Test]
        public void WithAnnotation_should_return_a_tracer_that_adds_given_annotation_when_value_is_null()
        {
            enrichedTracer = baseTracer.WithAnnotation("key", null as string);

            enrichedTracer.BeginSpan();

            spanBuilder.Received().SetAnnotation("key", null, Arg.Any<bool>());
        }

        [Test]
        public void WithAnnotation_should_not_overwrite_existing_values_by_default()
        {
            enrichedTracer = baseTracer.WithAnnotation("key", "value");

            enrichedTracer.BeginSpan();

            spanBuilder.Received().SetAnnotation("key", "value", false);
        }

        [Test]
        public void WithAnnotation_should_overwrite_existing_values_when_asked_to()
        {
            enrichedTracer = baseTracer.WithAnnotation("key", "value", true);

            enrichedTracer.BeginSpan();

            spanBuilder.Received().SetAnnotation("key", "value");
        }

        [Test]
        public void WithAnnotation_should_return_a_tracer_that_forwards_context_property_access_to_base_tracer()
        {
            enrichedTracer = baseTracer.WithAnnotation("key", "value");

            enrichedTracer.CurrentContext.Should().BeSameAs(baseTracer.CurrentContext);

            var newContext = new TraceContext(Guid.NewGuid(), Guid.NewGuid());

            enrichedTracer.CurrentContext = newContext;

            baseTracer.Received().CurrentContext = newContext;
        }

        [Test]
        public void WithAnnotation_with_dynamic_value_should_call_value_provider_delegate_for_each_event()
        {
            var counter = 0;

            enrichedTracer = baseTracer.WithAnnotation("key", () => ++counter, true);

            enrichedTracer.BeginSpan();

            spanBuilder.Received().SetAnnotation("key", 1, Arg.Any<bool>());

            enrichedTracer.BeginSpan();

            spanBuilder.Received().SetAnnotation("key", 2, Arg.Any<bool>());
        }

        [Test]
        public void WithAnnotation_with_dynamic_value_should_not_overwrite_existing_values_by_default()
        {
            enrichedTracer = baseTracer.WithAnnotation("key", () => "value");

            enrichedTracer.BeginSpan();

            spanBuilder.Received().SetAnnotation(Arg.Any<string>(), Arg.Any<string>(), false);
        }

        [Test]
        public void WithAnnotation_with_dynamic_value_should_overwrite_existing_values_when_explicitly_asked_to()
        {
            enrichedTracer = baseTracer.WithAnnotation("key", () => "value", true);

            enrichedTracer.BeginSpan();

            spanBuilder.Received().SetAnnotation(Arg.Any<string>(), Arg.Any<string>(), true);
        }

        [Test]
        public void WithAnnotation_with_dynamic_value_should_filter_out_null_values_by_default()
        {
            enrichedTracer = baseTracer.WithAnnotation("key", () => null as string);

            enrichedTracer.BeginSpan();

            spanBuilder.ReceivedCalls().Should().BeEmpty();
        }

        [Test]
        public void WithAnnotation_with_dynamic_value_should_pass_null_values_when_asked_to()
        {
            enrichedTracer = baseTracer.WithAnnotation("key", () => null as string, allowNullValues: true);

            enrichedTracer.BeginSpan();

            spanBuilder.Received().SetAnnotation("key", null, Arg.Any<bool>());
        }

        [Test]
        public void WithAnnotations_should_return_a_tracer_that_adds_given_annotations_to_span()
        {
            enrichedTracer = baseTracer.WithAnnotations(
                new Dictionary<string, object>
                {
                    {"key1", "value1"},
                    {"key2", "value2"}
                });

            enrichedTracer.BeginSpan();

            spanBuilder.Received().SetAnnotation("key1", "value1", Arg.Any<bool>());
            spanBuilder.Received().SetAnnotation("key2", "value2", Arg.Any<bool>());
        }

        [Test]
        public void WithAnnotations_should_filter_out_null_values_by_default()
        {
            enrichedTracer = baseTracer.WithAnnotations(
                new Dictionary<string, object>
                {
                    {"key1", "value1"},
                    {"key2", null}
                });

            enrichedTracer.BeginSpan();

            spanBuilder.Received().SetAnnotation("key1", "value1", Arg.Any<bool>());
            spanBuilder.DidNotReceive().SetAnnotation("key2", Arg.Any<string>(), Arg.Any<bool>());
        }

        [Test]
        public void WithAnnotations_should_pass_null_values_when_asked_to()
        {
            enrichedTracer = baseTracer.WithAnnotations(
                new Dictionary<string, object>
                {
                    {"key1", "value1"},
                    {"key2", null}
                }, allowNullValues: true);

            enrichedTracer.BeginSpan();

            spanBuilder.Received().SetAnnotation("key1", "value1", Arg.Any<bool>());
            spanBuilder.Received().SetAnnotation("key2", null, Arg.Any<bool>());
        }

        [TestCase(true)]
        [TestCase(false)]
        public void WithAnnotations_should_return_pass_overwrite_flag_to_span_builder(bool allowOverwrite)
        {
            enrichedTracer = baseTracer.WithAnnotations(
                new Dictionary<string, object>
                {
                    {"key1", "value1"},
                    {"key2", "value2"}
                },
                allowOverwrite);

            enrichedTracer.BeginSpan();

            spanBuilder.Received().SetAnnotation(Arg.Any<string>(), Arg.Any<string>(), allowOverwrite);
            spanBuilder.Received().SetAnnotation(Arg.Any<string>(), Arg.Any<string>(), allowOverwrite);
        }

        [Test]
        public void WithAnnotations_with_dynamic_provider_should_handle_null_object_returned()
        {
            enrichedTracer = baseTracer.WithAnnotations(() => null);

            enrichedTracer.BeginSpan();

            spanBuilder.ReceivedCalls().Should().BeEmpty();
        }

        [Test]
        public void WithAnnotations_with_dynamic_provider_should_call_the_delegate_for_each_span()
        {
            var counter = 0;

            enrichedTracer = baseTracer.WithAnnotations(
                () => new[]
                {
                    ("key1", ++counter as object),
                    ("key2", ++counter as object)
                });

            enrichedTracer.BeginSpan();

            spanBuilder.Received().SetAnnotation("key1", 1, Arg.Any<bool>());
            spanBuilder.Received().SetAnnotation("key2", 2, Arg.Any<bool>());

            enrichedTracer.BeginSpan();

            spanBuilder.Received().SetAnnotation("key1", 3, Arg.Any<bool>());
            spanBuilder.Received().SetAnnotation("key2", 4, Arg.Any<bool>());
        }

        [Test]
        public void WithAnnotations_with_dynamic_provider_should_filter_out_null_values_by_default()
        {
            enrichedTracer = baseTracer.WithAnnotations(
                () => new[]
                {
                    ("key1", null as object),
                    ("key2", null as object)
                });

            enrichedTracer.BeginSpan();

            spanBuilder.ReceivedCalls().Should().BeEmpty();
        }

        [Test]
        public void WithAnnotations_with_dynamic_provider_should_pass_null_values_when_asked_to()
        {
            enrichedTracer = baseTracer.WithAnnotations(
                () => new[]
                {
                    ("key1", null as object),
                    ("key2", null as object)
                }, allowNullValues: true);

            enrichedTracer.BeginSpan();

            spanBuilder.Received().SetAnnotation("key1", null, Arg.Any<bool>());
            spanBuilder.Received().SetAnnotation("key2", null, Arg.Any<bool>());
        }

        [TestCase(true)]
        [TestCase(false)]
        public void WithAnnotations_with_dynamic_provider_should_forward_overwrite_flag_to_span_builder(bool allowOverwrite)
        {
            enrichedTracer = baseTracer.WithAnnotations(
                () => new[]
                {
                    ("key1", "value1" as object),
                    ("key2", "value2" as object)
                }, allowOverwrite);

            enrichedTracer.BeginSpan();

            spanBuilder.Received().SetAnnotation("key1", "value1", allowOverwrite);
            spanBuilder.Received().SetAnnotation("key2", "value2", allowOverwrite);
        }
    }
}