// Include the namespaces (code libraries) you need below.
using Raylib_cs;
using System;
using System.Numerics;

// The namespace your code is in.
namespace MohawkGame2D
{
    /// <summary>
    ///     Your game code goes inside this class!
    /// </summary>
    public class Game
    {
        // Place your variables here

        // damage tracking + display

        int punches;
        int slaps;
        float totalDamage;
        float punchPower = 0f;
        float chargeStart = 0f;
        float slapPower = 0f;

        // dialogue system

        float messageStartTime = -1f;
        float messageActivity = 2f; //forces the message to show for 2 seconds
        bool messageActive25 = false;
        bool messageShown25 = false;
        bool messageActive50 = false;
        bool messageShown50 = false;
        bool messageActive75 = false;
        bool messageShown75 = false;
        bool guyBeatToDeath = false;

        /// <summary>
        ///     Setup runs once before the game loop begins.
        /// </summary>
        public void Setup()
        {
            Window.SetTitle("Beat a guy to Death");
            Window.SetSize(800, 600);
            Window.TargetFPS = 60;
        }


        /// <summary>
        ///     Update runs every frame.
        /// </summary>
        public void Update()
        {
            Window.ClearBackground(Color.White);
            // user is going for a punch
            if (Input.IsMouseButtonPressed(MouseInput.Left))
            {
                chargeStart = Time.SecondsElapsed;
            }
            // determines of the user is charging a punch or tapping the left mouse button
            if (Input.IsMouseButtonDown(MouseInput.Left))
            {
                float heldTime = Time.SecondsElapsed - chargeStart;
                if (heldTime > 5f) heldTime = 5f;
                if (heldTime < 0f) heldTime = 1f;
                punchPower = (heldTime) * 2;
                if (heldTime <= 1f) punchPower = 1;
            }

            if (Input.IsMouseButtonReleased(MouseInput.Left))
            {
                punches++;
                totalDamage += punchPower;

                int x = (int)Input.GetMouseX();
                int y = (int)Input.GetMouseY();

                int radius = (int)(punchPower * 30);

                Draw.FillColor = Color.Red;
                Draw.Circle(x, y, radius);
            }

            // user is going for a slap
            if (Input.IsMouseButtonPressed(MouseInput.Right))
            {
                chargeStart = Time.SecondsElapsed;
            }
            // determines of the user is charging a slap or tapping the right mouse button
            if (Input.IsMouseButtonDown(MouseInput.Right))
            {
                float heldTime = Time.SecondsElapsed - chargeStart;
                if (heldTime > 5f) heldTime = 5f;
                if (heldTime < 0f) heldTime = 0f;
                slapPower = (heldTime);
                if (heldTime < 1f) slapPower = 1;
            }

            if (Input.IsMouseButtonReleased(MouseInput.Right))
            {
                slaps++;
                totalDamage += slapPower;

                int x = (int)Input.GetMouseX();
                int y = (int)Input.GetMouseY();

                int width = (int)(slapPower * 20);
                int height = (int)(slapPower * 60);

                Draw.FillColor = Color.Red;
                Draw.Rectangle(x - width / 2, y - height / 2, width, height);
            }

            //HUD displays information

            Raylib.DrawText("Punch with Left Mouse Button", 10, 10, 20, Color.Black);
            Raylib.DrawText("Slap with Right Mouse Button", 10, 40, 20, Color.Black);
            Raylib.DrawText("Punches: " + punches, 10, 70, 20, Color.Black);
            Raylib.DrawText("Slaps: " + slaps, 10, 100, 20, Color.Black);
            Raylib.DrawText("Total Damage: " + (int)totalDamage, 10, 130, 20, Color.Black);

            // first message at >=25 damage

            if (totalDamage >= 25f && totalDamage < 50 && !messageShown25)
            {
                messageShown25 = true;
                messageActive25 = true;
                messageStartTime = Time.SecondsElapsed;
            }
            if (messageActive25)
            {
                if (Time.SecondsElapsed - messageStartTime < messageActivity)
                {
                    Raylib.DrawText("Please... I have a family...", 500, 10, 20, Color.Black);

                    if (Time.SecondsElapsed == 2f)
                    {
                        messageActive25 = false;
                        messageShown25 = false;
                    }
                }
            }

            // second message at >=50 damage

            if (totalDamage >= 50 && totalDamage < 75 && !messageShown50)
            {
                messageActive25 = false;
                messageShown25 = false;
                messageShown50 = true;
                messageActive50 = true;
                messageStartTime = Time.SecondsElapsed;
            }
            if (messageActive50)
            {
                if (Time.SecondsElapsed - messageStartTime < messageActivity)
                {
                    Raylib.DrawText("STOP!", 550, 10, 20, Color.Black);
                    Raylib.DrawText("WHAT DID I EVER DO TO YOU?!", 450, 40, 20, Color.Black);
                    if (Time.SecondsElapsed == 2f)
                    {
                        messageActive50 = false;
                        messageShown50 = false;
                    }
                }
            }

            if (totalDamage >= 75f && totalDamage < 100 && !messageShown75)
            {
                messageActive50 = false;
                messageShown50 = false;
                messageShown75 = true;
                messageActive75 = true;
                messageStartTime = Time.SecondsElapsed;
            }
            if (messageActive75)
            {
                if (Time.SecondsElapsed - messageStartTime < messageActivity)
                {
                    Raylib.DrawText("Make it stop...", 500, 10, 20, Color.Black);
                    if (Time.SecondsElapsed == 2f)
                    {
                        messageActive75 = false;
                        messageShown75 = false;
                    }
                }
            }

            if (totalDamage >= 100f)
            {
                guyBeatToDeath = true;
            }
            if (guyBeatToDeath)
            {
                Raylib.ClearBackground(Color.White);
                Raylib.DrawText("You have beaten the guy to death!", 250, 300, 30, Color.Black);
            }
        }
    }
}
