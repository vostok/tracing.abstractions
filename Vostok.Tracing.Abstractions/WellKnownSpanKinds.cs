using JetBrains.Annotations;

namespace Vostok.Tracing.Abstractions
{
    [PublicAPI]
    public static class WellKnownSpanKinds
    {
        [PublicAPI]
        public static class HttpRequest
        {
            public const string Client = HttpRequestPrefix + "client";
            public const string Server = HttpRequestPrefix + "server";
            public const string Cluster = HttpRequestPrefix + "cluster";

            private const string HttpRequestPrefix = "http-request-";
        }

        [PublicAPI]
        public static class Database
        {
            public const string Request = DatabasePrefix + "request";

            private const string DatabasePrefix = "db-";
        }

        [PublicAPI]
        public static class Queue
        {
            public const string Producer = QueuePrefix + "producer";
            public const string Manager = QueuePrefix + "manager";
            public const string Consumer = QueuePrefix + "consumer";
            public const string TaskLifecycle = QueueTaskPrefix + "lifecycle";
            public const string TaskLifecycleEvent = QueueTaskPrefix + "lifecycle-event";

            private const string QueuePrefix = "queue-";
            private const string QueueTaskPrefix = QueuePrefix + "task-";
        }
    }
}
