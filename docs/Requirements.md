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