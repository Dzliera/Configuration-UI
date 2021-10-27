FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Samples/SampleWebApp/SampleWebApp.csproj", "Samples/SampleWebApp/"]
COPY ["ConfigurationUi/ConfigurationUi.csproj", "ConfigurationUi/"]
RUN dotnet restore "Samples/SampleWebApp/SampleWebApp.csproj"
COPY . .
WORKDIR "/src/Samples/SampleWebApp"
RUN dotnet build "SampleWebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SampleWebApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
VOLUME /app/app_data/
ENTRYPOINT ["dotnet", "SampleWebApp.dll"]
