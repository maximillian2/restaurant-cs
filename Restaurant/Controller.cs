using System;
using System.Collections.Generic;
using System.Linq;

namespace Restaurant
{
	public class Controller
	{
		private List<Order> orderList;
		private List<Dish> predefinedDishes;

		public Order CurrentOrder { get; set; }

		public Controller ()
		{
			/* may be here'll be config file, whatever */ 
			// orderList should be populated here from data file, otherwise it's empty
			orderList = new List<Order> ();
			predefinedDishes = new List<Dish> ();

			// fill some predefined dishes
			var sampledish1 = new Dish ("predefined1", new List<Ingredient> { new Ingredient ("sample", 10) }, 10, 15);
			var sampledish2 = new Dish ("predefined2", new List<Ingredient> { new Ingredient ("sample", 15) }, 10, 15);
			predefinedDishes.Add (sampledish1);
			predefinedDishes.Add (sampledish2);

			// getting some fake info to simulate 
			var fakeOrder1 = new Order ();
			var fakeOrder2 = new Order ();
			var fakeOrder3 = new Order ();

			var ing1 = new Ingredient ("sample", 10); 
			var ing2 = new Ingredient ("sample2", 20); 

			var dish1 = new Dish ("Dish1", new List<Ingredient> { ing1 }, 10, 15);
			var dish2 = new Dish ("Dish2", new List<Ingredient> { ing1, ing2 }, 30, 25);

			fakeOrder1.AddDish (dish1);
			fakeOrder2.AddDish (dish2);
			fakeOrder3.AddDish (dish1);
			fakeOrder3.AddDish (dish2);

			orderList.Add (fakeOrder1);
			orderList.Add (fakeOrder2);
			orderList.Add (fakeOrder3);

			// always null so that user can choose with which he wants to work with 
			CurrentOrder = null;
		}

		// Method to be called in main class
		public void Run ()
		{
			// program starts with mainMenu after exiting write all info to file
			// using this method because user will have feature to get back to upper-level menus.
			while (ShowMainMenu () == 1)
				;
		}

		private int ShowMainMenu ()
		{
			int parsed_input = 0;

			Console.WriteLine ("Welcome! \ud83c\udf74"); // 🍴 emoji
			if (orderList.Count == 0) {
				Console.WriteLine ("No existing orders found. Creating new one...");
				CurrentOrder = CreateOrder ();
			} else {
				Console.WriteLine ("Some orders exist already:");
				DisplayOrders ();
				Console.WriteLine ("Choose order number to work with or [c]reate new one: ");

				var input = Console.ReadLine ();
				try {
					bool isNumber = int.TryParse (input.ToString (), out parsed_input);
					if (isNumber) {
						Console.WriteLine ("Number is chosen");
						// <= because parsed_input starts with 1, not 0
						if (parsed_input > 0 && parsed_input <= orderList.Count) {
							Console.WriteLine ("Valid number");
							CurrentOrder = orderList [parsed_input - 1];
						} else {
							throw new ArgumentOutOfRangeException ();	
						}
					} else {
						if (input.ToString ().Equals ("c") || input.ToString ().Equals ("create")) {
							CurrentOrder = CreateOrder ();
						} else {
							throw new FormatException ();
						}
					}
				} catch (ArgumentOutOfRangeException) {
					Console.WriteLine ("This order doesn't exist.");
					return 1;
				} catch (FormatException) {
					Console.WriteLine ("Got wrong input. Try again!");
					return 1;
				} catch (Exception e) {
					Console.WriteLine ($"Some other error happened: {e.Message}");
					return 1;
				}
			}
			return MenuWithOrder (CurrentOrder, parsed_input);
		}

		private Order CreateOrder ()
		{
			Console.WriteLine ("Order creation...");
			var order = new Order ();

			Console.WriteLine ("We have some dishes in our menu: ");
			DisplayPredefinedDishes();
			Console.Write ("Choose dishes and enter numbers (separate with white space) or [m]ake your own: ");

			var input = Console.ReadLine ().Split (' ');
			IEnumerable<int> array;
			if (input [0].Equals ("m") || input [0].Equals ("make")) {
				// CreateDish();
			} else {
				array = input.Select (x => int.Parse(x)-1);
				foreach (var i in array) {
					Console.WriteLine (i);
					order.AddDish (predefinedDishes [i]);
				}
			}
			return order;
		}

		private void DisplayPredefinedDishes()
		{
			for (int i = 0; i < predefinedDishes.Count; i++) {
				Console.WriteLine ($"{i+1}. {predefinedDishes[i]}\n");
			}
		}

		private void DisplayOrders ()
		{
			for (int i = 0; i < orderList.Count; i++) {
				Console.WriteLine ($"\ud83c\udf54 Замовлення #{i+1}:\n{orderList[i]}");
			}
		}

		private int MenuWithOrder (Order order, int orderNumber)
		{
			int input = 0;

			Console.Clear ();
			Console.WriteLine ($"Working with order#{orderNumber}");
			Console.WriteLine (CurrentOrder.ToString ());
			Console.WriteLine ("Choose option:");
			Console.WriteLine ("1. Delete order");
			Console.WriteLine ("2. Edit dishes");
			Console.WriteLine ("3. Set total cost (u wot)");
			Console.WriteLine ("4. Set table number");
			Console.WriteLine ("5. Finish order");
			Console.Write ("$ ");

			try { 
				input = int.Parse (Console.ReadLine ());
			} catch (FormatException) {
				Console.WriteLine ("Not digit entered.");
			}

			try {
				switch (input) {
				case 1:
					orderList.Remove (CurrentOrder);
					Console.WriteLine ("Current order removed. Getting back to main menu.");
					break;
				case 2:
					EditDishesInOrder (CurrentOrder);
					break;
				case 3:
					Console.Write ("Enter preferrable cost (just number): ");
					var newCost = float.Parse (Console.ReadLine ());
					CurrentOrder.TotalCost = newCost;
					break;
				case 4:
					Console.Write ("Enter new table number (1-50): ");
					var tableNumber = int.Parse (Console.ReadLine ());
					// put max Table Number in config
					if (tableNumber > 0 && tableNumber < 51) {
						CurrentOrder.TableNumber = tableNumber;
					} else {
						throw new ArgumentOutOfRangeException ();
					}
					break;
				case 5:
					// it works fine when we add new orders here, but when it comes to update
					// better to delete an example of order in orderList when assigning it to currentOrder
					// and then save it here
					orderList.Add(CurrentOrder);
					break;
				default:
					throw new IndexOutOfRangeException ();
				}
			} catch (FormatException e) {
				Console.WriteLine ("Not a number is used to set numeric value. {0}", e.Message);
			} catch (ArgumentOutOfRangeException e) {
				Console.WriteLine ("No such number on the list. {0}", e.Message);
			}
			return 1;
		}

		private void EditDishesInOrder (Order order)
		{
			if (order.Dishes.Count == 0) {
				Console.WriteLine ("No dishes inside");
			} else {
				Console.WriteLine ("Dishes in order:\n");
				Console.WriteLine (order.PrintDishes ());
			}

			Console.WriteLine ("Would you like to add ");
		}
	}
}

