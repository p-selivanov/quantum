FROM traefik:v2.9 AS final

WORKDIR /etc/traefik
COPY ./src/ManagerGateway/traefik.yaml .
COPY ./src/ManagerGateway/manager-gateway.yaml .