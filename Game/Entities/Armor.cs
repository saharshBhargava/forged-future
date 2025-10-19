class Armor : Collectable
{
    public Armor()
    {
        SetSprite(Engine.LoadTexture("Collectable Sprites/Armor.png"));
        drawingWidth = 1;
        drawingHeight = 1;
        hitboxWidth = 19.0f / 32;
        hitboxHeight = 13.0f / 32;
    }
}