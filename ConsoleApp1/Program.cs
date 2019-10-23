using Raylib;
using static Raylib.Raylib;

namespace ConsoleApp1
{
    static class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();

            InitWindow(1280, 960, "Tanks for Everything");

            game.Init();

            while (!WindowShouldClose())
            {
                game.Update();
                game.Draw();
            }

            game.Shutdown();

            CloseWindow();
        }
    }
}
