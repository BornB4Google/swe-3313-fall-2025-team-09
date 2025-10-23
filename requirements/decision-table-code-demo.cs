using System;

public class Program
{
	public static void Main()
	{
		bool isUser = IsUser();
		bool isAdmin = IsAdmin();
		
		string rule;
		if (!isUser && !isAdmin)
		{
			rule = "R1";
		}
		else if (isUser && !isAdmin)
		{
			rule = "R2";
		}
		else if (isUser && isAdmin)
		{
			rule = "R3";
		}
		else
		{
			// This case shouldn't happen based on the table (F,T doesn't exist)
			rule = "Invalid";
		}
		
		Console.WriteLine($"\nApplying Rule: {rule}");
		Console.WriteLine("======================");
		Console.WriteLine($"Register user account:		{CanRegisterUserAccount(isUser, isAdmin)}");
		Console.WriteLine($"Login:				{CanLogin(isUser, isAdmin)}");
		Console.WriteLine($"Search Inventory:		{CanSearchInventory(isUser, isAdmin)}");
		Console.WriteLine($"Add item to cart:		{CanAddItemToCart(isUser, isAdmin)}");
		Console.WriteLine($"Remove item from cart:		{CanRemoveItemFromCart(isUser, isAdmin)}");
		Console.WriteLine($"Checkout:			{CanCheckout(isUser, isAdmin)}");
		Console.WriteLine($"Add item to inventory:		{CanAddItemToInventory(isUser, isAdmin)}");
		Console.WriteLine($"Remove item from inventory:	{CanRemoveItemFromInventory(isUser, isAdmin)}");
		Console.WriteLine($"View sales report:		{CanViewSalesReport(isUser, isAdmin)}");
	}
	
	static bool IsUser()
	{
		Console.WriteLine("Is the person a user? (y/n): ");
		var input = Console.ReadLine();
		var isUser = input == "y" || input == "Y";
		Console.WriteLine($"Is User: {isUser}");
		return isUser;
	}
	
	static bool IsAdmin()
	{
		Console.WriteLine("Is the person an admin? (y/n)");
		var input = Console.ReadLine();
		var isAdmin = input == "y" || input == "Y";
		Console.WriteLine($"Is Admin: {isAdmin}");
		return isAdmin;
	}
	
	// Rule R1: isUser = F, isAdmin = F
	// Rule R2: isUser = T, isAdmin = F
	// Rule R3: isUser = T, isAdmin = T
	
	static bool CanRegisterUserAccount(bool isUser, bool isAdmin)
	{
		// Y for R1, N for R2, N for R3
		return !isUser && !isAdmin;
	}
	
	static bool CanLogin(bool isUser, bool isAdmin)
	{
		// N for R1, Y for R2, Y for R3
		return isUser;
	}
	
	static bool CanSearchInventory(bool isUser, bool isAdmin)
	{
		if (!isUser)
			return false;
		// N for R1, Y for R2, Y for R3
		return isUser || isAdmin;
	}
	
	static bool CanAddItemToCart(bool isUser, bool isAdmin)
	{
		if (!isUser)
			return false;
		// N for R1, Y for R2, N for R3
		return isUser || isAdmin;
	}
	
	static bool CanRemoveItemFromCart(bool isUser, bool isAdmin)
	{
		if (!isUser)
			return false;
		// N for R1, Y for R2, N for R3
		return isUser || isAdmin;
	}
	
	static bool CanCheckout(bool isUser, bool isAdmin)
	{
		if (!isUser)
			return false;
		// N for R1, Y for R2, N for R3
		return isUser || isAdmin;
	}
	
	static bool CanAddItemToInventory(bool isUser, bool isAdmin)
	{
		// N for R1, N for R2, Y for R3
		return isUser && isAdmin;
	}
	
	static bool CanRemoveItemFromInventory(bool isUser, bool isAdmin)
	{
		// N for R1, N for R2, Y for R3
		return isUser && isAdmin;
	}
	
	static bool CanViewSalesReport(bool isUser, bool isAdmin)
	{
		// N for R1, N for R2, Y for R3
		return isUser && isAdmin;
	}

