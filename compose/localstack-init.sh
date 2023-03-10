#!/bin/bash

awslocal dynamodb create-table \
  --table-name Customers \
  --attribute-definitions AttributeName=Id,AttributeType=S \
  --key-schema AttributeName=Id,KeyType=HASH \
  --provisioned-throughput ReadCapacityUnits=100,WriteCapacityUnits=100 \
  --stream-specification StreamEnabled=true,StreamViewType=NEW_AND_OLD_IMAGES

awslocal dynamodb create-table \
  --table-name AccountTransactions \
  --attribute-definitions AttributeName=CustomerId,AttributeType=S AttributeName=TransactionId,AttributeType=S \
  --key-schema AttributeName=CustomerId,KeyType=HASH AttributeName=TransactionId,KeyType=RANGE \
  --provisioned-throughput ReadCapacityUnits=100,WriteCapacityUnits=100 \
  --stream-specification StreamEnabled=true,StreamViewType=NEW_AND_OLD_IMAGES

touch ./function.txt
zip function.zip function.txt
awslocal lambda create-function \
  --function-name customer-stream-consumer-lambda \
  --zip-file fileb://function.zip \
  --handler Quantum.Customer.StreamConsumerLambda::Quantum.Customer.StreamConsumerLambda.Function::FunctionHandler \
  --runtime dotnet6 \
  --role arn:aws:iam::000000000000:role/lambda-role \
  --environment "Variables={Kafka__BootstrapServers=kafka:9094}"

customerStreamArn=$(awslocal dynamodb describe-table --table-name Customers --query 'Table.LatestStreamArn' --output text)
awslocal lambda create-event-source-mapping \
  --function-name customer-stream-consumer-lambda \
  --event-source $customerStreamArn \
  --batch-size 1 \
  --starting-position TRIM_HORIZON