class Laserbeam : Entity
{
    Texture sprite;
    public Laserbeam(float x, float y) 
    {
        this.x = x;
        this.y = y;
        //hitbox size for all platform blocks: 1 by default
        drawingWidth = 0;
        drawingHeight = 0.5F;
        hitboxWidth = 0;
        hitboxHeight = 0.5F;
        isRigid = false;
        SetSprite(Engine.LoadTexture("Platform Block Sprites/Laserbeam.png"));
    }
    public override Texture GetSprite() => sprite;
    public void SetSprite(Texture sprite) => this.sprite = sprite;
}