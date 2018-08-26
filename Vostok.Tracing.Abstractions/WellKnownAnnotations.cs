using JetBrains.Annotations;

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
                public const string Method = HttpPrefix + RequestPrefix + "method";
                public const string Url = HttpPrefix + RequestPrefix + "url";
                public const string Size = HttpPrefix + RequestPrefix + "contentLength";
                private const string RequestPrefix = "request.";
            }

            public static class Response
            {
                public const string Code = HttpPrefix + ResponsePrefix + "statusCode";
                public const string Size = HttpPrefix + ResponsePrefix + "contentLength";
                private const string ResponsePrefix = "response.";
            }

            public static class Client
            {
                public const string Name = HttpPrefix + ClientPrefix + "name";
                public const string Address = HttpPrefix + ClientPrefix + "address";
                private const string ClientPrefix = "client.";
            }
        }

        public static class Queue
        {
            public const string Type = QueuePrefix + "type";
            public const string Topic = QueuePrefix + "topic";
            public const string ActionResult = QueuePrefix + "actionResult";
            public const string TaskId = QueuePrefix + "taskId";
            public const string TaskTraceId = QueuePrefix + "taskTraceId";
            public const string ExecutionResult = QueuePrefix + "executionResult";
            private const string QueuePrefix = "queue.";

            public static class Source
            {
                public const string TraceId = QueuePrefix + SourcePrefix +  "traceId";
                public const string SpanId = QueuePrefix + SourcePrefix + "spanId";
                private const string SourcePrefix = "source.";
            }
        }

        public static class Cluster
        {
            public const string Strategy = ClusterPrefix+ "requestStrategy";
            public const string Status = ClusterPrefix + "status";
            private const string ClusterPrefix = "cluster.";
        }

        public static class Database
        {
            public const string Type = DbPrefix + "type";
            public const string ExecutionResult = DbPrefix + "executionResult";
            public const string Instance = DbPrefix + "instance";
            private const string DbPrefix = "db.";
        }
    }
}