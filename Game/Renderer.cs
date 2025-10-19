class Renderer
{
    public static Camera camera { get; internal set; }
    public static float tileSize { get; internal set; } = 64;
    private static float automaticParallaxOffsetX = 0f;
    private static float automaticParallaxMultiplier = 5;
    public static void DrawEntity(Entity entity, Camera c)
    {   
        camera = c;
        // Get the size of the block from the platformBlock object
        float entityWidth = entity.drawingWidth;
        float entityHeight = entity.drawingHeight;

        // Calculate the bounds (position and size) for the platform block based on its coordinates and block size
        Bounds2 bounds = new Bounds2(
            camera.CameraOffsetX(entity.x - entity.CalculateHitboxOffsetX()) * tileSize, // X position, scaled by block size
            camera.CameraOffsetY(entity.y - entity.CalculateHitboxOffsetY()) * tileSize, // Y position, scaled by block size
            entityWidth * tileSize, // Width of the block
            entityHeight * tileSize // Height of the block
        );
        if (entity.GetSprite() == null)
        {
            Engine.DrawTexture(Engine.LoadTexture("unimplemented_sprite.png"), new Vector2(bounds.Position.X, bounds.Position.Y), size: new Vector2(bounds.Size.X, bounds.Size.Y), mirror: entity.GetMirror(), scaleMode: TextureScaleMode.Nearest);

        }
        else Engine.DrawTexture(entity.GetSprite(), new Vector2(bounds.Position.X, bounds.Position.Y), size: new Vector2(bounds.Size.X, bounds.Size.Y), mirror: entity.GetMirror(), scaleMode: TextureScaleMode.Nearest);
    }

    public static void DrawParallax(Camera camera, bool automatic)
    {
        float deltaTime = Engine.TimeDelta;
        automaticParallaxOffsetX += deltaTime * automaticParallaxMultiplier;

        DrawBackground(camera, automatic);
        DrawMidground(camera, automatic);
        DrawForeground(camera, automatic);
    }



    private static void DrawBackground(Camera camera, bool automatic)
    {
        // Parallax factor for background (moves slower than the camera)
        float parallaxFactor = 2f;

        // Width, height, and spacing of each individual rectangle in the foreground layer
        float rectWidth = Game.Resolution.X / 4;
        float rectHeight = Game.Resolution.Y / 1.15f;
        float spacing = Game.Resolution.X / 20;

        // Calculate the offset for the background layer based on camera X position
        float backgroundOffsetX;

        if (!automatic)
        {
            backgroundOffsetX = camera.CameraPositionX * parallaxFactor;
        }
        else
        {
            backgroundOffsetX = automaticParallaxOffsetX * parallaxFactor;
        }

        // Draw repeating rectangles infinitely to fill the viewport
        for (float x = -backgroundOffsetX % (rectWidth + spacing); x < Game.Resolution.X; x += rectWidth + spacing)
        {
            Bounds2 bounds = new Bounds2(
                x,               
                Game.Resolution.Y - rectHeight,
                rectWidth,     
                Game.Resolution.Y     
            );

            Engine.DrawRectSolid(bounds, Color.DimGray);
        }
    }

    public static void DrawMidground(Camera camera, bool automatic)
    {
        //Parallax factor for midground (moves at a medium speed)
        float parallaxFactor = 10f;

        // Width, height, and spacing of each individual rectangle in the foreground layer
        float rectWidth = Game.Resolution.X / 5;
        float rectHeight = Game.Resolution.Y / 1.5f;
        float spacing = Game.Resolution.X / 25;

        //Calculate the offset for the midground layer based on camera X position
        float midgroundOffsetX;

        if (!automatic)
        {
            midgroundOffsetX = camera.CameraPositionX * parallaxFactor;
        }
        else
        {
            midgroundOffsetX = automaticParallaxOffsetX * parallaxFactor;
        }

        //Draw repeating rectangles infinitely to fill the viewport
        for (float x = -midgroundOffsetX % (rectWidth + spacing); x < Game.Resolution.X; x += rectWidth + spacing)
        {
            Bounds2 bounds = new Bounds2(
                x,               
                Game.Resolution.Y - rectHeight,
                rectWidth,      
                Game.Resolution.Y      
            );

            Engine.DrawRectSolid(bounds, Color.DarkGray);
        }
    }

    public static void DrawForeground(Camera camera, bool automatic = false)
    {
        //Parallax factor for foreground (moves at nearly the same speed as the camera)
        float parallaxFactor = 20;

        //Width, height, and spacing of each individual rectangle in the foreground layer
        float rectWidth = Game.Resolution.X / 6;
        float rectHeight = Game.Resolution.Y / 2.15f;
        float spacing = Game.Resolution.X / 30;

        //Calculate the offset for the foreground layer based on camera X position
        float foregroundOffsetX;

        if (!automatic)
        {
            foregroundOffsetX = camera.CameraPositionX * parallaxFactor;
        }
        else
        {
            foregroundOffsetX = automaticParallaxOffsetX * parallaxFactor;
        }

        //Draw repeating rectangles infinitely to fill the viewport
        for (float x = -foregroundOffsetX % (rectWidth + spacing); x < Game.Resolution.X; x += rectWidth + spacing)
        {
            Bounds2 bounds = new Bounds2(
                x,              
                Game.Resolution.Y - rectHeight, 
                rectWidth,       
                Game.Resolution.Y   
            );

            Engine.DrawRectSolid(bounds, Color.LightGray);
        }
    }

    public static void DrawScore(Level level)
    {
        Vector2 center;
        center.X = Game.Resolution.X / 2;
        center.Y = Game.Resolution.Y / 10;
        Engine.DrawString(level.levelScore + "", center, Color.White, Game.titleFont, TextAlignment.Center);
    }

    public static void DrawHealth(Level level)
    {
        Vector2 topLeft;

        topLeft.X = Game.Resolution.X / 20;
        topLeft.Y = Game.Resolution.Y / 10;

        Bounds2 healthBounds = new Bounds2(
            topLeft.X,
            topLeft.Y,
            tileSize,
            tileSize
        );

        Texture healthSprite = Engine.LoadTexture("Player UI Sprite/Health.png");

        for (int i = 0; i < level.player.health; i++)
        {
            
            Engine.DrawTexture(healthSprite, healthBounds.Position, color: null, healthBounds.Size, scaleMode: TextureScaleMode.Nearest);
            healthBounds.Position.X += tileSize;
        }
    }

    public static void DrawCollectedItems(Level level)
    {
        Vector2 topRight;
        topRight.X = 19 * Game.Resolution.X / 20;

        Vector2 topLeft;
        topLeft.X = Game.Resolution.X / 20;
        topLeft.Y = Game.Resolution.Y / 10;
        topRight.Y = Game.Resolution.Y / 10;

        Bounds2 labelBounds = Engine.DrawString("Inventory", topRight, Color.White, Game.titleFont, TextAlignment.Right);

        topRight.Y += labelBounds.Size.Y + 10;
        topLeft.Y += labelBounds.Size.Y + 10;

        Bounds2 collectableBounds = new Bounds2(
            topRight.X - tileSize,
            topRight.Y,
            tileSize,
            tileSize
        );

        Bounds2 tokenBounds = new Bounds2(
            topLeft.X,
            topLeft.Y,
            tileSize,
            tileSize
        );

        int i = 0;
        foreach (Collectable c in level.player.GetInventory().GetInventoryList())
        {
            if (c is Coin token)
            {
                Engine.DrawTexture(c.GetSprite(), tokenBounds.Position, color: null, tokenBounds.Size, scaleMode: TextureScaleMode.Nearest);
                tokenBounds.Position.X += tileSize;
            }
            else if (c is Materials)
            {
                i++;
                if (i == 1)
                {
                    Engine.DrawTexture(c.GetSprite(), collectableBounds.Position, color: null, new Vector2(collectableBounds.Size.X*c.drawingWidth, collectableBounds.Size.Y*c.drawingHeight), scaleMode: TextureScaleMode.Nearest);
                    collectableBounds.Position.X -= tileSize;
                }
            }
            else
            {
                Engine.DrawTexture(c.GetSprite(), collectableBounds.Position, color: null, collectableBounds.Size, scaleMode: TextureScaleMode.Nearest);
                collectableBounds.Position.X -= tileSize;
            }
        }
    }

    public static void DrawButton(string buttonText, float x, float y)
    {
        Bounds2 textBound = Engine.DrawString(buttonText, new Vector2(x, y), Color.White, Game.subtitleFont, TextAlignment.Center, true);
        Engine.DrawRectSolid(textBound, Color.Blue);
    }

    public static void DrawLeaderboard()
    {
        Vector2 center;
        center.X = Game.Resolution.X / 2;
        center.Y = Game.Resolution.Y / 10;

        Bounds2 textBound = Engine.DrawString(Game.Title, center, Color.White, Game.titleFont, TextAlignment.Center, true);

        float rectWidth = textBound.Size.X + 50;
        float rectHeight = Game.Resolution.Y;

        float centerX = Game.Resolution.X / 2;
        float centerY = Game.Resolution.Y / 2;

        Bounds2 blackScreen = new Bounds2(
            centerX - rectWidth / 2,
            centerY - rectHeight / 2,
            rectWidth,
            rectHeight
        );

        Engine.DrawRectSolid(blackScreen, Color.Black.WithAlpha(0.5f));

        Engine.DrawString(Game.Title, center, Color.LightSteelBlue, Game.titleFont, TextAlignment.Center);

        center.Y += (20 + textBound.Position.Y);

        Engine.DrawString("Leaderboard", center, Color.White, Game.subtitleFont, TextAlignment.Center);

        for (int i = 0; i < Game.scores.Length; i++)
        {
            string currentScore = $"Level {i + 1}: {Game.scores[i]}";

            center.Y += (5 + textBound.Position.Y);

            Engine.DrawString(currentScore, center, Color.White, Game.subtitleFont, TextAlignment.Center);
        }

        center.Y += textBound.Position.Y + 20;
        Engine.DrawString("Press [space] to continue.", center, Color.LightSteelBlue, Game.captionFont, TextAlignment.Center);
    }

    public static void DrawBossHealthBar(Level level)
    {
        if (level.levelNum == 3)
        {
            float rectWidth = Game.Resolution.X / 3.0f;
            float rectHeight = 25;

            float xPos = (Game.Resolution.X / 2.0f) - (rectWidth / 2.0f);
            float yPos = (0 + (rectHeight * 2.0f));

            Bounds2 healthBar = new Bounds2(xPos, yPos, rectWidth * (level.GetBoss().trueHealth / 10.0f), rectHeight);

            float padding = 5;

            Bounds2 background = new Bounds2(xPos - padding/2, yPos - padding/2, rectWidth + padding, rectHeight + padding);

            Engine.DrawRectSolid(background, Color.DarkGray);
            Engine.DrawRectSolid(healthBar, Color.Red);
        }
    }

    public static void DrawTask(Level level)
    {
        Vector2 topLeft;

        topLeft.X = Game.Resolution.X / 20;
        topLeft.Y = Game.Resolution.Y / 10 - 50;

        string taskString = $"Task: Stun Enemies ({level.player.enemiesStunned} / {level.GetEnemies().Count})";
        Bounds2 labelBounds = Engine.DrawString(taskString, topLeft, Color.White, Game.subtitleFont, TextAlignment.Left);
    }

    public static void DrawUIComponents() 
    {
        foreach (UIComponent ui in UIScreenManager.activeScreen.elements)
        {
            ui.DrawComponent();
        }
    }
}