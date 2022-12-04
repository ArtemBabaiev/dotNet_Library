#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Agregator/Agregator.csproj", "Agregator/"]
COPY ["Catalog.BLL/Catalog.BLL.csproj", "Catalog.BLL/"]
COPY ["Catalog.DAL/Catalog.DAL.csproj", "Catalog.DAL/"]
COPY ["EventBus.Messages/EventBus.Messages.csproj", "EventBus.Messages/"]
COPY ["RecordManagment.BL/RecordManagment.BLL.csproj", "RecordManagment.BL/"]
COPY ["RecordManagment.DAL/RecordManagment.DAL.csproj", "RecordManagment.DAL/"]
COPY ["WrittenOff.Application/WrittenOffManagement.Application.csproj", "WrittenOff.Application/"]
COPY ["WrittenOff.Domain/WrittenOffManagement.Domain.csproj", "WrittenOff.Domain/"]
RUN dotnet restore "Agregator/Agregator.csproj"
COPY . .
WORKDIR "/src/Agregator"
RUN dotnet build "Agregator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Agregator.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Agregator.dll"]