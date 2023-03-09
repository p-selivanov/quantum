Write-Host "> kafka create topic customer-events:"
kubectl exec kafka-0 -- `
  /opt/bitnami/kafka/bin/kafka-topics.sh --create --topic customer-events --partitions 10 --replication-factor 1 --bootstrap-server localhost:9092

Write-Host "> aws create-function customer-stream-consumer-lambda:"
awslocal lambda create-function `
  --function-name customer-stream-consumer-lambda `
  --zip-file fileb://../src/Quantum.Customer.StreamConsumerLambda/bin/function.zip `
  --handler Quantum.Customer.StreamConsumerLambda::Quantum.Customer.StreamConsumerLambda.Function::FunctionHandler `
  --runtime dotnet6 `
  --role arn:aws:iam::000000000000:role/lambda-role `
  --environment "Variables={Kafka__BootstrapServers=kafka:9094}"

Write-Host "> aws create-event-source-mapping for customer-stream-consumer-lambda:"
$customerTable = awslocal dynamodb describe-table --table-name Customers | ConvertFrom-Json
awslocal lambda create-event-source-mapping `
  --function-name customer-stream-consumer-lambda `
  --event-source $customerTable.Table.LatestStreamArn `
  --batch-size 1 `
  --starting-position TRIM_HORIZON