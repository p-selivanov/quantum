# Quantum

Quantum is a proof of concept project. It evaluates a few approaches to microservices design.

The system imitates an imaginary bank.
Any user can register and deposit some funds.
The bank then calculates extremely generous interest on the deposit: 0.1% every 2 minutes.
The user may check his balance and withdraw the money any time.

It is expected that the system will have extremelly high number of new registrations, deposits and withdraws.

This imaginery system is intended to highliht the performance and consistency challendges that most modern high-load systems have.

## Requirements
- The system has 2 types of users: customers and managers.
- A customer can register with an email address, first name, and last name. A phone number and country may be provided during registration or later.
- A customer email address must be unique.
- A customer can read or edit his profile. All fields can be changed (including email).
- A customer can deposit and withdraw funds in USD, EUR, and GBP.
- A customer can check his account balances.
- A customer has 3 statuses:
  - Lead - right after registration, but before the first deposit.
  - Active - after the first deposit.
  - Suspended - suspended by a manager. Deposits and withdrawals are blocked.
- A manager may search customer accounts by email address, first name, last name, country, phone number, status, currency, and balance.
- A manager may suspend or reactivate a customer.
- Deposit fee is configurable (2% by default).
- Withdrawal fee is configurable (10% by default).
- For customers from Ukraine fees are halved.
- The system generates interest on deposit of 0.1% every 2 minutes. It is configurable.
- All amounts are calculated and stored with 8 decimal places. If calculation produces more decimal places - they are rounded down.

Note: for simplicity, all API calls will be anonymous, no authentication is performed. The distinction between customers and managers is artificial.

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