using System;
using System.Collections.Generic;

namespace Restaurant
{
	public class Order
	{
		public List<Dish> Dishes { get; set; }
		// тз кривое, сумму написано сделать интовой, лол
		public float TotalCost { get; set; }
		public float TableNumber { get; set; }

		public Order ()
		{
			TotalCost = 0;
			TableNumber = 0;
			Dishes = new List<Dish> ();
		}

		public void AddDish(Dish dish)
		{
			Dishes.Add (dish);
		}

		public void RemoveDish(Dish dish)
		{
			Dishes.Remove (dish);
		}

		public string GetDishName(Dish dish)
		{
			return dish.Name;
		}

		public void SetDishName(Dish dish, string name)
		{
			// make name validation here
			// or override default setter
			dish.Name = name;
		}

		public void ChangeDishName(Dish dish, string name)
		{
			SetDishName (dish, name);
		}

		public double GetDishPrice(Dish dish)
		{
			return dish.Price;
		}

		public void SetDishPrice(Dish dish, double price)
		{
			// some validation here
			dish.Price = price;
		}

		public void ChangeDishPrice(Dish dish, double newPrice)
		{
			SetDishPrice (dish, newPrice);
		}
	}
}

