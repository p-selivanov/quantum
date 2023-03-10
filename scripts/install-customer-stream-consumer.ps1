Write-Host "> aws update-function-code customer-stream-consumer-lambda:"
awslocal lambda update-function-code `
  --function-name customer-stream-consumer-lambda `
  --zip-file fileb://../src/Quantum.Customer.StreamConsumerLambda/bin/function.zip
