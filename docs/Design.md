## Design

![Components](Components.png)

## Customer API
Features:
1. Get a customer
2. Create a customer
3. Update a customer

## Account API
Features:
1. Get customer accounts
2. Get customer account transactions
3. Deposit
4. Withdraw

## Account Search API
Features:
1. Search accounts by Email, FirstName, LastName, Currency, Balance

## Account Customer Consumer
Features:
1. Consume Customer events and saveCustomer Status and Country to the `AccountTransactions` table.

## Account Search Projection
Features:
1. Consumes Customer events and builds CustomerAccounts projection table.

## Customer Stream Consumer Lambda
Features:
1. Transalate the `Customers` table change stream to Kafka events.

## Account Stream Consumer Lambda
Features:
1. Transalate the `AccountTransactions` table change stream to Kafka events.

## Customer Gateway
Routes:
- GET   /api/profile
- POST  /api/profile
- PATCH /api/profile
- GET   /api/accounts
- GET   /api/accounts/:currency
- GET   /api/accounts/:currency/transactions
- POST  /api/deposits
- POST  /api/withdrawals

## Manager Gateway
Routes:
- GET   /api/customers/:customerId
- POST  /api/customers
- PATCH /api/customers
- GET   /api/customers/:customerId/accounts
- GET   /api/customers/:customerId/accounts/:currency
- GET   /api/customers/:customerId/accounts/:currency/transactions
- GET   /api/accounts