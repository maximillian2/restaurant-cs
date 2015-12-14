using System;

namespace BusinessLogic
{
	public class Ingredient : IIngredient
	{
		public string Name { get; set; }
		public double Price { get; set; }

		public Ingredient()
		{
			// Need empty constructor for valid work of serializer
			Name = "";
			Price = 0;
		}

		public Ingredient (string name, double price)
		{
			Name = name;
			Price = price;
		}

		public override string ToString ()
		{
			return string.Format ("{0} ({1})", Name, Price);
		}
	}
}

