Write-Host "> aws update-function-code account-stream-consumer-lambda:"
awslocal lambda update-function-code `
  --function-name account-stream-consumer-lambda `
  --zip-file fileb://../src/Quantum.Account.StreamConsumerLambda/bin/function.zip
