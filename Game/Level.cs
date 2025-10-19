using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

class Level
{
    private List<Entity> entities;
    private string levelName; // To store the file name of the loaded level
    private static List<string> levelFiles;
    public Player player { get; private set; }
    private Boss boss;
    private List<Enemy> enemies;

    public int levelNum { get; internal set; }
    public int levelScore = 1;

    public int width { get; private set; }
    public int height { get; private set;  }

    public float levelTime = 0;

    private static List<char> platformBlocksCharList = ['G', 'P', 'F', 'W'];

    Level()
    {
        entities = new();
        levelName = "";
        player = new Player(1, 13);
        enemies = new List<Enemy>();
        levelScore = 0;
    }

    // Load the level from a file and return the Level object
    public static Level LoadLevel(int num)
    {
        // Ensure the level number is valid
        if (num < 0 || num >= levelFiles.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(num), $"Invalid level number: {num}. Must be between 0 and {levelFiles.Count - 1}.");
        }

        string levelFilePath = @"..\..\..\Assets\Levels\" + levelFiles.ElementAt(num);

        // Check if the file exists before attempting to load
        if (!File.Exists(levelFilePath))
        {
            throw new FileNotFoundException($"Level file not found: {levelFilePath}");
        }

        Level level = new();
        bool parsingMap = false;
        int y = 0;
        level.levelNum = num;
        int numCollectables = 0;

        // Read the file and load the entities from it
        foreach (string line in File.ReadAllLines(levelFilePath))
        {
            if (parsingMap)
            {
                int x = 0;
                while (x < line.Length)
                {
                    if (line[x] != ' ') // Ignore empty spaces
                    {
                        if (platformBlocksCharList.Contains(line[x])) level.AddEntity(new PlatformBlock(x, y, line[x]));
                        else if (line[x] == '|')
                        {
                            level.AddEntity(new Door(x, y));
                        }
                        else if (line[x] == ']')
                        {
                            level.AddEntity(new FinalDoor(x, y));
                        }
                        else if (line[x] == 'R')
                        {
                            level.AddEntity(new BuildableBlock(x, y, line[x]));
                        }
                        else if (line[x] == 'M')
                        {
                            level.AddEntity(new MovingPlatform(x, y, 'F', 5f, 3));
                        }
                        else if (line[x] == 'C')
                        {
                            level.AddEntity(new MovingPlatform(x, y, line[x], 7.5f, 3, conveyor: true));
                        }
                        else if (line[x] == 'D')
                        {
                            level.AddEntity(new FallingPlatform(x, y, line[x]));
                        }
                        else if (line[x] == 'S')
                        {
                            Laser laser = new Laser(x, y, line[x]);
                            Laserbeam laserbeam = new Laserbeam(x - 50.0f, y + 0.25F);
                            laser.laserbeam = laserbeam;
                            level.AddEntity(laser);
                            level.AddEntity(laserbeam);
                        }
                        else if (line[x] == '>')
                        {
                            level.AddEntity(new Arrow(x, y));
                        }
                        else if (line[x] == 'E')
                        {
                            level.AddEntity(new AreaOfEffectPlatform(x, y, line[x], 1));

                        }
                        else if (line[x] == '@')
                        {
                            level.player.x = x;
                            level.player.y = y;
                        }
                        else if (line[x] == '$')
                        {
                            Enemy enemy = new Enemy(x, y);
                            level.AddEntity(enemy);
                        }
                        else if (line[x] == 'B')
                        {
                            Boss boss = new Boss(x, y);
                            level.SetBoss(boss);
                            level.AddEntity(boss);
                        }
                        else
                        {
                            CollectableManager collectableManager = new CollectableManager();
                            Collectable obj = collectableManager.ReturnCollectable(line[x]);
                            obj.id = numCollectables;
                            numCollectables++;
                            obj.x = x;
                            obj.y = y;
                            level.AddEntity(obj);
                        }
                    }

                    x++; // Skip spaces
                    if (x > level.width) level.width = x;
                }
                y++;
                if (x > level.height) level.height = x;
            }

            if (line.Equals("map:")) parsingMap = true;
        }

        level.AddEntity(level.player);
        return level;
    }

    public Boss GetBoss() => boss;
    public void SetBoss(Boss boss) => this.boss = boss;

    public List<Enemy> GetEnemies() => enemies;

    // Get the list of entities in the level
    public List<Entity> GetEntities() => entities;

    // Add an entity to the level
    public void AddEntity(Entity e)
    {
        entities.Add(e);

        if (e.GetType() == typeof(Enemy))
        {
            enemies.Add((Enemy) e);
        }
    }

    // Set list of level file names, called once from 
    public static void SetLevelFiles(List<string> levelList)
    {
        levelFiles = levelList;
    }
}
