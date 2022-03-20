FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim AS base
WORKDIR /app
EXPOSE 9090

FROM mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim AS build
WORKDIR /src
COPY . .
RUN dotnet restore "nacos-csharp-192.sln"
WORKDIR /src/WebApplication1
RUN dotnet build "WebApplication1.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApplication1.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV TZ=Asia/Shanghai

COPY ./wait-for-it.sh /usr/local/
COPY ./init-nacos.sh /usr/local/

CMD [ "/usr/local/wait-for-it.sh", "nacos:8848", "--", "/usr/local/init-nacos.sh" ]
# ENTRYPOINT ["dotnet", "WebApplication1.dll"]
