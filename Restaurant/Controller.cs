using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Configuration;

namespace Restaurant
{
	public class Controller
	{
		private List<Order> orderList;
		private List<Dish> predefinedDishes;
		private List<Ingredient> predefinedIngredients;

		private double minimalCookTime;
		private double dishMargin;
		private int maximumTableNumber;

		public Order CurrentOrder { get; set; }

		public Controller ()
		{
			orderList = new List<Order> ();
			predefinedDishes = new List<Dish> ();
			predefinedIngredients = new List<Ingredient> ();

			// CONFIG FILE VALUES
			try {
				minimalCookTime = Convert.ToDouble (ConfigurationManager.AppSettings ["minimalCookTime"]);
				dishMargin = Convert.ToDouble (ConfigurationManager.AppSettings ["dishMargin"]);
				maximumTableNumber = Convert.ToInt32 (ConfigurationManager.AppSettings ["maximumTableNumber"]);
				/* deserialize predefined files here */
			} catch (FormatException) {
				Console.WriteLine ("Error with config parsing.");
			}

			// fill some predefined dishes
			// TODO: to remove this
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
			// TODO: upto this


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
			// store order number here
			int parsed_input = 0;

			Console.Clear ();
			// nice ASCII label here
			Console.WriteLine ("______          _                              _   \n| ___ \\        | |                            | |  \n| |_/ /___  ___| |_ __ _ _   _ _ __ __ _ _ __ | |_ \n|    // _ \\/ __| __/ _` | | | | '__/ _` | '_ \\| __|\n| |\\ \\  __/\\__ \\ || (_| | |_| | | | (_| | | | | |_ \n\\_| \\_\\___||___/\\__\\__,_|\\__,_|_|  \\__,_|_| |_|\\__|\n                                                   \n                                                   ");
			if (orderList.Count == 0) {
				Console.WriteLine ("Немає замовлень. Створюємо...");
				CurrentOrder = CreateOrder ();
			} else {
				Console.WriteLine ("Існуючі замовлення:");
				DisplayOrders ();
				Console.WriteLine ($"Виберіть номер замовлення або створіть нове (1-{orderList.Count}/+): ");
				Console.Write ("~> ");

				var input = Console.ReadLine ();
				try {
					bool isNumber = int.TryParse (input.ToString (), out parsed_input);
					if (isNumber) {
						// less than or equal because parsed_input starts with 1, not 0
						if (parsed_input > 0 && parsed_input <= orderList.Count) {
							CurrentOrder = orderList [parsed_input - 1];
							orderList.RemoveAt (parsed_input - 1);
						} else {
							throw new ArgumentOutOfRangeException ();	
						}
					} else {
						if (input.ToString ().Equals ("+")) {
							CurrentOrder = CreateOrder ();
							// new order has orderList elements + 1 value
							parsed_input = orderList.Count + 1;
						} else {
							throw new FormatException ();
						}
					}
				} catch (ArgumentOutOfRangeException) {
					Console.WriteLine ("Такого замовлення не існує.");
					return 1;
				} catch (FormatException) {
					Console.WriteLine ("Неправильні вхідні дані. Спробуйте ще раз!");
					return 1;
				} catch (Exception e) {
					Console.WriteLine ($"Виникла помилка: {e.Message}");
					return 1;
				}
			}
			return OrderMenu (CurrentOrder, parsed_input);
		}

