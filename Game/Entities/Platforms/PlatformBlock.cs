using System.Collections.Generic;

class PlatformBlock : PhysicsEntity
{
    protected Texture sprite;
    protected Texture initialSprite;

    protected static readonly Dictionary<char, Texture> spriteMap = new()
    {
        { 'G', Engine.LoadTexture("Platform Block Sprites/Ground.png") }, // Ground
        { 'F', Engine.LoadTexture("Platform Block Sprites/Static Platform.png") },   // Static Platform
        { 'C', Engine.LoadTexture("Platform Block Sprites/Conveyor Belt Platform.png") },    // Conveyor Belt Platform
        { 'P', Engine.LoadTexture("Platform Block Sprites/One-Way Force Field Platform.png")  },    // One-Way Force Field Platform
        { 'D', Engine.LoadTexture("Platform Block Sprites/Falling Debris.png") },   // Falling Debris
        { 'W', Engine.LoadTexture("Platform Block Sprites/Walls.png") }, // Walls
        { 'L', Engine.LoadTexture("Platform Block Sprites/Colliding Blocks.png") },  // Colliding Blocks
        { 'R', Engine.LoadTexture("Platform Block Sprites/Empty Block.png") },               // Red Sprite
        { 'E', Engine.LoadTexture("Platform Block Sprites/Electric Panel.png") }, // Electric Panel
        { 'S', Engine.LoadTexture("Platform Block Sprites/Laser.png") } // Laser Block
    };

    public PlatformBlock(float x, float y, char sprite)
    {
        this.x = x;
        this.y = y;
        //hitbox size for all platform blocks: 1 by default
        drawingWidth = 1;
        drawingHeight = 1;
        hitboxWidth = 1;
        hitboxHeight = 1;
        isRigid = true;

        SetSprite(sprite);
        if (sprite=='F') collisionOverride = true;
        initialSprite = this.sprite;
    }

    public override Texture GetSprite() => sprite;

    public void SetSprite(char spriteChar) 
    {
        if (spriteMap.TryGetValue(spriteChar, out var mappedSprite))
        {
            sprite = mappedSprite;
        }
        else
        {
            sprite = Engine.LoadTexture("Platform Block Sprites/White.png"); // Default to white for unknown blocks
        }
    }

    public void InContactWithPlayer(bool inContact)
    {
        Game.audioManager.PlayFootsteps();
    }
}