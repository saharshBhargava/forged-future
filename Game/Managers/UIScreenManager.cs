using System;
using System.Collections.Generic;
using System.Linq;

static class UIScreenManager
{
    private static List<UIScreen> screens;
    public static UIScreen activeScreen { get; internal set; }

    public static void InitializeScreens()
    {
        screens = [];

        screens.Clear();
        screens.Add(CreateStartScreen());
        screens.Add(CreateScoreboardScreen());
        screens.Add(CreateRulesScreen());
        screens.Add(CreateCreditsScreen());

        activeScreen = screens.ElementAt(0);
    }

    public static void ShowScreen(UIScreen screen)
    {
        activeScreen = screen;
    }

    private static UIScreen CreateStartScreen()
    {
        UIScreen startScreen = new UIScreen();

        Container titleContainer = new Container(0, 0, Game.Resolution.X, Game.Resolution.Y /4.0f, Color.Black);
        UIText title = new UIText(titleContainer, "Forged Future", fontSize: 65, Alignment.MIDDLE, Alignment.MIDDLE, 0, 0, Color.LightSteelBlue);
        startScreen.AddElement(titleContainer);

        Action startGame = () => Game.gameStarted = true;
        Container playContainer = new Container(0, Game.Resolution.Y / 4.0f, Game.Resolution.X, Game.Resolution.Y / 4.0f);
        Button playButton = new Button(playContainer, 200, 100, Alignment.MIDDLE, Alignment.MIDDLE, 0, 0, Color.White, startGame);
        UIText playText = new UIText(playButton, "Play", fontSize: 25, Alignment.MIDDLE, Alignment.MIDDLE, 0, 0, Color.Black);
        startScreen.AddElement(playContainer);

        Container buttonsContainer = new Container(0, Game.Resolution.Y / 2.0f, Game.Resolution.X, Game.Resolution.Y / 4.0f);

        Action displayScores = () =>
        {
            screens[1] = CreateScoreboardScreen();
            ShowScreen(screens.ElementAt(1));
        };
        Button scoresButton = new Button(buttonsContainer, 200, 100, Alignment.MIDDLE, Alignment.TOP, 0, -50, Color.White, displayScores);
        UIText scoresText = new UIText(scoresButton, "Scores", fontSize: 25, Alignment.MIDDLE, Alignment.MIDDLE, 0, 0, Color.Black);
        startScreen.AddElement(scoresButton);

        Button rulesButton = new Button(buttonsContainer, 200, 100, Alignment.MIDDLE, Alignment.MIDDLE, 0, 0, Color.White, () => ShowScreen(screens.ElementAt(2)));
        UIText rulesText = new UIText(rulesButton, "Rules", fontSize: 25, Alignment.MIDDLE, Alignment.MIDDLE, 0, 0, Color.Black);
        startScreen.AddElement(rulesButton);

        Button creditsButton = new Button(buttonsContainer, 200, 100, Alignment.MIDDLE, Alignment.BOTTOM, 0, 50, Color.White, () => ShowScreen(screens.ElementAt(3)));
        UIText creditsText = new UIText(creditsButton, "Credits", fontSize: 25, Alignment.MIDDLE, Alignment.MIDDLE, 0, 0, Color.Black);
        startScreen.AddElement(creditsButton);

        return startScreen;
    }

