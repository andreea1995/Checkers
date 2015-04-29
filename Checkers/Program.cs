using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Checkers
{
    class Program
    {
        static void Main(string[] args)
        {
            string wanttoplay = "yes";
            int playerpoints = 0;
            int computerpoints = 0;
            Console.WriteLine("Welcome to Checkers \n In your turn, first enter the piece's row and column number you wish to play, \n and then enter thr direction number(1/3/7/9)");

            while (wanttoplay.ToLower() != "no")
            {
                int gameresult = Game();
                if (gameresult == 1)
                {
                    Console.WriteLine("The computer won!!");
                    computerpoints++;
                }
                else if (gameresult == -1)
                {
                    Console.WriteLine("You won!!");
                    playerpoints++;
                }
                WhoLeads(playerpoints, computerpoints);
                Console.WriteLine("Do you want to continue?");
                wanttoplay = Console.ReadLine();
            }

        }

        static double Minimax(char[,] state, int player, int depth)
        {//gameover, statevalue, nextstates,
            if (depth == 0 || GameOver(state, player) != 2) //GameOver(state, player, PlayerInput) !=2    added
                return StateValue(state, player);
            double maxValue = double.NegativeInfinity;


            foreach (char[,] nextState in NextStates(state, player))
            {
                //if (depth == 10)
                //    Console.WriteLine();
                double nextValue = -Minimax(nextState, -player, depth - 1);
                if (nextValue > maxValue)
                    maxValue = nextValue;
            }

            if (double.IsNegativeInfinity(maxValue))
                return StateValue(state, player);
            return maxValue;
        }

        private static List<char[,]> NextStates(char[,] state, int player)
        //PlayerInput (char) changed from player (int)
        {
            List<char[,]> result = new List<char[,]>();
            for (int i = 0; i < state.GetLength(0); i++)
            {
                for (int j = 0; j < state.GetLength(1); j++)
                {

                    if (state[i, j] == '-')
                    {
                        char[,] tempstate = (char[,])state.Clone();
                        tempstate[i, j] = PlayerInput(player) ;
                        result.Add(tempstate);
                    }
                }
            }
            return result;
        }

        private static double StateValue(char[,] state, int player)
        {
            //throw new NotImplementedException();
            double value = 0;
            int[,] winarray = new int[,] { { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 }, { 0, 3, 6 }, { 1, 4, 7 }, { 2, 5, 8 }, { 0, 4, 8 }, { 2, 4, 6 } };
            for (int i = 0; i < winarray.GetLength(0); i++)
            {
                if (state[winarray[i, 0] / 3, winarray[i, 0] % 3] == PlayerInput(player) && state[winarray[i, 1] / 3, winarray[i, 1] % 3] == PlayerInput(player) && state[winarray[i, 2] / 3, winarray[i, 2] % 3] == PlayerInput(player))
                    value = 100;
                else if (state[winarray[i, 0] / 3, winarray[i, 0] % 3] == PlayerInput(player) && state[winarray[i, 1] / 3, winarray[i, 1] % 3] == PlayerInput(player) && state[winarray[i, 2] / 3, winarray[i, 2] % 3] == PlayerInput(player))
                    value = -100;
            }
            return value;
        }
        static char[,] AddXO(char[,] board, int line, int column, int player)
        {
            if (board[line - 1, column - 1] == '-')
            {
                board[line - 1, column - 1] = PlayerInput(player);

            }
            return board;
        }
        static void PrintBoard(char[,] board)
        {
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    Console.Write(board[i, j] + "  ");
                }
                Console.WriteLine("\n");
            }
            Console.WriteLine();
        }
        static void WhoLeads(int playerpoints, int computerpoints)
        {
            if (playerpoints > computerpoints)
                Console.WriteLine(playerpoints + " - " + computerpoints + " You lead!");
            else if (playerpoints == computerpoints)
                Console.WriteLine(playerpoints + " - " + computerpoints + " It's a tie!");
            else
                Console.WriteLine(playerpoints + " - " + computerpoints + " The computer leads!");
        }
        static char PlayerInput(int player)
        {
            if (player == 1)
                return 'X';
            return 'O';
        }
        static int GameOver(char[,] board, int player)
        {
            if (StateValue(board, player) == 0)
            {
                if (IsFull(board))
                    return 0;
                return 2;
            }
            else if (Convert.ToInt32(StateValue(board, player)) >= 100)
                return 1;
            else
                return -1;
        }
        static bool IsFull(char[,] board)
        {
            foreach (char i in board)
            {
                if (i == '-')
                    return false;
            }
            return true;
        }
        static int Game()
        {
            int player;
            int turncounter = 0;
            Console.WriteLine("Do you want to start?");
            string wanttostart = Console.ReadLine();
            if (wanttostart.ToLower() != "no")
                player = -1;
            else
                player = 1;

            char[,] board = new char[8, 8];
            for (int i = 0; i < board.GetLength(0); i++) 
            {
                for (int j = 0; j < board.GetLength(1); j++)
                    board[i, j] = '-';
            }
            PrintBoard(board);
            do
            {
                if (player == -1)
                    board = PlayerTurn(board);
                else
                    board = ComputerTurn(board, turncounter, player);
                PrintBoard(board);
                turncounter++;
                player = -player;
            } while (GameOver(board, player) == 2);

            return GameOver(board, -player);
        }
        static char[,] PlayerTurn(char[,] board)
        {
            while (true)
            {
                Console.WriteLine("enter row");
                int row = int.Parse(Console.ReadLine());
                Console.WriteLine("enter column");
                int column = int.Parse(Console.ReadLine());
                if (row <= 3 && column <= 3 && row >= 1 && column >= 1)
                {
                    if (board[row - 1, column - 1] == '-')
                    {
                        return AddXO(board, row, column, -1);
                    }
                }
                Console.WriteLine("Learn to play... Please enter row and col again");
            }
        }
        static char[,] ComputerTurn(char[,] board, int turncounter, int player)
        {
            Console.WriteLine("Computer's turn:");
            double idealscore = double.PositiveInfinity;
            char[,] idealstate = null;
            foreach (char[,] nextstate in NextStates(board, PlayerInput(player)))
            {
                double score = Minimax(nextstate, -1, 4);
                if (score < idealscore)
                {
                    idealscore = score;
                    idealstate = nextstate;
                }
                //!!Randomising option!! if (Minimax(nextstate, -1, 4) = idialscore)
                //{
                //    in
                //    if(
                //}
            }
            return idealstate;
        }
    }
}
