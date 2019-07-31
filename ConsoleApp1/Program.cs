using Raylib;
using rl = Raylib.Raylib;

static class Program
{
    public static void Main()
    {
        rl.InitWindow(640, 480, "Hello World");

        while (!rl.WindowShouldClose())
        {
            rl.BeginDrawing();

            rl.ClearBackground(Color.WHITE);
            rl.DrawText("Hello, world!", 12, 12, 20, Color.BLACK);

            rl.EndDrawing();
        }

        rl.CloseWindow();
    }
}