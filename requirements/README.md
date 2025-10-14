# Requirements
___
## Version 1
### **Epic T9E-1:** User Registration and Authentication
- **Story T9S-1.1**
    - **Name:** Self-Register Account
    - **Priority:** Must Have
    - **Effort:** 1 day
    - **Type:** Functional
    - **Description:** User must be able to create an account by entering required details and registering successfully.

- **Story T9S-1.2**
    - **Name:** Username Requirements
    - **Priority:** Must Have
    - **Effort:** 0.25 day
    - **Type:** Non-Functional
    - **Description:** System will enforce users to create a unique usernames upon registration.

- **Story T9S-1.3**
    - **Name:** Password Requirements
    - **Priority:** Must Have
    - **Effort:** 0.25 day
    - **Type:** Non-Functional
    - **Description:** System enforces passwords of minimum 6 characters.

- **Story T9S-1.4**
    - **Name:** Login and Inventory Access
    - **Priority:** Must Have
    - **Effort:** 0.5 day
    - **Type:** Functional
    - **Description:** After login or registration, user must be directed to view all available inventory.

### **Epic T9E-2:** Inventory Page

- **Story T9S-2.1**
    - **Name:** Display Inventory List
    - **Priority:** Must Have
    - **Effort:** 1 day
    - **Type:** Functional
    - **Description:** Inventory will be listed from highest to lowest

- **Story T9S-2.2**
    - **Name:** Display Inventory Details
    -	**Priority:** Must Have
    -	**Effort:** 1 day
    -	**Type:** Functional
    -	**Description:** Each inventory item must include a name, picture, brief description, and an “Add to Cart” button.

- **Story T9S-2.3**
    -	**Name:** Add Items to Cart
    - **Priority:** **Must Have
    - **Effort:** 1 day
    - **Type:** Functional
    - **Description:** Button for user to add items to the shopping cart.

- **Story T9S-2.4**
    - **Name:** Single Image Requirement
    - **Priority:** Must Have
    - **Effort:** 0.25 day
    - **Type:** Non-Functional
    - **Description:** Every inventory item must include at least one image.

- **Story T9S-2.5**
    - **Name:** Currency Formatting
    - **Priority:** Must Have
    - **Effort:** 0.25 day
    - **Type:** Non-Functional
    - **Description:** All prices must display in USD with $, commas, and decimal format.

- **Story T9S-2.6**
    - **Name:** Search Function
    - **Priority:** Must Have
    - **Effort:** 0.25 day
    - **Type:** Non-Functional
    - **Description:** Search bar for user to be able to search inventory.

### Epic T9E-3: Shopping Cart
- **Story T9S-3.1**
    - **Name:** Checkout Button Validation
    - **Priority:** Must Have
    - **Effort:** 0.25 day
    - **Type:** Functional
    - **Description:** The “Checkout” button cannot be clicked if the shopping cart is empty.

- **Story T9S-3.2**
    - **Name:** Checkout Button
    - **Priority:** **Must Have
    - **Effort:** 1 day
    - **Type:** Functional
    - **Description:** Checkout button brings user to Checkout page

- **Story T9S-3.3**
    - **Name:** Add Multiple Items to Cart
    - **Priority:** **Must Have
    - **Effort:** 1 day
    - **Type:** Functional
    - **Description:** User can add multiple items to the shopping cart.

- **Story T9S-3.4**
    - **Name:** Subtotal Summary
    - **Priority:** **Must Have
    - **Effort:** 1 day
    - **Type:** Functional
    - **Description:** Subtotal showing the price before taxes and shipping in the users shopping cart

### Epic T9E-3: Checkout and Payment Flow

- **Story T9S-3.2**
    - **Name:** Checkout Summary
    -	**Priority:** Must Have
    -	**Effort:** 1 day
    -	**Type:** Functional
    -	**Description:** Clicking checkout displays all cart items with subtotal.

- **Story T9S-3.3**
    -	**Name:** Payment Process
    -	**Priority:** Must Have
    -	**Effort:** 2 days
    -	**Type:** Functional
    -	**Description:** User enters shipping address, credit card (number, expiration, CCV), phone number, and selects a shipping speed.

- **Story T9S-3.4**
    -	**Name:** Shipping Options
    -	**Priority:** Must Have
    -	**Effort:** 0.5 day
    -	**Type:** Functional
    -	**Description:** Provide three shipping speed options — Overnight ($29), 3-Day ($19), and Ground (Free).

- **Story T9S-3.5**
    -	**Name:** Confirm Order Button
    -	**Priority:** Must Have
    -	**Effort:** 1 day
    -	**Type:** Functional
    -	**Description:** User clicks “Confirm Order” to view the confirm order page.

###	Epic T9E-4: Confirm Order Page

