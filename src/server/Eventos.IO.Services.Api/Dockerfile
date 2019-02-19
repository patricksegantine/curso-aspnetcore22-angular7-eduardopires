FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["src/Eventos.IO.Services.Api/Eventos.IO.Services.Api.csproj", "src/Eventos.IO.Services.Api/"]
RUN dotnet restore "src/Eventos.IO.Services.Api/Eventos.IO.Services.Api.csproj"
COPY . .
WORKDIR "/src/src/Eventos.IO.Services.Api"
RUN dotnet build "Eventos.IO.Services.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Eventos.IO.Services.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Eventos.IO.Services.Api.dll"]