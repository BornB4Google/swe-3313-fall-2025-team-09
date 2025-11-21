using System.Collections.Generic;

namespace backend.Models;

public class Cart
    {
        public int CartId { get; set; }
        public int UserId { get; set; }

        public User User { get; set; } = null!;
        public List<CartItem> Items { get; set; } = new();
    }
