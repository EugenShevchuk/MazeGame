using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.Extensions
{
    public static class CollectionExtensions
    {
        public static T[] Concat<T>(this T[] array, T[] array2)
        {
            if (array == null) 
                throw new ArgumentNullException(nameof(array));
            if (array2 == null) 
                throw new ArgumentNullException(nameof(array2));
            
            var result = new T[array.Length + array2.Length];
            
            array.CopyTo(result, 0);
            array2.CopyTo(result, array.Length);
            
            return result;
        }
        
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action) 
        {
            if (items == null)
                return;
            
            foreach (var item in items)
                action(item);
        }
        
        public static T GetRandomElement<T>(this IEnumerable<T> items, Random random)
        {
            return items.Shuffle(random).First();
        }

        private static IEnumerable<T> Shuffle<T>(this IEnumerable<T> items, Random random)
        {
            if (items == null) 
                throw new ArgumentNullException(nameof(items));
            if (random == null) 
                throw new ArgumentNullException(nameof(random));

            return items.ShuffleIterator(random);
        }

        private static IEnumerable<T> ShuffleIterator<T>(this IEnumerable<T> items, Random random)
        {
            var buffer = items.ToList();
            for (int i = 0; i < buffer.Count; i++)
            {
                var j = random.Next(i, buffer.Count);
                yield return buffer[j];

                buffer[j] = buffer[i];
            }
        }
    }
}