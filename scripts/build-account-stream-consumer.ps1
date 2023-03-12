Write-Host "> dotnet build Account.StreamConsumerLambda:"
dotnet build ../src/Account.StreamConsumerLambda/Account.StreamConsumerLambda.csproj `
  --runtime linux-x64 `
  --no-self-contained `
  --output ../src/Account.StreamConsumerLambda/bin/aws

Write-Host "> zip Account.StreamConsumerLambda:"
Compress-Archive `
  -Path ../src/Account.StreamConsumerLambda/bin/aws/* `
  -DestinationPath ../src/Account.StreamConsumerLambda/bin/function.zip `
  -Force
Write-Host "ok"