    private static UIScreen CreateScoreboardScreen()
    {
        UIScreen scoreboardScreen = new UIScreen();

        Container fullScreen = new Container(0, 0, Game.Resolution.X, Game.Resolution.Y);

        Container titleContainer = new Container(0, 0, Game.Resolution.X, Game.Resolution.Y / 4.0f, Color.Black);

        UIText title = new UIText(titleContainer, "Scoreboard", fontSize: 65, Alignment.MIDDLE, Alignment.MIDDLE, px: 0, py: 0, Color.LightSteelBlue);

        scoreboardScreen.AddElement(titleContainer);

        float center = title.dimensions.Position.Y + 150;

        for (int i = 0; i < Game.scores.Length; i++)
        {
            string currentScore = $"Level {i + 1}: {Game.scores[i]}";

            center += 100;

            Container textContainer = new Container(Game.Resolution.X / 2.0f, center, 500, 100, new Color(0, 0, 0, 150));

            textContainer.dimensions = new Bounds2(new Vector2(textContainer.dimensions.Position.X - textContainer.dimensions.Size.X / 2.0f, textContainer.dimensions.Position.Y), textContainer.dimensions.Size);
            UIText text = new UIText(textContainer, currentScore, 25, Alignment.MIDDLE, Alignment.MIDDLE, 0, 0, Color.White);

            scoreboardScreen.AddElement(textContainer);
        }

        Action returnToHome = () => ShowScreen(screens.ElementAt(0));

        Button homeButton = new Button(fullScreen, 200, 100, Alignment.MIDDLE, Alignment.BOTTOM, 0, -100, Color.White, returnToHome);
        UIText homeText = new UIText(homeButton, "Home", 25, Alignment.MIDDLE, Alignment.MIDDLE, 0, 0, Color.Black);

        scoreboardScreen.AddElement(fullScreen);

        return scoreboardScreen;
    }

    private static UIScreen CreateRulesScreen()
    {
        UIScreen rulesScreen = new UIScreen();

        Container fullScreen = new Container(0, 0, Game.Resolution.X, Game.Resolution.Y);

        Container titleContainer = new Container(0, 0, Game.Resolution.X, Game.Resolution.Y / 4.0f, Color.Black);

        UIText title = new UIText(titleContainer, "Rules", fontSize: 65, Alignment.MIDDLE, Alignment.MIDDLE, px: 0, py: 0, Color.LightSteelBlue);

        rulesScreen.AddElement(titleContainer);

        float center = title.dimensions.Position.Y + 150;

        string[] texts = ["1. Horizontal movement: 'A' or 'D'", "2. Jumping: 'W'", "3. Ducking: 'S'", "4. Sliding: 'S' + 'A' or 'S' + 'D'", "5. Dash: 'Space'", "6. Get power-ups to unlock abilities", "7. Use materials to build at indicated areas", "8. Collect tokens to increase score", "9. Score is based on time and tokens"];

        for (int i = 0; i < texts.Length; i++)
        {
            center += 50;

            Container textContainer = new Container(Game.Resolution.X / 2.0f, center, 1000, 50, new Color(0, 0, 0, 150));

            textContainer.dimensions = new Bounds2(new Vector2(textContainer.dimensions.Position.X - textContainer.dimensions.Size.X / 2.0f, textContainer.dimensions.Position.Y), textContainer.dimensions.Size);
            UIText text = new UIText(textContainer, texts[i], 15, Alignment.MIDDLE, Alignment.MIDDLE, 0, 0, Color.White);

            rulesScreen.AddElement(textContainer);
        }

        Action returnToHome = () => ShowScreen(screens.ElementAt(0));

        Button homeButton = new Button(fullScreen, 200, 100, Alignment.MIDDLE, Alignment.BOTTOM, 0, -100, Color.White, returnToHome);
        UIText homeText = new UIText(homeButton, "Home", 25, Alignment.MIDDLE, Alignment.MIDDLE, 0, 0, Color.Black);

        rulesScreen.AddElement(fullScreen);

        return rulesScreen;
    }

