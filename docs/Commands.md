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
docker build . -f ./src/Quantum.Account.Api/Dockerfile -t quantum-account-api:1.0.1
docker build . -f ./src/Quantum.Account.CustomerConsumer/Dockerfile -t quantum-account-customer-consumer:1.0.1
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

## Minikube
```
minikube start
minikube stop
minikube addons enable ingress
minikube addons enable metrics-server
minikube dashboard
minikube tunnel

minikube image load quantum-customer-api:1.0.1
minikube image load quantum-account-api:1.0.1
minikube image load quantum-account-customer-consumer:1.0.1


kubectl port-forward service/localstack 4566:4566
kubectl port-forward service/kafka-ui 9080:80

```

## Helm
```
helm upgrade --install quantum ./helm
helm uninstall quantum
helm list

helm repo add localstack-repo https://helm.localstack.cloud
helm search repo localstack-repo

helm upgrade --install localstack localstack-repo/localstack `
  --set debug=false,lambdaExecutor=docker-reuse,ingress.enabled=true

helm repo add bitnami-repo https://charts.bitnami.com/bitnami
helm upgrade --install kafka bitnami-repo/kafka

helm repo add kafka-ui-repo https://provectus.github.io/kafka-ui
helm upgrade --install kafka-ui kafka-ui-repo/kafka-ui `
  --set envs.config.KAFKA_CLUSTERS_0_NAME=local `
  --set envs.config.KAFKA_CLUSTERS_0_BOOTSTRAPSERVERS=kafka:9092
```

```
helm upgrade --install quantum-customer-api ./helm/customer-api
helm upgrade --install quantum-account-api ./helm/account-api
helm upgrade --install quantum-account-customer-consumer ./helm/account-customer-consumer
helm upgrade --install quantum-ingress ./helm/ingress
```