FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Quantum.Account.Api/Quantum.Account.Api.csproj", "Quantum.Account.Api/"]
COPY ["src/Quantum.Lib.AspNet/Quantum.Lib.AspNet.csproj", "Quantum.Lib.AspNet/"]
COPY ["src/Quantum.Lib.Common/Quantum.Lib.Common.csproj", "Quantum.Lib.Common/"]
COPY ["src/Quantum.Lib.DynamoDb/Quantum.Lib.DynamoDb.csproj", "Quantum.Lib.DynamoDb/"]
RUN dotnet restore "Quantum.Account.Api/Quantum.Account.Api.csproj"
COPY src/ .
RUN dotnet publish "Quantum.Account.Api/Quantum.Account.Api.csproj" -c Release -o /app /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Quantum.Account.Api.dll"]
EXPOSE 80