FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Account.Api/Account.Api.csproj", "Account.Api/"]
COPY ["src/Lib.AspNet/Lib.AspNet.csproj", "Lib.AspNet/"]
COPY ["src/Lib.Common/Lib.Common.csproj", "Lib.Common/"]
COPY ["src/Lib.DynamoDb/Lib.DynamoDb.csproj", "Lib.DynamoDb/"]
RUN dotnet restore "Account.Api/Account.Api.csproj"
COPY src/ .
RUN dotnet publish "Account.Api/Account.Api.csproj" -c Release -r linux-x64 --no-self-contained -o /app

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Account.Api.dll"]
EXPOSE 80