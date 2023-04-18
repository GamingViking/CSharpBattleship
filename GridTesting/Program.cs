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
            int gridRows = 18;
            int gridCols = 18;
            int userRow = 0;
            int userCol = 0;
            bool shotOnBoard = true;

            //Grid Generation
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
                    Console.WriteLine($" Ship coordinates are X{shipRow} and Y{shipCol}");
                }
            }


            bool loop = true;

            while (loop)
            {
                while (shotOnBoard)
                {
                    //Draw the Board
                    //Board Lining
                    Console.WriteLine();
                    Console.Write("      | ");
                    for (int i = 0; i < gridCols - 1; i++)
                    {
                        if (i > 9)
                        {
                            Console.Write($"{i} ");
                        }
                        else
                        {
                            Console.Write($"{i}  ");
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
                    //Board Grid
                    for (int row = 0; row < gridRows; row++)
                    {
                        if (row > 9)
                        {
                            Console.Write($"    {row}| ");
                        }
                        else
                        {
                            Console.Write($"     {row}| ");
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


                    Console.WriteLine("(0,0), (3,5), (0,4)");


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

                    Console.WriteLine();
                    Console.WriteLine("What square would you like to shoot at?");
                    Console.WriteLine("Enter the row number (0-7)");
                    userRow = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Enter the column number (0-7)");
                    userCol = Convert.ToInt32(Console.ReadLine());
                    Console.Clear();

                    if (userRow > 7 || userRow < 0 || userCol > 7 || userCol < 0)
                    {
                        Console.WriteLine("That shot was off the board!\nTry again!");
                    }
                    else
                    {
                        shotOnBoard = false;
                    }
                }
                
                shotOnBoard = true;
                Console.WriteLine($"Last shot at: ({userRow}, {userCol})");


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
               
                //Check to see if ships are alive
                loop = false;
                for (int row = 0; row < gridRows; row++)
                {
                    for (int col = 0; col < gridCols; col++)
                    {
                        if (grid[row, col] == 1)
                        {
                            loop = true;
                        }
                    }
                }

                if (!loop)
                {
    //--------------------------Draw Victory Screen----------------------------------
                    Console.WriteLine("Congratulations! You sunk all of the ships!");
                }
            }
        }
    }
}