FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Quantum.Customer.Api/Quantum.Customer.Api.csproj", "src/Quantum.Customer.Api/"]
COPY ["src/Quantum.Customer.Persistence/Quantum.Customer.Persistence.csproj", "src/Quantum.Customer.Persistence/"]
COPY ["src/Quantum.Common/Quantum.Common.csproj", "src/Quantum.Common/"]
COPY ["src/Quantum.Common.AspNet/Quantum.Common.AspNet.csproj", "src/Quantum.Common.AspNet/"]
RUN dotnet restore "src/Quantum.Customer.Api/Quantum.Customer.Api.csproj"
COPY . .
WORKDIR "/src/src/Quantum.Customer.Api"
RUN dotnet publish "Quantum.Customer.Api.csproj" -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Quantum.Customer.Api.dll"]
EXPOSE 80