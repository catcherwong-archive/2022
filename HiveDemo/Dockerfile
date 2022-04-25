FROM mcr.microsoft.com/dotnet/runtime:6.0-bullseye-slim AS base

RUN sed -i s@/deb.debian.org/@/mirrors.aliyun.com/@g /etc/apt/sources.list\
    && apt-get update \
    && apt-get install -y --no-install-recommends \
        sasl2-bin \
        libsasl2-modules \
        unixodbc \
        alien \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/* 

COPY ./ClouderaHiveODBC-2.6.11.1011-1.x86_64.rpm /tmp/ClouderaHiveODBC-2.6.11.1011-1.x86_64.rpm
RUN alien /tmp/ClouderaHiveODBC-2.6.11.1011-1.x86_64.rpm
RUN dpkg -i ./clouderahiveodbc_2.6.11.1011-2_amd64.deb && rm -rf /tmp/ClouderaHiveODBC-2.6.11.1011-1.x86_64.rpm ./clouderahiveodbc_2.6.11.1011-2_amd64.deb

COPY ./odbc.ini /etc/odbc.ini
COPY ./odbcinst.ini /etc/odbcinst.ini

FROM mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim AS build
WORKDIR /src
COPY . .
RUN dotnet restore "HiveDemo.sln"

WORKDIR /src/HiveDemo
RUN dotnet build "HiveDemo.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HiveDemo.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HiveDemo.dll"]
