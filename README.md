# Quantum

Quantum is a proof of concept project. It evaluates a few approaches to microservices design.

The system imitates an imaginary bank.
Any user can register and deposit some funds.
The bank then calculates extremely generous interest on the deposit: 0.1% every 2 minutes.
The user may check his balance and withdraw the money any time.

It is expected that the system will have extremelly high number of new registrations, deposits and withdraws.
The system highlights the performance and consistency challendges that most modern high-load systems have.

[Requirements](/docs/Requirements.md)

## Components

### Customer Service
Features:
1. Get a customer
2. Create a customer
3. Update a customer

### Account Service
Features:
1. Get customer accounts
2. Get customer account transactions
3. Deposit
4. Withdraw

### Account Search Service
1. Search accounts by Email, FirstName, LastName, Currency, Balance

## TODO
- Switch to minikube
- Telemetry
- Load Simulation
- Unique email
- System.Text.Json
- Unite models and DTOs?