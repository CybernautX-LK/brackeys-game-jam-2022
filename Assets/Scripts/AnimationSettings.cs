using DG.Tweening;

[System.Serializable]
public class AnimationSettings
{
    public float duration;
    public Ease ease;

    public AnimationSettings(float duration, Ease ease)
    {
        this.duration = duration;
        this.ease = ease;
    }
}
