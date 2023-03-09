helm repo add localstack-repo https://helm.localstack.cloud
helm upgrade --install localstack localstack-repo/localstack `
  --set service.type=ClusterIP `
  --set service.externalServicePorts.start=4510 `
  --set service.externalServicePorts.end=4510 `
  --set lambdaExecutor=docker `
  --set mountDind.enabled=true

helm repo add bitnami-repo https://charts.bitnami.com/bitnami
helm upgrade --install kafka bitnami-repo/kafka `
  --set persistence.enabled=false

helm repo add kafka-ui-repo https://provectus.github.io/kafka-ui
helm upgrade --install kafka-ui kafka-ui-repo/kafka-ui `
  --set envs.config.KAFKA_CLUSTERS_0_NAME=local `
  --set envs.config.KAFKA_CLUSTERS_0_BOOTSTRAPSERVERS=kafka:9092
