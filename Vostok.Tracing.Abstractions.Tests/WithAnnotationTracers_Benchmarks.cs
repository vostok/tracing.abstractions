using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Running;
using NUnit.Framework;

namespace Vostok.Tracing.Abstractions.Tests
{
    [Explicit]
    public class WithAnnotationTracers_Benchmarks
    {
        private ITracer tracerWithDictionaryAnnotations;
        private ITracer tracerWithEnumerableAnnotations;

        [Test]
        public void RunBenchmark()
        {
            BenchmarkRunner.Run<WithAnnotationTracers_Benchmarks>(
                DefaultConfig.Instance
                    .AddDiagnoser(MemoryDiagnoser.Default)
                    .WithOption(ConfigOptions.DisableOptimizationsValidator, true));
        }

        [GlobalSetup]
        public void SetUp()
        {
            var tracer = new DevNullTracer();
            var annotationsDictionary = new Dictionary<string, object> {{"blabla", (object)"foo"}};
            tracerWithDictionaryAnnotations = tracer.WithAnnotations(annotationsDictionary);

            var annotationsEnumerable = new (string, object)[] {("blabla", "foo")};
            tracerWithEnumerableAnnotations = tracer.WithAnnotations(() => annotationsEnumerable);
        }

        [Benchmark(Baseline = true)]
        public void WithDict()
        {
            tracerWithDictionaryAnnotations.BeginSpan().Dispose();
        }


        [Benchmark]
        public void WithFunc()
        {
            tracerWithEnumerableAnnotations.BeginSpan().Dispose();
        }

        /*
        // * Summary*

        BenchmarkDotNet = v0.13.0, OS = Windows 10.0.19043.1415 (21H1/May2021Update)
        Intel Core i7-4771 CPU 3.50GHz(Haswell), 1 CPU, 8 logical and 4 physical cores
        .NET SDK= 6.0.101

        [Host]     : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT
        DefaultJob : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT

        |   Method |     Mean |    Error |   StdDev |   Median | Ratio | RatioSD |  Gen 0 | Allocated |
        |--------- |---------:|---------:|---------:|---------:|------:|--------:|-------:|----------:|
        | WithDict | 32.18 ns | 0.947 ns | 2.686 ns | 31.57 ns |  1.00 |    0.00 | 0.0076 |      32 B |
        | WithFunc | 30.90 ns | 0.606 ns | 1.680 ns | 30.28 ns |  0.97 |    0.09 | 0.0076 |      32 B |

         */

    }
}
