using JetBrains.Annotations;

namespace Vostok.Tracing.Abstractions
{
    /// <summary>
    /// <para>This class contains a set of constants with names of well-known standard annotations.</para>
    /// <para>See nested classes for details:</para>
    /// <list type="bullet">
    ///     <item><description><see cref="Common"/></description></item>
    ///     <item><description><see cref="Http"/></description></item>
    ///     <item><description><see cref="Database"/></description></item>
    ///     <item><description><see cref="Queue"/></description></item>
    /// </list>
    /// </summary>
    [PublicAPI]
    public static class WellKnownAnnotations
    {
        /// <summary>
        /// Annotations relevant to virtually any span regardless of its <see cref="Kind"/>.
        /// </summary>
        [PublicAPI]
        public static class Common
        {
            /// <summary>
            /// Human-readable logical operation or event name (e.g. create-user).
            /// </summary>
            public const string Operation = "operation";

            /// <summary>
            /// <para>Logical operation or event status.</para>
            /// <para>See <see cref="WellKnownStatuses"/> for possible values.</para>
            /// </summary>
            public const string Status = "status";

            /// <summary>
            /// DNS name of the host the span originated from.
            /// </summary>
            public const string Host = "host";

            /// <summary>
            /// <para>Span kind is a type of event/interaction the span describes.</para>
            /// <para>There are a number of predefined kinds for common use cases (see <see cref="WellKnownSpanKinds"/>).</para>
            /// </summary>
            public const string Kind = "kind";

            /// <summary>
            /// Name of a library or component in code responsible for producing the span.
            /// </summary>
            public const string Component = "component";

            /// <summary>
            /// Name of the application the span originated from.
            /// </summary>
            public const string Application = "application";

            /// <summary>
            /// Name of the environment the span originated from.
            /// </summary>
            public const string Environment = "environment";
        }

        /// <summary>
        /// <para>Annotations relevant to spans that represent sending and handling of HTTP requests.</para>
        /// <para>See <see cref="Request"/>, <see cref="Response"/>, <see cref="Cluster"/> and <see cref="Client"/> categories for details.</para>
        /// </summary>
        public static class Http
        {
            private const string HttpPrefix = "http.";

            /// <summary>
            /// Annotations that describe HTTP request properties and destination.
            /// </summary>
            [PublicAPI]
            public static class Request
            {
                /// <summary>
                /// Request method (e.g. GET, POST, PUT, etc).
                /// </summary>
                public const string Method = HttpRequestPrefix + "method";

                /// <summary>
                /// Request URL without query parameters (absolute or relative).
                /// </summary>
                public const string Url = HttpRequestPrefix + "url";

                /// <summary>
                /// Request body size in bytes.
                /// </summary>
                public const string Size = HttpRequestPrefix + "size";

                /// <summary>
                /// Name of the service to which request is sent.
                /// </summary>
                public const string TargetService = HttpRequestPrefix + "targetService";

                /// <summary>
                /// Name of the environment to which request is sent.
                /// </summary>
                public const string TargetEnvironment = HttpRequestPrefix + "targetEnvironment";

                private const string HttpRequestPrefix = HttpPrefix + "request.";
            }

            /// <summary>
            /// Annotations that describe HTTP response properties.
            /// </summary>
            [PublicAPI]
            public static class Response
            {
                /// <summary>
                /// Response code (e.g. 200 or 404).
                /// </summary>
                public const string Code = HttpResponsePrefix + "code";

                /// <summary>
                /// Response body size in bytes.
                /// </summary>
                public const string Size = HttpResponsePrefix + "size";

                private const string HttpResponsePrefix = HttpPrefix + "response.";
            }

            /// <summary>
            /// Annotations relevant to spans that represent sending HTTP requests to services with clusters of replicas/instances.
            /// </summary>
            [PublicAPI]
            public static class Cluster
            {
                /// <summary>
                /// Name of the strategy used to send request (e.g. 'sequential', 'parallel', ...)
                /// </summary>
                public const string Strategy = HttpClusterPrefix + "strategy";

                /// <summary>
                /// Status of interaction with a cluster (e.g. 'success', 'no-replicas', ...)
                /// </summary>
                public const string Status = HttpClusterPrefix + "status";

                private const string HttpClusterPrefix = HttpPrefix + "cluster.";
            }

