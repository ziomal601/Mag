using System;

namespace Magowie
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Render game = new Render())
            {
                game.Run();
            }
        }
    }
#endif
}

