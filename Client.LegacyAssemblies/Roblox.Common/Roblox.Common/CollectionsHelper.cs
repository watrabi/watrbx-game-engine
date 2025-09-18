using System;
using System.Collections.Generic;

namespace Roblox.Common;

public static class CollectionsHelper
{
	public static T GetRandomElement<T>(this IList<T> self)
	{
		if (self.Count == 0)
		{
			return default(T);
		}
		return self[new Random().Next(self.Count)];
	}

	public static void Swap<T>(T[] array, int i, int j)
	{
		T d = array[i];
		array[i] = array[j];
		array[j] = d;
	}

	public static IEnumerable<T> RandomizeCollection<T>(this ICollection<T> collection, int numberofItemsToReturn)
	{
		return collection.RandomizeCollection(numberofItemsToReturn, (T _) => true);
	}

	public static IEnumerable<T> RandomizeCollection<T>(this ICollection<T> collection, int numberofItemsToReturn, Func<T, bool> predicate)
	{
		int count = collection.Count;
		T[] array = new T[count];
		collection.CopyTo(array, 0);
		if (numberofItemsToReturn > count)
		{
			numberofItemsToReturn = count;
		}
		for (int newCount = 0; newCount < numberofItemsToReturn && newCount < count; newCount++)
		{
			int randIdx = new Random().Next(count);
			if (!predicate(array[randIdx]))
			{
				count--;
				Swap(array, randIdx, count);
				newCount--;
			}
			else
			{
				Swap(array, newCount, randIdx);
			}
		}
		if (numberofItemsToReturn > count)
		{
			numberofItemsToReturn = count;
		}
		for (int i = 0; i < numberofItemsToReturn; i++)
		{
			yield return array[i];
		}
	}
}
