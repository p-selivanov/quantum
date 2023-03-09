Write-Host "> docker build Customer.Api:"
docker build ../ -f ../src/Quantum.Customer.Api/Dockerfile -t quantum-customer-api:1.0.1

Write-Host "> image load quantum-customer-api:"
minikube image load quantum-customer-api:1.0.1
Write-Host "ok"