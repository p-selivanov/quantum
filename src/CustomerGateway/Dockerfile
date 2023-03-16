FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/CustomerGateway/CustomerGateway.csproj", "CustomerGateway/"]
RUN dotnet restore "CustomerGateway/CustomerGateway.csproj"
COPY ./src/CustomerGateway ./CustomerGateway
RUN dotnet publish "CustomerGateway/CustomerGateway.csproj" -c Release -r linux-x64 --no-self-contained -o /app

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "CustomerGateway.dll"]
EXPOSE 80