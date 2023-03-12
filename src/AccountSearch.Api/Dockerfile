FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Quantum.AccountSearch.Api/Quantum.AccountSearch.Api.csproj", "Quantum.AccountSearch.Api/"]
COPY ["src/Quantum.Lib.AspNet/Quantum.Lib.AspNet.csproj", "Quantum.Lib.AspNet/"]
COPY ["src/Quantum.Lib.Common/Quantum.Lib.Common.csproj", "Quantum.Lib.Common/"]
RUN dotnet restore "Quantum.AccountSearch.Api/Quantum.AccountSearch.Api.csproj"
COPY src/ .
RUN dotnet publish "Quantum.AccountSearch.Api/Quantum.AccountSearch.Api.csproj" -c Release -o /app /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Quantum.AccountSearch.Api.dll"]
EXPOSE 80