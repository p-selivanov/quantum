Write-Host "> aws dynamodb create-table AccountTransactions:"
awslocal dynamodb create-table `
  --table-name AccountTransactions `
  --attribute-definitions AttributeName=CustomerId,AttributeType=S AttributeName=TransactionId,AttributeType=S `
  --key-schema AttributeName=CustomerId,KeyType=HASH AttributeName=TransactionId,KeyType=RANGE `
  --provisioned-throughput ReadCapacityUnits=100,WriteCapacityUnits=100 `
  --stream-specification StreamEnabled=true,StreamViewType=NEW_AND_OLD_IMAGES

Write-Host "> helm install account-api:"
helm upgrade --install quantum-account-api ../helm/account-api