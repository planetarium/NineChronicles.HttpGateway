FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as build-env

ADD . /src
WORKDIR /src

RUN dotnet restore
RUN dotnet publish -c Release -o /out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1

COPY --from=build-env /out /app
WORKDIR /app

ENTRYPOINT ["dotnet", "NineChronicles.HttpGateway.dll"]
