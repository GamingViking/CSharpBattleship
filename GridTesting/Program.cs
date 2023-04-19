namespace GridTesting
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to BattleshipTM! Get ready to sink some ships!\n");
            //0 = water, 1 = ship, 2 = hit, 3 = miss
            Console.WriteLine("How many ships will each side deploy in this battle?");
            int numberOfShips = Convert.ToInt32(Console.ReadLine());
            int yourShipsRemaining = numberOfShips;
            int enemyShipsRemaining = numberOfShips;
            int gridRows = 08;
            int gridCols = 08;
            int userRow = 0;
            int userCol = 0;
            bool shotOnBoard = true;
            bool playingGame = true;
            int shipLimit = 20;

            //Grid generation
            int[,] grid = new int[gridRows, gridCols];
            for (int row = 0; row < gridRows; row++)
            {
                for (int col = 0; col < gridCols; col++)
                {
                    grid[row, col] = 0;
                }
            }

            //Battle ship positions
            grid[0, 0] = 1;
            grid[3, 5] = 1;
            grid[0, 4] = 1;

            Random rando = new Random();
            int deployedShips = 0;
            while (deployedShips < numberOfShips)
            {
                int shipRow = rando.Next(0, gridRows);
                int shipCol = rando.Next(0, gridCols);
                if (grid[shipRow, shipCol] == 0)
                {
                    grid[shipRow, shipCol] = 1;
                    deployedShips++;
                    Console.WriteLine($" Ship coordinates are X{shipCol + 1} and Y{shipRow + 1}");
                }
            }

            while (playingGame)
            {
                while (shotOnBoard)
                {
                    //Draw the Board
                    DrawTheBoard(gridRows, gridCols, grid);

                    //Hard-coded ship positions
                    Console.WriteLine("(1,1), (6,4), (5,1)");

                    //Draw ships remaining
                    DrawShipsRemaining(yourShipsRemaining, enemyShipsRemaining);
                    

                    Console.WriteLine();
                    Console.WriteLine("   What square would you like to shoot at?");
                    Console.WriteLine("Enter the X coordinate (column number)");
                    userCol = Convert.ToInt32(Console.ReadLine()) - 1;
                    Console.WriteLine("Enter the Y coordinate (row number)");
                    userRow = Convert.ToInt32(Console.ReadLine()) - 1;
                    Console.Clear();

                    if (userRow > gridRows || userRow < 0 || userCol > gridCols || userCol < 0)
                    {
                        Console.WriteLine("That shot was off the board!\nTry again!");
                    }
                    else
                    {
                        shotOnBoard = false;
                    }
                }
                
                shotOnBoard = true;
                Console.WriteLine($"Last shot at: ({userCol + 1}, {userRow + 1})");


                if (grid[userRow, userCol] == 1)
                {
                    Console.WriteLine("Shot landed - enemy ship down!");
                    grid[userRow, userCol] = 2;
                    enemyShipsRemaining--;
                }
                else if (grid[userRow, userCol] == 2)
                {
                    Console.WriteLine("That space has already been hit!");
                }
                //Edge-casing intensifies - corners
                //Top left corner
                else if (userRow == 0 && userCol == 0)
                {
                    if (grid[userRow, userCol + 1] == 1 || grid[userRow + 1, userCol] == 1)
                    {
                        Console.WriteLine("Close - an enemy ship must be nearby!");
                        grid[userRow, userCol] = 3;
                    }
                    else if (grid[userRow, userCol] == 0 || grid[userRow, userCol] == 3)
                    {
                        Console.WriteLine("Missed - nothing but water!");
                        grid[userRow, userCol] = 3;
                    }
                }
                //Bottom left corner
                else if (userRow == 7 && userCol == 0)
                {
                    if (grid[userRow, userCol + 1] == 1 || grid[userRow - 1, userCol] == 1)
                    {
                        Console.WriteLine("Close - an enemy ship must be nearby!");
                        grid[userRow, userCol] = 3;
                    }
                    else if (grid[userRow, userCol] == 0 || grid[userRow, userCol] == 3)
                    {
                        Console.WriteLine("Missed - nothing but water!");
                        grid[userRow, userCol] = 3;
                    }
                }
                //Bottom right corner
                else if (userRow == 7 && userCol == 7)
                {
                    if (grid[userRow, userCol - 1] == 1 || grid[userRow - 1, userCol] == 1)
                    {
                        Console.WriteLine("Close - an enemy ship must be nearby!");
                        grid[userRow, userCol] = 3;
                    }
                    else if (grid[userRow, userCol] == 0 || grid[userRow, userCol] == 3)
                    {
                        Console.WriteLine("Missed - nothing but water!");
                        grid[userRow, userCol] = 3;
                    }
                }
                //Top right corner
                else if (userRow == 0 && userCol == 7)
                {
                    if (grid[userRow, userCol - 1] == 1 || grid[userRow + 1, userCol] == 1)
                    {
                        Console.WriteLine("Close - an enemy ship must be nearby!");
                        grid[userRow, userCol] = 3;
                    }
                    else if (grid[userRow, userCol] == 0 || grid[userRow, userCol] == 3)
                    {
                        Console.WriteLine("Missed - nothing but water!");
                        grid[userRow, userCol] = 3;
                    }
                }
                //Edge-casing sides
                //Top side
                else if (userRow == 0)
                {
                    if (grid[userRow, userCol + 1] == 1 || grid[userRow, userCol - 1] == 1 || grid[userRow + 1, userCol] == 1)
                    {
                        Console.WriteLine("Close - an enemy ship must be nearby!");
                        grid[userRow, userCol] = 3;
                    }
                    else if (grid[userRow, userCol] == 0 || grid[userRow, userCol] == 3)
                    {
                        Console.WriteLine("Missed - nothing but water!");
                        grid[userRow, userCol] = 3;
                    }
                }
                //Bottom side
                else if (userRow == 7)
                {
                    if (grid[userRow, userCol + 1] == 1 || grid[userRow, userCol - 1] == 1 || grid[userRow - 1, userCol] == 1)
                    {
                        Console.WriteLine("Close - an enemy ship must be nearby!");
                        grid[userRow, userCol] = 3;
                    }
                    else if (grid[userRow, userCol] == 0 || grid[userRow, userCol] == 3)
                    {
                        Console.WriteLine("Missed - nothing but water!");
                        grid[userRow, userCol] = 3;
                    }
                }
                //Left side
                else if (userCol == 0)
                {
                    if (grid[userRow + 1, userCol] == 1 || grid[userRow - 1, userCol] == 1 || grid[userRow, userCol + 1] == 1)
                    {
                        Console.WriteLine("Close - an enemy ship must be nearby!");
                        grid[userRow, userCol] = 3;
                    }
                    else if (grid[userRow, userCol] == 0 || grid[userRow, userCol] == 3)
                    {
                        Console.WriteLine("Missed - nothing but water!");
                        grid[userRow, userCol] = 3;
                    }
                }
                //Right side
                else if (userCol == 7)
                {
                    if (grid[userRow + 1, userCol] == 1 || grid[userRow - 1, userCol] == 1 || grid[userRow, userCol - 1] == 1)
                    {
                        Console.WriteLine("Close - an enemy ship must be nearby!");
                        grid[userRow, userCol] = 3;
                    }
                    else if (grid[userRow, userCol] == 0 || grid[userRow, userCol] == 3)
                    {
                        Console.WriteLine("Missed - nothing but water!");
                        grid[userRow, userCol] = 3;
                    }
                }

                //General close edge case
                else if (grid[userRow - 1, userCol] == 1 || grid[userRow + 1, userCol] == 1 || grid[userRow, userCol - 1] == 1 || grid[userRow, userCol + 1] == 1)
                {
                    Console.WriteLine("Close - an enemy ship must be nearby!");
                    grid[userRow, userCol] = 3;
                }
                //General miss case
                else if (grid[userRow, userCol] == 0 || grid[userRow, userCol] == 3)
                {
                    Console.WriteLine("Missed - nothing but water!");
                    grid[userRow, userCol] = 3;
                }

                ////Check to see if a specific ship is still alive
                //ShipAliveCheck(int[,] grid, int gridRows, int gridCols, int shipLimit)
                //for (int row = 0; row < gridRows; row++)
                //{
                //    for (int col = 0; col < gridCols; col++)
                //    {
                //        if (grid[row, col] <= shipLimit && grid[row, col] >= 10)
                //        {
                //            playingGame = true;
                //        }
                //    }
                //}


                //Check to see if ships are alive
                playingGame = false;
                for (int row = 0; row < gridRows; row++)
                {
                    for (int col = 0; col < gridCols; col++)
                    {
                        if (grid[row, col] < 2 && grid[row, col] > 0)
                        {
                            playingGame = true;
                        }
                    }
                }

                if (!playingGame)
                {
    //--------------------------Draw Victory Screen----------------------------------
                    Console.WriteLine("Congratulations! You sunk all of the ships!");
                }
            }
        }

        static void DrawTheBoard(int gridRows, int gridCols, int[,] grid)
        {
            //Boarder for the grid
            Console.WriteLine();
            Console.Write("      | ");
            for (int i = 0; i < gridCols - 1; i++)
            {
                if (i > 9)
                {
                    Console.Write($"{i + 1} ");
                }
                else
                {
                    Console.Write($"{i + 1}  ");
                }
            }
            Console.Write($"{gridCols - 1}");

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
                if (row > 9)
                {
                    Console.Write($"    {row + 1}| ");
                }
                else
                {
                    Console.Write($"     {row + 1}| ");
                }
                for (int col = 0; col < gridCols; col++)
                {
                    if (grid[row, col] == 0 || grid[row, col] == 1)
                    {
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

        static void DrawShipsRemaining(int yourShipsRemaining, int enemyShipsRemaining)
        {
            Console.Write(" Your ships remaining: ");
            for (int i = 0; i < yourShipsRemaining; i++)
            {
                Console.Write("() ");
            }
            Console.Write("\nEnemy ships remaining: ");
            for (int i = 0; i < enemyShipsRemaining; i++)
            {
                Console.Write("() ");
            }
        }
    }
}