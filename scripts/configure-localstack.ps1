Write-Host "> DynamoDB create table Customers:"
kubectl exec -it deploy/localstack -- `
  awslocal dynamodb create-table `
  --table-name Customers1 `
  --attribute-definitions AttributeName=Id,AttributeType=S `
  --key-schema AttributeName=Id,KeyType=HASH `
  --provisioned-throughput ReadCapacityUnits=100,WriteCapacityUnits=100 `
  --stream-specification StreamEnabled=true,StreamViewType=NEW_AND_OLD_IMAGES