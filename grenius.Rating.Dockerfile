FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["grenius-rating/grenius-rating.csproj", "grenius-rating/"]
COPY ["MessageContracts/MessageContracts.csproj", "MessageContracts/"]
RUN dotnet restore "grenius-rating/grenius-rating.csproj"
COPY . .
WORKDIR "/src/grenius-rating"
RUN dotnet build "grenius-rating.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "grenius-rating.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "grenius-rating.dll"]