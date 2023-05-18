using DG.Tweening;

public static class TweenExtension
{
    public static Tween KillTo0(this Tween t)
    {
        if (t != null)
        {
            t.Goto(0, true);
            t.Kill();
        }

        return null;
    }
}