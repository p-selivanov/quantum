Write-Host "> dotnet build:"
dotnet build .

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