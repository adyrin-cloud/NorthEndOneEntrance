# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["NorthEndOneEntrance.csproj", "./"]
RUN dotnet restore "NorthEndOneEntrance.csproj"

# Copy everything else and build
COPY . .
RUN dotnet build "NorthEndOneEntrance.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "NorthEndOneEntrance.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage - serve with nginx
FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html

# Copy published files
COPY --from=publish /app/publish/wwwroot .

# Copy custom nginx configuration
COPY nginx.conf /etc/nginx/nginx.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
