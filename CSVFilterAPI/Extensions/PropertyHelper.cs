using System.Reflection;

namespace EmployeeAPI.Extensions;

public class PropertyHelper
{
    public static FilterPropertiesList GetAllProperties(Type type)
    {
        PropertyInfo[] propertiesInfo = type.GetProperties();
      

        FilterPropertiesList properties = new FilterPropertiesList()
        {
            EqualProperties = GetEqualProperties(propertiesInfo),
            DateRangeProperties = GetDateRangeProperties(propertiesInfo),
            RangeProperties = GetRangeProperties(propertiesInfo),
            AllProperties = GetPropertyNames(propertiesInfo)
        };
        return properties;
    }

    public static List<string> GetPropertyNames(PropertyInfo[] properties)
    {
        var propertyNames = properties
            .Where(
                property => !property.Name.Contains("Id") && property.CanRead && property.CanWrite
            )
            .Select(property => property.Name)
            .ToList();

        return propertyNames;
    }

    public static List<string> GetEqualProperties(PropertyInfo[] properties)
    {
        var propertyNames = properties
            .Where(
                property =>
                    property.GetSetMethod() != null && property.PropertyType == typeof(string)
            )
            .Select(property => property.Name)
            .ToList();

        return propertyNames;
    }

    public static List<string> GetRangeProperties(PropertyInfo[] properties)
    {
        var propertyNames = properties
            .Where(
                property =>
                    !property.Name.Contains("Id")
                    && property.GetSetMethod() != null
                    && (
                        property.PropertyType == typeof(int)
                        || property.PropertyType == typeof(double)
                    )
            )
            .Select(property => property.Name)
            .ToList();
        return propertyNames;
    }

    public static List<string> GetDateRangeProperties(PropertyInfo[] properties)
    {
        var propertyNames = properties
            .Where(
                property =>
                    property.GetSetMethod() != null && property.PropertyType == typeof(DateTime)
            )
            .Select(property => property.Name)
            .ToList();
        return propertyNames;
    }
}
