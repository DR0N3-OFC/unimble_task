FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base

WORKDIR /app

EXPOSE 5260

RUN apt-get update \
 && apt-get install -y --allow-unauthenticated \
 libc6-dev \
 libgdiplus \
 libx11-dev \
 && rm -rf /var/lib/apt/lists/*

ENV DISPLAY :99
ENV ASPNETCORE_URLS=http://+:5260
ENV DisableHttpsRedirection=true
ENV POSTGRES_PASSWORD: postgres

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["./TODOFront.csproj", "./"]

RUN dotnet restore "./TODOFront.csproj"
COPY . .

WORKDIR "/src/."

RUN dotnet build "TODOFront.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TODOFront.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TODOFront.dll"]

ENV TZ=America/Sao_Paulo
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone