#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
RUN dotnet dev-certs https --trust -ep /https/aspnetapp.pfx -p 5843184393c07dfa596f7d233e49fdebcdeb4c3cae440d5e19a1a3cc3f544ca8
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["IdentityApi/IdentityApi.csproj", "IdentityApi/"]
COPY ["Extensions/Extensions.csproj", "Extensions/"]
COPY ["ResponsePackage/ResponsePackage.csproj", "ResponsePackage/"]
RUN dotnet restore "./IdentityApi/./IdentityApi.csproj"
COPY . .
WORKDIR "/src/IdentityApi"
RUN dotnet build "./IdentityApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./IdentityApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
COPY --chmod=0755 --from=build /https/* /https/
ENV ASPNETCORE_ENVIRONMENT=Production
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IdentityApi.dll"]