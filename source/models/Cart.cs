using System.Collections.Generic;

namespace Backend.Models;

public class Cart
    {
        
        public int CartId { get; set; }
        public int UserId { get; set; }
        //true - active false - checked out
        public bool isActive { get; set; }
        public User User { get; set; } = null!;
        public List<CartItem> Items { get; set; } = new();
    }
