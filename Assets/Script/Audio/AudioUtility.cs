using UnityEngine;

public static class AudioUtility
{
    public static float LinearToDB(float value)
    {
        if (value <= 0.0001f)
            return -80f;

        return Mathf.Log10(value) * 20f;
    }

    public static float DBToLinear(float db)
    {
        return Mathf.Pow(10f, db / 20f);
    }
}