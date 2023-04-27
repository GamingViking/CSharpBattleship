using System.ComponentModel;
using System.ComponentModel.Design;
using System.Text;

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
            int gridRows = 8;
            int gridCols = 8;
            int userRow = 0;
            int userCol = 0;
            bool shotOnBoard = true;
            bool playingGame = true;
            int shipLimit = 20;
            int shipIdentifier = 1;
            bool largeShipGenerator = true;
            StringBuilder directionBuilder = new StringBuilder("");


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
            //grid[0, 0] = 1;
            //grid[3, 5] = 1;
            //grid[0, 4] = 1;
            //grid[2, 5] = 1; //(6,3)

            //Random rando = new Random();
            int deployedShips = 0;
            //while (deployedShips < numberOfShips)
            //{
            //    int shipRow = rando.Next(0, gridRows);
            //    int shipCol = rando.Next(0, gridCols);
            //    if (grid[shipRow, shipCol] == 0)
            //    {
            //        grid[shipRow, shipCol] = 1;
            //        deployedShips++;
            //        Console.WriteLine($" Ship coordinates are X{shipCol + 1} and Y{shipRow + 1}");
            //    }
            //}

            //Large ship generation
            while (deployedShips < numberOfShips)
            {
                //int shipRow = rando.Next(0, gridRows);
                //int shipCol = rando.Next(0, gridCols);

                //Console.WriteLine($"shipRow = {shipRow}, shipCol = {shipCol}");
                LargeShipGeneration(gridRows, gridCols, grid, largeShipGenerator, shipIdentifier, directionBuilder);
                deployedShips++;
                //shipIdentifier++; (better than incrementing via function?)
            }

            while (playingGame)
            {
                while (shotOnBoard)
                {
                    //Draw the Board
                    DrawTheBoard(gridRows, gridCols, grid);

                    ////Hard-coded ship positions
                    //Console.WriteLine("(1,1), (6,4), (5,1)");

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

            //TESTING STATION?????????????????????????????????????????
            //Console.WriteLine($"directionBuilder = {directionBuilder}");
            //Console.WriteLine("directionOption length = " + directionOptions.Length);
            //Console.WriteLine("directionOptions ->");
            //for (int i = 0; i < directionOptions.Length; i++)
            //{
            //    Console.Write(directionOptions[i] + " ");
            //}


            Random rando = new Random();
            int randomDirection = rando.Next(0, directionOptions.Length);
            if (directionOptions[randomDirection] == "left")
            {
                grid[shipRow, shipCol] = shipIdentifier;
                grid[shipRow, shipCol - 1] = shipIdentifier;
                grid[shipRow, shipCol - 2] = shipIdentifier;
                shipIdentifier++;
                largeShipGenerator = false;
            }
            else if (directionOptions[randomDirection] == "right")
            {
                grid[shipRow, shipCol] = shipIdentifier;
                grid[shipRow, shipCol + 1] = shipIdentifier;
                grid[shipRow, shipCol + 2] = shipIdentifier;
                shipIdentifier++;
                largeShipGenerator = false;
            }
            else if (directionOptions[randomDirection] == "up")
            {
                grid[shipRow, shipCol] = shipIdentifier;
                grid[shipRow - 1, shipCol] = shipIdentifier;
                grid[shipRow - 2, shipCol] = shipIdentifier;
                shipIdentifier++;
                largeShipGenerator = false;
            }
            else if (directionOptions[randomDirection] == "down")
            {
                grid[shipRow, shipCol] = shipIdentifier;
                grid[shipRow + 1, shipCol] = shipIdentifier;
                grid[shipRow + 2, shipCol] = shipIdentifier;
                shipIdentifier++;
                largeShipGenerator = false;
            }
            else if (directionOptions[randomDirection] == "leftright")
            {
                grid[shipRow, shipCol] = shipIdentifier;
                grid[shipRow, shipCol - 1] = shipIdentifier;
                grid[shipRow, shipCol + 1] = shipIdentifier;
                shipIdentifier++;
                largeShipGenerator = false;
            }
            else if (directionOptions[randomDirection] == "updown")
            {
                grid[shipRow, shipCol] = shipIdentifier;
                grid[shipRow - 1, shipCol] = shipIdentifier;
                grid[shipRow + 1, shipCol] = shipIdentifier;
                shipIdentifier++;
                largeShipGenerator = false;
            }
        }
        static void LargeShipGeneration(int gridRows, int gridCols, int[,] grid, bool largeShipGenerator, int shipIdentifier, StringBuilder directionBuilder)
        {
            Random rando = new Random();
            int shipRow = rando.Next(0, gridRows);
            int shipCol = rando.Next(0, gridCols);
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
                    if (grid[row, col] == 0)
                    {
                        Console.Write("~  ");
                    }
                    else if (grid[row, col] == 1)
                    {
                        Console.Write("M  ");
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