FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

COPY . ./

RUN dotnet publish DeployToProduction.WeatherForecast.App --configuration Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0
EXPOSE 80

COPY --from=build /out .

ENTRYPOINT ["dotnet", "DeployToProduction.WeatherForecast.App.dll"]