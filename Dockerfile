FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS backend-base

WORKDIR /app

EXPOSE 8000

RUN apt-get update \
 && apt-get install -y --allow-unauthenticated \
 libc6-dev \
 libgdiplus \
 libx11-dev \
 && rm -rf /var/lib/apt/lists/*

ENV DISPLAY :99
ENV ASPNETCORE_URLS=http://+:8000
ENV DisableHttpsRedirection=true
ENV POSTGRES_PASSWORD=postgres

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS backend-build
WORKDIR /src
COPY ["TODOBack/TODOBack.csproj", "TODOBack/"]

RUN dotnet restore "TODOBack/TODOBack.csproj"
COPY . .

WORKDIR "/src/."

RUN dotnet build "TODOBack/TODOBack.csproj" -c Release -o /app/build

FROM backend-build AS backend-publish
RUN dotnet publish "TODOBack/TODOBack.csproj" -c Release -o /app/publish

FROM backend-base AS backend-final
WORKDIR /app
COPY --from=backend-publish /app/publish .

COPY ["TODOBack/Certificate", "./Certificate"]

ENTRYPOINT ["dotnet", "TODOBack.dll"]

ENV TZ=America/Sao_Paulo
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS frontend-base

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
ENV POSTGRES_PASSWORD=postgres

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS frontend-build
WORKDIR /src
COPY ["TODOFront/TODOFront.csproj", "TODOFront/"]

RUN dotnet restore "TODOFront/TODOFront.csproj"
COPY . .

WORKDIR "/src/."

RUN dotnet build "TODOFront/TODOFront.csproj" -c Release -o /app/build

FROM frontend-build AS frontend-publish
RUN dotnet publish "TODOFront/TODOFront.csproj" -c Release -o /app/publish

FROM frontend-base AS frontend-final
WORKDIR /app
COPY --from=frontend-publish /app/publish .
ENTRYPOINT ["dotnet", "TODOFront.dll"]

ENV TZ=America/Sao_Paulo
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone
