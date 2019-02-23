FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["Log.Core.Api/Log.Core.Api.csproj", "Log.Core.Api/"]
RUN dotnet restore "Log.Core.Api/Log.Core.Api.csproj"
COPY . .
WORKDIR "/src/Log.Core.Api"
RUN dotnet build "Log.Core.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Log.Core.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Log.Core.Api.dll"]