- **Story T9S-4.1**
    -	**Name:** List of items
    -	**Priority:** Must Have
    -	**Effort:** 1 day
    -	**Type:** Functional
    -	**Description:** User clicks “Confirm Order” to view item list, subtotal, tax (6%), shipping cost, and grand total.

- **Story T9S-4.2**
    -	**Name:** Price breakdown and Determination of Total
    -	**Priority:** Must Have
    -	**Effort:** 1 day
    -	**Type:** Functional
    -	**Description:** Shows subtotal, tax (6%), shipping cost, and grand total.

###	Epic T9E-5: Complete Order Page

- **Story T9S-5.1**
    -	**Name:** Complete Order and Receipt
    -	**Priority:** Must Have
    -	**Effort:** 1 day
    -	**Type:** Functional
    -	**Description:** User clicks “Complete Order” to finalize purchase, then views a receipt showing masked card and shipping info.

- **Story T9S-5.2**
    -	**Name:** Complete Order Button
    -	**Priority:** Must Have
    -	**Effort:** 0.5 day
    -	**Type:** Functional
    -	**Description:** Button to complete the order.

- **Story T9S-5.3**
    -	**Name:** Remove Purchased Inventory and Recorded in Sales Report
    -	**Priority:** Must Have
    -	**Effort:** 0.5 day
    -	**Type:** Functional
    -	**Description:** All purchased inventory must no longer appear in search results and must be recorded in sales reports.

###	Epic T9E-6: Admin and Reporting
- **Story T9S-6.1**
    - **Name:** Sales Report Access
    - **Priority:** Must Have
    - **Effort:** 1.5 days
    - **Type:** Functional
    - **Description:** Admin users can run sales reports showing all purchases and corresponding buyers.

- **Story T9S-6.2**
    - **Name:** Creating a New Admin
    - **Priority:** Needs to Have
    - **Effort:** 0.5 day
    - **Type:** Functional
    - **Description:** Existing admins are able to create new admins.

- **Story T9S-6.3**
    - **Name:** Export Sales Reports
    - **Priority:** Needs to Have
    - **Effort:** 0.5 day
    - **Type:** Functional
    - **Description:** Admins can export sales reports in CSV format.

- **Story T9S-6.4**
    - **Name:** Add Inventory into Database
    - **Priority:** Needs to Have
    - **Effort:** 1 day
    - **Type:** Functional
    - **Description:** Admins can manually add inventory into the database.

###	Epic T9E-7: UI Mockup
- **Story T9S-7.1**
    - **Name:** High Fidelity Screen Mockups
    - **Priority:** Needs to Have
    - **Effort:** 2 days
    - **Type:** Non-Functional
    - **Description:** Show a preview of what screen will look like

- **Story T9S-7.2**
    - **Name:** Application Flow
    - **Priority:** Needs to Have
    - **Effort:** 2 days
    - **Type:** Non-Functional
    - **Description:** Interactive mockup of how the user will navigate between pages

___
## Version 2
### Epic T9E-8:** Advanced Admin Features
- **Story T9S-8.1**
    - **Name:** Create Additional Admins
    - **Priority:** Wants to Have
    - **Effort:** 1 day
    - **Type:** Functional
    - **Description:** Admin can create and manage other admin accounts.

- **Story T9S-8.2**
    - **Name:** View Receipts for Sold Items
    - **Priority:** Wants to Have
    - **Effort:** 0.5 day
    - **Type:** Functional
    - **Description:** Admin can click a sold item and view its related receipt.

- **Story T9S-8.3**
    - **Name:** Inventory Management Page
    - **Priority:** Needs to Have
    - **Effort:** 1.5 days
    - **Type:** Functional
    - **Description:** Admin can open a page and fill in information to add inventory with form inputs.

### Epic T9E-9: Enhanced User Experience
- **Story T9S-9.1**
    - **Name:** Simple Interface
    - **Priority:** Needs to Have
    - **Effort:** 1 day
    - **Type:** Non-Functional
    - **Description:** Interface should be designed to be visually simple and intuitive for users and admins.

- **Story T9S-9.2**
    - **Name:** Multiple Item Photos
    - **Priority:** Needs to Have
    - **Effort:** 1 day
    - **Type:** Functional
    - **Description:** Allow multiple images per inventory item.

- **Story T9S-9.3**
    - **Name:** Search Inventory
    - **Priority:** Needs to Have
    - **Effort:** 1.5 days
    - **Type:** Functional
    - **Description:** Add search functionality to find specific inventory items.

- **Story T9S-9.4**
    - **Name:** Auto-Email Receipts
    - **Priority:** Wants to Have
    - **Effort:** 1 day
    - **Type:** Functional
    - **Description:** Automatically email purchase receipts to users after successful payment.

