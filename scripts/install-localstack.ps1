helm repo add localstack-repo https://helm.localstack.cloud
helm upgrade --install localstack localstack-repo/localstack `
  --set service.type=ClusterIP `
  --set service.externalServicePorts.start=4510 `
  --set service.externalServicePorts.end=4510