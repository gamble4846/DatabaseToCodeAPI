FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["DatabaseToCode.API/DatabaseToCode.API.csproj", "DatabaseToCode.API/"]
RUN dotnet restore "DatabaseToCode.API/DatabaseToCode.API.csproj"
COPY . .
WORKDIR "/src/DatabaseToCode.API"
RUN dotnet build "DatabaseToCode.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "DatabaseToCode.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "DatabaseToCode.API.dll"]
