#!/bin/bash

awslocal dynamodb create-table \
  --table-name Customers \
  --attribute-definitions AttributeName=Id,AttributeType=S \
  --key-schema AttributeName=Id,KeyType=HASH \
  --provisioned-throughput ReadCapacityUnits=100,WriteCapacityUnits=100

awslocal dynamodb create-table \
  --table-name AccountTransactions \
  --attribute-definitions AttributeName=AccountId,AttributeType=S AttributeName=TransactionId,AttributeType=N \
  --key-schema AttributeName=AccountId,KeyType=HASH AttributeName=TransactionId,KeyType=RANGE \
  --provisioned-throughput ReadCapacityUnits=100,WriteCapacityUnits=100