using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Xml.Serialization;
using System.IO;

namespace Restaurant
{
	public class Controller
	{
		// Main store collections
		private List<Order> orderList;
		private List<Dish> predefinedDishes;
		private List<Ingredient> predefinedIngredients;

		// Configuration dependent values
		private double minimalCookTime;
		private double dishMargin;
		private int maximumTableNumber;
		private string ordersFile;
		private string predefinedDishesFile;
		private string predefinedIngredientsFile;



		public Order CurrentOrder { get; set; }

		// Method to print out promt with configurable foreground color
		private void CommandPromtWithColor (ConsoleColor color)
		{
			Console.ForegroundColor = color;
			Console.Write ("~> ");
			Console.ResetColor ();
		}

		public Controller ()
		{
			orderList = new List<Order> ();
			predefinedDishes = new List<Dish> ();
			predefinedIngredients = new List<Ingredient> ();
			CurrentOrder = null;

			// Reading configuration file values
			try {
				// Data files
				ordersFile = ConfigurationManager.AppSettings ["orders"];
				predefinedDishesFile = ConfigurationManager.AppSettings ["predefinedDishes"];
				predefinedIngredientsFile = ConfigurationManager.AppSettings ["predefinedIngredients"];

				minimalCookTime = Convert.ToDouble (ConfigurationManager.AppSettings ["minimalCookTime"]);
				dishMargin = Convert.ToDouble (ConfigurationManager.AppSettings ["dishMargin"]);
				maximumTableNumber = Convert.ToInt32 (ConfigurationManager.AppSettings ["maximumTableNumber"]);
				// Runtime store collections
				predefinedDishes = FileManager.DeserializeCollectionFromFile<Dish>(predefinedDishesFile);
				predefinedIngredients = FileManager.DeserializeCollectionFromFile<Ingredient>(predefinedIngredientsFile);
				orderList = FileManager.DeserializeCollectionFromFile<Order>(ordersFile);
			} catch (FormatException) {
				Console.WriteLine ("Помилка при читанні конфігураційного файла.");
			} catch (Exception e) {
				Console.WriteLine ("{0} -> {1}", e.Message, e.InnerException.Message);
			}
		}

		// The only method available outside the class and meant to be called to start the program
		public void Run ()
		{			
			// Nice "restaurant" ASCII label here
			string welcomeLabel = "______          _                              _   \n| ___ \\        | |                            | |  \n| |_/ /___  ___| |_ __ _ _   _ _ __ __ _ _ __ | |_ \n|    // _ \\/ __| __/ _` | | | | '__/ _` | '_ \\| __|\n| |\\ \\  __/\\__ \\ || (_| | |_| | | | (_| | | | | |_ \n\\_| \\_\\___||___/\\__\\__,_|\\__,_|_|  \\__,_|_| |_|\\__|\n                                                   \n                                                   ";
			Console.WriteLine (welcomeLabel);
			// Loop to hold user inside the program until he wants to exit it himself
			while (ShowMainMenu () == 1)
				;

			// After exiting the main loop all data from runtime store collections
			// is being written to external XML data files 
			Console.Write ("Збереження даних... ");
			FileManager.SerializeCollectionToFile (orderList, ordersFile);
			FileManager.SerializeCollectionToFile (predefinedDishes, predefinedDishesFile);
			FileManager.SerializeCollectionToFile (predefinedIngredients, predefinedIngredientsFile);
			Console.WriteLine ("успішно!");
		}

