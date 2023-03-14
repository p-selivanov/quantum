Write-Host "> dotnet build Customer.StreamConsumerLambda:"
dotnet build ../src/Customer.StreamConsumerLambda/Customer.StreamConsumerLambda.csproj `
  --runtime linux-x64 `
  --no-self-contained `
  --output ../src/Customer.StreamConsumerLambda/bin/aws

rm ../src/Customer.StreamConsumerLambda/bin/aws/librdkafka.so
rm ../src/Customer.StreamConsumerLambda/bin/aws/centos6-librdkafka.so
rm ../src/Customer.StreamConsumerLambda/bin/aws/alpine-librdkafka.so

Write-Host "> zip Customer.StreamConsumerLambda:"
Compress-Archive `
  -Path ../src/Customer.StreamConsumerLambda/bin/aws/* `
  -DestinationPath ../src/Customer.StreamConsumerLambda/bin/function.zip `
  -Force
Write-Host "ok"