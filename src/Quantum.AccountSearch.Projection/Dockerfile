FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Quantum.AccountSearch.Projection/Quantum.AccountSearch.Projection.csproj", "Quantum.AccountSearch.Projection/"]
COPY ["src/Quantum.AccountSearch.Persistence/Quantum.AccountSearch.Persistence.csproj", "Quantum.AccountSearch.Persistence/"]
COPY ["src/Quantum.Lib.Common/Quantum.Lib.Common.csproj", "Quantum.Lib.Common/"]
COPY ["src/Quantum.Lib.Kafka/Quantum.Lib.Kafka.csproj", "Quantum.Lib.Kafka/"]
RUN dotnet restore "Quantum.AccountSearch.Projection/Quantum.AccountSearch.Projection.csproj"
COPY src/ .
RUN dotnet publish "Quantum.AccountSearch.Projection/Quantum.AccountSearch.Projection.csproj" -c Release -o /app /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Quantum.AccountSearch.Projection.dll"]