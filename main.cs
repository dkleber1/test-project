//Author: Dana Kleber
//FileName: main.cs
//Project Name: PASS4
//Creation Date: Dec 28 2020
//Modified Date: Jan 27 2021
//Description: To play an educatonal game where the player is presented with a level and they need to enter commands to navigate through the game levels to get to the end goal, while collecting all necessary features.

using System;
using System.IO;
using System.Collections.Generic;

class MainClass : AbstractGame
{
  //Game States - Add/Remove/Modify as needed
  //These are the most common game states, but modify as needed
  //You will ALSO need to modify the two switch statements in Update and Draw
  private const int MENU = 0;
  private const int NAMES = 1;
  private const int INSTRUCTIONS = 2;
  private const int GAMEPLAY = 3;
  private const int PAUSE = 4;
  private const int ENDGAME = 5;
  private const int BOARD = 6;

  //Choose between UI_RIGHT, UI_LEFT, UI_TOP, UI_BOTTOM, UI_NONEs
  private static int uiLocation = Helper.UI_BOTTOM;

  ////////////////////////////////////////////
  //Set the game and user interface dimensions
  ////////////////////////////////////////////

  //Min: 5 top/bottom, 10 left/right, Max: 30
  private static int uiSize = 6;

  //On VS: Max: 120 - uiSize, UI_NONE gives full width up to 120
  //On Repl: Max: 80 - uiSize, UI_NONE can give full width up to 80
  private static int gameWidth = 80;

  //On VS: Max: 50 - uiSize, UI_NONE gives full height up to 50
  //On Repl: Max: 24 - uiSize, UI_NONE can give full height up to 24
  private static int gameHeight = 18;

  //Store and set the initial game state, typically MENU to start
  int gameState = MENU;

  ////////////////////////////////////////////
  //Define your Global variables here (They do NOT need to be static)
  ////////////////////////////////////////////

  // variables that represent the title menu o
  const int START = 1;
  const int EXPLAIN = 2;
  const int HIGHSCORES = 3;
  const int END = 4;

  // variable to store option picked and assign the first menu choice option to start option
  int pickedOpt = START;

  // storing variables for scores with name tracking
  int [] scoreLeading = new int[10];
  int lowest;
  string lowestNme;
  int pl1;
  int pl2;
  string nme = "USERNAME: ";
  string scre = "SCORE: ";
  string [] userNames = new string[10];
  string [] data;

  float deltaTime;
  
  // variables for reading and writing to file
  static StreamReader inFile;
  static StreamReader inFile2;

  // variables stored to be used for sorting
  bool chosen = false;
  bool selectedChoice1 = false;

  // variable as a string for the commands the user chooses
  string commands = "";

  // store variables for user names
  string data2;
  string nameDes; 
  List <string> listNames = new List <string>();

  // store variables to display message text
  string goalMsg = "The goal is to try to complete each level and to reach the goal you will need to collect all gems/keys. Press escape/return to return to menu.";

  string playMsg = "There are commands that will help you complete the level and reach the goal, utilize them in the game and try to use as less as possible";

  string commandsMsg = "d = move right, a = move left, c = collect object on space, e = jump right, q = jump left, + = push crate right, - = push crate left, s to start loop and repeat # times f = stop";

  string scoreMsg = "You want to get the lowest score possible similar to gold. The final score is a sum of individual level scores, so level time added with the number of commands used multiplied by 100. It is all in ms.";

  string systemMsg = "If you're sucessful you will get to go to the next level and continue going until you finish all levels. If you fail you will have to retry the level and try to be sucessful again. Press escape/return to return to the menu.";


  // assinging images for main menu images 
  Image mainImg;
  Image playImg;
  Image instructionsImg;
  Image scoresImg;
  Image exitImg;
  Image pointerImg;
  
  // assinging images for the objects in the game
  Image crateImg;
  Image doorImg;
  Image gemImg;
  Image goalImg;
  Image keyImg;
  Image playerImg;
  Image spikesImg;
  Image wallImg;

  // assinging game objects for main menu image logos
  GameObject main; 
  GameObject play;
  GameObject instructions;
  GameObject scores;
  GameObject exit;
  GameObject arrow;
  GameObject info;
  GameObject pointer;

  // storing player as a game objext
  GameObject player;

  // storing tiles as a list 
  List<GameObject> tiles2 = new List<GameObject>();

