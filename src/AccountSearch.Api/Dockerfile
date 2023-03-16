FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/AccountSearch.Api/AccountSearch.Api.csproj", "AccountSearch.Api/"]
COPY ["src/Lib.AspNet/Lib.AspNet.csproj", "Lib.AspNet/"]
COPY ["src/Lib.Common/Lib.Common.csproj", "Lib.Common/"]
RUN dotnet restore "AccountSearch.Api/AccountSearch.Api.csproj"
COPY src/ .
RUN dotnet publish "AccountSearch.Api/AccountSearch.Api.csproj" -c Release -r linux-x64 --no-self-contained -o /app

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "AccountSearch.Api.dll"]
EXPOSE 80