Push-Location ..

Write-Host "> dotnet build:"
dotnet build .

Write-Host "> docker build Account.Api:"
docker build . -f ./src/Quantum.Account.Api/Dockerfile -t quantum-account-api:1.0.1

Write-Host "> docker build Account.CustomerConsumer:"
docker build . -f ./src/Quantum.Account.CustomerConsumer/Dockerfile -t quantum-account-customer-consumer:1.0.1

Write-Host "> docker build Customer.Api:"
docker build . -f ./src/Quantum.Customer.Api/Dockerfile -t quantum-customer-api:1.0.1

Write-Host "> dotnet build StreamConsumerLambda:"
dotnet build ./src/Quantum.Customer.StreamConsumerLambda/Quantum.Customer.StreamConsumerLambda.csproj `
  --runtime linux-x64 `
  --no-self-contained `
  --output ./src/Quantum.Customer.StreamConsumerLambda/bin/aws

Write-Host "> ZIP StreamConsumerLambda:"
Compress-Archive `
  -Path ./src/Quantum.Customer.StreamConsumerLambda/bin/aws/* `
  -DestinationPath ./src/Quantum.Customer.StreamConsumerLambda/bin/function.zip `
  -Force
Write-Host "OK"

Pop-Location