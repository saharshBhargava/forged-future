using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class DataManager
{
    public static int defaultScore { get; internal set; } = 0;
    public static int defaultHealth { get; internal set; } = 1;
    public static (float, float) defaultPosition { get; internal set; } = (1f, 13f);
    public static List<int> defaultInventory { get; internal set; } = new List<int>();
    
    public static void LoadData()
    {
        for (int i = 0; i < Game.levelFiles.Count; i++)
        {
            string levelDataFilePath = $@"..\..\..\Assets\PlayerData\level{i}Data.txt";

            if (!File.Exists(levelDataFilePath))
            {
                // Create the file and initialize to default values
                SetDefaults(i);
            }
            else
            {
                string dataText = File.ReadAllText(levelDataFilePath);
                string[] parts = dataText.Split(';');

                if (parts.Length == 2)
                {
                    // Parse score and position
                    string[] playerDataString = parts[0].Split(',');
                    if (playerDataString.Length == 4 &&
                        int.TryParse(playerDataString[0], out int score) &&
                        int.TryParse(playerDataString[1], out int health) &&
                        float.TryParse(playerDataString[2], out float x) &&
                        float.TryParse(playerDataString[3], out float y))
                    {
                        Game.scores[i] = score;
                        Game.health[i] = health;
                        Game.positions[i] = (x, y);
                    }
                    else
                    {
                        // Default on invalid data
                        SetDefaults(i);
                    }

                    // Parse inventory
                    Game.inventories[i] = parts[1]
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(id => int.Parse(id))
                        .ToList();
                }
                else
                {
                    // Default on invalid data
                    SetDefaults(i);
                }
            }
        }
    }

    public static void SaveData(Game game)
    {
        Game.scores[game.currentLevel.levelNum] = game.currentLevel.levelScore;
        Game.health[game.currentLevel.levelNum] = game.currentLevel.player.health;
        Game.positions[game.currentLevel.levelNum] = (game.currentLevel.player.x, game.currentLevel.player.y);

        for (int i = 0; i < Game.levelFiles.Count; i++)
        {
            // Create a unique file path for each level's data
            string levelDataFilePath = $@"..\..\..\Assets\PlayerData\level{i}Data.txt";

            // Combine the score and position into a single line
            var position = Game.positions[i];
            string inventoryData = string.Join(",", Game.inventories[i]);
            string dataText = $"{Game.scores[i]},{Game.health[i]},{position.Item1},{position.Item2};{inventoryData}";

            // Write the data to the file
            File.WriteAllText(levelDataFilePath, dataText);
        }
    }

    public static void LoadLevel(Game game)
    {
        game.currentLevel.levelScore = Game.scores[game.currentLevel.levelNum];
        game.currentLevel.player.health = Game.health[game.currentLevel.levelNum];
        game.currentLevel.player.x = Game.positions[game.currentLevel.levelNum].Item1;
        game.currentLevel.player.y = Game.positions[game.currentLevel.levelNum].Item2;

        game.currentLevel.player.GetInventory().GetInventoryList().Clear();
        
        foreach (int collectableId in Game.inventories[game.currentLevel.levelNum])
        {
            foreach (Entity e in game.currentLevel.GetEntities())
            {
                if (e.GetId() == collectableId && e is Collectable c)
                {
                    if (e is JumpBoots boots)
                    {
                        game.currentLevel.player.SetMaxJumps(boots.maxJumps);
                    }

                    game.currentLevel.player.GetInventory().AddToInventory(c);
                }
            }
        }

        game.camera = new Camera(game);
    }

    public static void ClearData(Game game)
    {
        // Reset values to defaults
        SetDefaults(game.levelNum);
        Game.levels[game.levelNum] = Level.LoadLevel(game.levelNum);
        Game.inventories[game.levelNum] = new();
        
            // Create a unique file path for each level's data
        string levelDataFilePath = $@"..\..\..\Assets\PlayerData\level{game.levelNum}Data.txt";

            // Write the default values to the file
        File.WriteAllText(levelDataFilePath, $@"{defaultScore},{defaultHealth},{defaultPosition.Item1},{defaultPosition.Item2}");

        game.currentLevel = Game.levels[game.levelNum];
        game.camera = new Camera(game);
    }

    public static void SetDefaults(int levelNum)
    {
        Game.scores[levelNum] = defaultScore;
        Game.health[levelNum] = defaultHealth;
        Game.positions[levelNum] = defaultPosition;
        Game.inventories[levelNum] = defaultInventory;
    }
}