  // assinging game text objects for the scores with usernames
  GameTextObject leaderbrd;
  GameTextObject playerName;
  GameTextObject playerNumber;
  GameTextObject [] playerScores = new GameTextObject[10];
  GameTextObject name1;
  GameTextObject name2;
  GameTextObject playerScore;

  int screenIns;

  // assinging UI text objects to show the player the choices on the page
  UITextObject command1;
  UITextObject c2;
  UITextObject c3;
  UITextObject c4;
  UITextObject c5;
  UITextObject c6;
  UITextObject c7;
  UITextObject screenI;

  // assigning tiles as a game object to create tiles in the game
  GameObject [,] tiles = new GameObject[gameHeight,gameWidth];

  // creating and storing a new queue to be used for later 
  GameQueue queue = new GameQueue();

  static void Main(string[] args)
  {
    /***************************************************************
                DO NOT TOUCH THIS SECTION
    ***************************************************************/
    GameContainer gameContainer = new GameContainer(new MainClass(), uiLocation, uiSize, gameWidth, gameHeight);
    gameContainer.Start();
  }

  // pre: gc as a gamecontainer
  // post: none
  // desc: loads all game content including images gameobjects and uiobjects
  public override void LoadContent(GameContainer gc)
  {
    //Load all of your "Images", GameObjects and UIObjects here.
    //This is also the place to setup any other aspects of your program before the game loop starts

    // Load all menu images and logos
    playImg = Helper.LoadImage("Images/Play.txt");
    play = new GameObject(gc, playImg, 6, 5, true);

    instructionsImg = Helper.LoadImage("Images/Instructions.txt");
    info = new GameObject(gc, instructionsImg, 20, 10, true);

    scoresImg = Helper.LoadImage("Images/Instructions.txt");
    scores = new GameObject(gc, scoresImg, 32, 13, true);

    exitImg = Helper.LoadImage("Images/Exit.txt");
    exit = new GameObject(gc, exitImg, 63, 13, true);

    pointerImg = Helper.LoadImage("Images/Pointer.txt");
    pointer = new GameObject(gc, pointerImg, 11, 9, true);

    // load instruction info
    pickedOpt = START; 
    screenI = new UITextObject(gc, 1, 3, Helper.WHITE, true, "");
     screenIns = 0;


    // Load images of objects
    crateImg = Helper.LoadImage("Images/Crate.txt");
    doorImg = Helper.LoadImage("Images/Door.txt");
    gemImg = Helper.LoadImage("Images/Gem.txt");
    goalImg = Helper.LoadImage("Images/Goal.txt");
    keyImg = Helper.LoadImage("Images/Key.txt");
    playerImg = Helper.LoadImage("Images/Player.txt");
    spikesImg = Helper.LoadImage("Images/Spikes.txt");
    wallImg = Helper.LoadImage("Images/Wall.txt");

    inFile2 = File.OpenText("ExampleLevel.txt");

    // store variables to be used in loop 
    int row1 = 0;
    int col = 0; 
    int i2 = 0;
    int j2 = 0;

    // loop created for the grid of the game which is 9 rows 20 columns, to continue looping until these conditions are met
    for(row1 = 0; row1 < 9; row1++)
    {
      data2 = inFile2.ReadLine();
      for(col = 0; col < 20; col++)
      {
        // create new game object if the below conditions are met
        if (data[j2] == "0")
        {
          pl1 = j2;
          pl2 = i2;
          player = new GameObject(gc, playerImg, pl1 * 2, pl2 * 1, true);
        }
        else if (data[j2] == "1")
        {
          tiles2.Add(new GameObject(gc, wallImg, j2 * 2, i2 * 1, true));
        }
        else
        {
          tiles2.Add(new GameObject(gc, wallImg, j2 * 2, i2 * 1, true));
        }
      }
    }

    inFile2.Close();

    // load leader board
    leaderbrd = new GameTextObject(gc, 10, 4, Helper.WHITE, true, "leader board");
    
    // keep looping while the number of top scores is not yet 10 because we are showing the top 10 scores
    int l = 0;
    while (l < 11)
    {
      // To display the scores for later
      playerScores[l] = new GameTextObject(gc, 10, 4 + l, Helper.BLUE, true, "");
      l++;
    }

    // Load explainations on the commands for how to operate through the game
    name1 = new GameTextObject(gc, 30, 5, Helper.WHITE, true, "Name:");
    name2 = new GameTextObject(gc, 30, 6, Helper.WHITE, true, "");
    playerScore = new GameTextObject(gc, 30, 8, Helper.WHITE, true, scre);
    c4 = new UITextObject(gc, 20, 3, Helper.WHITE, true, "to search for a player's username, press the return/enter key");
    command1 = new UITextObject(gc, 30, 5, Helper.WHITE, true, "");
    c2 = new UITextObject(gc, 4, 5, Helper.WHITE, true, "");
    c3 = new UITextObject(gc, 24, 5, Helper.WHITE, true, "Press ESCAPE to return to the Menu");
    c4 = new UITextObject(gc, 20, 4, Helper.WHITE, true, "hit L to toggle through commands");
    c5 = new UITextObject(gc, 2, 4, Helper.WHITE, true, "");
    c6 = new UITextObject(gc, 10, 1, Helper.BLUE, true, "68 character limit exceeded.");
    c7 = new UITextObject(gc, 0, 0, Helper.BLUE, true, "d = move right, a = move left, c = collect object on space, e = jump right, q = jump left, + = push crate right, - = push crate left, s to start loop and repeat # times f = stop");
  }
  
