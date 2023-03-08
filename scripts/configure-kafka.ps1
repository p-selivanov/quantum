kubectl exec kafka-0 -- `
  /opt/bitnami/kafka/bin/kafka-topics.sh --create --topic test-3 --partitions 10 --replication-factor 1 --bootstrap-server localhost:9092
