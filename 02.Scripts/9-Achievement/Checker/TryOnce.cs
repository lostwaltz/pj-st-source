public class TryOnce : AchievementChecker
{
    public override float CheckIncrement(float curValue, float increment)
    {
        return increment;
    }
}