**ARCHIVED**
===================

This repository was archived since [NineChronicles.Headless](https://github.com/planetarium/NineChronicles.Headless) started to provide their states as GraphQL API, which provides more better API with strong-typed schema, to guide users to the best way to use states.

# NineChronicles.HttpGateway

![dotnet-core-version](https://img.shields.io/badge/dotnet-3.1-blue)

An application to provide HTTP API from NineChronicles RPC server.


<!-- FIXME: I'm not sure what's better between Dependencies and Installation
     about this section -->

## Dependencies

It imports [planetarium/NineChronicles.RPC.Shared] repository's code as git submodule 
because it must use RPC client created from code shared with NineChronicles RPC server.
 
```bash
$ git clone --recurse-submodules git@github.com:<username>/<repository>
```

[planetarium/NineChronicles.RPC.Shared]: https://github.com/planetarium/NineChronicles.RPC.Shared


## Build

```bash
$ dotnet build
```


## Run

```bash 
$ dotnet run -- /rpcServerHost=<string> /rpcServerPort=<short>

$ dotnet run -- /rpcServerHost=aafd1af9c5cf111ea824802399f8ed0e-1178879563.ap-northeast-2.elb.amazonaws.com /rpcServerPort=31234
```


## Docker Build

```bash
$ docker build .
```

