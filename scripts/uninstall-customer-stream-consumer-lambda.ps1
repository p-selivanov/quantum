Write-Host "> aws delete-event-source-mapping for customer-stream-consumer-lambda:"
awslocal lambda delete-event-source-mapping `
  --function-name customer-stream-consumer-lambda

Write-Host "> aws delete-function customer-stream-consumer-lambda:"
awslocal lambda delete-function `
  --function-name customer-stream-consumer-lambda