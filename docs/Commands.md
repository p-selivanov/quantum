## AWS Commands
```
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

awslocal lambda list-functions --endpoint-url http://192.168.49.2:31566
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

minikube start --extra-config=apiserver.service-node-port-range=4566

minikube start --extra-config=apiserver.service-node-port-range=31560-31567 --ports=127.0.0.1:31560-31567:31560-31567

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