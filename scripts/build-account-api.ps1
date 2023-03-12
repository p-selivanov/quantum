Write-Host "> docker build Account.Api:"
docker build ../ -f ../src/Account.Api/Dockerfile -t quantum-account-api:1.0.1