  // pre: gc as a gamecontainer and deltatime as a flooat
  // post: none
  // desc: updates the game during when its being played
  public override void Update(GameContainer gc, float deltaTime)
  {

    //This will exit your program with the x key.  You may remove this if you want       
    //if (Input.IsKeyDown(ConsoleKey.X)) gc.Stop();

    switch (gameState)
    {
      case MENU:
        //Get and implement menu interactions 
        UpdateMenu(gc, deltaTime);
        break;
      case INSTRUCTIONS:
        //Get user input to return to MENU
        UpdateInstructions(gc, deltaTime);
        break;
      case GAMEPLAY:
        //Implement standared game logic (input, update game objects, apply physics, collision detection)
        UpdateScores(gc);
        break;
      case PAUSE:
        //Get user input to resume the game
        break;
      case ENDGAME:
        //Wait for final input based on end of game options (end, restart, etc.)
        break;
      case NAMES:
      //
      break;
    }
  }

  // pre: gc as a GameContainer
  // post: none
  // desc: Draws anything including objects and etc for the game
  public override void Draw(GameContainer gc)
  {
    //NOTE: The only logic in this section should be draw commands and loops.
    //There may be some minor selection, but choosing what to draw 
    //should be handled in the Update and the visibility property 
    //of GameObject's

    switch (gameState)
    {
      case MENU:
        //Draw the possible menu options
        DrawMenu(gc);
        break;
      case INSTRUCTIONS:
        //Draw the game instructions including prompt to return to MENU 
        break;
      case GAMEPLAY:
        //Draw all game objects on each layers (background, middleground, foreground and user interface)
        break;
      case PAUSE:
        //Draw the pause screen, this may include the full game drawing behind
        break;
      case ENDGAME:
        //Draw the final feedback and prompt for available options (exit,restart, etc.)
        break;
    }
  }

  // Pre: gc as a GameContainer, deltaTime as a float
  // post: NONE
  // Description: Updates menu in the game all of its movemenet and other logic
  private void UpdateMenu(GameContainer gc, float deltaTime)
  {
      // if the user presses D perform the action of moving
      if (Input.IsKeyDown(ConsoleKey.D))
      {
        if (pickedOpt < END)
        {
          pickedOpt++;
          pointer.Move((float)gameWidth/5, 0f);
        }
      }
      // if previous condition is not met and user presses A , move left
      else if (Input.IsKeyDown(ConsoleKey.A))
      {
        if(pickedOpt > START)
        {
          pickedOpt--;
          pointer.Move((float)-gameWidth/5, 0f);
        }
      }
      // if previous conditions are not met and user presses enter perform the following command
      else if(Input.IsKeyDown(ConsoleKey.Enter))
      {
        // changing game states depending on what page we are on
        if (pickedOpt == START)
        {
          gameState = GAMEPLAY;
        }
        if (pickedOpt == EXPLAIN)
        {
          gameState = INSTRUCTIONS;
        }
        if (pickedOpt == HIGHSCORES)
        {
          gameState = BOARD;
        }
        else
        {
          gc.Stop();
        }
      }
  }

