using System;
using System.Collections.Generic;
public class DecisionTable
{

    public static void Main()
    {
        Inventory myInventory = new Inventory();
        User myUser = new User(myInventory);
        Admin myAdmin = new Admin(myInventory);

        AddTestItems(myInventory);
        myUser.AddItemToCart("Supertramp - Breakfast in America");
        myUser.AddItemToCart("ABBA - Arrival");
        myUser.AddItemToCart("Nirvana - Nevermind");

        Console.WriteLine("Items in user's cart");
        myUser.PrintCart();
        Console.WriteLine("");

        myUser.RemoveItemFromCart("Nirvana - Nevermind");

        Console.WriteLine("Items in user's cart");
        myUser.PrintCart();
        Console.WriteLine("");

        Console.Write("Checkout");
        myUser.Checkout();

        myUser.PrintCart();

        myAdmin.AddToInventory("Kino - Gruppa Krovi");
        myAdmin.RemoveFromInventory("Alice In Chains - Jar of Flies");

        myInventory.PrintInventory();






    }

    private static void AddTestItems(Inventory i)
    {
        Item dirt = new Item("Alice In Chains - Dirt");
        Item flies = new Item("Alice In Chains - Jar of Flies");
        Item nevermind = new Item("Nirvana - Nevermind");
        Item breakfast = new Item("Supertramp - Breakfast in America");
        Item arrival = new Item("ABBA - Arrival");
        i.Add(dirt);
        i.Add(flies);
        i.Add(nevermind);
        i.Add(breakfast);
        i.Add(arrival);
    }


    public class User
    {
        List<Item> cart = new List<Item>();
        public Inventory storesInventory;

        public User(Inventory inventory)
        {
            storesInventory = inventory;
            Login();
        }
        public virtual void Login()
        {
            Console.WriteLine("User logged in");
        }

        //search if any item has the same name as the item you're looking for
        public Item SearchInventory(string s)
        {
            foreach (Item i in storesInventory.items)
            {
                if (i.name == s) return i;
            }
            return null;
        }

        public virtual void AddItemToCart(string itemName)
        {
            Item item = SearchInventory(itemName);
            if (item == null)
            {
                Console.WriteLine("No such item in inventory. Cannot add to cart.");
                return;
            }
            else
            {
                //making seperate object so changes on the object in cart don't affect object in inventory
                cart.Add(item);
                storesInventory.Remove(item);
                Console.WriteLine("Added item " + itemName + " to cart.");
            }
        }

        public virtual void RemoveItemFromCart(string itemName)
        {
            Item item = null;

            foreach(Item i in cart)
            {
                if (i.name == itemName) item = i;
            }

            if (item == null)
            {
                Console.WriteLine("No such item in cart. Cannot add to cart.");
                return;
            }
            else
            {
                storesInventory.items.Add(item);
                cart.Remove(item);
                Console.WriteLine("Removed item " + itemName + " from cart.");
            }
        }

        public virtual Item SearchCart(string itemName)
        {
            foreach (Item i in cart)
            {
                if (i.name == itemName) return i;
            }
            return null;
        }

        public virtual void Checkout()
        {
            cart.Clear();
            Console.WriteLine("Money, money, money, must be funny \n In the rich man's world");
        }

        //not in decision table, here for debug
        public void PrintCart()
        {
            if(cart.Count == 0)
            {
                Console.WriteLine("Cart is empty.");
                return;
            }
            foreach(Item i in cart)
            {
                Console.WriteLine(i.name);
            }
        }


    }




    public class Admin : User
    {
        public Admin(Inventory inventory) : base(inventory)
        {
        }

        public override void Login()
        {
            Console.WriteLine("Admin logged in");
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

        public override Item SearchCart(string itemName)
        {
            return null;
        }

        public override void Checkout()
        {
            Console.WriteLine("Thief!");
        }

        public void AddToInventory(string itemName)
        {
            var item = SearchInventory(itemName);

            if (item != null)
            {
                Console.WriteLine("Item already in inventory.");
            }
            else
            {
                Item i = new Item(itemName);
                storesInventory.Add(i);
            }
        }

        public void RemoveFromInventory(string itemName)
        {
            var item = SearchInventory(itemName);

            if (item == null)
            {
                Console.WriteLine("Item is not in inventory.");
            }
            else
            {
                storesInventory.Remove(item);
            }
        }

        public void SalesReport()
        {
            Console.WriteLine("This is where a sales report would go");
        }
    }
}



    public class Inventory
    {
        public List<Item> items = new List<Item>();
        public Inventory()
        {
        }

    public void Add(Item i)
    {
        items.Add(i);
    }

    public void Remove(Item i)
    {
        items.Remove(i);
    }

    public void PrintInventory()
    {
        if(items.Count == 0)
            {
                Console.WriteLine("No items in inventory.");
                return;
            }
            foreach(Item i in items)
            {
                Console.WriteLine(i.name);
            }
    }  

    }

    public class Item
    {
        public string name;

        public Item(string name)
        {
            this.name = name;
        }
    }
    
