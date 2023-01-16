using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Extensions
{
    public static class Extensions
    {
        public static void AddRange<T>(this ConcurrentBag<T> @this, IEnumerable<T> toAdd)
        {
            foreach (var element in toAdd)
            {
                @this.Add(element);
            }
        }


        public static void AddRange<T>(this BindingList<T> observableCollection, IEnumerable<T> collection)
        {
            foreach (var i in collection)
                observableCollection.Add(i);
        }


        public static TAttribute GetAttribute<TAttribute>(this Enum enumValue)
                where TAttribute : Attribute
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<TAttribute>();
        }


        public static string GetDisplayAttributeValue(this Enum enumValue)
        {
            return enumValue.GetAttribute<DisplayAttribute>().Name;
        }

    }
}
