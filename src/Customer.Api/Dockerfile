FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Customer.Api/Customer.Api.csproj", "Customer.Api/"]
COPY ["src/Lib.AspNet/Lib.AspNet.csproj", "Lib.AspNet/"]
COPY ["src/Lib.Common/Lib.Common.csproj", "Lib.Common/"]
COPY ["src/Lib.DynamoDb/Lib.DynamoDb.csproj", "Lib.DynamoDb/"]
RUN dotnet restore "Customer.Api/Customer.Api.csproj"
COPY src/ .
RUN dotnet publish "Customer.Api/Customer.Api.csproj" -c Release -o /app /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:7.0 as final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Customer.Api.dll"]
EXPOSE 80