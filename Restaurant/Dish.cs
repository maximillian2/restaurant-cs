using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Restaurant
{
	public class Dish
	{
		public string Name { get; set; }
		public List<Ingredient> Ingredients { get; set; }
		public double Price { get; set; }
		public double Time { get; set; }

		private void Setup()
		{
			Price = 0;
			Time = 0;
			Ingredients = new List<Ingredient> ();
		}

		public Dish ()
		{
			Setup ();
		}

		public Dish(string name, List<Ingredient> ingredients, double price, double time)
		{
			Setup ();

			Name = name;
			Ingredients = ingredients;
			foreach (var i in Ingredients) {
				Price += i.Price;
			}

			Price *= price;

			Time = time;
		}

		public void AddIngredient(Ingredient ingredient)
		{
			Ingredients.Add (ingredient);
			Price += ingredient.Price;
		}

		public void RemoveIngredient(Ingredient ingredient)
		{
			Price -= ingredient.Price;
			Ingredients.Remove (ingredient);
		}

		public void RemoveIngredientById(int id)
		{
			Price -= Ingredients.ElementAt (id).Price;
			Ingredients.RemoveAt (id);
		}

		public void ChangeIngredient(Ingredient oldIngredient, Ingredient newIngredient)
		{
			this.RemoveIngredient (oldIngredient);
			this.AddIngredient (newIngredient);
		}

		public string PrintIngredients()
		{
			var builder = new StringBuilder ();
			for (int i = 0; i < Ingredients.Count; i++) {
				builder.Append($"\t{i+1}. ").Append (Ingredients[i]).Append("\n");
			}
//			foreach (var ingredient in Ingredients) 
//			{
//				builder.Append (ingredient).Append("\n");
//			}

			return builder.ToString ();
		}

		public override string ToString ()
		{
			return string.Format ("Страва: {0}\nІнгредієнти:\n{1}Ціна: {2}\nЧас приготування: {3}\n", Name, this.PrintIngredients(), Price, Time);
		}
	}
}

