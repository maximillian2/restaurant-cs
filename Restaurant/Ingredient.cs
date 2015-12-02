using System;

namespace Restaurant
{
	public class Ingredient
	{
		public string Name { get; set; }
		public double Price { get; set; }

		public Ingredient (string name, float price)
		{
			Name = name;
			Price = price;
		}
			
		public override string ToString ()
		{
			return string.Format ("-> Інгредієнт: Назва={0}, Ціна={1}]", Name, Price);
		}
	}
}	

