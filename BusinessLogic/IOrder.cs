using System;
using System.Collections.Generic;

namespace BusinessLogic
{
	public interface IOrder
	{
		List<Dish> Dishes { get; set; }

		double TotalCost { set; }

		int TableNumber { set; }

		void AddDish (Dish dish);

		void SetDishName (Dish dish, string dishName);

		void SetDishPrice (Dish dish, double dishPrice);

		void SetDishCookTime (Dish dish, double dishTime);

		string PrintDishes ();

		void RemoveDishById (int id);
	}
}

