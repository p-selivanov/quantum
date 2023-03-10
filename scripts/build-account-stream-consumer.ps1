Write-Host "> dotnet build Account.StreamConsumerLambda:"
dotnet build ../src/Quantum.Account.StreamConsumerLambda/Quantum.Account.StreamConsumerLambda.csproj `
  --runtime linux-x64 `
  --no-self-contained `
  --output ../src/Quantum.Account.StreamConsumerLambda/bin/aws

Write-Host "> zip Account.StreamConsumerLambda:"
Compress-Archive `
  -Path ../src/Quantum.Account.StreamConsumerLambda/bin/aws/* `
  -DestinationPath ../src/Quantum.Account.StreamConsumerLambda/bin/function.zip `
  -Force
Write-Host "ok"