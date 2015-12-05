using NUnit.Framework;
using System;
using Restaurant;

namespace Restaurant.Tests
{
	[TestFixture ()]
	public class OrdersTests
	{
		private Order order;

		[SetUp]
		public void BeforeRun()
		{
			order = new Order ();	
		}

		[Test]
		public void DishAdded ()
		{
			var dish1 = new Dish ();
			dish1.AddIngredient (new Ingredient ("Carrot", 10));
			dish1.AddIngredient (new Ingredient ("Water", 1));
			dish1.AddIngredient (new Ingredient ("Salt", 0.5));

			order.AddDish (dish1);
			Assert.Contains (dish1, order.Dishes);
		}

		[Test]
		public void DishRemoved()
		{
			var dish1 = new Dish ();
			dish1.AddIngredient (new Ingredient ("Carrot", 10));

			order.AddDish (dish1);
			order.RemoveDish (dish1);

			Assert.That (order.Dishes.Count == 0);
		}

		[Test]
		public void DishNameChanged()
		{
			var dish1 = new Dish ();
			dish1.Name = "Borsch";
			order.ChangeDishName (dish1, "Vareniki");

			Assert.AreEqual (order.GetDishName(dish1), "Vareniki");
		}

		[Test]
		public void DishPriceChanged()
		{
			var dish1 = new Dish { Price = 23.5 };
			order.ChangeDishPrice (dish1, 35.25);
			Assert.AreEqual (order.GetDishPrice(dish1), 35.25);
		}

		[Test]
		public void DishTimeChanged()
		{
			var dish1 = new Dish { Time = 10.5 };
			order.ChangeDishTime (dish1, 18.25);
			Assert.AreEqual (order.GetDishTime(dish1), 18.25);
		}

		[Test]
		public void DishInfoPrinted()
		{
			var dishInfo = "Страва: salad\nІнгредієнти:\n-> Назва: Carrot, Ціна: 10\n-> Назва: Potato, Ціна: 20\nЦіна: 10\nЧас приготування: 5\n";

			var dish1 = new Dish { Name = "salad", Price = 10, Time = 5 };
			dish1.AddIngredient (new Ingredient("Carrot", 10));
			dish1.AddIngredient (new Ingredient("Potato", 20));
			order.AddDish (dish1);

			Assert.AreEqual(order.GetDishInfo (dish1), dishInfo);
		}
	}
}

