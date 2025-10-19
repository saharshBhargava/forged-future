class DamageBoots : Collectable
{
    public DamageBoots()
    {
        SetSprite(Engine.LoadTexture("Collectable Sprites/Damage Boots.png"));
        drawingWidth = 1;
        drawingHeight = 1;
        hitboxWidth = 1;
        hitboxHeight = 1;
    }
}