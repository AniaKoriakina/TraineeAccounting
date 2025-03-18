FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app
COPY . .
RUN dotnet publish -c Release -o out

FROM nginx:alpine AS runtime
WORKDIR /usr/share/nginx/html
COPY --from=build /app/out/wwwroot .

COPY nginx.conf /etc/nginx/nginx.conf

EXPOSE 80