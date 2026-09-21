FROM mcr.microsoft.com/dotnet/sdk:10.0
WORKDIR /app

COPY src/ .
RUN dotnet restore GymTracker.Api/GymTracker.Api.csproj
RUN dotnet publish GymTracker.Api/GymTracker.Api.csproj -c Release -o /app/publish

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "/app/publish/GymTracker.Api.dll"]