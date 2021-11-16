#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["DBTemplateHandler.Studio/DBTemplateHandler.Studio.csproj", "DBTemplateHandler.Studio/"]
COPY ["DBTemplateHandler.Core/DBTemplateHandler.Core.csproj", "DBTemplateHandler.Core/"]
COPY ["DBTemplateHandler.Utils/DBTemplateHandler.Utils.csproj", "DBTemplateHandler.Utils/"]
COPY ["DBTemplateHandler.Service.Contracts/DBTemplateHandler.Service.Contracts.csproj", "DBTemplateHandler.Service.Contracts/"]
COPY ["DBTemplateHandler.Ace.Editor.Tools.Core.Console/DBTemplateHandler.Ace.Editor.Tools.Core.Console.csproj", "DBTemplateHandler.Ace.Editor.Tools.Core.Console/"]
COPY ["DBTemplateHandler.Ace.Editor.Tools.Core/DBTemplateHandler.Ace.Editor.Tools.Core.csproj", "DBTemplateHandler.Ace.Editor.Tools.Core/"]
COPY ["DBTemplateHandler.Persistance/DBTemplateHandler.Persistance.csproj", "DBTemplateHandler.Persistance/"]
COPY ["DBTemplateHandler.Studio.Deployment/DBTemplateHandler.Studio.Deployment.csproj", "DBTemplateHandler.Studio.Deployment/"]
COPY ["DBTemplateHander.DatabaseModel.Import/DBTemplateHander.DatabaseModel.Import.csproj", "DBTemplateHander.DatabaseModel.Import/"]
RUN dotnet restore "DBTemplateHandler.Studio/DBTemplateHandler.Studio.csproj"
COPY . .
WORKDIR "/src/DBTemplateHandler.Studio"
RUN dotnet build "DBTemplateHandler.Studio.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DBTemplateHandler.Studio.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DBTemplateHandler.Studio.dll"]