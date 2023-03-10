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
1. Search accounts by Email, FirstName, LastName, Currency, Balance

## Customer Stream Consumer Lambda
Features:
1. Transalate the `Customers` table change stream to Kafka events.

## Account Customer Consumer
Features:
1. Consume customer events from Kafka and save account-related data to the `AccountTransactions` table.