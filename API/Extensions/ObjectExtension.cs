using System.Reflection;

namespace API.Extensions
{
    public static class ObjectExtensions
    {
        public static Dictionary<string, string> ToDictionary(this object obj)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            PropertyInfo[] properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo property in properties)
            {
                dictionary[property.Name] = property.GetValue(obj)?.ToString();
            }

            return dictionary;
        }
    }
}
