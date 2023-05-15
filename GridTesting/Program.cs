using System.ComponentModel;
using System.ComponentModel.Design;
using System.Text;
using System.Data.SqlClient;

//TODO
//Center enemy name in settings menu
//Out of bounds error on Line 297 (6 columns, 5 rows, 4 ships), only shot at (7,1)
namespace GridTesting
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //VARIABLES - commented variables are declared elsewhere
            //KEY: 0 = water, 1 = ship, 2 = hit, 3 = miss
            int mainMenuMax = 4;
            int settingsMenuMax = 8;
            int numberOfShips = 8;
            //int yourShipsRemaining = numberOfShips;
            //int enemyShipsRemaining = numberOfShips;
            int gridRows = 8;
            int gridCols = 8;
            int userRow = 0;
            int userCol = 0;
            bool shotOnBoard = true;
            bool playingGame = true;
            bool gameOver = false;
            int shipIdentifier = 10;
            bool largeShipGenerator = true;
            StringBuilder directionBuilder = new StringBuilder("");
            int deployedShips = 0;
            //int[] shipLifeArray = new int[numberOfShips];
            //Array.Fill(shipLifeArray, 3);
            int shipLifeIdentifier = 0;
            //int chanceToBeHit = 3 * numberOfShips;
            int takeDamage = 0;
            //int yourShipLife = 3 * yourShipsRemaining;
            int turnCounter = 0;
            bool lastShotHit = false;
            bool cheats = false;
            bool settingsMenuing = true;
            int difficultyMultiplier = 1;
            string name = "";
            int score = 0;
            string enemySelection = "NARRATOR";

            //Settings Menu Options
            string cheatsDisplay = "     OFF    ";
            string hpDisplay = "   VISUAL   ";
            bool hpDisiplayNumber = false;
            string difficultyDisplay = "   NORMAL   ";

            SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\shino\source\repos\Session 16\GridTesting\GridTesting\Database1.mdf"";Integrated Security=True");
            //connection.Open();


            //Generate Menu
            string[,] mainMenu = new string[mainMenuMax, 3];

            for (int row = 0; row < mainMenuMax; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    mainMenu[row, col] = "       ";
                }              
            }

            int cursorRow = 0;
            int cursorCol = 1;
            bool mainMenuing = true;
            mainMenu[0, 1] = "     START    ";
            mainMenu[1, 1] = "   GAME TYPE  ";
            mainMenu[2, 1] = "   SETTINGS   ";
            mainMenu[3, 1] = "  HIGH SCORES ";

            Console.WriteLine("Standard Input Stream: {0}",
                             Console.In);

            while (mainMenuing)
            {
                Console.Clear();
                Console.WriteLine("       YOU ARE PLAYING BATTLESHIP       \n");
                Console.WriteLine("                MAIN MENU               \n");
                Console.WriteLine();

                for (int row = 0; row < mainMenuMax; row++)
                {
                    for (int col = 0; col < 3; col++)
                    {
                        if (cursorRow == row && cursorCol == col)                       
                            Console.Write($" [{mainMenu[row, col]}] ");                       
                        else                        
                            Console.Write($"  {mainMenu[row, col]}  ");                       
                    }
                    Console.WriteLine();
                }

                ConsoleKeyInfo keyPressed = Console.ReadKey();

                if ((keyPressed.Key == ConsoleKey.W || keyPressed.Key == ConsoleKey.UpArrow))
                {
                    if (cursorRow > 0)
                        cursorRow -= 1;
                    else if (cursorRow == 0)
                        cursorRow = mainMenuMax - 1;
                }
                else if ((keyPressed.Key == ConsoleKey.S || keyPressed.Key == ConsoleKey.DownArrow))
                {
                    if (cursorRow < mainMenuMax - 1)
                        cursorRow += 1;
                    else if (cursorRow == mainMenuMax - 1)
                        cursorRow = 0;
                }
                //START
                else if (keyPressed.Key == ConsoleKey.Enter && cursorRow == 0)
                {
                    mainMenuing = false;

                    //Opponent generation/selection
                    Enemy enemy = new Enemy();
                    
                    switch (enemySelection)
                    {
                        case "NARRATOR":
                            enemy.enemyName = "  NARRATOR  ";
                            enemy.enemyTurn0Action = "The enemy is prepairing to fire at your ships!";
                            enemy.enemySearching = "The enemy is searching for your ships";
                            enemy.enemyHunting = "The enemy is hunting down your ship!";
                            enemy.shipHitByEnemy = "One of your ships has been hit by the enemy!";
                            enemy.shipSunkByEnemy = "One of your ships has been sunk by the enemy!";
                            enemy.enemyShotMissed = "The enemy's shot missed our ships!";
                            enemy.shotOffBoard = "That shot was off the board!\nTry again!";
                            enemy.hitEnemyShip = "Hit detected - we made contact with an enemy ship!";
                            enemy.sunkEnemyShip = "You sunk a battleship!";
                            enemy.spaceAlreadyHit = "That space has already been hit!";
                            enemy.missedShot = "Missed - nothing but water!";
                            enemy.closeShot = "Close - an enemy ship must be nearby!";
                            enemy.enemyWins = "The enemy has bested you - all of your ships have sunk!";
                            enemy.enemyLoses = "Congratulations! You sunk all of the ships!";
                            break;
                        case "PIRATE":
                            enemy.enemyName = "   PIRATE   ";
                            enemy.enemyTurn0Action = "Arrrrr, I'm comin' fer yer ships!";
                            enemy.enemySearching = "Ye can run, but ye can't hide!";
                            enemy.enemyHunting = "A scent o' blood in the water!";
                            enemy.shipHitByEnemy = "Avast - take that, ye scallywag!";
                            enemy.shipSunkByEnemy = "Down she goes to meet Davey Jones!";
                            enemy.enemyShotMissed = "Alas, me shot be off the mark!";
                            enemy.shotOffBoard = "Yer shootin' in wrong sea, matey!\nTry again!";
                            enemy.hitEnemyShip = "I'm hit - water to the main deck!";
                            enemy.sunkEnemyShip = "Grrrr, you sunk my beauty!";
                            enemy.spaceAlreadyHit = "Ha, ya already shot there, ya landlubber!";
                            enemy.missedShot = "Missed me - ha!";
                            enemy.closeShot = "Arrr - that shot be too close fer comfort!";
                            enemy.enemyWins = "Yer ships are fish food - time to walk the plank!";
                            enemy.enemyLoses = "Ahhhh, ye got me - off I go to Davey Jones!";
                            break;
                        case "robot":
                            break;
                    }
                    
                    
                    Console.Clear();
                    //Run the Game
                    //Console.WriteLine("How many ships will each side deploy in this battle?");
                    //numberOfShips = Convert.ToInt32(Console.ReadLine());
                    
                    int[] shipLifeArray = new int[numberOfShips];
                    Array.Fill(shipLifeArray, 3);
                    int chanceToBeHit = 3 * numberOfShips;
                    int yourShipsRemaining = numberOfShips;
                    int enemyShipsRemaining = numberOfShips;
                    int yourShipLife = 3 * yourShipsRemaining;

                    Console.Clear();
                    //Grid generation
                    int[,] grid = new int[gridRows, gridCols];
                    for (int row = 0; row < gridRows; row++)
                    {
                        for (int col = 0; col < gridCols; col++)
                        {
                            grid[row, col] = 0;
                        }
                    }

                    //Large ship generation
                    while (deployedShips < numberOfShips)
                    {
                        //int shipRow = rando.Next(0, gridRows);
                        //int shipCol = rando.Next(0, gridCols);

                        //Console.WriteLine($"shipRow = {shipRow}, shipCol = {shipCol}");
                        LargeShipGeneration(gridRows, gridCols, grid, largeShipGenerator, shipIdentifier, directionBuilder);
                        deployedShips++;
                        shipIdentifier++;
                    }

                    while (playingGame)
                    {
                        while (shotOnBoard)
                        {
                            if (turnCounter == 0)
                            {
                                Console.WriteLine("No shots made yet");
                                Console.WriteLine("Preparing to fire");
                            }

                            //Draw the Board
                            DrawTheBoard(gridRows, gridCols, grid, shipIdentifier, cheats);

                            //Enemy attacks
                            Random rando = new Random();
                            if (lastShotHit)
                            {
                                Console.WriteLine(enemy.enemyHunting);
                                takeDamage = rando.Next((gridRows * gridCols) / (2 * difficultyMultiplier));
                            }
                            else
                            {
                                Console.WriteLine(enemy.enemySearching);
                                takeDamage = rando.Next(gridRows * gridCols);
                            }

                            if (turnCounter == 0)
                            {
                                Console.WriteLine(enemy.enemyTurn0Action);
                            }
                            else
                            {
                                if (takeDamage < chanceToBeHit)
                                {
                                    Console.WriteLine(enemy.shipHitByEnemy);
                                    lastShotHit = true;
                                    yourShipLife--;
                                    if (yourShipLife % 3 == 0)
                                    {
                                        yourShipsRemaining--;
                                        Console.WriteLine(enemy.shipSunkByEnemy);
                                        lastShotHit = false;
                                        if (yourShipsRemaining == 0)
                                        {
                                            gameOver = true;
                                        }
                                    }
                                }
                                else
                                {
                                    Console.WriteLine(enemy.enemyShotMissed);
                                }
                                if (turnCounter % Math.Floor((gridRows * gridCols) / (10.0 * difficultyMultiplier)) == 0)
                                {
                                    chanceToBeHit++;
                                }
                            }

                            //Console.WriteLine($" turnCounter = {turnCounter}");
                            //Console.WriteLine($"ChanceToBeHit = {chanceToBeHit}");

                            //Draw ships remaining
                            DrawShipsRemaining(yourShipsRemaining, enemyShipsRemaining, hpDisiplayNumber);

                            if (!gameOver)
                            {
                                Console.WriteLine();
                                Console.WriteLine("   What square would you like to shoot at?");
                                Console.WriteLine("Enter the X coordinate (column number)");

                                userCol = Convert.ToInt32(Console.ReadLine()) - 1;
                                Console.WriteLine("Enter the Y coordinate (row number)");

                                userRow = Convert.ToInt32(Console.ReadLine()) - 1;
                                Console.Clear();

                                if (userRow > gridRows || userRow < 0 || userCol > gridCols || userCol < 0)
                                {
                                    Console.WriteLine(enemy.shotOffBoard);
                                }
                                else
                                {
                                    shotOnBoard = false;
                                }
                            }
                            else
                            {
                                shotOnBoard = false;
                            }
                        }

                        if (!gameOver)
                        {
                            shotOnBoard = true;
                            turnCounter++;
                            Console.WriteLine($"Last shot at: ({userCol + 1}, {userRow + 1})");


                            if (grid[userRow, userCol] > 9)
                            {
                                Console.WriteLine(enemy.hitEnemyShip);
                                shipLifeIdentifier = grid[userRow, userCol];
                                shipLifeArray[shipLifeIdentifier - 10] -= 1;
                                grid[userRow, userCol] = 2;
                                if (shipLifeArray[shipLifeIdentifier - 10] == 0)
                                {
                                    Console.WriteLine(enemy.sunkEnemyShip);
                                    enemyShipsRemaining--;
                                    shipLifeArray[shipLifeIdentifier - 10] = 1;
                                }
                            }
                            else if (grid[userRow, userCol] == 2)
                            {
                                Console.WriteLine(enemy.spaceAlreadyHit);
                            }
                            //Edge-casing intensifies - corners
                            //Top left corner
                            else if (userRow == 0 && userCol == 0)
                            {
                                if (grid[userRow, userCol + 1] > 9 || grid[userRow + 1, userCol] > 9)
                                {
                                    Console.WriteLine(enemy.closeShot);
                                    grid[userRow, userCol] = 3;
                                }
                                else if (grid[userRow, userCol] == 0 || grid[userRow, userCol] == 3)
                                {
                                    Console.WriteLine(enemy.missedShot);
                                    grid[userRow, userCol] = 3;
                                }
                            }
                            //Bottom left corner
                            else if (userRow == 7 && userCol == 0)
                            {
                                if (grid[userRow, userCol + 1] > 9 || grid[userRow - 1, userCol] > 9)
                                {
                                    Console.WriteLine(enemy.closeShot);
                                    grid[userRow, userCol] = 3;
                                }
                                else if (grid[userRow, userCol] == 0 || grid[userRow, userCol] == 3)
                                {
                                    Console.WriteLine(enemy.missedShot);
                                    grid[userRow, userCol] = 3;
                                }
                            }
                            //Bottom right corner
                            else if (userRow == 7 && userCol == 7)
                            {
                                if (grid[userRow, userCol - 1] > 9 || grid[userRow - 1, userCol] > 9)
                                {
                                    Console.WriteLine(enemy.closeShot);
                                    grid[userRow, userCol] = 3;
                                }
                                else if (grid[userRow, userCol] == 0 || grid[userRow, userCol] == 3)
                                {
                                    Console.WriteLine(enemy.missedShot);
                                    grid[userRow, userCol] = 3;
                                }
                            }
                            //Top right corner
                            else if (userRow == 0 && userCol == 7)
                            {
                                if (grid[userRow, userCol - 1] > 9 || grid[userRow + 1, userCol] > 9)
                                {
                                    Console.WriteLine(enemy.closeShot);
                                    grid[userRow, userCol] = 3;
                                }
                                else if (grid[userRow, userCol] == 0 || grid[userRow, userCol] == 3)
                                {
                                    Console.WriteLine(enemy.missedShot);
                                    grid[userRow, userCol] = 3;
                                }
                            }
                            //Edge-casing sides
                            //Top side
                            else if (userRow == 0)
                            {
                                if (grid[userRow, userCol + 1] > 9 || grid[userRow, userCol - 1] > 9 || grid[userRow + 1, userCol] > 9)
                                {
                                    Console.WriteLine(enemy.closeShot);
                                    grid[userRow, userCol] = 3;
                                }
                                else if (grid[userRow, userCol] == 0 || grid[userRow, userCol] == 3)
                                {
                                    Console.WriteLine(enemy.missedShot);
                                    grid[userRow, userCol] = 3;
                                }
                            }
                            //Bottom side
                            else if (userRow == 7)
                            {
                                if (grid[userRow, userCol + 1] > 9 || grid[userRow, userCol - 1] > 9 || grid[userRow - 1, userCol] > 9)
                                {
                                    Console.WriteLine(enemy.closeShot);
                                    grid[userRow, userCol] = 3;
                                }
                                else if (grid[userRow, userCol] == 0 || grid[userRow, userCol] == 3)
                                {
                                    Console.WriteLine(enemy.missedShot);
                                    grid[userRow, userCol] = 3;
                                }
                            }
                            //Left side
                            else if (userCol == 0)
                            {
                                if (grid[userRow + 1, userCol] > 9 || grid[userRow - 1, userCol] > 9 || grid[userRow, userCol + 1] > 9)
                                {
                                    Console.WriteLine(enemy.closeShot);
                                    grid[userRow, userCol] = 3;
                                }
                                else if (grid[userRow, userCol] == 0 || grid[userRow, userCol] == 3)
                                {
                                    Console.WriteLine(enemy.missedShot);
                                    grid[userRow, userCol] = 3;
                                }
                            }
                            //Right side
                            else if (userCol == 7)
                            {
                                if (grid[userRow + 1, userCol] > 9 || grid[userRow - 1, userCol] > 9 || grid[userRow, userCol - 1] > 9)
                                {
                                    Console.WriteLine(enemy.closeShot);
                                    grid[userRow, userCol] = 3;
                                }
                                else if (grid[userRow, userCol] == 0 || grid[userRow, userCol] == 3)
                                {
                                    Console.WriteLine(enemy.missedShot);
                                    grid[userRow, userCol] = 3;
                                }
                            }

                            //General close edge case
                            else if (grid[userRow - 1, userCol] > 9 || grid[userRow + 1, userCol] > 9 || grid[userRow, userCol - 1] > 9 || grid[userRow, userCol + 1] > 9)
                            {
                                Console.WriteLine(enemy.closeShot);
                                grid[userRow, userCol] = 3;
                            }
                            //General miss case
                            else if (grid[userRow, userCol] == 0 || grid[userRow, userCol] == 3)
                            {
                                Console.WriteLine(enemy.missedShot);
                                grid[userRow, userCol] = 3;
                            }

                            //Check to see if ships are alive
                            playingGame = false;
                            for (int row = 0; row < gridRows; row++)
                            {
                                for (int col = 0; col < gridCols; col++)
                                {
                                    if (grid[row, col] < shipIdentifier + 1 && grid[row, col] > 9)
                                    {
                                        playingGame = true;
                                    }
                                }
                            }
                        }

                        if (!playingGame)
                        {
                            //--------------------------Draw Victory Screen----------------------------------
                            Console.WriteLine(enemy.enemyLoses);
                            Console.WriteLine();
                            Console.WriteLine($"Finshed in {turnCounter} turns.");
                            DrawShipsRemaining(yourShipsRemaining, enemyShipsRemaining, hpDisiplayNumber);
                            Console.WriteLine();
                            Console.WriteLine("    [ CONTINUE ]    ");
                            Console.ReadLine();
                            score = (50 + (15 * numberOfShips * difficultyMultiplier)) - turnCounter;


                            bool validName = false;
                            while (!validName)
                            {
                                //Enter Score Screen
                                Console.Clear();
                                Console.WriteLine();
                                Console.WriteLine("  YOU CLEARD THE GAME!   ");
                                Console.WriteLine();
                                
                                Console.WriteLine($"YOUR SCORE: {score}");
                                Console.WriteLine();
                                Console.WriteLine("Enter your name: ");
                                name = Console.ReadLine();
                                if (name.Length > 9)
                                {
                                    Console.WriteLine("Name is too long. Please choose a name less than 10 characters");
                                }
                                else 
                                    validName = true;
                            }
                            
                            connection.Open();
                            SqlCommand command = new SqlCommand($"INSERT INTO Scores (name, score) VALUES ('{name}', '{score}');", connection);
                            command.ExecuteNonQuery();
                            connection.Close();

                            Console.WriteLine("\n          [ MAIN MENU]          ");
                            mainMenuing = true;
                            Console.ReadLine();
                        }
                        if (gameOver)
                        {
                            //---------------Draw separate lose screen? --------------------
                            playingGame = false;
                            Console.WriteLine("\n    " + enemy.enemyWins);
                            Console.WriteLine($"That match lasted {turnCounter} turns.");
                            Console.WriteLine();
                        }
                    }
                }
                //GAME TYPE
                else if (keyPressed.Key == ConsoleKey.Enter && cursorRow == 1)
                {
                    Console.WriteLine("GAME TYPE MODE OPTIONS HERE");
                    mainMenuing = false;
                }
                //SETTINGS
                else if (keyPressed.Key == ConsoleKey.Enter && cursorRow == 2)
                {
                    mainMenuing = false;
                    cursorRow = 0;
                    cursorCol = 0;
                    settingsMenuing = true;
                    Console.Clear();

                    //Generate Settings Menu
                    string[,] settingsMenu = new string[settingsMenuMax, 3];

                    for (int row = 0; row < settingsMenuMax; row++)
                    {
                        for (int col = 0; col < 3; col++)
                        {
                            settingsMenu[row, col] = "       ";
                        }
                    }

                    while (settingsMenuing)
                    {
                        settingsMenu[0, 0] = "     ENEMY    ";
                        settingsMenu[1, 0] = "  DIFFICULTY  ";
                        settingsMenu[2, 0] = "  HP DISPLAY  ";
                        settingsMenu[3, 0] = " # OF COLUMNS ";
                        settingsMenu[4, 0] = "  # OF ROWS   ";
                        settingsMenu[5, 0] = "  # OF SHIPS  ";
                        settingsMenu[6, 0] = "    CHEATS    ";
                        settingsMenu[7, 0] = "   MAIN MENU  ";

                        settingsMenu[0, 2] = enemySelection.PadLeft(enemySelection.Length + 3);
                        settingsMenu[1, 2] = difficultyDisplay;
                        settingsMenu[2, 2] = hpDisplay;
                        settingsMenu[3, 2] = $"      {gridCols}     ";
                        settingsMenu[4, 2] = $"      {gridRows}     ";
                        settingsMenu[5, 2] = $"      {numberOfShips}     ";
                        settingsMenu[6, 2] = cheatsDisplay;
                        settingsMenu[7, 2] = "     -      ";


                        Console.Clear();
                        Console.WriteLine("      YOU ARE PLAYING BATTLESHIP      \n");
                        Console.WriteLine("            SETTINGS MENU             \n");
                        Console.WriteLine();

                        for (int row = 0; row < settingsMenuMax; row++)
                        {
                            for (int col = 0; col < 3; col++)
                            {
                                if (cursorRow == row && cursorCol == col)
                                    Console.Write($" [{settingsMenu[row, col]}] ");
                                else
                                    Console.Write($"  {settingsMenu[row, col]}  ");
                            }
                            Console.WriteLine();
                        }

                        keyPressed = Console.ReadKey();

                        if ((keyPressed.Key == ConsoleKey.W || keyPressed.Key == ConsoleKey.UpArrow))
                        {
                            if (cursorRow > 0)
                                cursorRow -= 1;
                            else if (cursorRow == 0)
                                cursorRow = settingsMenuMax - 1;
                        }
                        else if ((keyPressed.Key == ConsoleKey.S || keyPressed.Key == ConsoleKey.DownArrow))
                        {
                            if (cursorRow < settingsMenuMax - 1)
                                cursorRow += 1;
                            else if (cursorRow == settingsMenuMax - 1)
                                cursorRow = 0;
                        }


                        //SETTINGS BUTTONS
                        //Enemy Display
                        else if (keyPressed.Key == ConsoleKey.Enter && cursorRow == 0)
                        {
                            switch (enemySelection)
                            {
                                case "NARRATOR":
                                    enemySelection = "PIRATE";
                                    break;
                                case "PIRATE":
                                    enemySelection = "NARRATOR";
                                    break;
                                default:
                                    break;
                            }
                        }
                        //Difficulty Display
                        else if (keyPressed.Key == ConsoleKey.Enter && cursorRow == 1)
                        {
                            if (difficultyMultiplier == 1)
                            {
                                difficultyMultiplier = 2;
                                difficultyDisplay = "    HARD    ";
                            }
                            else
                            {
                                difficultyMultiplier = 1;
                                difficultyDisplay = "   NORMAL   ";
                            }
                        }
                        //HP Display type
                        else if (keyPressed.Key == ConsoleKey.Enter && cursorRow == 2)
                        {
                            if (hpDisiplayNumber)
                            {
                                hpDisiplayNumber = false;
                                hpDisplay = "   VISUAL   ";
                            }
                            else
                            {
                                hpDisiplayNumber = true;
                                hpDisplay = " NUMERICAL  ";
                            }
                        }
                        //number of gird columns
                        else if (keyPressed.Key == ConsoleKey.Enter && cursorRow == 3)
                        {
                            Console.Clear();
                            Console.WriteLine("\n\n        How columns will the field span?");
                            gridCols = Convert.ToInt32(Console.ReadLine());
                        }
                        //number of gird rows
                        else if (keyPressed.Key == ConsoleKey.Enter && cursorRow == 4)
                        {
                            Console.Clear();
                            Console.WriteLine("\n\n        How many rows will the field span?");
                            gridRows = Convert.ToInt32(Console.ReadLine());
                        }
                        //number of ships deployed
                        else if (keyPressed.Key == ConsoleKey.Enter && cursorRow == 5)
                        {
                            Console.Clear();
                            Console.WriteLine("\n\n        How many ships will each side deploy?");
                            numberOfShips = Convert.ToInt32(Console.ReadLine());
                        }
                        //Cheats on/off
                        else if (keyPressed.Key == ConsoleKey.Enter && cursorRow == 6)
                        {
                            if (cheats)
                            {
                                cheats = false;
                                cheatsDisplay = "     OFF    ";
                            }
                            else
                            {
                                cheats = true;
                                cheatsDisplay = "     ON     ";
                            }
                        }
                        //Main Menu
                        else if (keyPressed.Key == ConsoleKey.Enter && cursorRow == settingsMenuMax - 1)
                        {
                            mainMenuing = true;
                            settingsMenuing = false;
                            cursorRow = 0;
                            cursorCol = 1;
                        }
                    }
                }
                //HIGH SCORES
                else if (keyPressed.Key == ConsoleKey.Enter && cursorRow == mainMenuMax - 1)
                {
                    Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine("             HIGH SCORES          \n");

                    //SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\shino\source\repos\Session 16\GridTesting\GridTesting\Database1.mdf"";Integrated Security=True");

                    SqlCommand command = new SqlCommand("SELECT top 10 * FROM Scores ORDER BY score DESC;", connection);
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    int[,] highScores = new int[10, 3];
                    int rank = 1;

                    Console.Write("     Rank       NAME        SCORE\n");

                    while (reader.Read())
                    {
                        Console.Write($"      {rank.ToString().PadRight(9)}");
                        Console.Write($"{reader["name"].ToString().PadRight(14)}");
                        Console.WriteLine(reader["score"]);
                        rank++;

                        //for (int row = 0; row < 10; row++)
                        //    {
                        //        for (int col = 0; col < 3; col++)
                        //        {
                        //            if (col == 0)
                        //                Console.Write(row + 1 + "    ");
                        //            else if (col == 1)
                        //                Console.Write($"{reader["name"]}    ");
                        //            else if (col == 2)
                        //                Console.Write(reader["score"]);
                        //        }
                        //    }


                        // Console.WriteLine();
                    }

                    Console.WriteLine("\n            [ MAIN MENU ]       ");
                    Console.ReadLine();
                    connection.Close();
                    //button to go back
                }
            }













            
            
            
            
        }

        private static void CheckRight(StringBuilder directionBuilder, int[,] grid, int shipRow, int shipCol)
        {
            if (grid[shipRow, shipCol + 1] == 0 && grid[shipRow, shipCol + 2] == 0)
            {
                directionBuilder.Append("right ");
            }
        }

        private static void CheckLeft(StringBuilder directionBuilder, int[,] grid, int shipRow, int shipCol)
        {
            if (grid[shipRow, shipCol - 1] == 0 && grid[shipRow, shipCol - 2] == 0)
            {
                directionBuilder.Append("left ");
            }
        }
        private static void CheckUp(StringBuilder directionBuilder, int[,] grid, int shipRow, int shipCol)
        {
            if (grid[shipRow - 1, shipCol] == 0 && grid[shipRow - 2, shipCol] == 0)
            {
                directionBuilder.Append("up ");
            }
        }
        private static void CheckDown(StringBuilder directionBuilder, int[,] grid, int shipRow, int shipCol)
        {
            if (grid[shipRow + 1, shipCol] == 0 && grid[shipRow + 2, shipCol] == 0)
            {
                directionBuilder.Append("down ");
            }
        }
        private static void CheckUpDown(StringBuilder directionBuilder, int[,] grid, int shipRow, int shipCol)
        {
            if (grid[shipRow - 1, shipCol] == 0 && grid[shipRow + 1, shipCol] == 0)
            {
                directionBuilder.Append("updown ");
            }
        }
        private static void CheckLeftRight(StringBuilder directionBuilder, int[,] grid, int shipRow, int shipCol)
        {
            if (grid[shipRow, shipCol - 1] == 0 && grid[shipRow, shipCol + 1] == 0)
            {
                directionBuilder.Append("leftright ");
            }
        }
        static void ShipPlacement(int[,] grid, int shipRow, int shipCol, int shipIdentifier, StringBuilder directionBuilder, bool largeShipGenerator)
        {
            string directionString = directionBuilder.ToString();
            string[] directionOptions = directionString.Trim().Split(" ");

            Random rando = new Random();
            int randomDirection = rando.Next(0, directionOptions.Length);
            if (directionOptions[randomDirection] == "left")
            {
                grid[shipRow, shipCol] = shipIdentifier;
                grid[shipRow, shipCol - 1] = shipIdentifier;
                grid[shipRow, shipCol - 2] = shipIdentifier;
            }
            else if (directionOptions[randomDirection] == "right")
            {
                grid[shipRow, shipCol] = shipIdentifier;
                grid[shipRow, shipCol + 1] = shipIdentifier;
                grid[shipRow, shipCol + 2] = shipIdentifier;
            }
            else if (directionOptions[randomDirection] == "up")
            {
                grid[shipRow, shipCol] = shipIdentifier;
                grid[shipRow - 1, shipCol] = shipIdentifier;
                grid[shipRow - 2, shipCol] = shipIdentifier;
            }
            else if (directionOptions[randomDirection] == "down")
            {
                grid[shipRow, shipCol] = shipIdentifier;
                grid[shipRow + 1, shipCol] = shipIdentifier;
                grid[shipRow + 2, shipCol] = shipIdentifier;
            }
            else if (directionOptions[randomDirection] == "leftright")
            {
                grid[shipRow, shipCol] = shipIdentifier;
                grid[shipRow, shipCol - 1] = shipIdentifier;
                grid[shipRow, shipCol + 1] = shipIdentifier;
            }
            else if (directionOptions[randomDirection] == "updown")
            {
                grid[shipRow, shipCol] = shipIdentifier;
                grid[shipRow - 1, shipCol] = shipIdentifier;
                grid[shipRow + 1, shipCol] = shipIdentifier;
            }
        }
        static void LargeShipGeneration(int gridRows, int gridCols, int[,] grid, bool largeShipGenerator, int shipIdentifier, StringBuilder directionBuilder)
        {
            Random rando = new Random();
            int shipRow = rando.Next(0, gridRows);
            int shipCol = rando.Next(0, gridCols);

            //Console.WriteLine($"ship X = {shipCol + 1}, ship Y = {shipRow + 1}");

            //If the space is empty
            if (grid[shipRow, shipCol] == 0)
            {
                directionBuilder.Clear();
                //corner edge cases
                if (shipRow == 0 && shipCol == 0)
                {
                    //top left
                    CheckRight(directionBuilder, grid, shipRow, shipCol);
                    CheckDown(directionBuilder, grid, shipRow, shipCol);
                    if (directionBuilder.Length > 0)
                    {
                        ShipPlacement(grid, shipRow, shipCol, shipIdentifier, directionBuilder, largeShipGenerator);
                    }
                    else
                    {
                        LargeShipGeneration(gridRows, gridCols, grid, largeShipGenerator, shipIdentifier, directionBuilder);
                    }
                    
                }
                else if (shipRow == 0 && shipCol == gridCols - 1)
                {
                    //top right
                    CheckLeft(directionBuilder, grid, shipRow, shipCol);
                    CheckDown(directionBuilder, grid, shipRow, shipCol);
                    if (directionBuilder.Length > 0)
                    {
                        ShipPlacement(grid, shipRow, shipCol, shipIdentifier, directionBuilder, largeShipGenerator);
                    }
                    else
                    {
                        LargeShipGeneration(gridRows, gridCols, grid, largeShipGenerator, shipIdentifier, directionBuilder);
                    }
                }
                else if (shipRow == gridRows - 1 && shipCol == 0)
                {
                    //bottom left
                    CheckUp(directionBuilder, grid, shipRow, shipCol);
                    CheckRight(directionBuilder, grid, shipRow, shipCol);
                    if (directionBuilder.Length > 0)
                    {
                        ShipPlacement(grid, shipRow, shipCol, shipIdentifier, directionBuilder, largeShipGenerator);
                    }
                    else
                    {
                        LargeShipGeneration(gridRows, gridCols, grid, largeShipGenerator, shipIdentifier, directionBuilder);
                    }
                }
                else if (shipRow == gridRows - 1 && shipCol == gridCols - 1)
                {
                    //bottom right
                    CheckUp(directionBuilder, grid, shipRow, shipCol);
                    CheckLeft(directionBuilder, grid, shipRow, shipCol);
                    if (directionBuilder.Length > 0)
                    {
                        ShipPlacement(grid, shipRow, shipCol, shipIdentifier, directionBuilder, largeShipGenerator);
                    }
                    else
                    {
                        LargeShipGeneration(gridRows, gridCols, grid, largeShipGenerator, shipIdentifier, directionBuilder);
                    }
                }
                //Top/bottom corner edge cases with one off
                else if (shipRow == 0 && shipCol == 1)
                {
                    //top left
                    CheckLeftRight(directionBuilder, grid, shipRow, shipCol);
                    CheckDown(directionBuilder, grid, shipRow, shipCol);
                    CheckRight(directionBuilder, grid, shipRow, shipCol);
                    if (directionBuilder.Length > 0)
                    {
                        ShipPlacement(grid, shipRow, shipCol, shipIdentifier, directionBuilder, largeShipGenerator);
                    }
                    else
                    {
                        LargeShipGeneration(gridRows, gridCols, grid, largeShipGenerator, shipIdentifier, directionBuilder);
                    }
                }
                else if (shipRow == 0 && shipCol == gridCols - 2)
                {
                    //top right
                    CheckLeftRight(directionBuilder, grid, shipRow, shipCol);
                    CheckDown(directionBuilder, grid, shipRow, shipCol);
                    CheckLeft(directionBuilder, grid, shipRow, shipCol);
                    if (directionBuilder.Length > 0)
                    {
                        ShipPlacement(grid, shipRow, shipCol, shipIdentifier, directionBuilder, largeShipGenerator);
                    }
                    else
                    {
                        LargeShipGeneration(gridRows, gridCols, grid, largeShipGenerator, shipIdentifier, directionBuilder);
                    }
                }
                else if (shipRow == gridRows - 1 && shipCol == 1)
                {
                    //bottom left
                    CheckLeftRight(directionBuilder, grid, shipRow, shipCol);
                    CheckUp(directionBuilder, grid, shipRow, shipCol);
                    CheckRight(directionBuilder, grid, shipRow, shipCol);
                    if (directionBuilder.Length > 0)
                    {
                        ShipPlacement(grid, shipRow, shipCol, shipIdentifier, directionBuilder, largeShipGenerator);
                    }
                    else
                    {
                        LargeShipGeneration(gridRows, gridCols, grid, largeShipGenerator, shipIdentifier, directionBuilder);
                    }
                }
                else if (shipRow == gridRows - 1 && shipCol == gridCols - 2)
                {
                    //bottom right
                    CheckLeftRight(directionBuilder, grid, shipRow, shipCol);
                    CheckUp(directionBuilder, grid, shipRow, shipCol);
                    CheckLeft(directionBuilder, grid, shipRow, shipCol);
                    if (directionBuilder.Length > 0)
                    {
                        ShipPlacement(grid, shipRow, shipCol, shipIdentifier, directionBuilder, largeShipGenerator);
                    }
                    else
                    {
                        LargeShipGeneration(gridRows, gridCols, grid, largeShipGenerator, shipIdentifier, directionBuilder);
                    }
                }
                //Left/right corner edge cases with one off
                else if (shipRow == 1 && shipCol == 0)
                {
                    //top left
                    CheckUpDown(directionBuilder, grid, shipRow, shipCol);
                    CheckRight(directionBuilder, grid, shipRow, shipCol);
                    CheckDown(directionBuilder, grid, shipRow, shipCol);
                    if (directionBuilder.Length > 0)
                    {
                        ShipPlacement(grid, shipRow, shipCol, shipIdentifier, directionBuilder, largeShipGenerator);
                    }
                    else
                    {
                        LargeShipGeneration(gridRows, gridCols, grid, largeShipGenerator, shipIdentifier, directionBuilder);
                    }
                }
                else if (shipRow == gridRows - 2 && shipCol == 0)
                {
                    //bottom left
                    CheckUpDown(directionBuilder, grid, shipRow, shipCol);
                    CheckUp(directionBuilder, grid, shipRow, shipCol);
                    CheckRight(directionBuilder, grid, shipRow, shipCol);
                    if (directionBuilder.Length > 0)
                    {
                        ShipPlacement(grid, shipRow, shipCol, shipIdentifier, directionBuilder, largeShipGenerator);
                    }
                    else
                    {
                        LargeShipGeneration(gridRows, gridCols, grid, largeShipGenerator, shipIdentifier, directionBuilder);
                    }
                }
                else if (shipRow == 1 && shipCol == gridCols - 1)
                {
                    //top right
                    CheckUpDown(directionBuilder, grid, shipRow, shipCol);
                    CheckLeft(directionBuilder, grid, shipRow, shipCol);
                    CheckDown(directionBuilder, grid, shipRow, shipCol);
                    if (directionBuilder.Length > 0)
                    {
                        ShipPlacement(grid, shipRow, shipCol, shipIdentifier, directionBuilder, largeShipGenerator);
                    }
                    else
                    {
                        LargeShipGeneration(gridRows, gridCols, grid, largeShipGenerator, shipIdentifier, directionBuilder);
                    }
                }
                else if (shipRow == gridRows - 2 && shipCol == gridCols - 1)
                {
                    //bottom right
                    CheckUpDown(directionBuilder, grid, shipRow, shipCol);
                    CheckUp(directionBuilder, grid, shipRow, shipCol);
                    CheckLeft(directionBuilder, grid, shipRow, shipCol);
                    if (directionBuilder.Length > 0)
                    {
                        ShipPlacement(grid, shipRow, shipCol, shipIdentifier, directionBuilder, largeShipGenerator);
                    }
                    else
                    {
                        LargeShipGeneration(gridRows, gridCols, grid, largeShipGenerator, shipIdentifier, directionBuilder);
                    }
                }
                //corner edge cases off by one on each axis
                else if (shipRow == 1 && shipCol == 1)
                {
                    //top left
                    CheckUpDown(directionBuilder, grid, shipRow, shipCol);
                    CheckLeftRight(directionBuilder, grid, shipRow, shipCol);
                    CheckDown(directionBuilder, grid, shipRow, shipCol);
                    CheckRight(directionBuilder, grid, shipRow, shipCol);
                    if (directionBuilder.Length > 0)
                    {
                        ShipPlacement(grid, shipRow, shipCol, shipIdentifier, directionBuilder, largeShipGenerator);
                    }
                    else
                    {
                        LargeShipGeneration(gridRows, gridCols, grid, largeShipGenerator, shipIdentifier, directionBuilder);
                    }
                }
                else if (shipRow == 1 && shipCol == gridCols - 2)
                {
                    //top right
                    CheckUpDown(directionBuilder, grid, shipRow, shipCol);
                    CheckLeftRight(directionBuilder, grid, shipRow, shipCol);
                    CheckDown(directionBuilder, grid, shipRow, shipCol);
                    CheckLeft(directionBuilder, grid, shipRow, shipCol);
                    if (directionBuilder.Length > 0)
                    {
                        ShipPlacement(grid, shipRow, shipCol, shipIdentifier, directionBuilder, largeShipGenerator);
                    }
                    else
                    {
                        LargeShipGeneration(gridRows, gridCols, grid, largeShipGenerator, shipIdentifier, directionBuilder);
                    }
                }
                else if (shipRow == gridRows - 2 && shipCol == 1)
                {
                    //bottom left
                    CheckUpDown(directionBuilder, grid, shipRow, shipCol);
                    CheckLeftRight(directionBuilder, grid, shipRow, shipCol);
                    CheckUp(directionBuilder, grid, shipRow, shipCol);
                    CheckRight(directionBuilder, grid, shipRow, shipCol);
                    if (directionBuilder.Length > 0)
                    {
                        ShipPlacement(grid, shipRow, shipCol, shipIdentifier, directionBuilder, largeShipGenerator);
                    }
                    else
                    {
                        LargeShipGeneration(gridRows, gridCols, grid, largeShipGenerator, shipIdentifier, directionBuilder);
                    }
                }
                else if (shipRow == gridRows - 2 && shipCol == gridCols - 2)
                {
                    //bottom right
                    CheckUpDown(directionBuilder, grid, shipRow, shipCol);
                    CheckLeftRight(directionBuilder, grid, shipRow, shipCol);
                    CheckUp(directionBuilder, grid, shipRow, shipCol);
                    CheckLeft(directionBuilder, grid, shipRow, shipCol);
                    if (directionBuilder.Length > 0)
                    {
                        ShipPlacement(grid, shipRow, shipCol, shipIdentifier, directionBuilder, largeShipGenerator);
                    }
                    else
                    {
                        LargeShipGeneration(gridRows, gridCols, grid, largeShipGenerator, shipIdentifier, directionBuilder);
                    }
                }
                //top, bottom and sides edge cases
                else if (shipRow == 0)
                {
                    //top
                    CheckLeftRight(directionBuilder, grid, shipRow, shipCol);
                    CheckLeft(directionBuilder, grid, shipRow, shipCol);
                    CheckRight(directionBuilder, grid, shipRow, shipCol);
                    CheckDown(directionBuilder, grid, shipRow, shipCol);
                    if (directionBuilder.Length > 0)
                    {
                        ShipPlacement(grid, shipRow, shipCol, shipIdentifier, directionBuilder, largeShipGenerator);
                    }
                    else
                    {
                        LargeShipGeneration(gridRows, gridCols, grid, largeShipGenerator, shipIdentifier, directionBuilder);
                    }
                }
                else if (shipRow == gridRows - 1)
                {
                    //bottom
                    CheckLeftRight(directionBuilder, grid, shipRow, shipCol);
                    CheckLeft(directionBuilder, grid, shipRow, shipCol);
                    CheckRight(directionBuilder, grid, shipRow, shipCol);
                    CheckUp(directionBuilder, grid, shipRow, shipCol);
                    if (directionBuilder.Length > 0)
                    {
                        ShipPlacement(grid, shipRow, shipCol, shipIdentifier, directionBuilder, largeShipGenerator);
                    }
                    else
                    {
                        LargeShipGeneration(gridRows, gridCols, grid, largeShipGenerator, shipIdentifier, directionBuilder);
                    }
                }
                else if (shipCol == 0)
                {
                    //left
                    CheckUpDown(directionBuilder, grid, shipRow, shipCol);
                    CheckUp(directionBuilder, grid, shipRow, shipCol);
                    CheckRight(directionBuilder, grid, shipRow, shipCol);
                    CheckDown(directionBuilder, grid, shipRow, shipCol);
                    if (directionBuilder.Length > 0)
                    {
                        ShipPlacement(grid, shipRow, shipCol, shipIdentifier, directionBuilder, largeShipGenerator);
                    }
                    else
                    {
                        LargeShipGeneration(gridRows, gridCols, grid, largeShipGenerator, shipIdentifier, directionBuilder);
                    }
                }
                else if (shipCol == gridCols - 1)
                {
                    //right
                    CheckUpDown(directionBuilder, grid, shipRow, shipCol);
                    CheckLeft(directionBuilder, grid, shipRow, shipCol);
                    CheckUp(directionBuilder, grid, shipRow, shipCol);
                    CheckDown(directionBuilder, grid, shipRow, shipCol);
                    if (directionBuilder.Length > 0)
                    {
                        ShipPlacement(grid, shipRow, shipCol, shipIdentifier, directionBuilder, largeShipGenerator);
                    }
                    else
                    {
                        LargeShipGeneration(gridRows, gridCols, grid, largeShipGenerator, shipIdentifier, directionBuilder);
                    }
                }
                //top, bottom and side edge cases with one off
                else if (shipRow == 1)
                {
                    //top
                    CheckLeftRight(directionBuilder, grid, shipRow, shipCol);
                    CheckLeft(directionBuilder, grid, shipRow, shipCol);
                    CheckRight(directionBuilder, grid, shipRow, shipCol);
                    CheckUpDown(directionBuilder, grid, shipRow, shipCol);
                    CheckDown(directionBuilder, grid, shipRow, shipCol);
                    if (directionBuilder.Length > 0)
                    {
                        ShipPlacement(grid, shipRow, shipCol, shipIdentifier, directionBuilder, largeShipGenerator);
                    }
                    else
                    {
                        LargeShipGeneration(gridRows, gridCols, grid, largeShipGenerator, shipIdentifier, directionBuilder);
                    }
                }
                else if (shipRow == gridRows - 2)
                {
                    //bottom
                    CheckLeftRight(directionBuilder, grid, shipRow, shipCol);
                    CheckLeft(directionBuilder, grid, shipRow, shipCol);
                    CheckRight(directionBuilder, grid, shipRow, shipCol);
                    CheckUpDown(directionBuilder, grid, shipRow, shipCol);
                    CheckUp(directionBuilder, grid, shipRow, shipCol);
                    if (directionBuilder.Length > 0)
                    {
                        ShipPlacement(grid, shipRow, shipCol, shipIdentifier, directionBuilder, largeShipGenerator);
                    }
                    else
                    {
                        LargeShipGeneration(gridRows, gridCols, grid, largeShipGenerator, shipIdentifier, directionBuilder);
                    }
                }
                else if (shipCol == 1)
                {
                    //left
                    CheckLeftRight(directionBuilder, grid, shipRow, shipCol);
                    CheckUp(directionBuilder, grid, shipRow, shipCol);
                    CheckRight(directionBuilder, grid, shipRow, shipCol);
                    CheckUpDown(directionBuilder, grid, shipRow, shipCol);
                    CheckDown(directionBuilder, grid, shipRow, shipCol);
                    if (directionBuilder.Length > 0)
                    {
                        ShipPlacement(grid, shipRow, shipCol, shipIdentifier, directionBuilder, largeShipGenerator);
                    }
                    else
                    {
                        LargeShipGeneration(gridRows, gridCols, grid, largeShipGenerator, shipIdentifier, directionBuilder);
                    }
                }
                else if (shipCol == gridCols - 2)
                {
                    //right
                    CheckLeftRight(directionBuilder, grid, shipRow, shipCol);
                    CheckLeft(directionBuilder, grid, shipRow, shipCol);
                    CheckUp(directionBuilder, grid, shipRow, shipCol);
                    CheckUpDown(directionBuilder, grid, shipRow, shipCol);
                    CheckDown(directionBuilder, grid, shipRow, shipCol);
                    if (directionBuilder.Length > 0)
                    {
                        ShipPlacement(grid, shipRow, shipCol, shipIdentifier, directionBuilder, largeShipGenerator);
                    }
                    else
                    {
                        LargeShipGeneration(gridRows, gridCols, grid, largeShipGenerator, shipIdentifier, directionBuilder);
                    }
                }
                //general case
                else
                {
                    //all directionBuilders
                    CheckLeftRight(directionBuilder, grid, shipRow, shipCol);
                    CheckLeft(directionBuilder, grid, shipRow, shipCol);
                    CheckRight(directionBuilder, grid, shipRow, shipCol);
                    CheckUp(directionBuilder, grid, shipRow, shipCol);
                    CheckUpDown(directionBuilder, grid, shipRow, shipCol);
                    CheckDown(directionBuilder, grid, shipRow, shipCol);
                    if (directionBuilder.Length > 0)
                    {
                        ShipPlacement(grid, shipRow, shipCol, shipIdentifier, directionBuilder, largeShipGenerator);
                    }
                    else
                    {
                        LargeShipGeneration(gridRows, gridCols, grid, largeShipGenerator, shipIdentifier, directionBuilder);
                    }
                }
            }
            else
            {
                LargeShipGeneration(gridRows, gridCols, grid, largeShipGenerator, shipIdentifier, directionBuilder);
            }
        }
        static void DrawTheBoard(int gridRows, int gridCols, int[,] grid, int shipIdentifier, bool cheats)
        {
            //Boarder for the grid
            Console.WriteLine();
            Console.Write("      | ");
            for (int i = 0; i < gridCols - 1; i++)
            {
                if (i > 8)
                {
                    Console.Write($"{i + 1} ");
                }
                else
                {
                    Console.Write($"{i + 1}  ");
                }
            }
            Console.Write($"{gridCols}");

            Console.WriteLine();
            Console.Write("    --|-");
            for (int i = 0; i < gridCols - 1; i++)
            {
                Console.Write("---");
            }
            if (gridCols > 9)
            {
                Console.Write("--");
            }
            else
            {
                Console.Write("-");
            }
            Console.WriteLine();

            //The grid
            for (int row = 0; row < gridRows; row++)
            {
                if (row > 8)
                {
                    Console.Write($"    {row + 1}| ");
                }
                else
                {
                    Console.Write($"     {row + 1}| ");
                }
                for (int col = 0; col < gridCols; col++)
                {
                    if (grid[row, col] == 0)
                    {
                        Console.Write("~  ");
                    }
                    else if (grid[row, col] > 9)
                    {
                        if (cheats)
                            Console.Write("S  ");
                        else
                            Console.Write("~  ");
                    }
                    else if (grid[row, col] == 2 || grid[row, col] == 4)
                    {
                        Console.Write("X  ");
                    }
                    else if (grid[row, col] == 3)
                    {
                        Console.Write("O  ");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        static void DrawShipsRemaining(int yourShipsRemaining, int enemyShipsRemaining, bool hpDisplayNumber)
        {            
            Console.Write(" Your ships remaining: ");
            if (hpDisplayNumber)
                Console.Write(yourShipsRemaining);
            else
            {
                for (int i = 0; i < yourShipsRemaining; i++)
                {
                    Console.Write("() ");
                }
            }
            Console.Write("\n Enemy ships remaining: ");
            if (hpDisplayNumber)
                Console.Write(enemyShipsRemaining);
            else
            {
                for (int i = 0; i < enemyShipsRemaining; i++)
                {
                    Console.Write("() ");
                }
            }
        }
        class Enemy
        {
            public string enemyName;
            public string enemyTurn0Action;
            public string enemySearching;
            public string enemyHunting;
            public string shipHitByEnemy;
            public string shipSunkByEnemy;
            public string enemyShotMissed;
            public string shotOffBoard;
            public string hitEnemyShip;
            public string sunkEnemyShip;
            public string spaceAlreadyHit;
            public string missedShot;
            public string closeShot;
            public string enemyWins;
            public string enemyLoses;
            public Enemy()
            {
                enemyName = "  NARRATOR  ";
                enemyTurn0Action = "The enemy is prepairing to fire at your ships!";
                enemySearching = "The enemy is searching for your ships";
                enemyHunting = "The enemy is hunting down your ship!";
                shipHitByEnemy = "One of your ships has been hit by the enemy!";
                shipSunkByEnemy = "One of your ships has been sunk by the enemy!";
                enemyShotMissed = "The enemy's shot missed our ships!";
                shotOffBoard = "That shot was off the board!\nTry again!";
                hitEnemyShip = "Hit detected - we made contact with an enemy ship!";
                sunkEnemyShip = "You sunk a battleship!";
                spaceAlreadyHit = "That space has already been hit!";
                missedShot = "Missed - nothing but water!";
                closeShot = "Close - an enemy ship must be nearby!";
                enemyWins = "The enemy has bested you - all of your ships have sunk!";
                enemyLoses = "Congratulations! You sunk all of the ships!";
            }
        }
    }
}