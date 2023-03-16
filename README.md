# Quantum

Quantum is a proof of concept project. It evaluates a few approaches to microservices design.

The system imitates an imaginary bank.
Any user can register and deposit some funds.
The bank then calculates extremely generous interest on the deposit: 0.1% every 2 minutes.
The user may check his balance and withdraw the money any time.

It is expected that the system will have extremelly high number of new registrations, deposits and withdrawals.
The system highlights the performance and consistency challendges that most modern high-load systems have.

This project may be usefull to you if you are interseted in the following aspects of software development:
1. Building event-driven microservices.
2. Working with DynamoDB in .NET.
3. Working with Kafka in .NET.
4. Running DynamoDB and Lamdba Functions locally using LocalStack.
5. Local development using Docker.

## Desing
[Requirements](/docs/Requirements.md)

[Design](/docs/Design.md)

## Setup

### Prerequisites
1. [Docker](https://www.docker.com/products/docker-desktop/)
2. [.NET 7](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
3. [PowerShell](https://github.com/PowerShell/PowerShell)
4. [AWS CLI](https://aws.amazon.com/cli/)
5. [awslocal](https://github.com/localstack-dotnet/localstack-awscli-local)

### Running Everything in Docker
This repo provides a few PowerShell scripts to build and run the system in Docker.
CD to the `scripts` directory to run the scripts.

1. `./build` - builds all images and lambda functions.
2. `./up all` - starts all infrastructure and app containers using **docker compose**, installs lambdas.

## TODO
- Telemetry
- Activity Simulator
- Rearrange docs
- Unique email problem
- Interest calculation
- Customer name edit bug
- Migrate to System.Text.Json
- Unite models and DTOs?
- Timestamp prop names
- Imperative YARP gateway