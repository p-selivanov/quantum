## Docker Commands
Run only infrastructure (for local dev):
```
docker compose -f compose-infra.yaml --project-name quantum up -d
docker compose --project-name quantum down
```

Run infra and all services:
```
docker compose -f compose-infra.yaml -f ccompose-Quantum.yaml --project-name quantum up -d
docker compose --project-name quantum down
```

Build commands:
```
docker build . -f ./src/Quantum.Customer.Api/Dockerfile -t quantum-customer-api:1.0.1
```


## AWS Commands
```
awslocal iam create-role `
  --role-name dynamo-stream-consumer `
  --assume-role-policy-document '{"Version": "2012-10-17", "Statement": [{ "Effect": "Allow", "Principal": {"Service": "lambda.amazonaws.com"}, "Action": "sts:AssumeRole"}]}'

awslocal iam attach-role-policy `
  --role-name dynamo-stream-consumer `
  --policy-arn arn:aws:iam::aws:policy/service-role/AWSLambdaBasicExecutionRole

awslocal lambda invoke `
  --function-name customer-stream-consumer `
  --cli-binary-format raw-in-base64-out `
  --payload '{"name": "Bob"}' `
  response.json

awslocal lambda invoke `
  --function-name customer-stream-consumer `
  --payload 'eyJuYW1lIjogIkJvYiJ9' `
  response.json

awslocal lambda list-functions

awslocal lambda delete-function --function-name customer-stream-consumer

awslocal dynamodb describe-table --table-name Customers

awslocal dynamodb put-item `
  --table-name Customers `
  --item 'Id={S="104"},EmailAddress={S="bob@mail.dev"},FirstName={S="Bob"},LastName={S="Smith"}'

awslocal dynamodb scan --table-name AccountTransactions

awslocal dynamodb delete-item --table-name AccountTransactions --key 'CustomerId={S="05ffb9a8c6564b59b63682b78c6d6b32"},TransactionId={S="USA"}'
```

## Kafka
./bin/windows/kafka-topics.bat --create --topic test --partitions 10 --replication-factor 1 --bootstrap-server localhost:9092