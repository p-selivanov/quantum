Write-Host "> docker build ManagerGateway:"
docker build ../ -f ../src/ManagerGateway/Dockerfile -t quantum-manager-gateway:1.0.1