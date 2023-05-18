using System;

public static class MathExtension
{
    public static int RoundOff (this int i)
    {
        return ((int)Math.Round(i / 10.0)) * 10;
    }
    public static float RoundOff (this float i)
    {
        return (float)Math.Round(i / 10f) * 10f;
    }
}