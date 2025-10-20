# Use Case Diagram

This page contains the use-case diagram for our E-commerce site

```mermaid
flowchart LR
    shopper(["Shopper"])
    admin(["Admin"])
    payment(["Payment Processor"])
    shipping(["Shipping Service"])

    subgraph System["Storefront System"]
        UC_Auth["(Register and Log In to Account)"]
        UC_Inventory["(Browse and Search Inventory)"]
        UC_Cart["(Add and Manage Items in Cart)"]
        UC_Checkout["(Checkout and Make Payment)"]
        UC_Confirm["(Review and Confirm Order)"]
        UC_Complete["(Finalize and Complete Purchase)"]
        UC_Admin["(Manage Inventory and Generate Reports)"]
    end

    shopper --- UC_Auth
    shopper --- UC_Inventory
    shopper --- UC_Cart
    shopper --- UC_Checkout
    shopper --- UC_Confirm
    shopper --- UC_Complete
    admin --- UC_Admin
    UC_Checkout --- payment
    UC_Checkout --- shipping
    UC_Confirm --- shipping
```
