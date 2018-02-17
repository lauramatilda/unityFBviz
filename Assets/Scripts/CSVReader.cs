using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class CSVReader
{
	static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))"; // REg ex press wonderful
	static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r"; 
	static char[] TRIM_CHARS = { '\"' };

	public static List<Dictionary<string, object>> Read(string file) //Declare method
	{

		var list = new List<Dictionary<string, object>>(); 

		TextAsset data = Resources.Load(file) as TextAsset; 

		var lines = Regex.Split(data.text, LINE_SPLIT_RE); // Split data.text into lines using LINE_SPLIT_RE characters

		if (lines.Length <= 1) return list; 

		var header = Regex.Split(lines[0], SPLIT_RE); 

		// Loops through lines
		for (var i = 1; i < lines.Length; i++)
		{

			var values = Regex.Split(lines[i], SPLIT_RE); //Split lines according to SPLIT_RE, store in var (usually string array)
			if (values.Length == 0 || values[0] == "") continue; // Skip to end of loop (continue) if value is 0 length OR first value is empty

			var entry = new Dictionary<string, object>(); 

			// Loops through every value
			for (var j = 0; j < header.Length && j < values.Length; j++)
			{
				string value = values[j]; 
				value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", ""); 
				object finalvalue = value; 

				int n;

				float f; 

				if (int.TryParse(value, out n))
				{
					finalvalue = n;
				}
				else if (float.TryParse(value, out f))
				{
					finalvalue = f;
				}
				entry[header[j]] = finalvalue;
			}
			list.Add(entry); 
		}
		return list; 
	}
}