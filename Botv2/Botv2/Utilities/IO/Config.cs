using System;
using System.IO;
using System.Collections.Generic;

namespace Bot
{
	public class Config
	{
		static public String getValue(string key)
		{
			var config = File.OpenText("./config.cfg");
			while (config.BaseStream.CanSeek)
			{
				String[] entry = config.ReadLine().Split('=');
				if (entry[0].Equals(key))
				{
					config.Close();
					return entry[1].ToString();
				}
			}
			config.Close();
			return null;
		}

		static public Boolean containsKey(string key)
		{
			var config = File.OpenText("./config.cfg");
			while (config.BaseStream.CanSeek)
			{
				string line = config.ReadLine();
				if (line == null)
				{
					config.Close();
					return false;
				}

				String[] entry = line.Split('=');

				if (entry[0].Equals(key))
				{
					config.Close();
					return true;
				}
			}
			config.Close();
			return false;
		}

		static public void setValue(string key, object value)
		{
			var config = File.OpenText("./config.cfg");
			SortedList<string, string> entries = new SortedList<string, string>();

			while (config.Peek() >= 0)
			{
				string temp = config.ReadLine();

				if (temp != null)
				{
					string[] entry = temp.Split('=');
					entries.Add(entry[0], entry[1]);
				}
			}
			config.Close();

			if (!entries.ContainsKey(key)) entries.Add(key, value.ToString());
			else entries[key] = value.ToString();

			List<string> listToSave = new List<string>();
			for (int i = 0; i < entries.Count; i++) listToSave.Add(entries.Keys[i] + "=" + entries.Values[i]);

			File.WriteAllLines("./config.cfg", listToSave);
		}
	}
}