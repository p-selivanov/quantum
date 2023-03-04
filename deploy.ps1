Write-Host "> DynamoDB create table Customers:"
$response = awslocal dynamodb create-table `
  --table-name Customers `
  --attribute-definitions AttributeName=Id,AttributeType=S `
  --key-schema AttributeName=Id,KeyType=HASH `
  --provisioned-throughput ReadCapacityUnits=100,WriteCapacityUnits=100 `
  --stream-specification StreamEnabled=true,StreamViewType=NEW_AND_OLD_IMAGES
$response
$customerTable = $response | ConvertFrom-Json

Write-Host "> DynamoDB create table AccountTransactions:"
awslocal dynamodb create-table `
  --table-name AccountTransactions `
  --attribute-definitions AttributeName=AccountId,AttributeType=S AttributeName=TransactionId,AttributeType=N `
  --key-schema AttributeName=AccountId,KeyType=HASH AttributeName=TransactionId,KeyType=RANGE `
  --provisioned-throughput ReadCapacityUnits=100,WriteCapacityUnits=100 `
  --stream-specification StreamEnabled=true,StreamViewType=NEW_AND_OLD_IMAGES

Write-Host "> Create function customer-stream-consumer-lambda:"
awslocal lambda create-function `
  --function-name customer-stream-consumer-lambda `
  --zip-file fileb://src/Quantum.Customer.StreamConsumerLambda/bin/function.zip `
  --handler Quantum.Customer.StreamConsumerLambda::Quantum.Customer.StreamConsumerLambda.Function::FunctionHandler `
  --runtime dotnet6 `
  --role arn:aws:iam::000000000000:role/lambda-role

Write-Host "> Create event source mapping for customer-stream-consumer-lambda:"
awslocal lambda create-event-source-mapping `
  --function-name customer-stream-consumer-lambda `
  --event-source $customerTable.TableDescription.LatestStreamArn `
  --batch-size 1 `
  --starting-position TRIM_HORIZON