  // pre: gc as a gamecontainer
  // Post: none
  // desc: draws images to game menu
  private void DrawMenu(GameContainer gc)
  {
    // Draw the following images to background
    gc.DrawToBackground(play);
    gc.DrawToBackground(info);
    gc.DrawToBackground(instructions);
    gc.DrawToBackground(exit);
    gc.DrawToBackground(pointer);
    gc.DrawToBackground(main);
  }
 
  // pre: the total complete command as a string entered by the user for the game
  // post: none
  // desc: utilizing a queue to store the commands and add to it
  private void QueueGame(string a)
  {
    // store both variables holding the number of loops and the command the user enters as empty with values of zero and blank
    int loopNums = 0;
    string cmnd = "";

    // loop until the final indivudal command in the entire command is met
    for (int i = 0; i < a.Length; i++)
    {
      // if there are loops then loop and add to queue until the loops are complete 
      while (loopNums != 0)
      {
        // continue to add the commands to the queue 
        for (int h = 0; h < cmnd.Length; h++)
        {
          queue.Enqueue(Convert.ToString(cmnd[h]));
        }
        loopNums--;
      }
      // perform if the command has a loop number
      if(a[i] == 's')
      {
        // assigns the loops the player requested
        i++;
        cmnd = a.Substring(i).Substring(0, a.Substring(i).IndexOf("f"));
      }
     else if(a[i] != 's')
    {
      if (a[i] != 'f')
      {
        // continue to add the commands to the queue 
        queue.Enqueue(Convert.ToString(cmnd[i]));
      }
    }
   }  
 }

  // pre: gc as a gamecontainer
  // post: none
  // desc: update the scores of the leaderboard
  private void UpdateScores(GameContainer gc)
  {
    // store and assign variables as int to the size of 0 for the files
    int amountName = 0;
    int l1 = 0;
    
    // if the boolean variable has a value of false which is updated througout the game, read the files
    if (selectedChoice1 == false)
    {
      // read files of names and scores as well as assigns them back into other variables
      inFile = File.OpenText("HighScores.txt");
      data = inFile.ReadLine().Split(',');
      scoreLeading[0] = Convert.ToInt32(data[1]);
      userNames[0] = data[0];
      lowest = scoreLeading[0];
      lowestNme = userNames[0];
      inFile.Close();
        
      // continue looping until last name and score is read 
      for (int i = 0; i < scoreLeading.Length; i++)
      {
        // open the highscores text file
        inFile = File.OpenText("Highscores.txt");
        // continuing reading until the end of the file
        while(!inFile.EndOfStream)
        {
          // reads and stores data from files
          data = inFile.ReadLine().Split(',');
          if (Convert.ToInt32(data[1]) > lowest)
          {
            lowestNme = data[0];
          }

          // If conditions are met set score and name
          if ((Convert.ToInt32(data[1]) > scoreLeading[i - 1] && Convert.ToInt32(data[1]) < scoreLeading[i]) || (i == 0 && Convert.ToInt32(data[1]) < scoreLeading[i]))
          {
            scoreLeading[i] = Convert.ToInt32(data[1]);
            userNames[i] = data[0];
          }
        }
        
        // If conditions are met set score and name
        if (i < scoreLeading.Length - 1)
        {
          scoreLeading[i + 1] = lowest;
          userNames[i + 1] = lowestNme;
        }

        // updating text on the leaderboard
        playerScores[i].UpdateText(userNames[i] + ": " + scoreLeading[i]);

        // close file
        inFile.Close();
      }
      
      // open file called HighScores.txt
      inFile = File.OpenText("HighScores.txt");
      
      // contiue looping and adding user names until the end of the file is reached
      while(!inFile.EndOfStream)
      {
        listNames.Add(inFile.ReadLine());
      }

      // close file
      inFile.Close();

      // sort names and re-assign the boolean variable for future use
      listNames.Sort();
      selectedChoice1 = true;
    }
    
    // If conditions are meant search for its name to update
    if(chosen == false && Input.IsKeyDown(ConsoleKey.Enter))
    {
      nme = "USERNAME: ";
      name2.UpdateText(nme);
      scre = "USER SCORE: ";
      playerScore.UpdateText(scre);
      name1.UpdateText("Enter Username: ");
      chosen = true;
    }
    
    // search for name if conditions are met
    if (chosen == true)
    {
      // If there is actually a name, search for it
      if (nme.Length > l1 && Input.IsKeyDown(ConsoleKey.Enter))
      {
        // binary search for name
        int res = Binary(listNames, 0, listNames.Count - 1);
        // if the name cannot be found, display error
        if(res == -1)
        {
          scre += "Error";
        }
        else
        {
          data = listNames[res].Split(',');
          scre += data[1];
        }

        // update text and inform player name was found, reassign variables
        playerScore.UpdateText(scre);
        name1.UpdateText("Found name");
        chosen = false;
      }  
    }

    // if conditions are met and player presses escape return to menu
    if(Input.IsKeyDown(ConsoleKey.Escape))
    {
      gameState = MENU;
      chosen = false;
    }
  }

