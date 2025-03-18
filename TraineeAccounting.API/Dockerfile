FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app
COPY . .

RUN dotnet restore

RUN dotnet publish TraineeAccounting.API/TraineeAccounting.API.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "TraineeAccounting.API.dll"]

EXPOSE 80