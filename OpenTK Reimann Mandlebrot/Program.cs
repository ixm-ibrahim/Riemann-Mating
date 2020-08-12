using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTK_Riemann_Mating
{
    class Program
    {
        static void Main(string[] args)
        {
            // This line creates a new instance, and wraps the instance in a using statement so it's automatically disposed once we've exited the block.
            using (Game game = new Game(800, 800, "LearnOpenTK"))
            //using (Game game = new Game(1280, 720, "Riemann Julia"))
            //using (Game game = new Game(1920, 1080, "Riemann Julia"))
            {
                //Run takes a double, which is how many frames per second it should strive to reach.
                //You can leave that out and it'll just update as fast as the hardware will allow it.
                game.Run(60.0);
            }
        }
    }
}
