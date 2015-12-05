using System;
using System.Collections.Generic;
using System.Text;
//using System.Text.RegularExpressions;

namespace Restaurant
{
	public class Dish
	{
		public string Name { get; set; }
		public List<Ingredient> Ingredients { get; set; }
		public double Price { get; set; }
		public double Time { get; set; }

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

		public override string ToString ()
		{
			return string.Format ("Страва: {0}\nІнгредієнти:\n{1}Ціна: {2}\nЧас приготування: {3}\n", Name, this.PrintIngredients(), Price, Time);
		}
	}
}

