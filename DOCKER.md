# Docker Deployment

## Quick Start

### Build and Run with Docker Compose

```bash
docker-compose up -d
```

The application will be available at: http://localhost:8080

### Stop the Application

```bash
docker-compose down
```

## Manual Docker Commands

### Build the Image

```bash
docker build -t northend-checkin:latest .
```

### Run the Container

```bash
docker run -d -p 8080:80 --name northend-one-entrance northend-checkin:latest
```

### Stop and Remove the Container

```bash
docker stop northend-one-entrance
docker remove northend-one-entrance
```

## Architecture

- **Build Stage**: Uses .NET 8.0 SDK to build and publish the Blazor WebAssembly app
- **Runtime Stage**: Uses nginx:alpine to serve the static files
- **Port**: Exposed on port 80 (mapped to 8080 on host)

## Customization

### Change Port

Edit `docker-compose.yml`:

```yaml
ports:
  - "3000:80"  # Change 3000 to your desired port
```

### Environment Variables

Add any required environment variables in `docker-compose.yml`:

```yaml
environment:
  - ASPNETCORE_ENVIRONMENT=Production
  - YOUR_CUSTOM_VAR=value
```

## Production Deployment

For production, consider:

1. Use a reverse proxy (nginx, Traefik) for HTTPS
2. Set proper environment variables
3. Configure proper logging and monitoring
4. Use Docker secrets for sensitive data
5. Enable HTTPS in nginx configuration

## Troubleshooting

### View Logs

```bash
docker-compose logs -f
```

### Access Container Shell

```bash
docker exec -it northend-one-entrance sh
```

### Rebuild After Changes

```bash
docker-compose down
docker-compose build --no-cache
docker-compose up -d
```
