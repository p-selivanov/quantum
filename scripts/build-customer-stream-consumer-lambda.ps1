Write-Host "> dotnet build Customer.StreamConsumerLambda:"
dotnet build ../src/Quantum.Customer.StreamConsumerLambda/Quantum.Customer.StreamConsumerLambda.csproj `
  --runtime linux-x64 `
  --no-self-contained `
  --output ../src/Quantum.Customer.StreamConsumerLambda/bin/aws

Write-Host "> zip StreamConsumerLambda:"
Compress-Archive `
  -Path ../src/Quantum.Customer.StreamConsumerLambda/bin/aws/* `
  -DestinationPath ../src/Quantum.Customer.StreamConsumerLambda/bin/function.zip `
  -Force
Write-Host "ok"