﻿using System.Collections;
using System.Collections.Generic;
using Cards;
public static class Extensions
{
    /// <summary>
    /// Shuffles the element order of the specified list.
    /// </summary>
    public static void Shuffle<T>(this IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }

    public static void EnqueueQueue(this Queue<CardData> cards, Queue<CardData> pool)
    {
        while(pool.Count > 0)
        {
            cards.Enqueue(pool.Dequeue());
        }
    }
}