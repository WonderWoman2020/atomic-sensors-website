FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env 
WORKDIR /app

COPY . . 
RUN dotnet restore AtomicSensors/AtomicSensors.csproj
RUN dotnet publish -c Release -o out AtomicSensors/AtomicSensors.csproj

FROM mcr.microsoft.com/dotnet/aspnet:6.0 
WORKDIR /app 
COPY --from=build-env /app/out . 

HEALTHCHECK --interval=30s --timeout=30s --start-period=20s --retries=3 CMD [ "curl", "-f", "http://localhost" ]

ENTRYPOINT [ "dotnet", "AtomicSensors.dll" ]