#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["RecordManagment.API/RecordManagment.API.csproj", "RecordManagment.API/"]
COPY ["RecordManagment.BL/RecordManagment.BLL.csproj", "RecordManagment.BL/"]
COPY ["RecordManagment.DAL/RecordManagment.DAL.csproj", "RecordManagment.DAL/"]
RUN dotnet restore "RecordManagment.API/RecordManagment.API.csproj"
COPY . .
WORKDIR "/src/RecordManagment.API"
RUN dotnet build "RecordManagment.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RecordManagment.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RecordManagment.API.dll"]