    private static UIScreen CreateCreditsScreen()
    {
        UIScreen creditsScreen = new UIScreen();

        Container fullScreen = new Container(0, 0, Game.Resolution.X, Game.Resolution.Y);

        Container titleContainer = new Container(0, 0, Game.Resolution.X, Game.Resolution.Y / 4.0f, Color.Black);

        UIText title = new UIText(titleContainer, "Game Credits", fontSize: 65, Alignment.MIDDLE, Alignment.MIDDLE, px: 0, py: 0, Color.LightSteelBlue);

        creditsScreen.AddElement(titleContainer);

        float center = title.dimensions.Position.Y + 100;

        string[] texts = ["Minimalist Game Framework", "Programmers:", "Bhanu Atmakuri, Saharsh Bhargava", "Vivaan Pradhan, Sacchin Saravanan", "Game Art created in Adobe Photoshop", "Sound Effects:", "Sound Effect by FreeSoundsxx from Pixabay", "Hurt1 by MrEchobot -- https://freesound.org/s/745185/ -- License: Attribution 4.0", "Damage by qubodup -- https://freesound.org/s/211634/ -- License: Attribution 4.0", "Retro, Coin 07.wav by MATRIXXX_ -- https://freesound.org/s/515738/ -- License: Creative Commons 0", "Music by Vasil Trofimov from Pixabay"];

        for (int i = 0; i < texts.Length; i++)
        {
            center += 50;

            Container textContainer = new Container(Game.Resolution.X / 2.0f, center, 1000, 50, new Color(0, 0, 0, 150));

            textContainer.dimensions = new Bounds2(new Vector2(textContainer.dimensions.Position.X - textContainer.dimensions.Size.X / 2.0f, textContainer.dimensions.Position.Y), textContainer.dimensions.Size);
            UIText text = new UIText(textContainer, texts[i], 15, Alignment.MIDDLE, Alignment.MIDDLE, 0, 0, Color.White);

            creditsScreen.AddElement(textContainer);
        }

        Action returnToHome = () => ShowScreen(screens.ElementAt(0));
        Button homeButton = new Button(fullScreen, 200, 100, Alignment.MIDDLE, Alignment.BOTTOM, 0, -100, Color.White, returnToHome);
        UIText homeText = new UIText(homeButton, "Home", 25, Alignment.MIDDLE, Alignment.MIDDLE, 0, 0, Color.Black);

        creditsScreen.AddElement(fullScreen);

        return creditsScreen;
    }

    public static UIScreen CreateGameOverScreen(Game game, bool win)
    {
        UIScreen gameOverScreen = new UIScreen();

        Container fullScreen = new Container(0, 0, Game.Resolution.X, Game.Resolution.Y);

        Container titleContainer = new Container(0, 0, Game.Resolution.X, Game.Resolution.Y / 4.0f, Color.Black);

        string gameOverText = "You Won!";

        if (!win)
        {
            gameOverText = "You Lost!";
        }

        UIText title = new UIText(titleContainer, gameOverText, fontSize: 65, Alignment.MIDDLE, Alignment.MIDDLE, px: 0, py: 0, Color.LightSteelBlue);

        UIText subTitle = new UIText(titleContainer, "Score: " + game.currentLevel.levelScore, fontSize: 35, Alignment.MIDDLE, Alignment.BOTTOM, px: 0, py: -50, Color.White);

        gameOverScreen.AddElement(titleContainer);

        Container buttonsContainer = new Container(0, Game.Resolution.Y / 2.0f, Game.Resolution.X, Game.Resolution.Y / 4.0f);

        Action playAgain = () => 
        {
            Game.gameStarted = true;
            DataManager.ClearData(game); 
        };

        Button playAgainButton = new Button(buttonsContainer, 200, 100, Alignment.MIDDLE, Alignment.TOP, 0, -50, Color.White, playAgain);
        UIText playAgainText = new UIText(playAgainButton, "Play Again", fontSize: 25, Alignment.MIDDLE, Alignment.MIDDLE, 0, 0, Color.Black);

        Action displayScores = () =>
        {
            DataManager.ClearData(game);
            screens[1] = CreateScoreboardScreen();
            ShowScreen(screens.ElementAt(1));
        };

        Button scoresButton = new Button(buttonsContainer, 200, 100, Alignment.MIDDLE, Alignment.MIDDLE, 0, 0, Color.White, displayScores);
        UIText scoresText = new UIText(scoresButton, "Scores", fontSize: 25, Alignment.MIDDLE, Alignment.MIDDLE, 0, 0, Color.Black);

        Action returnToHome = () =>
        {
            ShowScreen(screens.ElementAt(0));
            DataManager.ClearData(game);
        };
        Button homeButton = new Button(buttonsContainer, 200, 100, Alignment.MIDDLE, Alignment.BOTTOM, 0, 50, Color.White, returnToHome);
        UIText homeText = new UIText(homeButton, "Home", fontSize: 25, Alignment.MIDDLE, Alignment.MIDDLE, 0, 0, Color.Black);

        gameOverScreen.AddElement(buttonsContainer);

        gameOverScreen.AddElement(fullScreen);

        return gameOverScreen;
    }
}