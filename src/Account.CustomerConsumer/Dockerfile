FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Quantum.Account.CustomerConsumer/Quantum.Account.CustomerConsumer.csproj", "Quantum.Account.CustomerConsumer/"]
COPY ["src/Quantum.Lib.Common/Quantum.Lib.Common.csproj", "Quantum.Lib.Common/"]
COPY ["src/Quantum.Lib.DynamoDb/Quantum.Lib.DynamoDb.csproj", "Quantum.Lib.DynamoDb/"]
COPY ["src/Quantum.Lib.Kafka/Quantum.Lib.Kafka.csproj", "Quantum.Lib.Kafka/"]
RUN dotnet restore "Quantum.Account.CustomerConsumer/Quantum.Account.CustomerConsumer.csproj"
COPY src/ .
RUN dotnet publish "Quantum.Account.CustomerConsumer/Quantum.Account.CustomerConsumer.csproj" -c Release -o /app /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Quantum.Account.CustomerConsumer.dll"]