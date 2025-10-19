using System.Collections.Generic;
public class AnimationController
{
    private List<Animation> animations = new();
    public int currAnimationNum { get; private set; } = 0;

    public AnimationController() { }

    public void AdvanceCurrAnim() { animations[currAnimationNum].Advance(Engine.TimeDelta); }

    public Animation GetCurrAnimation() => animations[currAnimationNum];

    public void SetCurrAnim(int currAnim)
    {
        GetCurrAnimation().Reset();
        this.currAnimationNum = currAnim;
    }
    public void AddAnimation(Animation animation)
    {
        animations.Add(animation);
    }
}
