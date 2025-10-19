using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

class FallingPlatform : PlatformBlock
{
    private bool falling = false;
    private float gravitationalConstant = 70f;
    private float currentYVelocity = 1;
    private float deltaTime = 0.0f;
    public static bool playerHit = false;
    private int damage = 1;

    public FallingPlatform(float x, float y, char sprite) : base(x, y, sprite)
    {
        isRigid = false;
    }

    public void UpdatePosition(Player player)
    {

        if (this.x - player.x < 1.0f || falling)
        {
            falling = true;
            deltaTime += Engine.TimeDelta;
            currentYVelocity = currentYVelocity * deltaTime + gravitationalConstant * deltaTime;
            if (currentYVelocity > 10) { currentYVelocity = 10; }
            this.y = currentYVelocity * deltaTime;
        }

        if (this.CollidesWith(player))
        {
            playerHit = true;
            player.ReduceHealth(damage);
        }
        else playerHit = false;

        if (playerHit)
        {
            player.pc.SetCurrAnim(6);
        }

    }
}