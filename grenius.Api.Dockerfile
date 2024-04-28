FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["grenius-api/grenius-api.csproj", "grenius-api/"]
COPY ["MessageContracts/MessageContracts.csproj", "MessageContracts/"]
RUN dotnet restore "grenius-api/grenius-api.csproj"
COPY . .
WORKDIR "/src/grenius-api"
RUN dotnet build "grenius-api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "grenius-api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "grenius-api.dll"]