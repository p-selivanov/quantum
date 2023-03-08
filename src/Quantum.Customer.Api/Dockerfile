FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Quantum.Customer.Api/Quantum.Customer.Api.csproj", "Quantum.Customer.Api/"]
COPY ["src/Quantum.Lib.AspNet/Quantum.Lib.AspNet.csproj", "Quantum.Lib.AspNet/"]
COPY ["src/Quantum.Lib.Common/Quantum.Lib.Common.csproj", "Quantum.Lib.Common/"]
COPY ["src/Quantum.Lib.DynamoDb/Quantum.Lib.DynamoDb.csproj", "Quantum.Lib.DynamoDb/"]
RUN dotnet restore "Quantum.Customer.Api/Quantum.Customer.Api.csproj"
COPY src/ .
RUN dotnet publish "Quantum.Customer.Api/Quantum.Customer.Api.csproj" -c Release -o /app /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:7.0 as final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Quantum.Customer.Api.dll"]
EXPOSE 80