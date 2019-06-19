FROM mcr.microsoft.com/dotnet/core/sdk:2.2-alpine AS build

MAINTAINER Igor Agapie <igoragapie@gmail.com>

WORKDIR /app

EXPOSE 80 443

# copy csproj and restore as distinct layers
COPY src/IA.Finance.Domain/*.csproj ./IA.Finance.Domain/
COPY src/IA.Finance.Infrastructure/*.csproj ./IA.Finance.Infrastructure/
COPY src/IA.Finance.Api/*.csproj ./IA.Finance.Api/
WORKDIR /app/IA.Finance.Api
RUN dotnet restore

# copy and publish app and libraries
WORKDIR /app/
COPY src/IA.Finance.Domain/. ./IA.Finance.Domain/
COPY src/IA.Finance.Infrastructure/. ./IA.Finance.Infrastructure/
COPY src/IA.Finance.Api/. ./IA.Finance.Api/
WORKDIR /app/IA.Finance.Api
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/runtime:2.2-alpine AS runtime

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT false
RUN apk add --no-cache icu-libs

ENV LC_ALL en_US.UTF-8
ENV LANG en_US.UTF-8

WORKDIR /app
COPY --from=build /app/IA.Finance.Api/out ./
ENTRYPOINT ["dotnet", "IA.Finance.Api.dll"]
