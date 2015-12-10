using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Restaurant
{
	public class Controller
	{
		private List<Order> orderList;
		private List<Dish> predefinedDishes;
		private List<Ingredient> predefinedIngredients;

		public Order CurrentOrder { get; set; }

		public Controller ()
		{
			/* may be here'll be config file, whatever */ 
			// orderList should be populated here from data file, otherwise it's empty
			orderList = new List<Order> ();
			predefinedDishes = new List<Dish> ();
			predefinedIngredients = new List<Ingredient> ();

			// fill some predefined dishes
			var sampledish1 = new Dish ("predefined1", new List<Ingredient> { new Ingredient ("sample", 10) }, 10, 15);
			var sampledish2 = new Dish ("predefined2", new List<Ingredient> { new Ingredient ("sample", 15) }, 10, 15);
			predefinedDishes.Add (sampledish1);
			predefinedDishes.Add (sampledish2);

			var predefined1 = new Ingredient ("predefined1", 10);
			var predefined2 = new Ingredient ("predefined2", 15);

			predefinedIngredients.Add (predefined1);
			predefinedIngredients.Add (predefined2);

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
						// <= because parsed_input starts with 1, not 0
						if (parsed_input > 0 && parsed_input <= orderList.Count) {
							CurrentOrder = orderList [parsed_input - 1];
							orderList.RemoveAt (parsed_input - 1);
						} else {
							throw new ArgumentOutOfRangeException ();	
						}
					} else {
						if (input.ToString ().Equals ("c") || input.ToString ().Equals ("create")) {
							CurrentOrder = CreateOrder ();
							parsed_input = orderList.Count + 1;
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
			return OrderMenu (CurrentOrder, parsed_input);
		}

		private Order CreateOrder ()
		{
			Console.WriteLine ("Створення замовлення...");
			var order = new Order ();
			AddDishesTo (ref order);
			return order;
		}

		private void DisplayPredefinedDishes ()
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

		private void AddDishesTo (ref Order order)
		{
			Console.WriteLine ("Меню: ");
			DisplayPredefinedDishes ();
			Console.WriteLine ("Choose dishes and enter numbers (separate with white space) or [m]ake your own: ");
			Console.Write ("~> ");

			var input = Console.ReadLine ().Split (' ');
			IEnumerable<int> array;
			if (input [0].Equals ("m") || input [0].Equals ("make")) {
				order.AddDish (CreateDish ());
			} else {
				array = input.Select (x => int.Parse (x) - 1);
				foreach (var i in array) {
					order.AddDish (predefinedDishes [i]);
				}
			}
		}

		private Dish CreateDish ()
		{
			Console.Write ("Enter dish name: ");

			var dishName = Console.ReadLine ();
			var cookTime = 0.0;
			var ingredientsList = new List<Ingredient> ();

			// get ingredients here
			DisplayPredefinedIngredients();
			Console.WriteLine ("Choose ingredients to add or [m]ake new one (will be auto added): ");
			Console.Write ("~> ");

			var ingredients = Console.ReadLine ().Split (' ');
			IEnumerable<int> ingredientsArray;
			if (ingredients [0].Equals ("m") || ingredients [0].Equals ("make")) {
				ingredientsList.Add (CreateIngredient(predefinedIngredients));
//				CreateIngredient(
			} else {
				ingredientsArray = ingredients.Select (x => int.Parse (x) - 1);
				foreach (var i in ingredientsArray) {
					ingredientsList.Add (predefinedIngredients [i]);
				}
			}
			// add some price to contain restaurant work in producing dishes

			Console.Write ("Enter time to cook: ");
			try {
				cookTime = double.Parse (Console.ReadLine ());
				if (cookTime < 0.1 /* fucking minumum in config */) {
					throw new ArgumentException ("Cook time should be bigger.");
				}
			} catch (FormatException) {
				Console.WriteLine ("Not digit entered.");
			} catch (ArgumentException) {
			}

			return new Dish (dishName, ingredientsList, /* put here minimum overprice from config */ 10, cookTime);
		}

		private int OrderMenu (Order order, int orderNumber)
		{
			int input = 0;
			bool done = false;

			while (!done) {
				Console.Clear ();
				Console.WriteLine ($"Working with order#{orderNumber}");
				Console.WriteLine (CurrentOrder.ToString ());
				Console.WriteLine ("Choose option:");
				Console.WriteLine ("1. Delete order");
				Console.WriteLine ("2. Edit dishes");
				Console.WriteLine ("3. Set total cost (u wot)");
				Console.WriteLine ("4. Set table number");
				Console.WriteLine ("5. Finish order");
				Console.Write ("~> ");

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
						done = true;
						break;
					case 2:
						EditOrderDishesMenu (CurrentOrder);
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
						orderList.Add (CurrentOrder);
						done = true;
						break;
					default:
						throw new ArgumentOutOfRangeException ();
					}
				} catch (FormatException e) {
					Console.WriteLine ("Not a number is used to set numeric value. {0}", e.Message);
				} catch (ArgumentOutOfRangeException e) {
					Console.WriteLine ("No such number on the list. {0}", e.Message);
				}
			}
			return 1;
		}

		private void EditOrderDishesMenu (Order order)
		{
			bool done = false;
			while (!done) {
				Console.WriteLine ("Dishes in order:\n");
				Console.WriteLine (order.PrintDishes ());

				Console.WriteLine ("1. Add dish");
				Console.WriteLine ("2. Edit dish");
				Console.WriteLine ("3. Remove dish");
				Console.WriteLine ("4. Return");

				Console.Write ("~> ");
				try {
					var input = int.Parse (Console.ReadLine ());

					switch (input) {
					case 1:
						AddDishesTo (ref order);
						break;
					case 2:
						EditSpecificDishMenu (ref order);
						break;
					case 3:
						Console.Write ("Choose dishes and enter numbers (separate with white space): ");
					// TODO: make parseMethod i.g. ParseStringToInt(string[] first, int[] second )
						var dishesToRemove = Console.ReadLine ().Split (' ');
						IEnumerable<int> arrayOfDishIndexes = dishesToRemove.Select (x => int.Parse (x) - 1);
						foreach (var i in arrayOfDishIndexes) {
							// TODO: put this logic into Order.RemoveDishById method (price deduction).
							order.TotalCost -= order.Dishes.ElementAt (i).Price;
							order.Dishes.RemoveAt (i);
						}
						Console.WriteLine ("Successfully removed!!1");
						break;
					case 4:
						done = true;
						break;
					default:
						throw new ArgumentOutOfRangeException ();
					}
				} catch (FormatException e) {
					Console.WriteLine ("Not a number entered. {0}", e.Message);
				} catch (ArgumentOutOfRangeException e) {
					Console.WriteLine (e.Message);
				}
			}
		}
		// TODO: remove all ref's
		private void EditSpecificDishMenu (ref Order order)
		{
			bool done = false;

			Console.WriteLine (order.PrintDishes ());
			Console.Write ("Choose dish to edit: ");
			try {
				var input = int.Parse (Console.ReadLine ()) - 1;
				while (!done) {
					if (input >= 0 && input < order.Dishes.Count) {
						Console.WriteLine (order.Dishes.ElementAt (input));
						Console.WriteLine ("1. Add dish ingredients");
						Console.WriteLine ("2. Remove dish ingredients");
						Console.WriteLine ("3. Change dish name");
						Console.WriteLine ("4. Change dish price");
						Console.WriteLine ("5. Change dish time to get cooked");
						Console.WriteLine ("6. Return");

						Console.Write ("~> "); 
						switch (int.Parse (Console.ReadLine ())) {
						case 1:
							Console.WriteLine ("Some predefined ingredients: ");
							DisplayPredefinedIngredients ();

							Console.Write ("Choose ingredients to add (numbers separate with white space) or [m]ake to create new: ");
							var ingredientInput = Console.ReadLine ().Split (' ');
							IEnumerable<int> ingredientsArray;
							if (ingredientInput [0].Equals ("m") || ingredientInput [0].Equals ("make")) {
								order.Dishes[input].AddIngredient(CreateIngredient (predefinedIngredients));
							} else {
								ingredientsArray = ingredientInput.Select (x => int.Parse (x) - 1);
								foreach (var i in ingredientsArray) {
									// TODO: add new created ingredients to predefinedIngredients
									order.Dishes [input].AddIngredient (predefinedIngredients [i]);
									order.Dishes [input].Price += predefinedIngredients [i].Price;
								}
							}
							break;
						case 2:
							Console.Write ("Choose ingredients to remove and enter numbers (separate with white space): ");
							// TODO: make parseMethod i.g. ParseStringToInt(string[] first, int[] second )
							var ingredientsToRemove = Console.ReadLine ().Split (' ');
							IEnumerable<int> arrayOfIngredientsIndexes = ingredientsToRemove.Select (x => int.Parse (x) - 1);
							foreach (var i in arrayOfIngredientsIndexes) {
								// TODO: put this logic into Order.RemoveDishById method (price deduction).
								// TODO: maybe put this logic to method
								order.Dishes [input].Price -= order.Dishes [input].Ingredients [i].Price;
								order.Dishes [input].RemoveIngredient (order.Dishes [input].Ingredients [i]);
							}
							Console.WriteLine ("Successfully removed!!1");
							break;
						case 3:
							Console.Write ("Preferable dish name: ");
							order.SetDishName (order.Dishes.ElementAt (input), Console.ReadLine ());
							break;
						case 4:
							Console.Write ("Enter dish price: ");
							var dishPrice = double.Parse (Console.ReadLine ());
							order.SetDishPrice (order.Dishes.ElementAt (input), dishPrice);
							break;
						case 5:
							Console.Write ("Time to get dish ready: ");
							var dishTime = double.Parse (Console.ReadLine ());
							order.SetDishTime (order.Dishes.ElementAt (input), dishTime);
							Console.WriteLine ("Successfully changed time!\n");
							break;
						case 6:
							done = true;
							break;
						default:
							throw new ArgumentOutOfRangeException ();	
						}
					} else {
						throw new ArgumentOutOfRangeException ();
					}
				}
			} catch (FormatException) {
				Console.WriteLine ("Not a number entered.");	
			} catch (ArgumentOutOfRangeException) {
				Console.WriteLine ("Thing was just caught in being out of range.");
			} catch (ArgumentNullException) {
				Console.WriteLine ("Seems like you triggered null exception.");
			}
		}

		private Ingredient CreateIngredient (List<Ingredient> predefinedIngredients)
		{
			Console.Write ("Enter ingredient name: ");
			var unparsedName = Console.ReadLine ();
			string inputName = "";
			double inputPrice = 0;

			try {
				// TODO: refactor this, сделать только если не проходит эксепшн
				if (new Regex (@"[A-Za-z]").IsMatch (unparsedName)) {
					inputName = unparsedName;
				} else {
					throw new FormatException ();
				}
				Console.Write ("Enter ingredient price: ");
				inputPrice = double.Parse (Console.ReadLine ());
			} catch (FormatException) {
				Console.WriteLine ("Not digit entered.");
			}

			var newIngredient = new Ingredient (inputName, inputPrice);
		
			predefinedIngredients.Add (newIngredient);
			return newIngredient;
		}

		private void DisplayPredefinedIngredients ()
		{
			for (int i = 0; i < predefinedIngredients.Count; i++) {
				Console.WriteLine ($"{i+1}. {predefinedIngredients[i]}\n");
			}
		}
	}
}

