# Etap build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .

RUN dotnet restore Src/EAOR.UI/EAOR.UI.csproj

RUN dotnet build Src/EAOR.UI/EAOR.UI.csproj -c Release -o /app/build

RUN dotnet publish Src/EAOR.UI/EAOR.UI.csproj -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
ENV ASPNETCORE_HTTP_PORTS=5001
EXPOSE 5001
WORKDIR /App
COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "EAOR.UI.dll"]