            /// <summary>
            /// Annotations that describe HTTP client properties from server standpoint. Relevant to spans of <see cref="WellKnownSpanKinds.HttpRequest.Server"/> kind.
            /// </summary>
            [PublicAPI]
            public static class Client
            {
                /// <summary>
                /// Name of the client application that sent the request.
                /// </summary>
                public const string Name = HttpClientPrefix + "name";

                /// <summary>
                /// Address of the client application instance (host name or IP address).
                /// </summary>
                public const string Address = HttpClientPrefix + "address";

                private const string HttpClientPrefix = HttpPrefix + "client.";
            }
        }

        /// <summary>
        /// Annotations relevant to spans that represent database queries.
        /// </summary>
        [PublicAPI]
        public static class Database
        {
            /// <summary>
            /// Database type ('mssql', 'cassandra', 'mongodb', 'elastic', etc).
            /// </summary>
            public const string Type = DatabasePrefix + "type";

            /// <summary>
            /// Address of the database server instance (URL, hostname, connection string).
            /// </summary>
            public const string Instance = DatabasePrefix + "instance";

            private const string DatabasePrefix = "db.";
        }

        /// <summary>
        /// Annotations relevant to spans that represent operations with tasks in distributed queues.
        /// </summary>
        [PublicAPI]
        public static class Queue
        {
            /// <summary>
            /// Queue type ('echelon', 'rabbit', etc).
            /// </summary>
            public const string Type = QueuePrefix + "type";

            /// <summary>
            /// Name of the task type or the topic/queue it was inserted to.
            /// </summary>
            public const string Topic = QueuePrefix + "topic";

            /// <summary>
            /// Task unique identifier.
            /// </summary>
            public const string TaskId = QueuePrefix + "taskId";

            /// <summary>
            /// Trace identifier assigned to the task.
            /// </summary>
            public const string TaskTraceId = QueuePrefix + "taskTraceId";

            /// <summary>
            /// Trace identifier of the producer's (client app that created the task) request.
            /// </summary>
            public const string ProducerTraceId = QueuePrefix + "producerTraceId";

            /// <summary>
            /// Trace identifier of the client app request that triggered the event and changed the task state (if any).
            /// </summary>
            public const string ExternalTraceId = QueuePrefix + "externalTraceId";

            public const string ActionResult = QueuePrefix + "actionResult";

            public const string ExecutionResult = QueuePrefix + "executionResult";

            private const string QueuePrefix = "queue.";
        }

        /// <summary>
        /// Annotations relevant to spans that represent custom operations.
        /// </summary>
        [PublicAPI]
        public static class Custom
        {
            private const string CustomPrefix = "custom.";

            /// <summary>
            /// Annotations that describe custom request properties and destination.
            /// </summary>
            [PublicAPI]
            public static class Request
            {
                /// <summary>
                /// Name of the service to which request is sent.
                /// </summary>
                public const string TargetService = CustomRequestPrefix + "targetService";

                /// <summary>
                /// Name of the environment to which request is sent.
                /// </summary>
                public const string TargetEnvironment = CustomRequestPrefix + "targetEnvironment";

                /// <summary>
                /// Request body size in bytes.
                /// </summary>
                public const string Size = CustomRequestPrefix + "size";

                /// <summary>
                /// Replica name.
                /// </summary>
                public const string Replica = CustomRequestPrefix + "replica";

                private const string CustomRequestPrefix = CustomPrefix + "request.";
            }

            /// <summary>
            /// Annotations that describe custom response properties.
            /// </summary>
            [PublicAPI]
            public static class Response
            {
                /// <summary>
                /// Response body size in bytes.
                /// </summary>
                public const string Size = CustomResponsePrefix + "size";

                /// <summary>
                /// Status of interaction (e.g. 'success', 'no-replicas', ...)
                /// </summary>
                public const string Status = CustomResponsePrefix + "status";

                private const string CustomResponsePrefix = CustomPrefix + "response.";
            }

            /// <summary>
            /// Annotations that describe custom server operation properties.
            /// </summary>
            [PublicAPI]
            public static class Operation
            {
                /// <summary>
                /// Processed data size in bytes.
                /// </summary>
                public const string Size = CustomOperationPrefix + "size";

                /// <summary>
                /// Status of operation (e.g. 'success', 'no-replicas', ...)
                /// </summary>
                public const string Status = CustomOperationPrefix + "status";

                private const string CustomOperationPrefix = CustomPrefix + "operation.";
            }
        }
    }
}