class Coin : Collectable
{
    private static int numCoins = 0;

    public Coin() 
    {
        SetSprite(Engine.LoadTexture("Collectable Sprites/Token.png"));
        drawingWidth = 1;
        drawingHeight = 1;
        hitboxWidth = 18.0f / 32;
        hitboxHeight = 18.0f / 32;
    }
    public Coin(float x, float y)
    {
        this.x = x;
        this.y = y;
        SetSprite(Engine.LoadTexture("Collectable Sprites/Token.png"));
        drawingWidth = 1;
        drawingHeight = 1;
        hitboxWidth = 18.0f/32;
        hitboxHeight = 18.0f/32;
    }
    public void AddCoin()
    {
        numCoins++;
    }

    public int ReturnNumCoins()
    {
        return numCoins;
    }
}
