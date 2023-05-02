FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TestTcpServer/TestTcpServer.csproj", "TestTcpServer/"]
RUN dotnet restore "TestTcpServer/TestTcpServer.csproj"
COPY . .
WORKDIR "/src/TestTcpServer"
RUN dotnet build "TestTcpServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TestTcpServer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TestTcpServer.dll"]
