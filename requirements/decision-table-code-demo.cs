public class DecisionTable
{

    public static void Main()
    {



    }

    private static void AddTestItems()
    {
        
    }

    
    public class User
    {
        List<Item> cart = new List<Item>();
        private Inventory storesInventory;
        
        public User(Inventory inventory)
        {
            storesInventory = inventory;
            Login();
        }
        public void Login()
        {
            Console.WriteLine("Give us your money!");
        }

        //search if any item has the same name as the item you're looking for
        public Item SearchInventory(string s)
        {
            foreach (Item i in storesInventory)
            {
                if (i.name == s) return i;
            }
            return null;
        }

        public void AddItemToCart(string itemName)
        {
            //tuple of item and quantity
            var item = SearchInventory(itemName);
            if (item == null)
            {
                Console.WriteLine("No such item in inventory. Cannot add to cart.");
                return;
            }
            else if (item.quantity < 1)
            {
                Console.WriteLine("Item is out of stock.");
                return;
            }
            else
            {
                //making seperate object so changes on the object in cart don't affect object in inventory
                Item cartItem = new Item(itemName, 1);
                cart.Add(cartItem);
                item.quantity--;
                Console.WriteLine("Added item " + itemName + "to cart.");
            }
        }

        public void RemoveItemFromCart(string itemName)
        {
            var item = SearchInventory(itemName);

            if (item == null)
            {
                Console.WriteLine("No such item in cart. Cannot add to cart.");
                return;
            }
            else
            {
                cart.Remove(item);
                var.quantity++;
                Console.WriteLine("Removed item " + itemName + "to cart.");
            }
        }
        
        private Item SearchCart(string itemName){
            foreach (Item i in cart)
            {
                if (i.name == itemName) return i;
            }
            return null;
        }
        
        public void Checkout()
        {
            cart = null;
            Console.WriteLine("Money, money, money, must be funny \n In the rich man's world");
        }
        
        
    }




    public class Admin : User
    {
        public Admin(Inventory inventory)
        {
            storeInventory = inventory;
        }

        public override void Login()
        {
            Console.WriteLine("Wa wa wee wa! King in the castle, king in the castle!");
        }

        //Don't need to override SearchInventory

        public override void AddItemToCart(string itemName)
        {
            Console.WriteLine("Admins cannot add items to cart.");
        }

        public override void RemoveItemFromCart(string itemName)
        {
            Console.WriteLine("Admins cannot remove items from cart.");
        }

        //Shouldn't need since private and no other methods call, just here in case
        private override void SearchCart(string itemName)
        { }

        public override void Checkout()
        {
            Console.WriteLine("Thief!");
        }

        public void AddToInventory(string itemName, int quantity)
        {
            var item = SearchInventory(itemName);

            if (item != null)
            {
                item.quantity += quantity;
            }
            else
            {
                item i = new Item();
                storesInventory.add
            }


        }
    }



    public class Inventory
    {
        List<Item> items = new List<Item>();
        public Inventory()
        {
        }
    }

    public class Item
    {
        public string name;
        public int quantity;

        public Item(string name, int price, int quantity)
        {
            this.name = name;
            this.price = price;
            this.quantity = quantity;
        }
    }
    
}