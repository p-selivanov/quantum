# Quantum

Quantum is a PoC playground. It evaluates a few approaches to microservices design.

The system imitates an imaginary investment bank that is unrealistically profitable. 
Any user can register and deposit some funds. 
The bank then calculates extremely generous interest on the deposit: 0.1% every 2 minutes. 
The user may check his balance and withdraw the money any time.

It is expected that the system will have extremelly high number of new registrations, deposits and withdraws.

This imaginery system is intended to highliht the performance and consistency challendges that most modern high-load systems have.

## Requirements
TBD

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

## Docker Commands
Run only infrastructure (for local dev):
```
docker compose -f compose-infra.yaml --project-name quantum up -d
docker compose --project-name quantum down
```

Run infra and all services:
```
docker compose -f compose-infra.yaml -f ccompose-Quantum.yaml --project-name quantum up -d
docker compose --project-name quantum down
```

Build commands:
```
docker build . -f ./src/Quantum.Customer.Api/Dockerfile -t quantum-customer-api:1.0.1
```