using System;
using Polenter.Serialization;
using System.Collections.Generic;
using BusinessLogic;

namespace Restaurant
{
	public static class FileManager
	{
		public static void SerializeCollectionToFile<T>(List<T> collection, string fileName)
		{
			new SharpSerializer ().Serialize (collection, fileName);
		}

		public static List<T> DeserializeCollectionFromFile<T> (string fileName)
		{
			return new SharpSerializer ().Deserialize (fileName) as List<T>;
		}
	}
}

