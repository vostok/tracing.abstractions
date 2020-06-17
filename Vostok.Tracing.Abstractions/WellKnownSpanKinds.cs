using JetBrains.Annotations;

namespace Vostok.Tracing.Abstractions
{
    /// <summary>
    /// <para>This class contains a set of constants with <see cref="WellKnownAnnotations.Common.Kind"/>s of well-known standard span types.</para>
    /// <para>See nested classes for details:</para>
    /// <list type="bullet">
    ///     <item><description><see cref="HttpRequest"/></description></item>
    ///     <item><description><see cref="Database"/></description></item>
    ///     <item><description><see cref="Queue"/></description></item>
    /// </list>
    /// </summary>
    [PublicAPI]
    public static class WellKnownSpanKinds
    {
        /// <summary>
        /// Spans related to sending and handling of HTTP requests.
        /// </summary>
        [PublicAPI]
        public static class HttpRequest
        {
            /// <summary>
            /// <para>Spans that represent submitting an HTTP request directly to an external URL or a service replica.</para>
            /// <para>Relevant annotation sets: <see cref="WellKnownAnnotations.Common"/>, <see cref="WellKnownAnnotations.Http.Request"/>, <see cref="WellKnownAnnotations.Http.Response"/>.</para>
            /// </summary>
            public const string Client = HttpRequestPrefix + "client";

            /// <summary>
            /// <para>Spans that represent submitting an HTTP request to a clustered application with several replicas.</para>
            /// <para>Relevant annotation sets: <see cref="WellKnownAnnotations.Common"/>, <see cref="WellKnownAnnotations.Http.Request"/>, <see cref="WellKnownAnnotations.Http.Response"/>, <see cref="WellKnownAnnotations.Http.Cluster"/>.</para>
            /// </summary>
            public const string Server = HttpRequestPrefix + "server";

            /// <summary>
            /// <para>Spans that represent handling an HTTP request on server.</para>
            /// <para>Relevant annotation sets: <see cref="WellKnownAnnotations.Common"/>, <see cref="WellKnownAnnotations.Http.Request"/>, <see cref="WellKnownAnnotations.Http.Response"/>, <see cref="WellKnownAnnotations.Http.Client"/>.</para>
            /// </summary>
            public const string Cluster = HttpRequestPrefix + "cluster";

            private const string HttpRequestPrefix = "http-request-";
        }

        /// <summary>
        /// Spans related to database queries.
        /// </summary>
        [PublicAPI]
        public static class Database
        {
            /// <summary>
            /// <para>Spans that represent submitting requests to databases.</para>
            /// <para>Relevant annotation sets: <see cref="WellKnownAnnotations.Common"/>, <see cref="WellKnownAnnotations.Database"/>.</para>
            /// </summary>
            public const string Request = DatabasePrefix + "request";

            private const string DatabasePrefix = "db-";
        }

        /// <summary>
        /// Spans related to tasks in distributed queues.
        /// </summary>
        [PublicAPI]
        public static class Queue
        {
            /// <summary>
            /// <para>Spans that represent insertion of a task to queue (from the producer standpoint).</para>
            /// <para>Relevant annotation sets: <see cref="WellKnownAnnotations.Common"/>, <see cref="WellKnownAnnotations.Queue"/>.</para>
            /// </summary>
            public const string Producer = QueuePrefix + "producer";

            /// <summary>
            /// <para>Spans that represent management actions on the task, but from the client's standpoint, as opposed to <see cref="TaskLifecycleEvent"/> spans.</para>
            /// <para>Relevant annotation sets: <see cref="WellKnownAnnotations.Common"/>, <see cref="WellKnownAnnotations.Queue"/>.</para>
            /// </summary>
            public const string Manager = QueuePrefix + "manager";

            /// <summary>
            /// <para>Spans that represent execution of a queued task on the consumer.</para>
            /// <para>Relevant annotation sets: <see cref="WellKnownAnnotations.Common"/>, <see cref="WellKnownAnnotations.Queue"/>.</para>
            /// </summary>
            public const string Consumer = QueuePrefix + "consumer";

            /// <summary>
            /// <para>Spans that represent an event that somehow changes task state.</para>
            /// <para>Relevant annotation sets: <see cref="WellKnownAnnotations.Common"/>, <see cref="WellKnownAnnotations.Queue"/>.</para>
            /// </summary>
            public const string TaskLifecycle = QueueTaskPrefix + "lifecycle";

            /// <summary>
            /// <para>Spans that represent whole lifecycle of the task in queue. They serve as root spans in the personal task traces.</para>
            /// <para>Relevant annotation sets: <see cref="WellKnownAnnotations.Common"/>, <see cref="WellKnownAnnotations.Queue"/>.</para>
            /// </summary>
            public const string TaskLifecycleEvent = QueueTaskPrefix + "lifecycle-event";

            private const string QueuePrefix = "queue-";
            private const string QueueTaskPrefix = QueuePrefix + "task-";
        }

        /// <summary>
        /// <para>Spans that represent custom operations.</para>
        /// <para>Relevant annotation sets: <see cref="WellKnownAnnotations.Common"/>, <see cref="WellKnownAnnotations.Custom"/>.</para>
        /// </summary>
        [PublicAPI]
        public static class Custom
        {
            /// <summary>
            /// <para>Spans that represent submitting some not HTTP request to an external service replica.</para>
            /// <para>Relevant annotation sets: <see cref="WellKnownAnnotations.Common"/>, <see cref="WellKnownAnnotations.Custom.Request"/>, <see cref="WellKnownAnnotations.Custom.Response"/>.</para>
            /// </summary>
            public const string Client = CustomRequestPrefix + "client";

            /// <summary>
            /// <para>Spans that represent submitting some not HTTP request to an external service with several replicas..</para>
            /// <para>Relevant annotation sets: <see cref="WellKnownAnnotations.Common"/>, <see cref="WellKnownAnnotations.Custom.Request"/>, <see cref="WellKnownAnnotations.Custom.Response"/>.</para>
            /// </summary>
            public const string Cluster = CustomRequestPrefix + "cluster";

            /// <summary>
            /// <para>Spans that represent custom server operations.</para>
            /// <para>Relevant annotation sets: <see cref="WellKnownAnnotations.Common"/>, <see cref="WellKnownAnnotations.Custom.Operation"/>.</para>
            /// </summary>
            public const string Operation = CustomPrefix + "operation";
            private const string CustomPrefix = "custom-";
            private const string CustomRequestPrefix = CustomPrefix + "request-";
        }
    }
}