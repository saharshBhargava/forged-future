using System;
using System.Diagnostics;

class InputManager
{

    public static void CheckMouseClick()
    {
        float mouseX = Engine.MousePosition.X;
        float mouseY = Engine.MousePosition.Y;

        foreach (UIComponent component in UIScreenManager.activeScreen.elements)
        {
            bool mouseOnComponent = component.IsHover(mouseX, mouseY);
            if (component is Button button) button.onHover(mouseOnComponent);
            if (mouseOnComponent && Engine.GetMouseButtonUp(MouseButton.Left)) component.OnClick();
        }
    }

    public static void CheckGameOver(Game game, bool win)
    {
        if (game.currentLevel.player.health == 0)
        { 
            Game.gameStarted = false;
            UIScreenManager.activeScreen = UIScreenManager.CreateGameOverScreen(game, win);
        }
        if (win == true)
        {
            Game.gameStarted = false;
            UIScreenManager.activeScreen = UIScreenManager.CreateGameOverScreen(game, win);
        }
    }

    public static void SwitchLevels(Game game)
    {
        Key[] numKeys = [Key.NumRow1, Key.NumRow2, Key.NumRow3, Key.NumRow4];

        // Check for key press to load levels and update the game instance's currentLevel
        DataManager.SaveData(game);

        for (int i = 0; i < numKeys.Length; i++) {
            if (Engine.GetKeyUp(numKeys[i]))
            {
                game.currentLevel = Game.levels[i];
                DataManager.LoadLevel(game);
                game.levelNum = i;
            }
        }
    }

    public static void SwitchLevels(Game game, int levelNum)
    {
        Key[] numKeys = [Key.NumRow1, Key.NumRow2, Key.NumRow3, Key.NumRow4];

        // Check for key press to load levels and update the game instance's currentLevel
        DataManager.SaveData(game);

        game.currentLevel = Game.levels[levelNum];
        DataManager.LoadLevel(game);  
        game.levelNum = levelNum;
    }

    public static void MoveCamera(Camera camera, Player player)
    {
        if (player.x > camera.leftMapBound && player.x < camera.rightMapBound)
        {
            camera.CameraPositionX = (player.x - camera.leftMapBound);
        }
        if (player.x < camera.leftMapBound || player.x > camera.rightMapBound) camera.CameraPositionX = float.Round(camera.CameraPositionX);
    }

    public static void MovePlayer(Game game, Player player)
    {
        if (Engine.GetKeyUp(Key.D)) player.movingRight = false;
        if (Engine.GetKeyUp(Key.A)) player.movingLeft = false;

        if (Engine.GetKeyDown(Key.D))
        {
            player.movingRight = true;
            player.facingRight = true;
        }
        if (Engine.GetKeyDown(Key.A))
        {
            player.movingLeft = true;
            player.facingRight = false;
        }

        if (Engine.GetKeyDown(Key.W) && !player.jumpState)
        {
            player.jumpState = true;
        }
        
        if (Engine.GetKeyDown(Key.S) && !player.isSliding && !player.isDucking)
        {
            if (Math.Abs(player.currentXVelocity) > 0) player.Slide();
            else player.Duck();
        }

        if (Engine.GetKeyDown(Key.Space))
        {
            player.Dash();
        }
    }

    // Adding score for debugging
    public static void CheckIncrementScore(Level level)
    {
        if (Engine.GetKeyUp(Key.Equals))
        {
            level.levelScore += 10;
        }
    }

    public static void CheckClearData(Game game)
    {
        if (Engine.GetKeyUp(Key.C)/* || game.currentLevel.player.health == 0*/)
        {
            DataManager.ClearData(game);
            game.currentLevel.player.ResetHealth(DataManager.defaultHealth);
            Game.gameReset = true;
        }
    }
}