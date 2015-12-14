using System;
using System.Collections.Generic;
using System.Text;
using System.Linq; // for ElementAt() method

namespace BusinessLogic
{
	public class Order : IOrder
	{
		public List<Dish> Dishes { get; set; }

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

		public void SetDishName (Dish dish, string name)
		{
			dish.Name = name;
		}

		public void SetDishPrice (Dish dish, double price)
		{
			dish.Price = price;
		}

		public void SetDishCookTime (Dish dish, double dishCookTime)
		{ 
			dish.Time = dishCookTime;
		}

		public string PrintDishes ()
		{
			var builder = new StringBuilder ();

			for (int i = 0; i < Dishes.Count; i++) {
				builder.Append ($"{i+1}. ").Append (Dishes[i]).Append("\n");
			}
			return builder.ToString ();
		}

		public void RemoveDishById (int id)
		{
			TotalCost -= Dishes.ElementAt (id).Price;
			Dishes.RemoveAt (id);
		}

		public override string ToString ()
		{
			return string.Format ("Загальна вартість: {0}\nНомер столика: {1}\n", TotalCost, TableNumber);
		}

		public void UpdateTotalCost()
		{
			TotalCost = 0;
			foreach (var d in Dishes) {
				TotalCost += d.Price;
			}
		}
	}
}

