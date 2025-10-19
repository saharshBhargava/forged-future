using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Door: PhysicsEntity
{

    bool isOpened = false;
    public Door(float x,  float y)
    {
        this.x = x;
        this.y = y-1;
        drawingHeight = 2;
        hitboxHeight = 2;
        drawingWidth = 1;
        hitboxWidth = 1;
        isRigid = true;
    }

    public void HandleDoor(Player player)
    {
        Collectable usedCollectable = null;
        float dist = 0;
        foreach (Collectable c in player.GetInventory().GetInventoryList())
        {
            if (c is Keycard)
            {
                dist = (float)Math.Sqrt(Math.Pow((player.x - x), 2) + Math.Pow((player.y - y), 2));
                if (dist < 1f && !isOpened)
                {
                    usedCollectable = c;
                    //SetSprite('F');
                    isRigid = false;
                    //collisionOverride = true;
                    isOpened = true;
                }
            }
        }
        if (dist < 1f && isOpened) player.GetInventory().RemoveFromInventory(usedCollectable);
        if (isOpened)
        {
            //SetSprite('F');
            isRigid = false;
            //collisionOverride = true;
        }
    }

    public override Texture GetSprite() => Engine.LoadTexture("Platform Block Sprites/Door.png");
}
