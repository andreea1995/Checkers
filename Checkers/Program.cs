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
            if (depth == 0 || GameOver(state) != 0) //GameOver(state, player, InputFromPlayer) !=0    added
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
        //InputFromPlayer (char) changed from player (int)
        {
            List<char[,]> result = new List<char[,]>();
            for (int i = 0; i < state.GetLength(0); i++)
            {
                for (int j = (i + 1) % 2; j < state.GetLength(1); j += 2)
                {

                    if (state[i, j] == '-')
                    {
                        char[,] tempstate = (char[,])state.Clone();
                        tempstate[i, j] = InputFromPlayer(player) ;
                        result.Add(tempstate);
                    }
                }
            }
            return result;
        }

        private static double StateValue(char[,] state, int player) //לשנות כך שיהיה שימוש ב GameOver
        {   
            /*
            1) more pieces than the other (regular and kings calculation)
            2) + eat and be eaten (not many points because it could lead to being eaten next turn) 
               - be eaten and eat (")
               * could be affected by if you have more pieces or not! (like atack and defence mode)
            3) + eat with no harm (win many points)
               - be eaten with no eating (lose many point)
            4) traps (????????)
            5) do not abandone 1st line
            6) clear way towards king
            7) building triangles and squares
            8) double and triple eating
            9)
            10)
            */
            //throw new NotImplementedException();
            double value = 0;
                if (GameOver(state)==player)
                    return 1000;
                else if (GameOver(state) == -player)
                    return -1000;
            return value;
        }
        static char[,] AddXO(char[,] state, int line, int column, int player)
        {
            if (state[line - 1, column - 1] == '-')
            {
                state[line - 1, column - 1] = InputFromPlayer(player);

            }
            return state;
        }
        static void PrintState(char[,] state)
        {
            for (int i = 0; i < state.GetLength(0); i++)
            {
                for (int j = 0; j < state.GetLength(1); j++)
                {
                    Console.Write(state[i, j] + "  ");
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
        static char InputFromPlayer(int player)
        {
            if (player == 1)
                return 'X';
            return 'O';
        }
        static int PlayerFromInput(char input)
        {
            if (input == 'X')
                return 1;
            return -1;
        }
        static int GameOver(char[,] state) //updated
        {
            bool flagwhiteO = false;
            bool flagblackX = false;
            for (int i=0; i<state.GetLength(0); i++)
            {
                for (int j = (i + 1) % 2; j < state.GetLength(1); j += 2)
                {
                    if (state[i,j] == 'X')
                        flagblackX = true;
                    else if (state[i, j] == 'O')
                        flagwhiteO = true;
            }
            }
            if(!flagwhiteO) //no more white pieces (O)
                return 1; //computer won
            else if (!flagblackX) //no more black pieces (X)
                return -1; //player won
            else
                return 0;
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

            char[,] state = new char[8, 8] {{'-','X','-','X','-','X','-','X'},
                                            {'X','-','X','-','X','-','X','-'},
                                            {'-','X','-','X','-','X','-','X'},                                           
                                            {'-','-','-','-','-','-','-','-'},
                                            {'-','-','-','-','-','-','-','-'},
                                            {'O','-','O','-','O','-','O','-'},
                                            {'-','O','-','O','-','O','-','O'},
                                            {'O','-','O','-','O','-','O','-'}};
            PrintState(state);
            do
            {
                if (player == -1)
                    state = PlayerTurn(state);
                else
                    state = ComputerTurn(state, turncounter, player);
                PrintState(state);
                turncounter++;
                player = -player;
            } while (GameOver(state) == 0);

            return GameOver(state);
        }
        static char[,] PlayerTurn(char[,] state)
        {
            while (true)
            {
                Console.WriteLine("enter row");
                int row = int.Parse(Console.ReadLine());
                Console.WriteLine("enter column");
                int column = int.Parse(Console.ReadLine());
                if (row <= 3 && column <= 3 && row >= 1 && column >= 1)
                {
                    if (state[row - 1, column - 1] == '-')
                    {
                        return AddXO(state, row, column, -1);
                    }
                }
                Console.WriteLine("Learn to play... Please enter row and col again");
            }
        }
        static char[,] ComputerTurn(char[,] state, int turncounter, int player)
        {
            Console.WriteLine("Computer's turn:");
            double idealscore = double.PositiveInfinity;
            char[,] idealstate = null;
            foreach (char[,] nextstate in NextStates(state, InputFromPlayer(player)))
            {
                double score = Minimax(nextstate, -1, 4);
                if (score < idealscore)
                {
                    idealscore = score;
                    idealstate = nextstate;
                }
                //!!Randomising option!! if (Minimax(nextstate, -1, 4) = idealscore)
                //{
                //    
                //    if(
                //}
            }
            return idealstate;
        }
        /*           for (int i=0; i<state.GetLength(0); i++)
       {
           for (int j = (i + 1) % 2; j < state.GetLength(1); j += 2)
           {
           
           }
       }*/
    }
}
