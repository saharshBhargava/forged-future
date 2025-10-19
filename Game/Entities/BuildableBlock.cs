using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

class BuildableBlock: PlatformBlock
{

    //public Texture sprite; 
    bool isBuilt = false;

    public BuildableBlock(float x, float y, char sprite): base(x, y, sprite)
    {
        isRigid = false;
    }

    //public override Texture GetSprite() => Engine.LoadTexture("Platform Block Sprites/Empty Block.png");

    public void HandleBuild(Player player)
    {
        Collectable usedCollectable = null;
        float dist = 0;
        foreach (Collectable c in player.GetInventory().GetInventoryList())
        {
            if (c is Materials)
            {
                dist = (float)Math.Sqrt(Math.Pow((player.x - x), 2) + Math.Pow((player.y - y), 2));
                if (dist < 3f && !isBuilt)
                {
                    usedCollectable = c;
                    SetSprite('F');
                    isRigid = true;
                    collisionOverride = true;
                    isBuilt = true;
                }
            }
        }
        if (dist < 3f && isBuilt) player.GetInventory().RemoveFromInventory(usedCollectable);
        if (isBuilt) 
        {
            SetSprite('F');
            isRigid = true;
            collisionOverride = true;
        }
    }
}
