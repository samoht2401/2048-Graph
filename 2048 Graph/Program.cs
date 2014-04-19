using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2048_Graph
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Run(20, 60);
            game.Title = "Game";
        }
    }
}
