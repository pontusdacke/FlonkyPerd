using System;

namespace Flonky_Perd
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (FlonkyPerd game = new FlonkyPerd())
            {
                game.Run();
            }
        }
    }
#endif
}

