# Quantum

Quantum is a proof of concept project. It evaluates a few approaches to microservices design.

The system imitates an imaginary bank.
Any user can register and deposit some funds.
The bank then calculates extremely generous interest on the deposit: 0.1% every 2 minutes.
The user may check his balance and withdraw the money any time.

It is expected that the system will have extremelly high number of new registrations, deposits and withdraws.

This imaginery system is intended to highliht the performance and consistency challendges that most modern high-load systems have.

[Requirements](/docs/Requirements.md)

## Components

### Customer Service
Features:
1. Get customer
2. Create a lead
3. Convert a lead to a customer

### Account Service
Features:
1. Get customer account(s)
2. Get customer/account transactions
3. Deposit
4. Withdraw

### Account Search Service
1. Search accounts by Email, FirstName, LastName, Currency, Balance

## TODO
- ID format
- Dockerfiles
- Account creation
- Telemetry
- Load Simulation
- Unique email
- System.Text.Json
- Unite models and DTOs?

GET /customer/123/accounts
GET /customer/123/accounts/546
GET /accounts/546

POST /accounts/546/deposit
POST /customer/123/deposit

Customers
PK: Id
SK:-
{
  Id: 123,
  EmailAddress: bob@mail.dev,
  Country: Poland
}

AccountTransactions
PS: AccountId, 
SK: TransactionId | 0
{
  AccountId: 123-usd,
  TransactionId: 0,
  CustomerId: 123,
}

PS: CustomerId, 
SK: 0 | Currency | Currency#TransactionId