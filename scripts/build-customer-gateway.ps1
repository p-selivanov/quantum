Write-Host "> docker build CustomerGateway:"
docker build ../ -f ../src/CustomerGateway/Dockerfile -t quantum-customer-gateway:1.0.1