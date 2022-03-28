# Vostok tracing

[![Build & Test & Publish](https://github.com/vostok/tracing.abstractions/actions/workflows/ci.yml/badge.svg)](https://github.com/vostok/tracing.abstractions/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/Vostok.Tracing.Abstractions.svg)](https://www.nuget.org/packages/Vostok.Tracing.Abstractions/)

Distributed tracing allows to reconstruct the history of the logical operation spanning many applications and machines in time as a tree of smaller actions or events called spans. Spans can represent HTTP requests, database queries or any other significant interactions or events in a distributed system. A single span always describes a local event in a single process: an HTTP request usually produces two spans (client-side and server-side). Each kind of span stores specific information about performed action.

<br/>

* [Span structure](#span-structure)
* [Common annotations](#common-annotations)
* [Kind-specific annotations](#kind-specific-annotations)
  * [HTTP requests](#http-requests)
    * [HTTP client (direct)](#http-client-direct)
    * [HTTP client (cluster)](#http-client-cluster)
    * [HTTP server](#http-server)
  * [Custom (not HTTP) requests](#custom-not-http-requests)
    * [Custom (not HTTP) client (direct)](#custom-not-http-client-direct)
    * [Custom (not HTTP) client (cluster)](#custom-not-http-client-cluster)
  * [Custom operations](#custom-operations)

<br/>

## Span structure

Every span consists of following fields:
* `TraceId` — unique identifier of the trace containing the span (Guid).
  * Gets assigned on first operation (usually on a front-end application instance).
  * Serves as a correlation identifier between spans
  
* `SpanId` — unique identifier of the span itself (Guid).

* `ParentSpanId` — identifier of the parent span in the tree (Guid). 
  * May be absent for root span in the tree.
  
* `BeginTimestamp` — beginning timestamp of the event or interaction described by this span (UTC timestamp + timezone offset).

* `EndTimestamp` — ending timestamp of the event or interaction described by this span (UTC timestamp + timezone offset).
  * Always measured with the same clock as `BeginTimestamp`. This allows to derive span duration as a difference between `EndTimestamp` and `BeginTimestamp`.
  * May be absent for a special kind of 'endless' spans described further.
  
* `Annotations` — payload in key-value format (string --> object). Keys are case-sensitive.

<br/>

## Common annotations

These are the annotations relevant for any span:

| Name | Description |
|----|-----|
| `kind` | Span kind. There are a number of predefined span kinds for common use cases (e.g. `http-request-server`). |
| `operation` | Human-readable logical operation or event name (e.g. `create-user`). |
| `status` | Logical operation or event status (`success`, `error`, or `warning`). Might not have operation-specific values. |
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

| Name | Description | Default value |
|----|-----|----|
| `kind` | See [common annotations](#common-annotations).  | `http-request-client` |
| `http.request.targetService` | Name of the service to which request is sent. | `N/A` |
| `http.request.targetEnvironment` | Name of the environment to which request is sent. | `N/A` |

<br/>

#### HTTP client (cluster)

Submitting an HTTP request to a clustered application with several replicas.

| Name | Description | Default value |
|----|-----|----|
| `kind` | See [common annotations](#common-annotations).  | `http-request-cluster` |
| `http.cluster.strategy` | Name of the strategy used to send request (e.g. `sequential`, `parallel`, ...) | `N/A` |
| `http.cluster.status` | Status of interaction with a cluster (e.g. `success`, `no-replicas`, ...)  | `N/A` |
| `http.request.targetService` | Name of the service to which request is sent. | `N/A` |
| `http.request.targetEnvironment` | Name of the environment to which request is sent. | `N/A` |

<br/>

#### HTTP server

Handling an HTTP request on server.

| Name | Description | Default value |
|----|-----|----|
| `kind` | See [common annotations](#common-annotations). | `http-request-server` |
| `http.client.name` | Name of the client application that sent the request. | `N/A` |
| `http.client.address` | Address of the client application instance (host name or IP address).  | `N/A` |

<br/>
<br/>

### Custom (not HTTP) requests

Common annotations for all spans related to custom requests:

| Name | Description | Default value |
|----|-----|----|
| `operation` | See [common annotations](#common-annotations). | `N/A` |
| `status` | See [common annotations](#common-annotations). | `N/A` |
| `custom.response.status` | Custom request-specific status. | `N/A` |
| `custom.request.size` | Request size in bytes. | `N/A` |
| `custom.response.size` | Response size in bytes. | `N/A` |
| `custom.request.targetService` | Name of the service to which request is sent. | `N/A` |
| `custom.request.targetEnvironment` | Name of the environment to which request is sent. | `N/A` |

<br/>

#### Custom (not HTTP) client (direct)

Submitting custom request directly to a service replica.

| Name | Description | Default value |
|----|-----|----|
| `kind` | See [common annotations](#common-annotations).  | `custom-request-client` |
| `custom.request.replica` | Name of the replica to which request is sent. | `N/A` |

<br/>

#### Custom (not HTTP) client (cluster)

Submitting custom request to a clustered application with several replicas.

| Name | Description | Default value |
|----|-----|----|
| `kind` | See [common annotations](#common-annotations).  | `custom-request-cluster` |

<br/>
<br/>

### Custom operations

Performing custom server operation.

| Name | Description | Default value |
|----|-----|----|
| `kind` | See [common annotations](#common-annotations).  | `custom-operation` |
| `operation` | See [common annotations](#common-annotations). | `N/A` |
| `status` | See [common annotations](#common-annotations). | `N/A` |
| `custom.operation.status` | Custom operation-specific status. | `N/A` |
| `custom.operation.size` | Processed data size in bytes. | `N/A` |
| `custom.operation.targetService` | Name of the service with which this operation is associated. | `{application}` |
| `custom.operation.targetEnvironment` | Name of the environment with which this operation is associated. | `{environment}` |
