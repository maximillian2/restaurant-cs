using System;
using System.IO;
using Polenter.Serialization;
using System.Collections.Generic;

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
			if (!File.Exists (fileName)) {
				File.Create (fileName);
			}

			return new SharpSerializer ().Deserialize (fileName) as List<T>;
		}
	}
}

