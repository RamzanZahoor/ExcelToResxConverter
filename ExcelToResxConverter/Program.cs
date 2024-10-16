
using System.Data;


using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Resources;
using System.Resources.NetStandard;

class Program
{
	static void Main()
	{
		// Path to your Excel file
		string excelFilePath = "samplefile.xlsx";
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        // Reading Excel file
        using var stream = File.Open(excelFilePath, FileMode.Open, FileAccess.Read);
		using var reader = ExcelReaderFactory.CreateReader(stream);
		var dataSet = reader.AsDataSet();

		// Assuming Name is in Column 0 and Value is in Column 1
		var table = dataSet.Tables[0];  // Get the first sheet

		Dictionary<string, string> nameValuePairs = new Dictionary<string, string>();

		foreach (DataRow row in table.Rows)
		{
			string name = row[0]?.ToString();
			string value = row[1]?.ToString();

			if (!string.IsNullOrWhiteSpace(name) && !nameValuePairs.ContainsKey(name))
			{
				nameValuePairs.Add(name, value);
			}
		}

		// Creating the .resx file
		using var resxWriter = new ResXResourceWriter("Localization.resx");
		foreach (var entry in nameValuePairs)
		{
			resxWriter.AddResource(entry.Key, entry.Value);
		}
		resxWriter.Generate();

		Console.WriteLine("Resx file created successfully!");
	}
}
