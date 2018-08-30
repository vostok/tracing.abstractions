﻿using JetBrains.Annotations;

namespace Vostok.Tracing.Abstractions
{
    [PublicAPI]
    public static class WellKnownAnnotations
    {
        public const string Operation = "operation";
        public const string Host = "host";
        public const string Kind = "kind";
        public const string Component = "component";
        public const string Service = "service";

        public static class Http
        {
            private const string HttpPrefix = "http.";

            public class Request
            {
                public const string Method = HttpRequestPrefix + "method";
                public const string Url = HttpRequestPrefix + "url";
                public const string Size = HttpRequestPrefix + "size";

                private const string HttpRequestPrefix = HttpPrefix + "request.";
            }

            public static class Response
            {
                public const string Code = HttpResponsePrefix + "code";
                public const string Size = HttpResponsePrefix + "size";

                private const string HttpResponsePrefix = HttpPrefix + "response.";
            }

            public static class Client
            {
                public const string Name = HttpClientPrefix + "name";
                public const string Address = HttpClientPrefix + "address";

                private const string HttpClientPrefix = HttpPrefix + "client.";
            }
        }

        public static class Queue
        {
            public const string Type = QueuePrefix + "type";
            public const string Topic = QueuePrefix + "topic";
            public const string TaskId = QueuePrefix + "taskId";
            public const string TaskTraceId = QueuePrefix + "taskTraceId";
            public const string ActionResult = QueuePrefix + "actionResult";
            public const string ExecutionResult = QueuePrefix + "executionResult";

            private const string QueuePrefix = "queue.";

            public static class Source
            {
                public const string TraceId = QueueSourcePrefix +  "traceId";
                public const string SpanId = QueueSourcePrefix + "spanId";

                private const string QueueSourcePrefix = QueuePrefix + "source.";
            }
        }

        public static class Cluster
        {
            public const string Strategy = ClusterPrefix + "strategy";
            public const string Status = ClusterPrefix + "status";

            private const string ClusterPrefix = "cluster.";
        }

        public static class Database
        {
            public const string Type = DatabasePrefix + "type";
            public const string ExecutionResult = DatabasePrefix + "executionResult";
            public const string Instance = DatabasePrefix + "instance";

            private const string DatabasePrefix = "db.";
        }
    }
}