		private int ShowMainMenu ()
		{
			// Store order number 
			int parsedInput = 0;
		
			if (orderList.Count == 0) {
				Console.WriteLine ("Немає замовлень. Створюємо...");
				CurrentOrder = CreateOrder ();
			} else {
				Console.WriteLine ("Існуючі замовлення:");
				DisplayOrders ();
				Console.WriteLine ($"Виберіть номер замовлення або створіть нове (1-{orderList.Count}/+), подвійний Enter для виходу: ");
				CommandPromtWithColor (ConsoleColor.Cyan);
					
				var input = Console.ReadLine ();
				try {
					bool isNumber = int.TryParse (input.ToString (), out parsedInput);
					if (isNumber) {
						CurrentOrder = orderList.ElementAt (parsedInput - 1);
						orderList.RemoveAt (parsedInput - 1);
					} else {
						if (input.ToString ().Equals ("+")) {
							CurrentOrder = CreateOrder ();
							// Incrementing number of all orders to get new order index.
							parsedInput = orderList.Count + 1;
						} else if (input.ToString ().Equals ("")) {
							// First Enter pressed
							Console.WriteLine ("Натисніть ще раз для виходу");
							CurrentOrder = null;
							if (Console.ReadLine ().Equals ("")) 
								// Second Enter pressed
								return 0;
							else
								return 1;
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
			return OrderMenu (CurrentOrder, parsedInput);
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
			for (int i = 0; i < predefinedDishes.Count; i++)
				Console.WriteLine ($"{i+1}. {predefinedDishes[i]}\n");
		}

		private void DisplayOrders ()
		{
			for (int i = 0; i < orderList.Count; i++)
				Console.WriteLine ($"\ud83c\udf54 Замовлення #{i+1}:\n{orderList[i]}");
		}

		private void AddDishesTo (Order order)
		{
			Console.WriteLine ("Меню: ");
			DisplayPredefinedDishes ();
			Console.WriteLine ("Виберіть страви (перелічуйте через один пустий символ) або створіть нову (+): ");
			do {
				CommandPromtWithColor (ConsoleColor.Cyan);
				try {
					var dishesToAdd = Console.ReadLine ().Split (' ');
					IEnumerable<int> parsedDishesToAdd;
					if (dishesToAdd [0].Equals ("+")) {
						order.AddDish (CreateDish ());
					} else {
						parsedDishesToAdd = dishesToAdd.Select (x => int.Parse (x) - 1);
						foreach (var i in parsedDishesToAdd) {
							order.AddDish (predefinedDishes [i]);
						}
					}
				} catch (Exception) {
					Console.WriteLine ("Помилка при доданні страви до замовлення. Спробуйте ще раз!");
					continue;
				}
				break;
			} while(true);
		}

		private Dish CreateDish ()
		{
			Console.Write ("Введіть назву страви: ");

			// Dish name can contain any characters
			var dishName = Console.ReadLine ();
			var cookTime = 0.0;
			var ingredients = new List<Ingredient> ();

			DisplayPredefinedIngredients ();
			Console.WriteLine ("Виберіть інгрідієнти (перелічуйте через один пустий символ) або створіть новий (+) (буде автоматично додано до замовлення): ");
			do {	
				CommandPromtWithColor (ConsoleColor.Cyan);
				try {
					var ingredientsToAdd = Console.ReadLine ().Split (' ');
					IEnumerable<int> parsedIngredientsToAdd;
					if (ingredientsToAdd [0].Equals ("+")) {
						ingredients.Add (CreateIngredientUsing (predefinedIngredients));
					} else {
						parsedIngredientsToAdd = ingredientsToAdd.Select (x => int.Parse (x) - 1);
						foreach (var i in parsedIngredientsToAdd) {
							ingredients.Add (predefinedIngredients [i]);
						}
					}
				} catch (Exception) {
					Console.WriteLine ("Помилка при створенні страви. Спробуйте ще раз!");
					continue;
				}
				break;
			} while(true);

			do {
				Console.Write ("Введіть час приготування (в хв.): ");
				try {
					cookTime = double.Parse (Console.ReadLine ());
					if (cookTime < minimalCookTime) {
						throw new ArgumentException ("Час приготування має бути більшим.");
					}
				} catch (Exception) {
					Console.WriteLine ("Помилка при введенні часу приготування. Спробуйте ще раз!");
					continue;
				}
				break;
			} while(true);

			var dish = new Dish (dishName, ingredients, dishMargin, cookTime);
			predefinedDishes.Add (dish);

			return dish; 
		}

		private int OrderMenu (Order order, int orderNumber)
		{
			int userOption = 0;
			bool finished = false;

			while (!finished) {
				Console.WriteLine ($"Працюємо із замовленням №{orderNumber}");
				Console.WriteLine (CurrentOrder.ToString ());
				Console.WriteLine ("Виберіть опцію:");
				Console.WriteLine ("1. Видалити замовлення");
				Console.WriteLine ("2. Редагувати замовлення");
				Console.WriteLine ("3. Задати кінцеву ціну (???)");
				Console.WriteLine ("4. Задати номер столика");
				Console.WriteLine ("5. Оформити замовлення");
				CommandPromtWithColor (ConsoleColor.Cyan);
				try { 
					userOption = int.Parse (Console.ReadLine ());
				} catch (FormatException) {
					Console.WriteLine ("На вході отримано не число.");
				}

				try {
					switch (userOption) {
					case 1:
						orderList.Remove (CurrentOrder);
						Console.WriteLine ("Поточне замовлення видалене. Повертаємось в головне меню");
						finished = true;
						break;
					case 2:
						EditOrderDishesMenu (CurrentOrder);
						break;
					case 3:
						Console.Write ("Введіть прийнятне число (в регіональній валюті): ");
						var newOrderTotalCost = float.Parse (Console.ReadLine ());
						if (newOrderTotalCost > 0) {
							CurrentOrder.TotalCost = newOrderTotalCost;
						} else {
							throw new ArgumentException ("Сума не може бути від'ємною.");
						}
						break;
					case 4:
						Console.Write ($"Виберіть номер столика (1-{maximumTableNumber}): ");
						var tableNumber = int.Parse (Console.ReadLine ());
						if (tableNumber > 0 && tableNumber <= maximumTableNumber) {
							CurrentOrder.TableNumber = tableNumber;
						} else {
							throw new ArgumentOutOfRangeException ($"Немає столика №{tableNumber}.");
						}
						break;
					case 5:
						orderList.Add (CurrentOrder);
						finished = true;
						break;
					default:
						throw new ArgumentOutOfRangeException ("Немає такої опції.");
					}
				} catch (FormatException) {
					Console.WriteLine ("В числове поле не можна записати таке значення.");
				} catch (ArgumentOutOfRangeException e) {
					Console.WriteLine (e.ParamName);
				} catch (Exception e) {
					Console.WriteLine (e.Message);
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
				CommandPromtWithColor (ConsoleColor.Cyan);
				try {
					var userOption = int.Parse (Console.ReadLine ());

					switch (userOption) {
					case 1:
						AddDishesTo (order);
						break;
					case 2:
						EditSpecificDishMenuIn (order);
						break;
					case 3:
						do {
							Console.Write ("Виберіть страви (розділяйте одним пустим символом): ");
							try {
								var dishesToRemove = Console.ReadLine ().Split (' ');
								IEnumerable<int> parsedDishesToRemove = dishesToRemove.Select (x => int.Parse (x) - 1);
								foreach (var i in parsedDishesToRemove) {
									order.RemoveDishById (i);
								}
								Console.WriteLine ("Видалення успішне.");
							} catch (Exception) {
								Console.WriteLine ("Проблема при видаленні страв(и)");
								continue;
							}
							break;
						} while(true);
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

		private void EditSpecificDishMenuIn (Order currentOrder)
		{
			bool done = false;

			Console.Write ("Виберіть номер страви для редагування: ");
			try {
				var userOption = int.Parse (Console.ReadLine ()) - 1;
				while (!done) {
					if (userOption >= 0 && userOption < currentOrder.Dishes.Count) {
						Console.WriteLine (currentOrder.Dishes.ElementAt (userOption));
						Console.WriteLine ("1. Додати інгрідієнтів");
						Console.WriteLine ("2. Видалити інгрідієнти");
						Console.WriteLine ("3. Змінити назву страви");
						Console.WriteLine ("4. Змінити ціну на страву");
						Console.WriteLine ("5. Змінити час приготування страви");
						Console.WriteLine ("6. Повернутися");
						CommandPromtWithColor (ConsoleColor.Cyan);

						switch (int.Parse (Console.ReadLine ())) {
						case 1:
							Console.WriteLine ("Існуючі інгредієнти: ");
							DisplayPredefinedIngredients ();
							do {
								Console.Write ("Виберіть інгредінти для додавання (розділяйте через один пустий символ) або створіть свій (+): ");
								var ingredientsInput = Console.ReadLine ().Split (' ');
								try {
									IEnumerable<int> parsedIngedientsInput;
									if (ingredientsInput [0].Equals ("+")) {
										currentOrder.Dishes.ElementAt (userOption).AddIngredient (CreateIngredientUsing (predefinedIngredients));
									} else {
										parsedIngedientsInput = ingredientsInput.Select (x => int.Parse (x) - 1);
										foreach (var i in parsedIngedientsInput) {
											currentOrder.Dishes.ElementAt (userOption).AddIngredient (predefinedIngredients.ElementAt (i));
										}
									}
								} catch (Exception) {
									Console.WriteLine ("Помилка при додаванні інгредієнтів.");
									continue;
								}
								break;
							} while(true);
							break;
						case 2:
							do {
								Console.Write ("Виберіть номера інгредієнтів для видалення (розділяйте через один пустий символ): ");
								var ingredientsToRemove = Console.ReadLine ().Split (' ');
								try {
									IEnumerable<int> parsedIngredientsToRemove = ingredientsToRemove.Select (x => int.Parse (x) - 1);
									foreach (var i in parsedIngredientsToRemove) {
										currentOrder.Dishes.ElementAt (userOption).RemoveIngredientById (i);
									}
								} catch (Exception) {
									Console.WriteLine ("Помилка при введенні номерів інгредієнтів для видалення.");
									continue;
								}
								Console.WriteLine ("Видалення успішне.");	
								break;
							} while (true);
							break;
						case 3:
							Console.Write ("Бажане ім'я для страви: ");
							currentOrder.SetDishName (currentOrder.Dishes.ElementAt (userOption), Console.ReadLine ());
							break;
						case 4:
							do {
								Console.Write ("Ціна на страву: ");
								try {
									var dishPrice = double.Parse (Console.ReadLine ());
									currentOrder.SetDishPrice (currentOrder.Dishes.ElementAt (userOption), dishPrice);
								} catch (Exception) {
									continue;
								}
								break;
							} while(true);
							break;
						case 5:
							Console.Write ("Час приготування: ");
							var dishTime = double.Parse (Console.ReadLine ());
							currentOrder.SetDishTime (currentOrder.Dishes.ElementAt (userOption), dishTime);
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

		private Ingredient CreateIngredientUsing (List<Ingredient> predefinedIngredients)
		{
			Console.Write ("Введіть назву інгрідієнта: ");
			string ingredientName = Console.ReadLine ();
			double ingredientPrice = 0;

			try {
				if (!(new Regex (@"[A-Za-z\p{IsCyrillic}]+").IsMatch (ingredientName))) {
					throw new FormatException ();
				}
				Console.Write ("Введіть ціну інгрідієнта: ");
				ingredientPrice = double.Parse (Console.ReadLine ());
			} catch (FormatException) {
				Console.WriteLine ("Назва інгредієнта містить недопустимі символи.");
			}

			var newIngredient = new Ingredient (ingredientName, ingredientPrice);
		
			predefinedIngredients.Add (newIngredient);
			return newIngredient;
		}

		private void DisplayPredefinedIngredients ()
		{
			for (int i = 0; i < predefinedIngredients.Count; i++) {
				Console.WriteLine ($"{i+1}. {predefinedIngredients[i]}");
			}
		}
	}
}

