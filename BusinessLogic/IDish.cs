using System;

namespace BusinessLogic
{
	public interface IDish
	{
		void AddIngredient (Ingredient ingredient);

		void RemoveIngredient (Ingredient ingredient);

		void RemoveIngredientById (int id);

		string PrintIngredients ();
	}
}