  // pre: gc as gamecontainer and delatime as a float variable
  // post: none
  // desc: move through and update instructions shown on how to play the game 
  private void UpdateInstructions(GameContainer gc, float deltaTime)
  {
    // depending on which instruction the user is, display the message for each type of instruction
    switch(screenIns)
    {
      case 0:
      screenI.UpdateText(goalMsg);
      break;
      case 1:
      screenI.UpdateText(playMsg);
      break;
      case 2:
      screenI.UpdateText(commandsMsg);
      break;
      case 3:
      screenI.UpdateText(scoreMsg);
      break;
      case 4:
      screenI.UpdateText(systemMsg);
      break;
    }

    // move to the next instruction if user presses enter
      if(Input.IsKeyDown(ConsoleKey.Enter))
    {
      screenIns++;

      // if there the variable screenIns is larger than the amount required return to menu and re-assign variable to zero
      if(screenIns > 4)
      {
        gameState = MENU;
        screenIns = 0;
      }
    }

    // if user presses escape go back to menu  
    if(Input.IsKeyDown(ConsoleKey.Escape))
    {
      gameState = MENU;
      screenIns = 0;
    }
  } 

   // pre: gc as gamecontainer and delatime as a float variable
  // post: none
  // desc: update the game play as the game is being excuted
  private void UpdateGameExecution(GameContainer gc, float deltaTime)
  {
    PlayerInfo(GAMEPLAY);
    // if player presses escape return to menu
    if(Input.IsKeyDown(ConsoleKey.Escape))
    {
      gameState = MENU;
    }
  }
 
  // pre: the user names as a list of strings and the "left" side and the right side
  private int Binary(List <string> userNames, int l, int r)
  {
    // store first row as a string and assign the value as the middle
    string [] firstR;
    firstR = userNames[(l + r) / 2].Split(',');

    // recursively search for the name being desired by the user
    if (l == r)
    {
      return -1;
    }
    else 
    {
      return Binary(userNames, l, ((l + r) /2));
    }
   }
  
