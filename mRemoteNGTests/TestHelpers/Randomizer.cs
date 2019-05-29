using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Enum = System.Enum;

namespace mRemoteNGTests.TestHelpers
{
	internal static class Randomizer
    {
        private static readonly Random Random = new Random();

        internal static string RandomString(params string[] excludedStrings)
        {
            return GetNonExcludedValue(() => Guid.NewGuid().ToString("N"), excludedStrings);
        }

        internal static bool RandomBool(bool? excludedBool = null)
        {
            if (excludedBool.HasValue)
                return !excludedBool.Value;

            return Random.Next() % 2 == 0;
        }

        internal static int RandomInt(int minValue = int.MinValue, int maxValue = int.MaxValue, params int[] excludeInts)
        {
            return GetNonExcludedValue(() => Random.Next(minValue, maxValue), excludeInts);
        }

        internal static DateTime RandomDateTime(params DateTime[] excludeTimes)
        {
            var date = GetNonExcludedValue(() =>
                new DateTime(
                    RandomInt(minValue: 1990, maxValue: 2019),
                    RandomInt(minValue: 1, maxValue: 13),
                    RandomInt(minValue: 1, maxValue: 29),
                    RandomInt(minValue: 0, maxValue: 24),
                    RandomInt(minValue: 0, maxValue: 60),
                    RandomInt(minValue: 0, maxValue: 60)),
                excludeTimes);

            return date;
        }

        internal static T RandomEnum<T>(params object[] excludeValues) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enum");

            return (T)RandomEnum(typeof(T), excludeValues);
        }

        internal static object RandomEnum(Type enumType, params object[] excludeValues)
        {
            if (!enumType.IsEnum)
                throw new ArgumentException("enumType must be an enum");

            var values = Enum.GetValues(enumType);
            return GetNonExcludedValue(() => values.GetValue(Random.Next(values.Length)), excludeValues);
        }

        private static T GetNonExcludedValue<T>(Func<T> builder, params object[] excludedValues)
        {
            do
            {
                var value = builder();
                if (!excludedValues.Contains(value))
                    return value;
            } while (true);
        }

        /// <summary>
        /// Randomizes the primitive-type settable properties of the given object.
        /// Returns the same object instance to enable fluent method calls. It will
        /// never choose the value that the property current holds. For booleans, this
        /// means they will always be toggled rather than truly randomized.
        /// </summary>
        internal static T RandomizeValues<T>(this T con)
            where T : class
        {
            var opByType = new Dictionary<Type, Action<PropertyInfo, T>>
            {
                { typeof(int), (p, c) =>  p.SetValue(c, RandomInt(minValue: 0, excludeInts:(int)p.GetValue(c))) },
                { typeof(bool), (p, c) =>  p.SetValue(c, !(bool)p.GetValue(c)) },
                { typeof(string), (p, c) =>  p.SetValue(c, RandomString((string)p.GetValue(c))) },
                { typeof(DateTime), (p, c) =>  p.SetValue(c, RandomDateTime((DateTime)p.GetValue(c))) },
                { typeof(Enum), (p, c) =>  p.SetValue(c, RandomEnum(p.PropertyType, p.GetValue(c))) },
            };

            var settableProperties = con
                .GetType()
                .GetProperties()
                .Where(p => p.GetSetMethod() != null);

            foreach (var property in settableProperties)
            {
                if (opByType.TryGetValue(property.PropertyType, out var mutator))
                    mutator(property, con);

                else if (property.PropertyType.BaseType != null && 
                         opByType.TryGetValue(property.PropertyType.BaseType, out var mutator2))
                    mutator2(property, con);
            }

            return con;
        }

        /// <summary>
        /// Toggles all <see cref="bool"/> settable properties
        /// on the given object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="excludeProperties"></param>
        /// <returns></returns>
        internal static T ToggleAllBooleanProperties<T>(this T obj, params string[] excludeProperties)
            where T : class
        {
            var settableBooleanProperties = obj
                .GetType()
                .GetProperties()
                .Where(p => 
                    p.GetSetMethod() != null && 
                    p.PropertyType == typeof(bool) &&
                    !excludeProperties.Contains(p.Name));

            foreach (var property in settableBooleanProperties)
            {
                var currentValue = (bool)property.GetValue(obj);
                property.SetValue(obj, !currentValue);
            }

            return obj;
        }
    }
}
