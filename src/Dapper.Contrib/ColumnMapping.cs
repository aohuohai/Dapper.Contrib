using System.Linq;
using System.Reflection;

namespace Dapper.Contrib
{
    internal class ColumnMapping
    {

        public static string ColumnName(string propertyName)
        {
            return DefaultTypeMap.MatchNamesWithUnderscores ? PascalCaseToSnakeCase(propertyName) : propertyName;
        }

        public static string PascalCaseToSnakeCase(string pascalCaseString)
        {
            return string.Concat(pascalCaseString.Select(
                (character, index) => index > 0 && char.IsUpper(character) ? "_" + character.ToString() : character.ToString()))
                .ToLower();
        }

        public static object PascalCaseToSnakeCaseForEntity<T>(T entity)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                dynamicParameters.Add(ColumnName(property.Name), property.GetValue(entity));
            }
            return dynamicParameters;
        }

        public static object PascalCaseToSnakeCaseForEntityNonNullProperty<T>(T entity)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                var val = property.GetValue(entity);
                if (val != null)
                {
                    dynamicParameters.Add(ColumnName(property.Name), val);
                }
            }
            return dynamicParameters;
        }

    }
}