  // pre: the chosen game state
  // post: none
  // Pre: updates all the information about the player as the player is working through the game 
  private void PlayerInfo(int chosenGameState)
  {
    // assign length for the players name to zero to begin with
    int l2 = 0;
    // if player is on the game state highscores and about to enter their name on a highscore
    if (chosenGameState == HIGHSCORES)
    {
      // depending on the letter the player selects for its name, add to its name
      if(Input.IsKeyDown(ConsoleKey.A))
      {
        nme += "A";
      }
      if(Input.IsKeyDown(ConsoleKey.B))
      {
        nme += "B";
      }
      else if(Input.IsKeyDown(ConsoleKey.C))
      {
        nme += "C";
      }
      else if(Input.IsKeyDown(ConsoleKey.D))
      {
        nme += "D";
      }
      else if(Input.IsKeyDown(ConsoleKey.E))
      {
        nme += "E";
      }
      else if(Input.IsKeyDown(ConsoleKey.F))
      {
        nme += "F";
      }
      else if(Input.IsKeyDown(ConsoleKey.G))
      {
        nme += "G";
      }
      else if(Input.IsKeyDown(ConsoleKey.H))
      {
        nme += "H";
      }
      else if(Input.IsKeyDown(ConsoleKey.I))
      {
        nme += "I";
      }
      else if(Input.IsKeyDown(ConsoleKey.J))
      {
        nme += "J";
      }
      else if(Input.IsKeyDown(ConsoleKey.K))
      {
        nme += "K";
      }
      else if(Input.IsKeyDown(ConsoleKey.L))
      {
        nme += "L";
      }
      else if(Input.IsKeyDown(ConsoleKey.M))
      {
        nme += "M";
      }
      else if(Input.IsKeyDown(ConsoleKey.N))
      {
        nme += "N";
      }
      else if(Input.IsKeyDown(ConsoleKey.O))
      {
        nme += "O";
      }
      else if(Input.IsKeyDown(ConsoleKey.P))
      {
        nme += "P";
      }
      else if(Input.IsKeyDown(ConsoleKey.Q))
      {
        nme += "Q";
      }
      else if(Input.IsKeyDown(ConsoleKey.R))
      {
        nme += "R";
      }
      else if(Input.IsKeyDown(ConsoleKey.S))
      {
        nme += "S";
      }
      else if(Input.IsKeyDown(ConsoleKey.T))
      {
        nme += "T";
      }
      else if(Input.IsKeyDown(ConsoleKey.U))
      {
        nme += "U";
      }
      else if(Input.IsKeyDown(ConsoleKey.V))
      {
        nme += "V";
      }
      else if(Input.IsKeyDown(ConsoleKey.W))
      {
        nme += "W";
      }
      else if(Input.IsKeyDown(ConsoleKey.X))
      {
        nme += "X";
      }
      else if(Input.IsKeyDown(ConsoleKey.Y))
      {
        nme += "Y";
      }
      else if(Input.IsKeyDown(ConsoleKey.Z))
      {
        nme += "Z";
      }
      // if the players name is longer than zero and they choose the backspace button, delete a character letter from their name
      else if(nme.Length > l2 && Input.IsKeyDown(ConsoleKey.Backspace))
      {
        int r = nme.Length - 1;
        nme = nme.Remove(r);
      }

      // update the players name text
      name2.UpdateText(nme);
    }

    // perform the follwing commands the player chooses if the game state is gameplay meaning the players playing the game 
    if(chosenGameState == GAMEPLAY)
    {
      // assign and store the last command
      int lst = commands.Length - 1;
      char startLoop = 's';
      // if the players commands are under the limit of 68 characters
      if (commands.Length < 68)
      {
        // if the player's last command isn't to start loop or is blank and empty, perform the actions they choose
        if (commands[lst]!= 's' || commands == "")
        {
          // depending on the letter (command) the player chooses, add to commands
          if(Input.IsKeyDown(ConsoleKey.D))
          {
            commands += "d";
          }
          else if(Input.IsKeyDown(ConsoleKey.A))
          {
            commands += "a";
          }
          else if(Input.IsKeyDown(ConsoleKey.C))
          {
            commands += "c";
          }
          else if(Input.IsKeyDown(ConsoleKey.E))
          {
            commands += "e";
          }
          else if(Input.IsKeyDown(ConsoleKey.Q))
          {
            commands += "q";
          }
          else if(Input.IsKeyDown(ConsoleKey.Add))
          {
            commands += "+";
          }
          else if(Input.IsKeyDown(ConsoleKey.Subtract))
          {
            commands += "-";
          }
          else if(Input.IsKeyDown(ConsoleKey.F))
          {
            commands += "f";
          }
          else if (Input.IsKeyDown(ConsoleKey.S))
          {
           commands += "s";
          }
        }
        
        // if the conditions of having a number can be met perofm the following commands the player chooses to create a loop
        if (commands.Length != 0 && commands[commands.Length - 1] == 's')
        {
          // depending on which option the player chooses, add a number to commands to loop a number of times
          if (Input.IsKeyDown(ConsoleKey.D0))
          {
            commands += "0";
          }
          else if (Input.IsKeyDown(ConsoleKey.D1))
          {
            commands += "1";
          }
          else if (Input.IsKeyDown(ConsoleKey.D2))
          {
            commands += "2";
          }
          else if (Input.IsKeyDown(ConsoleKey.D3))
          {
            commands += "3";
          }
          else if (Input.IsKeyDown(ConsoleKey.D4))
          {
            commands += "4";
          }
          else if (Input.IsKeyDown(ConsoleKey.D5))
          {
            commands += "5";
          }
          else if (Input.IsKeyDown(ConsoleKey.D6))
          {
            commands += "6";
          }
          else if (Input.IsKeyDown(ConsoleKey.D7))
          {
            commands += "7";
          }
          else if (Input.IsKeyDown(ConsoleKey.D8))
          {
            commands += "8";
          }
          else if (Input.IsKeyDown(ConsoleKey.D9))
          {
            commands += "9";
          }
        }
      }
    }
  }
}