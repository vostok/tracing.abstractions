# Vostok tracing

Distributed tracing allows to reconstruct the history of the logical operation spanning many applications and machines in time as a tree of smaller actions or events called spans. Spans can represent HTTP requests, database queries or any other significant interactions or events in a distributed system. A single span always describes a local event in a single process: an HTTP request usually produces two spans (client-side and server-side). Each kind of span stores specific information about performed action.

<br/>

* [Span structure](#span-structure)
* [Common annotations](#common-annotations)
* [Kind-specific annotations](#kind-specific-annotations)
  * [HTTP requests](#http-requests)
    * [HTTP client (direct)](#http-client-direct)
    * [HTTP client (cluster)](#http-client-cluster)
    * [HTTP server](#http-server)
  * [Database](#database)
  * [Queue (producer)](#queue-producer)
  * [Queue (task-lifecycle)](#queue-task-lifecycle)
  * [Queue (task-lifecycle-event)](#queue-task-lifecycle-event)
  * [Queue (consumer)](#queue-consumer)
  * [Queue (manager)](#queue-manager)
  * [Custom events](#custom-events)
* [Queue tracing](#queue-tracing)

<br/>

## Span structure

Every span consists of following fields:
* `TraceId` — unique identifier of the trace containing the span (Guid).
  * Gets assigned on first operation (usually on a front-end application instance).
  * Serves as a correlation identifier between spans
  
* `SpanId` — unique identifier of the span itself (Guid).

* `ParentSpanId` — identifier of the parent span in the tree (Guid). 
  * May be absent for root span in the tree.
  
* `BeginTimestamp` — beginning timestamp of the event or interaction described by this span.

* `EndTimestamp` — ending timestamp of the event or interaction described by this span.
  * Always measured with the same clock as `BeginTimestamp`. This allows to derive span duration as a difference between `EndTimestamp` and `BeginTimestamp`.
  * May be absent for a special kind of 'endless' spans described further.
  
* `Annotations` — payload in key-value format (string --> string).

<br/>

## Common annotations

These are the annotations relevant for any span:

| Name | Description |
|----|-----|
| `kind` | Span kind. There are a number of predefined span kinds for common use cases (e.g. `http-request-server`). |
| `operation` | Human-readable logical operation or event name (e.g. `create-user`). |
| `application` | Name of the application the span originated from. |
| `environment` | Name of the environment the span originated from. |
| `host` | DNS name of the host the span originated from.  |
| `component` | Name of a library or component in code responsible for producing the span. |

<br/>
<br/>

## Kind-specific annotations 

### HTTP requests

Common annotations for all spans related to HTTP requests:

| Name | Description | Default value |
|----|-----|----|
| `operation` | See [common annotations](#common-annotations).  | `{http.request.method} {normalized http.request.url}`. Example: `POST /page/{num}/process`  |
| `http.request.method` | Request method (e.g. `GET`, `POST`, `PUT`, etc). | `N/A` |
| `http.request.url` | Request URL without query parameters.  | `N/A` |
| `http.request.size` | Request body size in bytes. | `N/A` |
| `http.response.code` | Response code (e.g. `200` or `404`). | `N/A`  |
| `http.response.size` | Response body size in bytes. | `N/A` |

*Normalized URL is a short URL without scheme, authority and query parameters. Unique path segments (entity ids, search queries, hex values) are replaced with placeholders. Example before and after normalization: `http://vm-app1/users/a534bcbd/` --> `users/{id}`*

<br/>

#### HTTP client (direct)

Submitting an HTTP request directly to an external URL or a service replica.

Specific annotations:

| Name | Description | Default value |
|----|-----|----|
| `kind` | See [common annotations](#common-annotations).  | `http-request-client` |
| `http.request.targetService` | Name of the service to which request is sent. | `N/A` |
| `http.request.targetEnvironment` | Name of the environment to which request is sent. | `N/A` |

<br/>

### HTTP client (cluster)

Submitting an HTTP request to a clustered application with several replicas.

Specific annotations:

| Name | Description | Default value |
|----|-----|----|
| `kind` | See [common annotations](#common-annotations).  | `http-request-cluster` |
| `cluster.strategy` | Name of the strategy used to send request (e.g. `sequential`, `parallel`, ...) | `N/A` |
| `cluster.status` | Status of interaction with a cluster (e.g. `success`, `no-replicas`, ...)  | `N/A` |
| `http.request.targetService` | Name of the service to which request is sent. | `N/A` |
| `http.request.targetEnvironment` | Name of the environment to which request is sent. | `N/A` |

<br/>

### HTTP server

Handling an HTTP request on server.

Specific annotations:

| Name | Description | Default value |
|----|-----|----|
| `kind` | See [common annotations](#common-annotations). | `http-request-server` |
| `http.client.name` | Name of the client application that sent the request. | `N/A` |
| `http.client.address` | Address of the client application instance (host name or IP address).  | `N/A` |

<br/>
<br/>

### Database

Submitting a request to database.

| Name | Description | Default value |
|----|-----|----|
| kind | See [common annotations](#common-annotations). | `db-request` |
| operation | See [common annotations](#common-annotations). Possible examples: `read from table X`, `insert into  table Z`. | `N/A` |
| db.type | Database type (`mssql`, `cassandra`, `mongodb`, `redis`, etc). | `N/A` |
| db.executionResult | Result of performing request to a database. | `N/A` |
| db.instance | Address of the database server instance. | `N/A` |

<br/>

### Queue (producer)

Inserting a task to queue (from the producer standpoint).

| Name | Description | Default value |
|----|-----|----|
| kind | See [common annotations](#common-annotations). | `queue-producer` |
| operation | See [common annotations](#common-annotations). | `({queue.type}) Put to '{queue.topic}'`. Example: `(echelon) Put to 'reports'`. |
| queue.type | Queue type (`echelon`, `rabbit`, etc). | `N/A` |
| queue.topic | Name of the task type or the topic/queue it was inserted to.  | `N/A` |
| queue.actionResult | Result of action (`success` or something else). | `N/A` |
| queue.taskId | Task unique identifier. | `N/A` |
| queue.taskTraceId | Trace identifier assigned to the task. | `N/A` |

<br/>

### Queue (task-lifecycle)

A span that represents whole lifecycle of the task in queue. It serves as a root span in the task's personal trace.

Spans of this kind do not have an ending timestamp: it must be inferred from the rightmost end timestamp of all the child spans.

| Name | Description | Default value |
|----|-----|----|
| kind | See [common annotations](#common-annotations). | `queue-task-lifecycle` |
| queue.type | Queue type (`echelon`, `rabbit`, etc). | `N/A` |
| queue.topic | Name of the task type or the topic/queue it belongs to. | `N/A` |
| queue.taskId | Task unique identifier. | `N/A` |

<br/>

### Queue (task-lifecycle-event)

A span that represents an event that somehow changes task state.

Such spans usually have zero duration and are produced by brokers or client libraries.

| Name | Description | Default value |
|----|-----|----|
| kind | See [common annotations](#common-annotations). | `queue-task-lifecycle-event` |
| operation | See [common annotations](#common-annotations). Examples: `pass-to-consumer`, `prolong-execution`, ... | `N/A` |
| queue.type | Queue type (`echelon`, `rabbit`, etc). | `N/A` |
| queue.topic | Name of the task type or the topic/queue it belongs to. | `N/A` |
| queue.taskId | Task unique identifier. | `N/A` |

<br/>

### Queue (consumer)

A span that represents execution of a queued task on the consumer.

| Name | Description | Default value |
|----|-----|----|
| kind | See [common annotations](#common-annotations). | `queue-consumer` |
| queue.executionResult | Result of task execution (`success`, `error`, etc). | `N/A` |

<br/>

### Queue (manager)

A span that represents a management action on the task, but from the client's standpoint, as opposed to `queue-task-lifecycle-event` spans.

| Name | Description | Default value |
|----|-----|----|
| kind | See [common annotations](#common-annotations). | `queue-manager` |
| operation | See [common annotations](#common-annotations). Examples: `delete`, `prolong-execution`, ... | `N/A` |
| queue.type | Queue type (`echelon`, `rabbit`, etc). | `N/A` |
| queue.topic | Name of the task type or the topic/queue it belongs to. | `N/A` |
| queue.taskId | Task unique identifier. | `N/A` |
| queue.actionResult | Result of action (`success` or something else). | `N/A` |

<br/>

### Custom events

A span for custom user-defined event in the application.

| Name | Description | Default value |
|----|-----|----|
| kind | See [common annotations](#common-annotations). | `custom-event` |

<br/>
<br/>

## Queue tracing

Following conventions apply to tracing of queue tasks:

* Each task should have a dedicated trace with a root span of `queue-task-lifecycle` kind. This span does not have an explicit end timestamp. It gets "stretched" by child spans instead.
* Each task should contain its personal `traceId` and root span's `spanId` embedded in content, available for consumers.
* Root lifecycle spans can have child spans of `queue-task-lifecycle-event` and `queue-consumer` kinds. External operations from producers and management client happen in their own traces.
* A span of `queue-task-lifecycle-event` kind can have a link to `traceId` of external operation that triggered the event.
* A span of `queue-producer` kind should have a link to `traceId` of the produced task.

![General queue tracing scheme](docs/images/general.jpg)

<br/>

### Echelon tracing 

![General queue tracing scheme](docs/images/echelon.jpg)

<br/>

### Tracing queues without a broker
If a queue has no broker or there's no access to broker code, its tracing duties have to be performed by client libraries (creation of `queue-task-lifecycle` and `queue-task-lifecycle-event` spans).
