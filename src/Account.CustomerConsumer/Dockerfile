FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Account.CustomerConsumer/Account.CustomerConsumer.csproj", "Account.CustomerConsumer/"]
COPY ["src/Lib.Common/Lib.Common.csproj", "Lib.Common/"]
COPY ["src/Lib.DynamoDb/Lib.DynamoDb.csproj", "Lib.DynamoDb/"]
COPY ["src/Lib.Kafka/Lib.Kafka.csproj", "Lib.Kafka/"]
RUN dotnet restore "Account.CustomerConsumer/Account.CustomerConsumer.csproj"
COPY src/ .
RUN dotnet publish "Account.CustomerConsumer/Account.CustomerConsumer.csproj" -c Release -r linux-x64 --no-self-contained -o /app

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Account.CustomerConsumer.dll"]