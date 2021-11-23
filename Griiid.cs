using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using static AutoBattle.Types;

namespace AutoBattle
{
    public class Grid
    {
        public List<GridBox> grids = new List<GridBox>();

        public int xLenght;
        
        public int yLength;

        public int Size
        {
            get { return xLenght * yLength; }
        }

        public Grid(int Lines, int Columns)
        {
            xLenght = Lines;
            yLength = Columns;

            for (int i = 0; i < Lines; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    GridBox newBox = new GridBox(j, i, false, (Columns * i + j));
                    grids.Add(newBox);
                }
            }
            
            Console.WriteLine("The battle field has been created\n");
        }


        // prints the matrix that indicates the tiles of the battlefield
        public void drawBattlefield(List<Character> players)
        {
            int currentgridIndex = 0;

            for (int i = 0; i < xLenght; i++)
            {
                for (int j = 0; j < yLength; j++)
                {
                    GridBox currentgrid = grids[currentgridIndex];
                    
                    if (currentgrid.ocupied)
                    {
                        string playerID = "?";
                        for(int p = 0; p < players.Count; p++){
                            if(players[p].currentBox.Index == currentgrid.Index)
                            playerID = players[p].PlayerIndex.ToString();
                        }
                        Console.Write($"[{playerID}]\t");
                    }
                    else
                    {
                        Console.Write($"[ ]\t");
                    }
                    currentgridIndex++;
                }
                Console.Write(Environment.NewLine + Environment.NewLine);
            }
            Console.Write(Environment.NewLine + Environment.NewLine);
        }
        

    }
}
