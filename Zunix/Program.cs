namespace Zunix
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (ZunixGame game = new ZunixGame())
            {
                game.Run();
            }
        }
    }
}
