public class BossAnimationController : AnimationController
{
    public BossAnimationController()
    {
        Animation idle = new Animation(0.1f);
        idle.AddSprite(Engine.LoadTexture("Boss Enemy Animations/Boss Enemy (Idle - 3).png"));

        Animation walking = new Animation(0.1f);
        walking.AddSprite(Engine.LoadTexture("Boss Enemy Animations/Boss Enemy (Step 2).png"));
        walking.AddSprite(Engine.LoadTexture("Boss Enemy Animations/Boss Enemy (Idle - 3).png"));
        walking.AddSprite(Engine.LoadTexture("Boss Enemy Animations/Boss Enemy (Step 4).png"));

        Animation stunned = new Animation(0.1f);
        stunned.AddSprite(Engine.LoadTexture("Boss Enemy Animations/Boss Enemy (Stunned).png"));

        Animation damaged = new Animation(0.1f);
        damaged.AddSprite(Engine.LoadTexture("Boss Enemy Animations/Boss Enemy (Damaged).png"));

        Animation hurt = new Animation(0.1f);
        hurt.AddSprite(Engine.LoadTexture("Boss Enemy Animations/Boss Enemy (Destroy).png"));

        AddAnimation(idle);
        AddAnimation(walking);
        AddAnimation(stunned);
        AddAnimation(damaged);
        AddAnimation(hurt);
    }
}
