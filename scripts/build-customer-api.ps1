Write-Host "> docker build Customer.Api:"
docker build ../ -f ../src/Customer.Api/Dockerfile -t quantum-customer-api:1.0.1