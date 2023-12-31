using System.Collections;
using System.Reflection;

namespace CSVFilters.Helpers;

public class CsvReaderHelper
{
   public  static IList PopulateListWithData(string[] data, PropertyInfo[] properties, Type type)
    {
        IList typedList = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(type));
        foreach (var line in data)
        {
            var obj = Activator.CreateInstance(type);
            var elementsInLine = line.Split(',');

            for (int i = 0; i < elementsInLine.Length && i < properties.Length; i++)
            {
                var element = elementsInLine[i].Trim();
                var property = properties[i];

                SetValueForProperty(obj, property, element);
            }

            typedList.Add(obj);
        }
        return typedList;
    }

    static void SetValueForProperty(object obj, PropertyInfo property, string element)
    {
        var convertedValue = Convert.ChangeType(element, property.PropertyType);

        if (property.PropertyType.IsAssignableFrom(convertedValue.GetType()))
        {
            property.SetValue(obj, convertedValue);
        }
        else
        {
            Console.WriteLine(
                $"Type mismatch for property {property.Name}. Expected {property.PropertyType}, but got {convertedValue.GetType()}."
            );
        }
    }
}
