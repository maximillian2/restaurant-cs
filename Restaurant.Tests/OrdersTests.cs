using NUnit.Framework;
using System;
using BusinessLogic;
using System.Collections.Generic;

namespace Restaurant.Tests
{
	[TestFixture ()]
	public class OrdersTests
	{
		private BusinessLogic.Order order;

		[SetUp]
		public void BeforeRun()
		{
			order = new BusinessLogic.Order ();	
		}

		[Test]
		public void DishAdded ()
		{
			var dish1 = new BusinessLogic.Dish ();
			dish1.AddIngredient (new BusinessLogic.Ingredient ("Carrot", 10));
			dish1.AddIngredient (new BusinessLogic.Ingredient ("Water", 1));
			dish1.AddIngredient (new BusinessLogic.Ingredient ("Salt", 0.5));

			order.AddDish (dish1);
			Assert.Contains (dish1, order.Dishes);
		}

		[Test]
		public void DishNameSet()
		{
			var dish1 = new BusinessLogic.Dish ();
			var dishName = "sample_name";

			order.AddDish (dish1);
			order.SetDishName (dish1, dishName);

			Assert.AreSame (order.Dishes[0].Name, dishName);
		}

		[Test]
		public void DishPriceSet()
		{
			var dish1 = new BusinessLogic.Dish ();
			var dishPrice = 10.4;

			order.AddDish (dish1);
			order.SetDishPrice (dish1, dishPrice);
			Assert.AreEqual (dishPrice, order.Dishes[0].Price);
		}

		[Test]
		public void DishCookTimeSet()
		{
			var dish1 = new BusinessLogic.Dish ();
			var dishCookTime = 5.25;

			order.AddDish (dish1);
			order.SetDishCookTime (dish1, dishCookTime);

			Assert.AreEqual (dishCookTime, order.Dishes [0].Time);
		}

		[Test]
		public void DishRemovedById()
		{
			var dish1 = new BusinessLogic.Dish ();
			var id = 0;

			order.AddDish (dish1);
			order.RemoveDishById (id);

			Assert.That (order.Dishes.Count == 0);
		}

		[Test]
		public void TotalCostUpdated()
		{
			var dish1 = new BusinessLogic.Dish ("sample", new List<Ingredient> { new Ingredient("1", 10), new Ingredient("2", 10) }, 4, 10);
			order.TotalCost = 0;

			order.AddDish (dish1);
			order.UpdateTotalCost ();

			Assert.AreEqual (order.TotalCost, dish1.Price);
		}
	}
}

