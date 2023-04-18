namespace GridTesting
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to BattleshipTM! Get ready to sink some ships!\n");
            //0 = water, 1 = ship, 2 = hit, 3 = miss, 4 = previous hit
            Console.WriteLine("How many ships will each side deploy in this battle?");
            int numberOfShips = Convert.ToInt32(Console.ReadLine());
            int yourShipsRemaining = numberOfShips;
            int enemyShipsRemaining = numberOfShips;
            int gridRows = 8;
            int gridCols = 8;
            int userRow = 0;
            int userCol = 0;
            bool shotOnBoard = true;

            int[,] grid = new int[gridRows, gridCols];
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    grid[row, col] = 0;
                }
            }

            //Inital Board Draw
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Console.Write("~ ");
                }
                Console.WriteLine();
            }

            //Battle ship positions
            grid[0, 0] = 1;
            grid[3, 5] = 1;
            grid[0, 4] = 1;

            Random rando = new Random();
            int deployedShips = 0;
            while (deployedShips < numberOfShips)
            {
                int shipX = rando.Next(0, 8);
                int shipY = rando.Next(0, 8);
                if (grid[shipX, shipY] == 0)
                {
                    grid[shipX, shipY] = 1;
                    deployedShips++;
                    Console.WriteLine($" Ship coordinates are X{shipX} and Y{shipY}");
                }
            }


            bool loop = true;

            while (loop)
            {




                while (shotOnBoard == true)
                {


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
                        Console.WriteLine("You couldn't hit water if you fell out of a boat!\nKeep your shots on the board!");
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
                for (int row = 0; row < 8; row++)
                {
                    for (int col = 0; col < 8; col++)
                    {
                        if (grid[row, col] == 1)
                        {
                            loop = true;
                        }
                    }
                }

                //Draw the Board
                Console.WriteLine();
                for (int row = 0; row < 8; row++)
                {
                    for (int col = 0; col < 8; col++)
                    {
                        if (grid[row, col] == 0 || grid[row, col] == 1)
                        {
                            Console.Write("~ ");
                        }
                        else if (grid[row, col] == 2 || grid[row, col] == 4)
                        {
                            Console.Write("X ");
                        }
                        else if (grid[row, col] == 3)
                        {
                            Console.Write("O ");
                        }
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();

                //Win condition result
                if (!loop)
                {
                    Console.WriteLine("Congratulations! You sunk all of the ships!");
                }
            }
        }
    }
}