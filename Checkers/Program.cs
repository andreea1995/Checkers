using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Checkers
{
    class Program
    {
        static void Main(string[] args) //OK
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


        static double Minimax(char[,] state, int player, int depth) //OK (needs checking)
        {//gameover, statevalue, nextstates,
            double statescore = StateValue(state, player);
            if (depth == 0 || GameOver(state) != 0)
                return statescore;
            double maxValue = double.NegativeInfinity;
            foreach (char[,] nextState in NextStates(state, player))
            {
                double nextValue = -Minimax(nextState, -player, depth - 1);
                if (nextValue > maxValue)
                    maxValue = nextValue;
            }
            if (double.IsNegativeInfinity(maxValue))
                return statescore;
            return maxValue;
        }


        private static List<char[,]> NextStates(char[,] state, int player) //add kings. + option to add multiple eating in here.
        {
            List<char[,]> result = new List<char[,]>();
            for (int i = (1 - player) / 2; i < state.GetLength(0) - 1; i++) //change to (int i = 0; i < state.GetLength(0); i++) if added kings to function
            {
                for (int j = (i + 1) % 2; j < state.GetLength(1); j += 2) 
                {

                    if (state[i, j] == InputFromPlayer(player))
                    {
                        if (state[i + player, j - 1] == '-' && j > 0)
                        {
                            char[,] tempstate = (char[,])state.Clone();
                            tempstate[i, j] = '-';
                            tempstate[i + player, j - 1] = InputFromPlayer(player);
                            result.Add(tempstate);
                        }
                        else if (state[i + player, j + 1] == '-' && j<state.GetLength(1)-1)
                        {
                            char[,] tempstate = (char[,])state.Clone();
                            tempstate[i, j] = '-';
                            tempstate[i + player, j + 1] = InputFromPlayer(player);
                            result.Add(tempstate);
                        }
                        else if (state[i + player, j - 1] == InputFromPlayer(-player) && state[i + 2 * player, j - 2] == '-' && j > 1)
                        {
                            char[,] tempstate = (char[,])state.Clone();
                            tempstate[i, j] = '-';
                            tempstate[i + player, j - 1] = '-';
                            tempstate[i + 2*player, j - 2] = InputFromPlayer(player);
                            result.Add(tempstate);
                        }
                        else if (state[i + player, j + 1] == InputFromPlayer(-player) && state[i + 2 * player, j + 2] == '-' && j < state.GetLength(1) - 2)
                        {
                            char[,] tempstate = (char[,])state.Clone();
                            tempstate[i, j] = '-';
                            tempstate[i + player, j + 1] = '-';
                            tempstate[i + 2 * player, j + 2] = InputFromPlayer(player);
                            result.Add(tempstate);
                        }
                        //+kings?
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
            int whowon = GameOver(state);
            if (whowon == player)
                    return 1000;
            else if (whowon == -player)
                    return -1000;
            return value;
        }


        static char[,] MovePiece(char[,] state, int i, int j, int player, int dir)
        {
            if (i >= 0 && i < state.GetLength(0) && j >= 0 && j < state.GetLength(1))
            {
                if (i!=0 && state[i, j] == InputFromPlayer(player) && (dir==7 || dir==9))
                {  
                    if(state[i-1,dir-8]=='-')
                    {
                        state[i, j] = '-';
                        state[i - 1, dir - 8] = 'O';
                    }
                    else if (i != 1 && state[i - 1, dir - 8] == 'X' && state[i - 2, 2*(dir - 8)] == '-')
                    {
                        state[i, j] = '-';
                        state[i - 1, dir - 8] = '-';
                        state[i - 2, 2 * (dir - 8)] = 'O';
                    }
                    else
                    {
                        Console.WriteLine("you can't move your piece there");
                        return null;
                    }
                }
                else if (state[i, j] == 'W' && (dir==1 || dir==3 || dir==7 || dir==9)) //complete kings
                {
                    //if((i==0
                }
                else
                {
                    Console.WriteLine("Something wrong with the player's location or the direction you chose");
                    return null;
                }
                return null; //not really null!!!!!! sould be the new state
            }
            else
            {
                Console.WriteLine("Something wrong with row or col number");
                return null;
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
                int col = int.Parse(Console.ReadLine());
                Console.WriteLine("enter direction (for regular player 7/9, for king 1/3/7/9)");
                int dir = int.Parse(Console.ReadLine());
                char[,] newstate = MovePiece(state, row-1, col-1, -1, dir);
                if (row <= state.GetLength(0) && col <= state.GetLength(1) && row >= 1 && col >= 1)
                {
                    if (newstate!=null)
                    {
                        return newstate;
                    }
                }
                Console.WriteLine("Please enter row, column and direction again");
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
