using System;

abstract class Entity
{
    public float x { get; internal set; }
    public float y { get; internal set; }
    public float hitboxWidth { get; internal set; }
    public float hitboxHeight { get; internal set; }
    public float drawingWidth { get; internal set; }
    public float drawingHeight { get; internal set; }
    public bool isRigid { get; internal set; }
    public int id { get; internal set; }

    public int GetId() => id;
    public virtual TextureMirror GetMirror() => TextureMirror.None;
    public virtual Texture GetSprite() => null;

    public virtual float CalculateHitboxOffsetX() => (this.drawingWidth - this.hitboxWidth) / 2;
    public virtual float CalculateHitboxOffsetY() => (this.drawingHeight - this.hitboxHeight);

    //  This method whether or not another entity is colliding with this entity.
    public virtual bool CollidesWith(Entity e)
    {
        if(e == null) throw new ArgumentNullException("Inputted entity is null");
        return !(y+hitboxHeight <= e.y || y >= e.y+e.hitboxHeight) && !(x + hitboxWidth <= e.x || x >= e.x + e.hitboxWidth);
    }
    public virtual bool InContactWith(Entity e)
    {
        if (e == null) throw new ArgumentNullException("Inputted entity is null");
        return !(y + hitboxHeight < e.y || y > e.y + e.hitboxHeight) && !(x + hitboxWidth < e.x || x > e.x + e.hitboxWidth);
    }

    public virtual bool Under(Entity e)
    {
        return !(x + hitboxWidth <= e.x || x >= e.x + e.hitboxWidth) && (y > e.y + e.hitboxHeight) && ((y - hitboxHeight) <= e.y + e.hitboxHeight);
    }

    public virtual bool OnTopOf(Entity e)
    {
        return InContactWith(e) && (y + hitboxHeight <= e.y);
    }
    public virtual bool OnRightSideOf(Entity e)
    {
        return InContactWith(e) && (x >= e.x + e.hitboxWidth && !(y < e.y));
    }
    public virtual bool OnLeftSideOf(Entity e)
    {
        return InContactWith(e) && (x + hitboxWidth <= e.x) && !(y < e.y);
    }
}