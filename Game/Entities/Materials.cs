using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Materials: Collectable
{
    //public int count { get; internal set; }
    public Materials()
    {
        SetSprite(Engine.LoadTexture("Collectable Sprites/Materials.png"));
        drawingHeight = 0.75f;
        drawingWidth = 0.75f;
        hitboxHeight = 23.0f/32 * 0.75f;
        hitboxWidth = 0.75f;
        //count = 3;
    }
}
