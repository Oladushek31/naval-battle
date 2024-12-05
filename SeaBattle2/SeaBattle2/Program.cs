using System;
using System.Collections.Generic;
using System.Threading;

class Program
{
    static int boardSize = 10;
    static char[,] playerBoard = new char[boardSize, boardSize];
    static char[,] playerShots = new char[boardSize, boardSize];
    static char[,] shipDisplay = new char[boardSize, boardSize];
    static int remainingShips = 20;

    static Dictionary<int, int> shipCounts = new Dictionary<int, int>
    {
        {4, 1},
        {3, 2},
        {2, 3},
        {1, 4}
    };

    static void Main(string[] args)
    {
        CreateField();
        LocationShips();
        FieldWithShips();

        while (true)
        {
            Console.WriteLine("Ваше поле:");
            DisplayBoard();

            Console.WriteLine("Поле с кораблями:");
            DisplayShips();

            Console.Write("Введите координаты выстрела (например, A3): ");
            string input = Console.ReadLine().ToUpper();

            if (input.Length > 3 || input[0] < 'A' || input[0] > 'J' || !int.TryParse(input.Substring(1), out int rowNum) || rowNum < 1 || rowNum > boardSize)
            {
                Console.WriteLine("Неверный формат ввода. Попробуйте снова.");
                Console.Write("Повторите попытку через ");
                for (int time = 3; time > 0; time--)
                {
                    Console.Write($"{time}...");
                    Thread.Sleep(1000);
                }
                continue;
            }

            int row = rowNum - 1;
            int col = input[0] - 'A';

            if (playerShots[row, col] == 'X' || playerShots[row, col] == 'O')
            {
                Console.WriteLine("Вы уже стреляли в эту ячейку. Попробуйте снова.");
                Console.Write("Повторите попытку через ");
                for (int time = 3; time > 0; time--)
                {
                    Console.Write($"{time}...");
                    Thread.Sleep(1000);
                } 
                continue;
            }
            else
            {
                if (playerBoard[row, col] == '*')
                {
                    playerShots[row, col] = 'X';
                    remainingShips--;

                    if (remainingShips == 0)
                    {
                        DisplayBoard();
                        Console.WriteLine("Вы победили!");
                        break;
                    }
                }
                else
                {
                    playerShots[row, col] = 'O';
                }
                
            }
            
        }
    


        static void CreateField()
        {
            for (int row = 0; row < boardSize; row++)
            {
                for (int col = 0; col < boardSize; col++)
                {
                    playerBoard[row, col] = ' ';
                    playerShots[row, col] = ' ';
                }
            }
        }

        static void LocationShips()
        {
            Random random = new Random();
            foreach (var kvp in shipCounts)
            {
                int shipSize = kvp.Key;
                int shipCount = kvp.Value;

                for (int i = 0; i < shipCount; i++)
                {
                    bool isFree = false;
                    while (!isFree)
                    {
                        int row = random.Next(boardSize);
                        int col = random.Next(boardSize);
                        bool isHorizontal = (random.Next(2) == 0);

                        if (CanPlaceShip(row, col, shipSize, isHorizontal))
                        {
                            PlaceShip(row, col, shipSize, isHorizontal);
                            isFree = true;
                        }
                    }
                }
            }
        }

        static bool CanPlaceShip(int startRow, int startCol, int shipSize, bool isHorizontal)
        {
            if (isHorizontal)
            {
                if (startCol + shipSize > boardSize)
                {
                    return false;
                }

                for (int col = startCol; col < startCol + shipSize; col++)
                {
                    if (playerBoard[startRow, col] != ' ')
                    {
                        return false;
                    }
                }
            }
            else
            {
                if (startRow + shipSize > boardSize)
                {
                    return false;
                }

                for (int row = startRow; row < startRow + shipSize; row++)
                {
                    if (playerBoard[row, startCol] != ' ')
                    {
                        return false;
                    }
                }
            }

            for (int row = Math.Max(0, startRow - 1); row <= Math.Min(boardSize - 1, isHorizontal ? startRow + 1 : startRow + shipSize); row++)
            {
                for (int col = Math.Max(0, startCol - 1); col <= Math.Min(boardSize - 1, isHorizontal ? startCol + shipSize : startCol + 1); col++)
                {
                    if (playerBoard[row, col] != ' ')
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        static void PlaceShip(int startRow, int startCol, int shipSize, bool isHorizontal)
        {
            if (isHorizontal)
            {
                for (int col = startCol; col < startCol + shipSize; col++)
                {
                    playerBoard[startRow, col] = '*';
                }
            }
            else
            {
                for (int row = startRow; row < startRow + shipSize; row++)
                {
                    playerBoard[row, startCol] = '*';
                }
            }

            return;
        }

        static void DisplayBoard()
        {
            Console.Clear();
            Console.WriteLine("A  B  C  D  E  F  G  H  I  J".PadLeft(32));
            for (int row = 0; row < boardSize; row++)
            {
                Console.Write($"{row + 1:D2} ");
                for (int col = 0; col < boardSize; col++)
                {
                    char shotResult = playerShots[row, col];
                    if (shotResult == 'X')
                    {
                        Console.Write("[X]");
                    }
                    else if (shotResult == 'O')
                    {
                        Console.Write("[O]");
                    }
                    else
                    {
                        Console.Write("[ ]");
                    }
                }
                Console.WriteLine();
            }
        }

        static void DisplayShips()
        {
            Console.WriteLine("A  B  C  D  E  F  G  H  I  J".PadLeft(32));
            for (int row = 0; row < boardSize; row++)
            {
                Console.Write($"{row + 1:D2} ");
                for (int col = 0; col < boardSize; col++)
                {
                    char cell = shipDisplay[row, col];
                    if (cell == '*')
                    {
                        Console.Write("[*]");
                    }
                    else
                    {
                        Console.Write("[ ]");
                    }
                }
                Console.WriteLine();
            }
        }

        static void FieldWithShips()
        {
            for (int row = 0; row < boardSize; row++)
            {
                for (int col = 0; col < boardSize; col++)
                {
                    shipDisplay[row, col] = playerBoard[row, col]; ;
                }
            }
        }
    }
}
