Write-Host "> aws update-function-code customer-stream-consumer-lambda:"
awslocal lambda update-function-code `
  --function-name customer-stream-consumer-lambda `
  --zip-file fileb://../src/Customer.StreamConsumerLambda/bin/function.zip `
  --query 'LastUpdateStatusReasonCode' `
  --output text
