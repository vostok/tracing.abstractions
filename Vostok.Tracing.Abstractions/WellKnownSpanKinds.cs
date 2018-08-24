using JetBrains.Annotations;

namespace Vostok.Tracing.Abstractions
{
    [PublicAPI]
    public static class WellKnownSpanKinds
    {
        public const string Database = "db-request";

        public static class HttpRequest
        {
            public const string Client = HttpRequestPrefix + "client";
            public const string Server = HttpRequestPrefix + "server";
            public const string Cluster = HttpRequestPrefix + "cluster";
            private const string HttpRequestPrefix = "http-request-";
        }

        public static class Queue
        {
            private const string QueuePrefix = "queue-";
            private const string TaskPrefix = "task-";

            public const string Producer = QueuePrefix + "producer";
            public const string Manager = QueuePrefix + "manager";
            public const string Consumer = QueuePrefix + "consumer";
            public const string TaskLifecycle = QueuePrefix + TaskPrefix + "lifecycle";
            public const string TaskLifecycleEvent = QueuePrefix + TaskPrefix + "lifecycle-event";
        }
    }
}