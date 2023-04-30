﻿using System.ComponentModel;
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
            bool gameOver = false;
            int shipIdentifier = 10;
            bool largeShipGenerator = true;
            StringBuilder directionBuilder = new StringBuilder("");
            int deployedShips = 0;
            int[] shipLifeArray = new int[numberOfShips];
            Array.Fill(shipLifeArray, 3);
            int shipLifeIdentifier = 0;
            int chanceToBeHit = 3 * numberOfShips;
            int takeDamage = 0;
            int yourShipLife = 3 * yourShipsRemaining;
            int turnCounter = 0;
            bool lastShotHit = false;


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
                    //Draw the Board
                    DrawTheBoard(gridRows, gridCols, grid, shipIdentifier);

                    //Enemy attacks
                    Random rando = new Random();
                    if (lastShotHit)
                    {
                        Console.WriteLine("The enemy is hunting down your ship!");
                        takeDamage = rando.Next((gridRows * gridCols) / 2);
                    }
                    else
                    {
                        Console.WriteLine("The enemy is searching for your ships");
                        takeDamage = rando.Next(gridRows * gridCols);
                    }

                    if (takeDamage < chanceToBeHit)
                    {
                        Console.WriteLine("One of your ships has been hit by the enemy!");
                        lastShotHit = true;
                        yourShipLife--;
                        if (yourShipLife % 3 == 0)
                        {
                            yourShipsRemaining--;
                            Console.WriteLine("One of your ships has been sunk by the enemy!");
                            lastShotHit = false;
                            if (yourShipsRemaining == 0)
                            {
                                gameOver = true;
                            }
                        }
                    }
                    else
                    {
                        if (turnCounter == 0)
                        {
                            Console.WriteLine("The enemy is prepairing to fire at your ships!");
                        }
                        else
                        {
                            Console.WriteLine("The enemy's shot missed our ships!");
                        }
                    }
                    if (turnCounter % Math.Floor((gridRows * gridCols) / 10.0) == 0)
                    chanceToBeHit++;
                    Console.WriteLine($" turnCounter = {turnCounter}");
                    Console.WriteLine($"ChanceToBeHit = {chanceToBeHit}");

                    //Draw ships remaining
                    DrawShipsRemaining(yourShipsRemaining, enemyShipsRemaining);

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
                            Console.WriteLine("That shot was off the board!\nTry again!");
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

                shotOnBoard = true;
                turnCounter++;
                Console.WriteLine($"Last shot at: ({userCol + 1}, {userRow + 1})");


                if (grid[userRow, userCol] > 9)
                {
                    Console.WriteLine("Hit detected - we made contact with an enemy ship!");
                    shipLifeIdentifier = grid[userRow, userCol];
                    shipLifeArray[shipLifeIdentifier - 10] -= 1;
                    grid[userRow, userCol] = 2;
                    if (shipLifeArray[shipLifeIdentifier - 10] == 0)
                    {
                        Console.WriteLine("You sunk a battleship!");
                        enemyShipsRemaining--;
                        shipLifeArray[shipLifeIdentifier - 10] = 1;
                    }
                }
                else if (grid[userRow, userCol] == 2)
                {
                    Console.WriteLine("That space has already been hit!");
                }
                //Edge-casing intensifies - corners
                //Top left corner
                else if (userRow == 0 && userCol == 0)
                {
                    if (grid[userRow, userCol + 1] > 9 || grid[userRow + 1, userCol] > 9)
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
                    if (grid[userRow, userCol + 1] > 9 || grid[userRow - 1, userCol] > 9)
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
                    if (grid[userRow, userCol - 1] > 9 || grid[userRow - 1, userCol] > 9)
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
                    if (grid[userRow, userCol - 1] > 9 || grid[userRow + 1, userCol] > 9)
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
                    if (grid[userRow, userCol + 1] > 9 || grid[userRow, userCol - 1] > 9 || grid[userRow + 1, userCol] > 9)
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
                    if (grid[userRow, userCol + 1] > 9 || grid[userRow, userCol - 1] > 9 || grid[userRow - 1, userCol] > 9)
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
                    if (grid[userRow + 1, userCol] > 9 || grid[userRow - 1, userCol] > 9 || grid[userRow, userCol + 1] > 9)
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
                    if (grid[userRow + 1, userCol] > 9 || grid[userRow - 1, userCol] > 9 || grid[userRow, userCol - 1] > 9)
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
                else if (grid[userRow - 1, userCol] > 9 || grid[userRow + 1, userCol] > 9 || grid[userRow, userCol - 1] > 9 || grid[userRow, userCol + 1] > 9)
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

                if (!playingGame)
                {
                    //--------------------------Draw Victory Screen----------------------------------
                    Console.WriteLine("Congratulations! You sunk all of the ships!");
                }
                if (gameOver)
                {
                    //---------------Draw separate lose screen? --------------------
                    playingGame = false;
                        Console.WriteLine("The enemy has bested you - all of your ships have sunk!");
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

            Console.WriteLine($"ship X = {shipCol + 1}, ship Y = {shipRow + 1}");

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
        static void DrawTheBoard(int gridRows, int gridCols, int[,] grid, int shipIdentifier)
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