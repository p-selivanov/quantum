#!/bin/bash

awslocal dynamodb create-table \
  --table-name Customers \
  --attribute-definitions AttributeName=Id,AttributeType=S \
  --key-schema AttributeName=Id,KeyType=HASH \
  --provisioned-throughput ReadCapacityUnits=100,WriteCapacityUnits=100