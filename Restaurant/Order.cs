using System;
using System.Collections.Generic;
using System.Text;
using System.Linq; // for ElementAt() method

namespace Restaurant
{
	public class Order
	{
		public List<Dish> Dishes { get; set; }
		// тз кривое, сумму написано сделать интовой, лол
		public double TotalCost { get; set; }

		public double TableNumber { get; set; }

		public Order ()
		{
			TotalCost = 0;
			TableNumber = 0;
			Dishes = new List<Dish> ();
		}

		public void AddDish (Dish dish)
		{
			Dishes.Add (dish);
			TotalCost += dish.Price;
		}

		public void RemoveDish (Dish dish)
		{
			Dishes.Remove (dish);
		}

		public string GetDishName (Dish dish)
		{
			return dish.Name;
		}

		public void SetDishName (Dish dish, string name)
		{
			// make name validation here
			// or override default setter
			dish.Name = name;
		}

		public void ChangeDishName (Dish dish, string newName)
		{
			SetDishName (dish, newName);
		}

		public double GetDishPrice (Dish dish)
		{
			return dish.Price;
		}

		public void SetDishPrice (Dish dish, double price)
		{
			// some validation here
			dish.Price = price;
		}

		public void ChangeDishPrice (Dish dish, double newPrice)
		{
			SetDishPrice (dish, newPrice);
		}

		public double GetDishTime (Dish dish)
		{
			return dish.Time;
		}

		public void SetDishTime (Dish dish, double newTime)
		{ 
			// validation time!
			dish.Time = newTime;
		}

		public void ChangeDishTime (Dish dish, double time)
		{
			SetDishTime (dish, time);
		}

		public string GetDishInfo (Dish dish)
		{
			return dish.ToString ();
		}

		public string PrintDishes ()
		{
			var builder = new StringBuilder ();

			for (int i = 0; i < Dishes.Count; i++) {
				builder.Append($"{i+1}. ").Append (Dishes[i]).Append("\n");
			}
			return builder.ToString ();
		}
		public void RemoveDishById(int id)
		{
			TotalCost -= Dishes.ElementAt (id).Price;
			Dishes.RemoveAt (id);
		}

		public override string ToString ()
		{
			return string.Format ("Загальна вартість: {0}\nНомер столика: {1}\n", TotalCost, TableNumber);
		}
	}
}

