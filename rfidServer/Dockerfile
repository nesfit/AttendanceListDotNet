FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
EXPOSE 80/tcp
EXPOSE 443/tcp

FROM microsoft/dotnet:2.2-sdk AS build
COPY . /app
WORKDIR /app
RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS runtime
WORKDIR /app
COPY --from=build /app/RfidServer/out/ .
ENTRYPOINT ["dotnet", "RfidServer.dll"]
