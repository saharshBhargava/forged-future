class Keycard : Collectable
{ 
    public Keycard()
    {
        SetSprite(Engine.LoadTexture("Collectable Sprites/Keycard.png"));
        drawingHeight = 1;
        drawingWidth = 1;
        hitboxHeight = 19.0f/32;
        hitboxWidth = 13.0f/32;
    }
}