using System.Collections.Generic;

namespace Backend.Models;

public class CartItem
    {
        public int CartId { get; set; }
        public int ItemId { get; set; }
        //Andrew Tressler
        //Always assumed to be 1, CartItems only made when adding items to the cart, no quantity in project
        public int Quantity = 1;

        public Cart Cart { get; set; } = null!;
        public InventoryItem InventoryItem { get; set; } = null!;
    }
