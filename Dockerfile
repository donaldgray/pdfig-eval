FROM mcr.microsoft.com/dotnet/sdk:5.0.102-ca-patch-buster-slim-amd64 AS build
WORKDIR /src

COPY . .
RUN dotnet publish "PdfTest/PdfTest.csproj" -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT [ "dotnet", "PdfTest.dll" ]