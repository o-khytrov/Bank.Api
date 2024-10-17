# Bank.Api 🏦

This services provides APIs to create orders and search orders in the bank system.

## Summary

### Endpoints:

- ***POST /Order/create***: Creates a new order with necessary details like client ID, amount, and department address.
- ***POST /Order/search***: Searches for existing orders by order ID or a combination of client ID and department
  address.

## 🚀 Getting started

### To run solution locally

- #### clone the repository

```bash
git clone git@github.com:o-khytrov/Bank.Api.git

```

- #### `cd` into repository root

- #### run command

```bash
docker compose up -d --build

```

- ### Navigate to [http://localhost:8080/swagger/index.html](http://localhost:8080/swagger/index.html) in the browser

## 📝 TODO

- Add Departments table
    - Reference Orders by department id
    - Validate Departments address on order creation
- Extend integration tests
    - Cover validation errors
    - Cover database errors