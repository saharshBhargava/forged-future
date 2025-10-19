using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class FinalDoor: PhysicsEntity
{
    bool isOpened = false;
    public FinalDoor(float x, float y)
    {
        this.x = x;
        this.y = y - 1;
        drawingHeight = 2;
        hitboxHeight = 2;
        drawingWidth = 1;
        hitboxWidth = 1;
        isRigid = true;
    }

    public void HandleFinalDoor(Player player, Level level)
    {
        float dist = 0;
        dist = (float)Math.Sqrt(Math.Pow((player.x - x), 2) + Math.Pow((player.y - y), 2));
        if (dist < 1f && !isOpened && player.enemiesStunned>=level.GetEnemies().Count)
        {
            //SetSprite('F');
            isRigid = false;
            //collisionOverride = true;
            isOpened = true;
        }
        if (isOpened)
        {
            //SetSprite('F');
            isRigid = false;
            //collisionOverride = true;
        }
    }

    public override Texture GetSprite() => Engine.LoadTexture("Platform Block Sprites/OpenDoor.png");

}
