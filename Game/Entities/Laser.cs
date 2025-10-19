using System;

class Laser : PlatformBlock
{
    public Laserbeam laserbeam;
    private float timeOff = 0.0f;
    private float timeOn = 0.0f;
    private int damage = 1;
    public static bool playerHit = false;


    public Laser(float x, float y, char sprite) : base(x, y, sprite)    
    {
        this.x = x;
        this.y = y;
        //hitbox size for all platform blocks: 1 by default
        drawingWidth = 1;
        drawingHeight = 1;
        hitboxWidth = 1;
        hitboxHeight = 1;
        isRigid = true;

    }

    public Laserbeam GetLaserbeam() { return laserbeam; }

    public void HandleLaserbeam(Player player)
    {
        if (this.x - player.x < 20.0f || player.x - this.x > -20.0f)
        {
            if (timeOff < 3.0f)
            {
                timeOn = 0.0f;
                timeOff += Engine.TimeDelta;
                laserbeam.hitboxWidth = 0;
                laserbeam.drawingWidth = 0;
            }

            else
            {
                if (timeOn < 3.0f)
                {
                    timeOn += Engine.TimeDelta;
                    laserbeam.hitboxWidth = 50;
                    laserbeam.drawingWidth = 50;
                }

                else
                {
                    timeOff = 0;
                    laserbeam.hitboxWidth = 0;
                    laserbeam.drawingWidth = 0;
                }
            }


        }

        if (laserbeam.CollidesWith(player) && laserbeam.hitboxWidth>0)
        {
            playerHit = true;
            player.ReduceHealth(damage);
            Game.audioManager.PlayHurtPlayer();
        }
        else playerHit = false;

        if (playerHit)
        {
            player.pc.SetCurrAnim(6);
        }
        
    }
}