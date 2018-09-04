# Vostok tracing

Distributed tracing allows to reconstruct the history of the logical operation spanning many applications and machines in time as a tree of smaller actions or events called spans. Spans can represent HTTP requests, database queries or any other significant interactions or events in a distributed system. A single span always describes a local event in a single process: an HTTP request usually produces two spans (client-side and server-side). Each kind of span stores specific information about performed action.


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
  
* `Annotatons` — payload in key-value format (string --> string).


## Common annotations

These are the annotations relevant for any kind of span:

| Name | Description |
|----|-----|
| operation | Human-readable logical operation or event name (e.g. `create-user`). |
| host | DNS name of the host the span originated from.  |
| kind | Span kind. There are a number of predefined span kinds for common use cases (e.g. `http-request-server`). |
| component | Name of a library or component in code responsible for producing the span. |


## Kind-specific annotations 

### HTTP client (direct)

Submitting an HTTP request directly to an external URL or a service replica.

| Name | Description | Default value |
|----|-----|----|
| kind | See [common annotations](#common-annotations).  | `http-request-client` |
| operation | See [common annotations](#common-annotations).  | `{http.request.method} {normalized http.request.url}`. Example: `POST /page/{num}/process`  |
| service | Name of the service to which request is sent. | `N/A` |
| http.request.method | Request method (e.g. `GET`, `POST`, `PUT`, etc). | `N/A` |
| http.request.url | Request URL without query parameters.  | `N/A` |
| http.request.size | Request body size in bytes. | `N/A` |
| http.response.code | Response code (e.g. `200` or `404`). | `N/A`  |
| http.response.size | Response body size in bytes. | `N/A` |

*Normalized URL is a short URL without scheme, authority and query parameters. Unique path segments (entity ids, search queries, hex values) are replaced with placeholders. Example before and after normalization: `http://vm-app1/users/a534bcbd/` --> `users/{id}`*

### HTTP client (cluster)

Submitting an HTTP request to a clustered application with several replicas.

| Name | Description | Default value |
|----|-----|----|
| kind | See [common annotations](#common-annotations).  | `http-request-cluster` |
| operation | See [common annotations](#common-annotations). | `{http.request.method} {normalized http.request.url}`. Example: `POST /page/{num}/process`  |
| service | Name of the service to which request is sent. | `N/A` |
| cluster.strategy | Name of the strategy used to send request (e.g. `sequential`, `parallel`, ...) | `N/A` |
| cluster.status | Status of interaction with a cluster (e.g. `success`, `no-replicas`, ...)  | `N/A` |
| http.request.method | Request method (e.g. `GET`, `POST`, `PUT`, etc). | `N/A` |
| http.request.url | Request URL without query parameters.  | `N/A` |
| http.request.size | Request body size in bytes. | `N/A` |
| http.response.code | Response code (e.g. `200` or `404`). | `N/A` |
| http.response.size | Response body size in bytes. | `N/A`  |

### HTTP server

Handling an HTTP request on server.

| Name | Description | Default value |
|----|-----|----|
| kind | See [common annotations](#common-annotations). | `http-request-server` |
| operation | See [common annotations](#common-annotations). | `{http.request.method} {normalized http.request.url}`. Example: `POST /page/{num}/process`  |
| service | Name of the service handling the request. | `N/A` |
| http.client.name | Name of the client application that sent the request. | `N/A` |
| http.client.address | Address of the client application instance (host name or IP address).  | `N/A` |
| http.request.method | Request method (e.g. `GET`, `POST`, `PUT`, etc). | `N/A` |
| http.request.url | Request URL without query parameters.  | `N/A` |
| http.request.size | Request body size in bytes. | `N/A` |
| http.response.code | Response code (e.g. `200` or `404`). | `N/A` |
| http.response.size | Response body size in bytes. | `N/A` |

### Database

Submitting a request to database.

| Name | Description | Default value |
|----|-----|----|
| kind | See [common annotations](#common-annotations). | `db-request` |
| operation | See [common annotations](#common-annotations). Possible examples: `read from table X`, `insert into  table Z`. | `N/A` |
| db.type | Database type (`mssql`, `cassandra`, `mongodb`, `redis`, etc). | `N/A` |
| db.executionResult | Result of performing request to a database. | `N/A` |
| db.instance | Address of the database server instance. | `N/A` |

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

### Queue (task-lifecycle)

A span that represents whole lifecycle of the task in queue. It serves as a root span in the task's personal trace.

Spans of this kind do not have an ending timestamp: it must be inferred from the rightmost end timestamp of all the child spans.

| Name | Description | Default value |
|----|-----|----|
| kind | See [common annotations](#common-annotations). | `queue-task-lifecycle` |
| queue.type | Queue type (`echelon`, `rabbit`, etc). | `N/A` |
| queue.topic | Name of the task type or the topic/queue it belongs to. | `N/A` |
| queue.taskId | Task unique identifier. | `N/A` |

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


### Queue (consumer)

A span that represents execution of a queued task on the consumer.

| Name | Description | Default value |
|----|-----|----|
| kind | See [common annotations](#common-annotations). | `queue-consumer` |
| queue.executionResult | Result of task execution (`success`, `error`, etc). | `N/A` |


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



### Custom events

A span for custom user-defined event in the application.

| Name | Description | Default value |
|----|-----|----|
| kind | See [common annotations](#common-annotations). | `custom-event` |



# Queue tracing

Tracing a queue task is a collection of events that occurred within the lifecycle of the task processing.

1. Each task has own trace
2. General information about task stores in endless root span (doesn't endtimespamp)
3. Before placing task in queue, it need to be enriched with data: create a traceId task, create root span. TraceId can pass the producer
4. After placing task in queue, producer can return traceId and taskId (if they where created by the queue). This data saves in the producer span
5. Root span has `queue-task-lifecycle` kind. All child spans has `queue-task-lifecycle-event` or `queue.consumer` kind
6. Span of `queue-task-lifecycle-event` kind can have link to process trace, which triggered the event
7. Events can be zero duration. For example, the event pass task to the consumer.
8. During the execution of the task, events that change the state of the task can occur (for example, deleting a task by another request). Such events are child of rootspan
9. Spans that change the status of a task are of the type queue-manager

![General queue tracing scheme](docs/images/general.jpg)

## Echelon tracing 

Since the interaction with the queue broker occurs via http api, the http call spans are stored in the process trace. 

![General queue tracing scheme](docs/images/echelon.jpg)

## Tracing queues without a broker or without access to the source code of the broker
Broker's trace actions are performed by a process that works with the task
* When creating a task in the queue, the producer must create a span not only for its own trace, but also create a trace for the task + put in the meta-information of the task.
* If someone needs to change a task, then this action should be reflected not only in the trace of this process, but also in the task trace.
