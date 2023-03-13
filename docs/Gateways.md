## Customer Gateway

GET   /api/profile
POST  /api/profile
PATCH /api/profile
GET   /api/accounts
GET   /api/accounts/:currency
GET   /api/accounts/:currency/transactions
POST  /api/deposits
POST  /api/withdrawals

## Manager Gateway

GET   /api/customers/:customerId
POST  /api/customers
PATCH /api/customers
GET   /api/customers/:customerId/accounts
GET   /api/customers/:customerId/accounts/:currency
GET   /api/customers/:customerId/accounts/:currency/transactions
GET   /api/accounts