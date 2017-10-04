using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace CustomComparer
{
    public static class ExtensionMethods
    {
        private static bool IsEqualTo(object o1, object o2, int maxRecursionLevel, int level)
        {
            if (level == maxRecursionLevel)
            {
                return true;
            }
            if ((o1 == null) && (o2 == null))
            {
                return true;
            }
            if (((o1 == null) && (o2 != null)) || ((o1 != null) && (o2 == null)))
            {
                return false;
            }

            var objType = o1.GetType();
            if (!objType.Equals(o2.GetType()))
            {
                return false;
            }

            // value type or some sort of string 
            if ((objType.IsValueType) || (typeof(string).IsAssignableFrom(objType)))
            {
                return o1.Equals(o2);
            }

            // covers collections, lists
            if (typeof(IEnumerable).IsAssignableFrom(objType))
            {
                return ((IEnumerable)o1).AreEqualTo((IEnumerable)o2, maxRecursionLevel);
            }

            // check all public properties which have get and set methods and are not indexed and stop at level maxRecursionLevel
            var properties = objType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty)
                .Where(a => (a.GetSetMethod() != null) && (a.GetIndexParameters().Length == 0)).ToArray();
            foreach (var property in properties)
            {
                var result = IsEqualTo(property.GetValue(o1, null), property.GetValue(o2, null),
                    maxRecursionLevel, level + 1);
                if (result == false)
                {
                    Console.WriteLine("Property {0} differs.", property.Name);
                    return false;
                }
            }

            // check all public non read-only members and stop at level maxRecursionLevel 
            var fields = objType.GetFields(BindingFlags.Public | BindingFlags.Instance).Where(a => (!a.IsInitOnly)).ToArray(); 
            foreach (var fieldInfo in fields)
            {
                var result = IsEqualTo(fieldInfo.GetValue(o1), fieldInfo.GetValue(o2), maxRecursionLevel, level + 1);
                if (result == false)
                {
                    Console.WriteLine("Field {0} differs.", fieldInfo.Name);
                    return false;
                }
            }


            return true;
        }

        public static bool IsEqualTo(this object o1, object o2, int maxRecursionLevel = 10)
        {
            return IsEqualTo(o1, o2, maxRecursionLevel, 0); 
        }

        public static bool AreEqualTo(this IEnumerable enumerable, IEnumerable otherEnumerable, int maxRecursionLevel = 10)
        {
            if ((enumerable != null) && (otherEnumerable != null))
            {
                var enumerator = enumerable.GetEnumerator();
                var otherEnumerator = otherEnumerable.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    // end of second IEnumerable before the first one 
                    if (!otherEnumerator.MoveNext())
                    {
                        return false;
                    }
                    
                    if (!IsEqualTo(enumerator.Current, otherEnumerator.Current, maxRecursionLevel - 1))
                    {
                        return false;
                    }
                }
                // second IEnumerable has more items than the first 
                if (otherEnumerator.MoveNext())
                {
                    return false;
                }
            }
            else
            {
                if (((enumerable == null) && (otherEnumerable != null)) || ((enumerable != null) && otherEnumerable == null))
                {
                    return false;
                }
            }

            return true;
        }

    }
}