		private Order CreateOrder ()
		{
			Console.WriteLine ("Створення замовлення...");
			var order = new Order ();
			AddDishesTo (order);
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

		private void AddDishesTo (Order order)
		{
			Console.WriteLine ("Меню: ");
			DisplayPredefinedDishes ();
			Console.WriteLine ("Виберіть страви (перелічуйте через одне пустий символ) або створіть нову (+): ");
			Console.Write ("~> ");

			var input = Console.ReadLine ().Split (' ');
			IEnumerable<int> array;
			if (input [0].Equals ("+")) {
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
			Console.Write ("Введіть назву страви: ");

			var dishName = Console.ReadLine ();
			var cookTime = 0.0;
			var ingredientsList = new List<Ingredient> ();

			DisplayPredefinedIngredients ();
			Console.WriteLine ("Виберіть інгрідієнти (перелічуйте через один пустий символ) або створіть новий (+) (буде автоматично додано до замовлення): ");
			Console.Write ("~> ");

			var ingredients = Console.ReadLine ().Split (' ');
			IEnumerable<int> ingredientsArray;
			if (ingredients [0].Equals ("+")) {
				ingredientsList.Add (CreateIngredient (predefinedIngredients));
			} else {
				ingredientsArray = ingredients.Select (x => int.Parse (x) - 1);
				foreach (var i in ingredientsArray) {
					ingredientsList.Add (predefinedIngredients [i]);
				}
			}

			Console.Write ("Введіть час приготування (в хв.): ");
			try {
				cookTime = double.Parse (Console.ReadLine ());
				if (cookTime < minimalCookTime) {
					throw new ArgumentException ("Час приготування має бути більшим.");
				}
			} catch (FormatException) {
				Console.WriteLine ("На вході отримано не число.");
			} catch (ArgumentException e) {
				Console.WriteLine (e.Message);
			}

			return new Dish (dishName, ingredientsList, dishMargin, cookTime);
		}

		private int OrderMenu (Order order, int orderNumber)
		{
			int input = 0;
			bool done = false;

			while (!done) {
				Console.Clear ();
				Console.WriteLine ($"Працюємо із замовленням №{orderNumber}");
				Console.WriteLine (CurrentOrder.ToString ());
				Console.WriteLine ("Виберіть опцію:");
				Console.WriteLine ("1. Видалити замовлення");
				Console.WriteLine ("2. Редагувати замовлення");
				Console.WriteLine ("3. Задати кінцеву ціну (???)");
				Console.WriteLine ("4. задати номер столика");
				Console.WriteLine ("5. Оформити замовлення");
				Console.Write ("~> ");

				try { 
					input = int.Parse (Console.ReadLine ());
				} catch (FormatException) {
					Console.WriteLine ("На вході отримано не число.");
				}

				try {
					switch (input) {
					case 1:
						orderList.Remove (CurrentOrder);
						Console.WriteLine ("Поточне замовлення видалене. Повертаємось в головне меню");
						done = true;
						break;
					case 2:
						EditOrderDishesMenu (CurrentOrder);
						break;
					case 3:
						Console.Write ("Введіть прийнятне число (в регіональній валюті): ");
						var newCost = float.Parse (Console.ReadLine ());
						if (newCost > 0) {
							CurrentOrder.TotalCost = newCost;
						} else {
							throw new ArgumentException ("Сума не може бути від'ємною.");
						}
						break;
					case 4:
						Console.Write ($"Виберіть номер столика (1-{maximumTableNumber}): ");
						var tableNumber = int.Parse (Console.ReadLine ());
						if (tableNumber > 0 && tableNumber < maximumTableNumber) {
							CurrentOrder.TableNumber = tableNumber;
						} else {
							throw new ArgumentOutOfRangeException ($"Немає столика №{tableNumber}.");
						}
						break;
					case 5:
						orderList.Add (CurrentOrder);
						done = true;
						break;
					default:
						throw new ArgumentOutOfRangeException ("Немає такої опції.");
					}
				} catch (FormatException) {
					Console.WriteLine ("В числове поле не можна записати таке значення.");
				} catch (ArgumentOutOfRangeException e) {
					Console.WriteLine (e.ParamName);
				}
			}
			return 1;
		}

		private void EditOrderDishesMenu (Order order)
		{
			bool done = false;
			while (!done) {
				Console.WriteLine ("Страви в замовленні:\n");
				Console.WriteLine (order.PrintDishes ());

				Console.WriteLine ("1. Додати страву");
				Console.WriteLine ("2. Редагувати страву");
				Console.WriteLine ("3. Видалити страву");
				Console.WriteLine ("4. Повернутися");

				Console.Write ("~> ");
				try {
					var input = int.Parse (Console.ReadLine ());

					switch (input) {
					case 1:
						AddDishesTo (order);
						break;
					case 2:
						EditSpecificDishMenu (order);
						break;
					case 3:
						Console.Write ("Виберіть страви (розділяйте одним пустим символом): ");
					// TODO: make parseMethod i.g. ParseStringToInt(string[] first, int[] second )
						var dishesToRemove = Console.ReadLine ().Split (' ');
						IEnumerable<int> arrayOfDishIndexes = dishesToRemove.Select (x => int.Parse (x) - 1);
						foreach (var i in arrayOfDishIndexes) {
							// TODO: put this logic into Order.RemoveDishById method (price deduction).
							order.TotalCost -= order.Dishes.ElementAt (i).Price;
							order.Dishes.RemoveAt (i);
						}
						Console.WriteLine ("Видалення успішне.");
						break;
					case 4:
						done = true;
						break;
					default:
						throw new ArgumentOutOfRangeException ("Немає такої опції.");
					}
				} catch (FormatException) {
					Console.WriteLine ("На вході отримано не число.");
				} catch (ArgumentOutOfRangeException e) {
					Console.WriteLine (e.ParamName);
				}
			}
		}
		// TODO: remove all ref's
		private void EditSpecificDishMenu (Order order)
		{
			bool done = false;

			Console.WriteLine (order.PrintDishes ());
			Console.Write ("Виберіть номер страви для редагування: ");
			try {
				var input = int.Parse (Console.ReadLine ()) - 1;
				while (!done) {
					if (input >= 0 && input < order.Dishes.Count) {
						Console.WriteLine (order.Dishes.ElementAt (input));
						Console.WriteLine ("1. Додати інгрідієнтівAdd dish ingredients");
						Console.WriteLine ("2. Видалити інгрідієнти");
						Console.WriteLine ("3. Змінити назву страви");
						Console.WriteLine ("4. Змінити ціну на страву");
						Console.WriteLine ("5. Змінити час приготування страви");
						Console.WriteLine ("6. Повернутися");

						Console.Write ("~> "); 
						switch (int.Parse (Console.ReadLine ())) {
						case 1:
							Console.WriteLine ("Існуючі інгрідієнти: ");
							DisplayPredefinedIngredients ();

							Console.Write ("Виберіть інгрідінти для додавання (розділяйте через один пустий символ) або створіть свій (+): ");
							var ingredientInput = Console.ReadLine ().Split (' ');
							// TODO: try here to collect wrong items written with spaces
							IEnumerable<int> ingredientsArray;
							if (ingredientInput [0].Equals ("+")) {
								order.Dishes [input].AddIngredient (CreateIngredient (predefinedIngredients));
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
							Console.Write ("Виберіть номера інгрідієнтів для видалення (розділяйте через один пустий символ): ");
							// TODO: make parseMethod i.g. ParseStringToInt(string[] first, int[] second )
							// TODO: validate split array
							var ingredientsToRemove = Console.ReadLine ().Split (' ');
							IEnumerable<int> arrayOfIngredientsIndexes = ingredientsToRemove.Select (x => int.Parse (x) - 1);
							foreach (var i in arrayOfIngredientsIndexes) {
								// TODO: put this logic into Order.RemoveDishById method (price deduction).
								// TODO: maybe put this logic to method
								order.Dishes [input].Price -= order.Dishes [input].Ingredients [i].Price;
								order.Dishes [input].RemoveIngredient (order.Dishes [input].Ingredients [i]);
							}
							Console.WriteLine ("Видалення успішне.");
							break;
						case 3:
							Console.Write ("Бажане ім'я для страви: ");
							order.SetDishName (order.Dishes.ElementAt (input), Console.ReadLine ());
							break;
						case 4:
							Console.Write ("Ціна на страву: ");
							var dishPrice = double.Parse (Console.ReadLine ());
							order.SetDishPrice (order.Dishes.ElementAt (input), dishPrice);
							break;
						case 5:
							Console.Write ("Час приготування: ");
							var dishTime = double.Parse (Console.ReadLine ());
							order.SetDishTime (order.Dishes.ElementAt (input), dishTime);
							Console.WriteLine ("Успішне змінений час приготування!\n");
							break;
						case 6:
							done = true;
							break;
						default:
							throw new ArgumentOutOfRangeException ("Немає такої опції.");	
						}
					} else {
						throw new ArgumentOutOfRangeException ("Немає такої опції.");
					}
				}
			} catch (FormatException) {
				Console.WriteLine ("На вході отримано не число.");	
			} catch (ArgumentOutOfRangeException e) {
				Console.WriteLine (e.ParamName);
			} catch (ArgumentNullException) {
				Console.WriteLine ("Отримано пустий параметр.");
			}
		}

		private Ingredient CreateIngredient (List<Ingredient> predefinedIngredients)
		{
			Console.Write ("Введіть назву інгрідієнта: ");
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
				Console.Write ("Введіть ціну інгрідієнта: ");
				inputPrice = double.Parse (Console.ReadLine ());
			} catch (FormatException) {
				Console.WriteLine ("На вході отримано не число.");
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

