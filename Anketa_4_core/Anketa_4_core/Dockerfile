#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Anketa_4_core/Anketa_4_core.csproj", "Anketa_4_core/"]
RUN dotnet restore "Anketa_4_core/Anketa_4_core.csproj"
COPY . .
WORKDIR "/src/Anketa_4_core"
RUN dotnet build "Anketa_4_core.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Anketa_4_core.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Anketa_4_core.dll"]