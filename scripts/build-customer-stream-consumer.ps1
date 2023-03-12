Write-Host "> dotnet build Customer.StreamConsumerLambda:"
dotnet build ../src/Customer.StreamConsumerLambda/Customer.StreamConsumerLambda.csproj `
  --runtime linux-x64 `
  --no-self-contained `
  --output ../src/Customer.StreamConsumerLambda/bin/aws

Write-Host "> zip Customer.StreamConsumerLambda:"
Compress-Archive `
  -Path ../src/Customer.StreamConsumerLambda/bin/aws/* `
  -DestinationPath ../src/Customer.StreamConsumerLambda/bin/function.zip `
  -Force
Write-Host "ok"