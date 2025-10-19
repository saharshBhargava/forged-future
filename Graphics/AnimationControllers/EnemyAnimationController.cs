public class EnemyAnimationController: AnimationController
{
    public EnemyAnimationController()
    {
        Animation idle = new Animation(0.1f);
        idle.AddSprite(Engine.LoadTexture("Enemy Animations/Enemy (Idle - 4).png"));

        Animation walking = new Animation(0.1f);
        walking.AddSprite(Engine.LoadTexture("Enemy Animations/Enemy (1 - 3).png"));
        walking.AddSprite(Engine.LoadTexture("Enemy Animations/Enemy (2).png"));
        walking.AddSprite(Engine.LoadTexture("Enemy Animations/Enemy (1 - 3).png"));
        walking.AddSprite(Engine.LoadTexture("Enemy Animations/Enemy (Idle - 4).png"));
        walking.AddSprite(Engine.LoadTexture("Enemy Animations/Enemy (5 - 7).png"));
        walking.AddSprite(Engine.LoadTexture("Enemy Animations/Enemy (6).png"));
        walking.AddSprite(Engine.LoadTexture("Enemy Animations/Enemy (5 - 7).png"));

        Animation stunned = new Animation(0.1f);
        stunned.AddSprite(Engine.LoadTexture("Enemy Animations/Enemy (Stunned).png"));

        Animation damaged = new Animation(0.1f);
        damaged.AddSprite(Engine.LoadTexture("Enemy Animations/Enemy (Damaged).png"));

        AddAnimation(idle);
        AddAnimation(walking);
        AddAnimation(stunned);
        AddAnimation(damaged);
    }
}
