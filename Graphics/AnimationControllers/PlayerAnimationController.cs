public class PlayerAnimationController: AnimationController
{
    public PlayerAnimationController()
    {
        Animation idle = new Animation(0.1f);
        idle.AddSprite(Engine.LoadTexture("Player Animations/Player (Idle - 4).png"));

        Animation walking = new Animation(0.1f);
        walking.AddSprite(Engine.LoadTexture("Player Animations/Player (Step 1 - 3).png"));
        walking.AddSprite(Engine.LoadTexture("Player Animations/Player (Step 2).png"));
        walking.AddSprite(Engine.LoadTexture("Player Animations/Player (Step 1 - 3).png"));
        walking.AddSprite(Engine.LoadTexture("Player Animations/Player (Idle - 4).png"));
        walking.AddSprite(Engine.LoadTexture("Player Animations/Player (Step 5 - 7).png"));
        walking.AddSprite(Engine.LoadTexture("Player Animations/Player (Step 6).png"));
        walking.AddSprite(Engine.LoadTexture("Player Animations/Player (Step 5 - 7).png"));

        Animation jump = new Animation(0.1f);
        jump.AddSprite(Engine.LoadTexture("Player Animations/Player (Step 6).png"));

        Animation dash = new Animation(0.1f);
        dash.AddSprite(Engine.LoadTexture("Player Animations/Player (Step 5 - 7).png"));

        Animation duck = new Animation(0.1f);
        duck.AddSprite(Engine.LoadTexture("Player Animations/Player (Duck).png"));

        Animation slide = new Animation(0.1f);
        slide.AddSprite(Engine.LoadTexture("Player Animations/Player (Slide).png"));

        Animation shock = new Animation(0.1f);
        shock.AddSprite(Engine.LoadTexture("Player Animations/Player (Shocked).png"));

        Animation damaged = new Animation(0.1f);
        damaged.AddSprite(Engine.LoadTexture("Player Animations/Player (Damaged).png"));

        AddAnimation(idle);
        AddAnimation(walking);
        AddAnimation(jump);
        AddAnimation(dash);
        AddAnimation(duck);
        AddAnimation(slide);
        AddAnimation(shock);
        AddAnimation(damaged);
    }
}
