FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build-env
WORKDIR /App
COPY . ./

RUN dotnet publish Arrowgene.Ddon.Cli /p:Version=1.0.0.0 /p:DebugType=None /p:DebugSymbols=false --self-contained false -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:10.0
#RUN apt-get update && apt-get install -y apt-transport-https && rm -rf /var/lib/apt/lists/*

# Database
EXPOSE 3306/tcp
# Game server
EXPOSE 52000/tcp
# Web server
EXPOSE 52099/tcp
# Login server
EXPOSE 52100/tcp
ENV DOTNET_EnableDiagnostics=0

WORKDIR /var/ddon/server
COPY --from=build-env /App/out .
RUN chown -R 10001:10001 .
USER 10001:10001

CMD ["dotnet", "/var/ddon/server/Arrowgene.Ddon.Cli.dll", "server", "start", "--service"]
