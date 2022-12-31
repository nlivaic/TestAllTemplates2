#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/TestAllPipelines2.WorkerServices/TestAllPipelines2.WorkerServices.csproj", "src/TestAllPipelines2.WorkerServices/"]
COPY ["src/TestAllPipelines2.Core/TestAllPipelines2.Core.csproj", "src/TestAllPipelines2.Core/"]
COPY ["src/TestAllPipelines2.Common/TestAllPipelines2.Common.csproj", "src/TestAllPipelines2.Common/"]
RUN dotnet restore "src/TestAllPipelines2.WorkerServices/TestAllPipelines2.WorkerServices.csproj"
COPY . .
WORKDIR "/src/src/TestAllPipelines2.WorkerServices"
RUN dotnet build "TestAllPipelines2.WorkerServices.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TestAllPipelines2.WorkerServices.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ["src/TestAllPipelines2.WorkerServices/entrypoint.sh", "src/entrypoint.sh"]
COPY ["src/TestAllPipelines2.WorkerServices/wait-for-it.sh", "src/wait-for-it.sh"]
CMD ["/bin/bash", "src/entrypoint.sh"]
