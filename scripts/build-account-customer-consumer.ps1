Write-Host "> docker build Account.CustomerConsumer:"
docker build ../ -f ../src/Quantum.Account.CustomerConsumer/Dockerfile -t quantum-account-customer-consumer:1.0.1