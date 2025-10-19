using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

class Game
{
    public static readonly string Title = "Forged Future";
    public static readonly Point2 Resolution = new(1600, 1024);

    public bool DEBUG = true;

    public Level currentLevel { get; internal set; }
    public int levelNum { get; internal set; }
    public Camera camera { get; internal set; }

    public static AudioManager audioManager { get; internal set; }

    public static Font titleFont { get; internal set; }
    public static Font subtitleFont { get; internal set; }
    public static Font captionFont { get; internal set; }

    public static int[] scores { get; internal set; }
    public static (float, float)[] positions { get; internal set; }
    public static List<List<int>> inventories { get; internal set; }
    public static int[] health { get; internal set; }


    public static List<string> levelFiles { get; internal set; }

    public static bool gameStarted { get; internal set; } = false;
    public static bool gameReset { get; internal set; } = false;

    public static Level[] levels { get; internal set; }

    public Game() { }

    public void Initialize()
    {
        // Initialize level file names in a List
        levelFiles = new List<string>
        {
            "level1.txt", // First level
            "level2.txt", // Second level
            "level3.txt", // Third level
            "boss.txt"    // Boss level
        };

        levels = new Level[levelFiles.Count];

        scores = new int[levelFiles.Count];
        positions = new (float, float)[levelFiles.Count];
        inventories = new List<List<int>>();
        health = new int[levelFiles.Count];

        for (int i = inventories.Count; i < levelFiles.Count; i++)
        {
            inventories.Add(new List<int>());
        }

        DataManager.LoadData();

        // Update level names into static list in Level class
        Level.SetLevelFiles(levelFiles);

        // Load all levels and set first level as default
        for (int i = 0; i < levelFiles.Count; i++)
        {
            levels[i] = Level.LoadLevel(i);
        }
        currentLevel = levels[0];

        // Initialize camera
        camera = new Camera(this);

        // Audio manager instance to play sounds
        audioManager = new AudioManager();

        // Initialize score and position for current level
        DataManager.LoadLevel(this);

        UIScreenManager.InitializeScreens();
    }
    
    public void LoadContent()
    {
        titleFont = Engine.LoadFont("Retro Gaming.ttf", 35);
        subtitleFont = Engine.LoadFont("Retro Gaming.ttf", 20);
        captionFont = Engine.LoadFont("Retro Gaming.ttf", 15);
    }

    public void Update()
    {
        //get player and boss
        Player player = currentLevel.player;
        Boss boss = currentLevel.GetBoss();
        
        InputManager.CheckMouseClick();

        //Handle level switching
        InputManager.MoveCamera(camera, currentLevel.player);

        Renderer.DrawParallax(camera, automatic: !gameStarted);

        if (gameStarted)
        {
            
            currentLevel.levelTime += Engine.TimeDelta;
            if (currentLevel.levelTime > 1.0)
            {
                if (currentLevel.levelScore > 0) currentLevel.levelScore -= (int) (currentLevel.levelTime);
                currentLevel.levelTime = 0;
            }

            //Update player
            player.UpdatePosition(currentLevel.GetEntities());
            foreach (Enemy enemy in currentLevel.GetEnemies())
            {
                enemy.UpdatePosition(currentLevel, player, currentLevel.GetEntities());
            }
            if (boss!=null) boss.UpdatePosition(player, currentLevel.GetEntities());


            if (DEBUG) InputManager.SwitchLevels(this);
            InputManager.MovePlayer(this, player);
            InputManager.CheckIncrementScore(currentLevel);
            InputManager.CheckClearData(this);

            if (player.GetPlayerMoving() && gameReset)
            {
                gameReset = false;
            }

            foreach (Entity e in currentLevel.GetEntities())
            {
                if (e is Collectable entity)
                {
                    // Only draw collectables that haven't been collected
                    if (!player.GetInventory().GetInventoryList().Contains(e) && !inventories.ElementAt(currentLevel.levelNum).Contains(entity.GetId()))
                    {
                        if (player.InContactWith(e) && !gameReset)
                        {
                            player.GetInventory().AddToInventory(entity);
                            if (entity is Materials mat)
                            {
                                /*for (int i = 0; i<1; i++) */player.GetInventory().AddToInventory(entity);
                            }
                            inventories[currentLevel.levelNum].Add(entity.GetId());

                            if (entity is Coin coin)
                            {
                                currentLevel.levelScore += 100;
                            }
                            if (entity is JumpBoots j)
                            {
                                player.SetMaxJumps(j.maxJumps);
                            }

                            audioManager.PlayCollectable();
                            
                        }
                        Renderer.DrawEntity(e, camera);
                    }
                }
                else Renderer.DrawEntity(e, camera);

                if (e is not Player)
                {
                    if (e is AreaOfEffectPlatform areaOfEffectPlatform) areaOfEffectPlatform.InContactWithPlayer(player, player.InContactWith(e), 'L');
                    if (e is PlatformBlock platformBlock) platformBlock.InContactWithPlayer(player.InContactWith(e));
                    if (e is MovingPlatform movingPlatform) movingPlatform.UpdatePosition(currentLevel.GetEntities());
                    if (e is AreaOfEffectPlatform aoe) aoe.UpdatePosition(currentLevel.GetEntities());
                    if (e is Arrow arrow) arrow.UpdatePosition(currentLevel.GetEntities());
                    if (e is Laser laser) laser.HandleLaserbeam(player);
                    if (e is FallingPlatform fallingPlatform) fallingPlatform.UpdatePosition(player);
                    if (e is BuildableBlock buildableBlock) buildableBlock.HandleBuild(player);
                    if (e is Door door) door.HandleDoor(player);
                    if (e is FinalDoor finalDoor) finalDoor.HandleFinalDoor(player, currentLevel);
                }
            }

            Renderer.DrawScore(currentLevel);
            Renderer.DrawHealth(currentLevel);
            Renderer.DrawCollectedItems(currentLevel);
            Renderer.DrawBossHealthBar(currentLevel);
            Renderer.DrawTask(currentLevel);

            InputManager.CheckGameOver(this, false);
        }
        else
        {
            Renderer.DrawUIComponents();
        }

        if (player.y > 100f)
        {
            //Debug.WriteLine(levelNum);
            if (levelNum < 3)
            {
                InputManager.SwitchLevels(this, levelNum + 1);
            }
            else InputManager.CheckGameOver(this, true);
        }

    }
}