FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/AccountSearch.Projection/AccountSearch.Projection.csproj", "AccountSearch.Projection/"]
COPY ["src/AccountSearch.Persistence/AccountSearch.Persistence.csproj", "AccountSearch.Persistence/"]
COPY ["src/Lib.Common/Lib.Common.csproj", "Lib.Common/"]
COPY ["src/Lib.Kafka/Lib.Kafka.csproj", "Lib.Kafka/"]
RUN dotnet restore "AccountSearch.Projection/AccountSearch.Projection.csproj"
COPY src/ .
RUN dotnet publish "AccountSearch.Projection/AccountSearch.Projection.csproj" -c Release -o /app /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "AccountSearch.Projection.dll"]