Write-Host "> aws dynamodb create-table Customers:"
awslocal dynamodb create-table `
  --table-name Customers `
  --attribute-definitions AttributeName=Id,AttributeType=S `
  --key-schema AttributeName=Id,KeyType=HASH `
  --provisioned-throughput ReadCapacityUnits=100,WriteCapacityUnits=100 `
  --stream-specification StreamEnabled=true,StreamViewType=NEW_AND_OLD_IMAGES

Write-Host "> helm install customer-api:"
helm upgrade --install quantum-customer-api ../helm/customer-api