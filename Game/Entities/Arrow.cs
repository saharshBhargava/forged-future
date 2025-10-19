using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Arrow: PhysicsEntity
{

   private Texture sprite;
   public Arrow(float x, float y)
   {
        this.x = x;
        this.y = y;
        isRigid = false;
        currentYVelocity = -10;

        drawingWidth = 1f;
        drawingHeight = 1f;
        hitboxWidth = 30.0f / 32;
        hitboxHeight = 2.0f / 32;
        SetSprite(Engine.LoadTexture("arrow_sprite.png"));
    }

    public void UpdatePosition(List<Entity> entities)
    {
        currentXVelocity = 0;
        if (currentGroundEntities.Count == 0)
        {
            currentXVelocity = -10;
        }
        UpdatePosition(true, entities);
    }

    public override Texture GetSprite() => sprite;
    public void SetSprite(Texture sprite) => this.sprite = sprite;
}
