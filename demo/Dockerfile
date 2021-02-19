FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

COPY ./Orleans.AspNetCore.Prometheus.sln ./Orleans.AspNetCore.Prometheus.sln
COPY ./src/Orleans.AspNetCore.Prometheus/Orleans.AspNetCore.Prometheus.csproj ./src/Orleans.AspNetCore.Prometheus/Orleans.AspNetCore.Prometheus.csproj
COPY ./demo/Demo.Orleans.AspNetCore.Prometheus/Demo.Orleans.AspNetCore.Prometheus.csproj ./demo/Demo.Orleans.AspNetCore.Prometheus/Demo.Orleans.AspNetCore.Prometheus.csproj
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Demo.Orleans.AspNetCore.Prometheus.dll" ]
