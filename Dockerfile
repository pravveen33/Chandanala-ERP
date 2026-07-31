FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build

WORKDIR /src

COPY . .

RUN dotnet restore src/Chandanala.ERP.Web/Chandanala.ERP.Web.csproj

RUN dotnet publish src/Chandanala.ERP.Web/Chandanala.ERP.Web.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview

WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet","Chandanala.ERP.Web.dll"]
