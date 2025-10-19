class Camera
{
    public float CameraPositionX { get; internal set; }
    public float CameraPositionY { get; internal set; }

    public float leftMapBound { get; private set; }
    public float rightMapBound { get; private set; }
    public float bottomMapBound { get; private set; }
    public float topMapBound { get; private set; }

    public Camera() { }

    public Camera(Game game)
    {
        CameraPositionX = 0;
        CameraPositionY = -1 - game.currentLevel.player.drawingHeight/Renderer.tileSize;

        leftMapBound = 4;
        rightMapBound = game.currentLevel.width - (Game.Resolution.X/Renderer.tileSize - 4);
    }

    public float CameraOffsetX(float objectXPosition)
    {
        return objectXPosition - CameraPositionX;
    }
    public float CameraOffsetY(float objectYPosition)
    {
        return objectYPosition - CameraPositionY;
    }
}