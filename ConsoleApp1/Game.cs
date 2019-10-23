using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using Raylib;
using static Raylib.Raylib;

namespace ConsoleApp1
{
    class Game
    {

        Stopwatch stopwatch = new Stopwatch();

        private long currentTime = 0;
        private long lastTime = 0;
        private float timer = 0;
        private int fps = 1;
        private int frames;
        private bool fired = false;

        SceneObject tankObject = new SceneObject();
        SceneObject turretObject = new SceneObject();
        SceneObject bulletObject = new SceneObject();
        SceneObject tankHitboxPoint1 = new SceneObject();
        SceneObject tankHitboxPoint2 = new SceneObject();
        SceneObject tankHitboxPoint3 = new SceneObject();
        SceneObject tankHitboxPoint4 = new SceneObject();

        SpriteObject tankSprite = new SpriteObject();
        SpriteObject turretSprite = new SpriteObject();
        SpriteObject bulletSprite = new SpriteObject();

        private float deltaTime = 0.005f;

        public void Init()
        {
            stopwatch.Start();
            lastTime = stopwatch.ElapsedMilliseconds;

            

            tankSprite.Load("tankBlue_outline.png");
            tankSprite.SetRotate(-90 * (float)(Math.PI / 180.0f));
            tankSprite.SetPosition(-tankSprite.Width / 2.0f, tankSprite.Height / 2.0f);

            tankHitboxPoint1.SetPosition(-tankSprite.Width / 2.0f, -tankSprite.Height / 2.0f);
            tankHitboxPoint2.SetPosition(tankSprite.Width / 2.0f, -tankSprite.Height / 2.0f);
            tankHitboxPoint3.SetPosition(tankSprite.Width / 2.0f, tankSprite.Height / 2.0f);
            tankHitboxPoint4.SetPosition(-tankSprite.Width / 2.0f, tankSprite.Height / 2.0f);

            turretSprite.Load("barrelBlue.png");
            turretSprite.SetRotate(-90 * (float)(Math.PI / 180.0f));
            turretSprite.SetPosition(0, turretSprite.Width / 2.0f);

            bulletSprite.Load("FriendBullet.png");
            bulletSprite.SetRotate(-90 * (float)(Math.PI / 180f));
            bulletSprite.SetPosition(50, bulletSprite.Width / 2.0f);

            turretObject.AddChild(turretSprite);
            tankObject.AddChild(tankSprite);
            bulletObject.AddChild(bulletSprite);
            tankObject.AddChild(turretObject);
            tankHitboxPoint1.AddChild(bulletObject);
            tankHitboxPoint2.AddChild(bulletObject);
            tankHitboxPoint3.AddChild(bulletObject);
            tankHitboxPoint4.AddChild(bulletObject);
            tankObject.AddChild(tankHitboxPoint1);
            tankObject.AddChild(tankHitboxPoint2);
            tankObject.AddChild(tankHitboxPoint3);
            tankObject.AddChild(tankHitboxPoint4);
            tankObject.SetPosition(GetScreenWidth() / 2.0f, GetScreenHeight() / 2.0f);

            
        }

        public void Shutdown()
        { }

        public void Update()
        {
            currentTime = stopwatch.ElapsedMilliseconds;
            deltaTime = (currentTime - lastTime)/1000.0f;

            timer += deltaTime;
            if (timer >= 1)
            {
                fps = frames;
                frames = 0;
                timer--;
            }
            frames++;

            if (IsKeyDown(KeyboardKey.KEY_A))
            {
                tankObject.Rotate(-deltaTime * 10);
            }
            if (IsKeyDown(KeyboardKey.KEY_D))
            {
                tankObject.Rotate(deltaTime * 10);
            }
            if (IsKeyDown(KeyboardKey.KEY_W))
            {
                Vector3 facing = new Vector3(tankObject.LocalTransform.m1, tankObject.LocalTransform.m2, 1) * deltaTime * 500;
                tankObject.Translate(facing.x, facing.y);
            }
            if (IsKeyDown(KeyboardKey.KEY_S))
            {
                Vector3 facing = new Vector3(tankObject.LocalTransform.m1, tankObject.LocalTransform.m2, 1) * deltaTime * -500;
                tankObject.Translate(facing.x, facing.y);
            }
            if (IsKeyDown(KeyboardKey.KEY_Q))
            {
                turretObject.Rotate(-deltaTime * 50);
            }
            if (IsKeyDown(KeyboardKey.KEY_E))
            {
                turretObject.Rotate(deltaTime * 50);
            }
            if (IsKeyDown(KeyboardKey.KEY_SPACE))
            {
                tankObject.RemoveChild(bulletObject);
                fired = true;
            }
            if (fired == true)
            {
                Vector3 facing = new Vector3(bulletObject.LocalTransform.m1, bulletObject.LocalTransform.m2, 1) * deltaTime * 500;
                bulletObject.Translate(facing.x, facing.y);
            }

            /*if (IsKeyPressed(KeyboardKey.KEY_SPACE))
            {
                bulletObject.GlobalTransform.m7++;
            }*/

            tankObject.Update(deltaTime);

            lastTime = currentTime;
        }

        public void Draw()
        {
            BeginDrawing();

            ClearBackground(Color.WHITE);
            DrawText(fps.ToString(), 10, 10, 12, Color.RED);

            tankObject.Draw();
            tankHitboxPoint1.Draw();
            tankHitboxPoint2.Draw();
            tankHitboxPoint3.Draw();
            tankHitboxPoint1.Draw();
            EndDrawing();
        }
    }
}
