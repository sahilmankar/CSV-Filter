using System.Text.RegularExpressions;

namespace CSVFilters.Helpers;
public class CsvToClass
    {
        public static string CSharpClassCodeFromCsvFile(string filePath, out string className,out string[] data)
        {
        

            string[] lines = File.ReadAllLines(filePath);
            string[] columnNames = lines.First().Split(',').Select(str => str.Trim()).ToArray();
            data = lines.Skip(1).ToArray();
            className = Path.GetFileNameWithoutExtension(filePath);

            string code = String.Format(" using System; \n public class {0} {{ \n",  className);

            for (int columnIndex = 0; columnIndex < columnNames.Length; columnIndex++)
            {
                var columnName = Regex.Replace(columnNames[columnIndex], @"[\s\.]", string.Empty, RegexOptions.IgnoreCase);
                if (string.IsNullOrEmpty(columnName))
                    columnName = "Column" + (columnIndex + 1);
                code += "\t" + GetVariableDeclaration(data, columnIndex, columnName) + "\n\n";
            }

            code += "}\n";
            return code;
        }

        public static string GetVariableDeclaration(string[] data, int columnIndex, string columnName)
        {
            string[] columnValues = data.Select(line => line.Split(',')[columnIndex].Trim()).ToArray();
            string typeAsString;

            if (AllDateTimeValues(columnValues))
            {
                typeAsString = "DateTime";
            }
            else if (AllIntValues(columnValues))
            {
                typeAsString = "int";
            }
            else if (AllDoubleValues(columnValues))
            {
                typeAsString = "double";
            }
            else
            {
                typeAsString = "string";
            }

            string declaration = String.Format("public {0} {1} {{ get; set; }}",  typeAsString, columnName);
            return declaration;
        }

        public static bool AllDoubleValues(string[] values)
        {
            double d;
            return values.All(val => string.IsNullOrEmpty(val) || double.TryParse(val, out d));
        }

        public static bool AllIntValues(string[] values)
        {
            int d;
            return values.All(val => string.IsNullOrEmpty(val) || int.TryParse(val, out d));
        }

        public static bool AllDateTimeValues(string[] values)
        {
            DateTime d;
            return values.All(val => string.IsNullOrEmpty(val) || DateTime.TryParse(val, out d));
        }

    }