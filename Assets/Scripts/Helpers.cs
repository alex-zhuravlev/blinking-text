using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core;

public static class Helpers
{
    public static readonly float[] Frequencies = new float[]
    {
        7.7777777f,
        77.777777f
    };

    public static float GetRandomFrequency()
    {
        int iRandomIndex = enRandom.Get(2);
        return Frequencies[iRandomIndex];
    }

    public static float GetFrequency(int iIndex)
    {
        return Frequencies[iIndex];
    }

    public static float GetRandomLifeTime()
    {
        return enRandom.Get(1000, 3000) / 1000.0f;
    }

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = enRandom.Get(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}