# Vostok tracing

Distributed tracing allows to reconstruct the history of the logical operation spanning many applications and machines in time as a tree of smaller actions or events called spans. Spans can represent HTTP requests, database queries or any other significant interactions or events in a distributed system. A single span always describes a local event in a single process: an HTTP request usually produces two spans (client-side and server-side). 

Each kind of span stores specific information about performed action.


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
| service | Name of the service to which request is sent. | N/A |
| http.request.method | Request method (e.g. `GET`, `POST`, `PUT`, etc). | N/A |
| http.request.url | Request URL without query parameters.  | N/A |
| http.request.size | Request body size in bytes. | N/A |
| http.response.code | Response code (e.g. `200` or `404`). | N/A  |
| http.response.size | Response body size in bytes. | N/A |

*Normalized URL is a short URL without scheme, authority and query parameters. Unique path segments (entity ids, search queries, hex values) are replaced with placeholders. Example before and after normalization: `http://vm-app1/users/a534bcbd/` --> `users/{id}`*

### HTTP client (cluster)

Submitting an HTTP request to a clustered application with several replicas.

| Name | Description | Default value |
|----|-----|----|
| kind | See [common annotations](#common-annotations).  | `http-request-cluster` |
| operation | See [common annotations](#common-annotations). | `{http.request.method} {normalized http.request.url}`. Example: `POST /page/{num}/process`  |
| service | Name of the service to which request is sent. | N/A |
| cluster.requestStrategy | Name of the strategy used to send request (e.g. `sequential`, `parallel`, ...) | N/A |
| cluster.status | Status of interaction with a cluster (e.g. `success`, `no-replicas`, ...)  | N/A |
| http.request.method | Request method (e.g. `GET`, `POST`, `PUT`, etc). | N/A |
| http.request.url | Request URL without query parameters.  | N/A |
| http.request.size | Request body size in bytes. | N/A |
| http.response.code | Response code (e.g. `200` or `404`). | N/A |
| http.response.size | Response body size in bytes. | N/A  |

### HTTP server

Handling an HTTP request on server.

| Name | Description | Default value |
|----|-----|----|
| kind | See [common annotations](#common-annotations). | `http-request-server` |
| operation | See [common annotations](#common-annotations). | `{http.request.method} {normalized http.request.url}`. Example: `POST /page/{num}/process`  |
| service | Name of the service handling the request. | N/A |
| http.client.name | Name of the client application that sent the request. | N/A |
| http.client.address | Address of the client application instance (host name or IP address).  | N/A |
| http.request.method | Request method (e.g. `GET`, `POST`, `PUT`, etc). | N/A |
| http.request.url | Request URL without query parameters.  | N/A |
| http.request.size | Request body size in bytes. | N/A |
| http.response.code | Response code (e.g. `200` or `404`). | N/A |
| http.response.size | Response body size in bytes. | N/A |

### Database

Perform query to database

| Name | Description | Default value |
|----|-----|----|
| kind |  | db-request |
| operation | Database operation name. For example, reading data from table X, execute stored procedure Y, insert data to table Z, etc  |  For example, Insert data to [dbo].[Tasks], Execute stored procedure [UpdateData] |
| db.type | Database type (mssql, cassandra, red, etc) |  |
| db.executionResult | Result of performing request to database |  |
| db.instance | (Usefull for MS SQL) Instanse database server |  |

### Queue (producer)

Insert task to queue

| Name | Description | Default value |
|----|-----|----|
| kind |  | queue-producer |
| operation |  | ({queue.type}) Put to {queue.topic}. Example, (Echelon) Put to reports |
| queue.type | Queue type (Echelon, RabbitMQ etc) |  |
| queue.topic | Name of type, topic, thread of queue, which inserted task  |  |
| queue.actionResult | Result of action (in this case, insert to queue) |  |
| queue.taskId | Recieved task id |  |
| queue.taskTraceId | Recieved task traceid |  |

### Queue (task-lifecycle)

Root span for processing task. Contains general task description

| Name | Description | Default value |
|----|-----|----|
| kind |  | queue-task-lifecycle |
| queue.type | Queue type (Echelon, RabbitMQ etc) |  |
| queue.topic | Name of type, topic, thread of queue |  |
| queue.taskId | Processing task id |  |

### Queue (task-lifecycle-event)

Describes event by processing task. Usually with empty duration

| Name | Description | Default value |
|----|-----|----|
| kind |  | queue-task-lifecycle-event |
| operation | Operation name (put task to queue, pass task to consumer, prolong, delete etc) |  |
| queue.source.traceId | process TraceId  that trigger the event |  |
| queue.source.spanId | process SpanId  that trigger the event |  |

### Queue (manager)

Change state task operation

| Name | Description | Default value |
|----|-----|----|
| kind |  | queue-manager |
| operation | Operation name (delete, prolong, etc) |  |
| queue.type | Queue type (Echelon, RabbitMQ etc) |  |
| queue.topic | Name of type, topic, thread of queue  |  |
| queue.taskId | Processing task id |  |
| queue.actionResult |  Result of action |  |

### Queue (consumer)

Describes processing task operation

| Name | Description | Default value |
|----|-----|----|
| kind |  | queue-consumer |
| queue.executionResult | Result of processing |  |

### Business logic

Describes the execution of part of the application. 



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
