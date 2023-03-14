Write-Host "> dotnet build Account.StreamConsumerLambda:"
dotnet build ../src/Account.StreamConsumerLambda/Account.StreamConsumerLambda.csproj `
  --runtime linux-x64 `
  --no-self-contained `
  --output ../src/Account.StreamConsumerLambda/bin/aws

rm ../src/Account.StreamConsumerLambda/bin/aws/librdkafka.so
rm ../src/Account.StreamConsumerLambda/bin/aws/centos6-librdkafka.so
rm ../src/Account.StreamConsumerLambda/bin/aws/alpine-librdkafka.so

Write-Host "> zip Account.StreamConsumerLambda:"
Compress-Archive `
  -Path ../src/Account.StreamConsumerLambda/bin/aws/* `
  -DestinationPath ../src/Account.StreamConsumerLambda/bin/function.zip `
  -Force
Write-Host "ok"