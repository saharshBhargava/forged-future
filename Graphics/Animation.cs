using System.Collections.Generic;

public class Animation
{
    private List<Texture> sprites = new();
    private int currSprite = 0;
    private float timeSinceLastFrame = 0;
    private float animationSpeed;

    public Animation() { }

    internal Animation(float animationSpeed)
    {
        this.animationSpeed = animationSpeed; 
    }

    public void Advance(float dt)
    {
        timeSinceLastFrame += dt;
        if (timeSinceLastFrame > animationSpeed)
        {
            timeSinceLastFrame = timeSinceLastFrame % animationSpeed;
            currSprite = (currSprite+1)%(sprites.Count);
        }
    }

    internal void AddSprite(Texture sprite)
    {
        sprites.Add(sprite);
    }

    public void Reset()
    {
        currSprite = 0;
        timeSinceLastFrame = 0;
    }

    internal Texture GetCurrSprite() => sprites[currSprite];
}
