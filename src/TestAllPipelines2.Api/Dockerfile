#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
# Uncomment if you have other certs this services needs to trust.
#COPY nginx/id-local.crt /usr/local/share/ca-certificates/id-local.crt
#RUN update-ca-certificates
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/TestAllPipelines2.Api/TestAllPipelines2.Api.csproj", "src/TestAllPipelines2.Api/"]
COPY ["src/TestAllPipelines2.Data/TestAllPipelines2.Data.csproj", "src/TestAllPipelines2.Data/"]
COPY ["src/TestAllPipelines2.Common/TestAllPipelines2.Common.csproj", "src/TestAllPipelines2.Common/"]
COPY ["src/TestAllPipelines2.Core/TestAllPipelines2.Core.csproj", "src/TestAllPipelines2.Core/"]
RUN dotnet restore "src/TestAllPipelines2.Api/TestAllPipelines2.Api.csproj"
COPY . .
WORKDIR "/src/src/TestAllPipelines2.Api"
RUN dotnet build "TestAllPipelines2.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TestAllPipelines2.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TestAllPipelines2.Api.dll"]