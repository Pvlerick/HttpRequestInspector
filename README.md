# HTTP Request Inspector

[![Build status](https://ci.appveyor.com/api/projects/status/id0ob1757ut2ierw?svg=true)](https://ci.appveyor.com/project/Pvlerick/httprequestinspector)

A CLI tool to inspect HTTP requests.

This tool starts a Kestrel server that listen for HTTP requests on the given port and prints their content in the console.
It has very limited functionalities, but I found if to be the easiest to quickly debug applications that are issuing HTTP requests.

To use HTTPS, you will need to use the ```dotnet dev-certs https --trust``` first.

If you are looking for something more flexible in terms of responses, have a look at the [mountebank](http://www.mbtest.org/) project.

## Installation & Update

```dotnet tool <install|update> -g HttpRequestInspector```

## Usage

```hri [-p <port>] [-u] [-s <status-code>] [-r <path>]```

### Options

Long | Short | Arg | Description
--- | --- | --- | ---
--port | -p | &lt;port&gt; | Specifies the port to listen on. Default is 5000.
--unsecure | -u | N/A | Listen for HTTP requests instead of HTTPS.
--response-status-code | -s | &lt;status-code&gt; | The status code to return. Default is 200.
--response-content | -r | &lt;content&gt; | TODO - the content of the response to be returned. Default response is empty.
--response-content-file | | &lt;path&gt; | TODO - the content of the response to be returned, read from a file. This file must be UTF-8 encoded. Default response is empty.
--help | | | Display help.

## Example Output
