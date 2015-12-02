using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant
{
	public class Dish
	{
		public string Name { get; set; }
		public List<Ingredient> Ingredients { get; set; }
		public float Price { get; set; }
		public float Time { get; set; }

		public Dish ()
		{
			Price = 0;
			Time = 0;
			Ingredients = new List<Ingredient> ();
		}

		public void AddIngredient(Ingredient ingredient)
		{
			Ingredients.Add (ingredient);
		}

		public void RemoveIngredient(Ingredient ingredient)
		{
			Ingredients.Remove (ingredient);
		}

		public void ChangeIngredient(Ingredient oldIngredient, Ingredient newIngredient)
		{
			this.RemoveIngredient (oldIngredient);
			this.AddIngredient (newIngredient);
		}

		public string PrintIngredients()
		{
			var builder = new StringBuilder ();
			foreach (var i in Ingredients) 
			{
				builder.Append (i).Append("\n");
			}

			return builder.ToString ();
		}
	}
}

