## AWS Commands
```
awslocal lambda invoke `
  --function-name customer-stream-consumer `
  --cli-binary-format raw-in-base64-out `
  --payload '{"name": "Bob"}' `
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
