# InstanceMetadataDemo

This is an app demonstrating how to retrieve Instance Metadata from within an AWS EC2 Instance. The Metadata will be output as JSON. It uses the AWS SDK to enumerate through Metadata paths and retrieve values, which it recursively adds to a Dictionary. The populated Dictionary is then serialised into a Json string.

This app _must_ be executed from within an AWS EC2 Instance - it will return no data otherwise. EC2 Instance Metadata is not available remotely.

To use this app, execute it from the shell and follow the instructions. When prompted, you can either enter a blank line to retrieve all Metadata, or you can specify a path to retrieve a particular key, for example:

```
ami-id
```
or
```
network/interfaces
```

# Pre-requisites
.Net 7 Runtime (dotnet-runtime-7.0)
