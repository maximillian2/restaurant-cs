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
		public void IngredientRemovedById()
		{
			dish.AddIngredient (ingredient1);
			dish.AddIngredient (ingredient2);

			dish.RemoveIngredientById (0);
			Assert.That (dish.Ingredients.Count == 1);
		}

		[Test]
		public void IngredientsPrinted()
		{
			var result = "\t1. sample1 (10)\n\t2. sample2 (15)\n";

			dish.AddIngredient (ingredient1);
			dish.AddIngredient (ingredient2);

			Assert.AreEqual (dish.PrintIngredients(), result);
		}
	}
}

