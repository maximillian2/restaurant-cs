using NUnit.Framework;
using System;
//using Restaurant;
using System.Collections.Generic;
using BusinessLogic;

namespace Restaurant.Tests
{
	[TestFixture ()]
	public class IngredientsTests
	{
		private BusinessLogic.Dish dish;
		private BusinessLogic.Ingredient ingredient1, ingredient2;

		[SetUp]
		public void Initialize()
		{
			dish = new BusinessLogic.Dish ();
			ingredient1 = new BusinessLogic.Ingredient ("sample1", 10);
			ingredient2 = new BusinessLogic.Ingredient ("sample2", 15);
		}

		[Test]
		public void IngredientAddedToDish()
		{
			dish.AddIngredient (ingredient1);
			Assert.Contains (ingredient1, dish.Ingredients);
		}

		[Test]
		public void IngredientRemoved()
		{
			dish.AddIngredient (ingredient1);
			dish.RemoveIngredient (ingredient1);
			Assert.That (dish.Ingredients.Count == 0);
		}

		[Test]
		public void IngredientChanged()
		{
			var list = new List<Ingredient> {ingredient1};

			dish.AddIngredient (ingredient2);
			dish.ChangeIngredient (ingredient2, ingredient1);

			Assert.AreEqual (list, dish.Ingredients);
		}

		[Test]
		public void IngredientsPrinted()
		{
			var result = "-> Назва: sample1, Ціна: 10\n-> Назва: sample2, Ціна: 15\n";

			dish.AddIngredient (ingredient1);
			dish.AddIngredient (ingredient2);

			Assert.AreEqual (dish.PrintIngredients(), result);
		}
	}
}

