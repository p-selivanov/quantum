Write-Host "> docker build Account.CustomerConsumer:"
docker build ../ -f ../src/Quantum.Account.CustomerConsumer/Dockerfile -t quantum-account-customer-consumer:1.0.1

Write-Host "> image load quantum-account-customer-consumer:"
minikube image load quantum-account-customer-consumer:1.0.1
Write-Host "ok"