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
        // Place your variables here:


        int punches;
        int slaps;
        float totalDamage;
        float punchPower = 0f;
        float chargeStart = 0f;
        bool isChargingPunch = false;
        float slapPower = 0f;
        bool isChargingSlap = false;


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
            // determines of the user is charging a punch
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

                int radius = (int)(punchPower * 10);

                Draw.FillColor = Color.Red;
                Draw.Circle(x, y, radius);
            }

            // user is going for a slap
            if (Input.IsMouseButtonPressed(MouseInput.Right))
            {
                chargeStart = Time.SecondsElapsed;
            }
            // determines of the user is charging a slap
            if (Input.IsMouseButtonDown (MouseInput.Right))
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


            }
            
            Raylib.DrawText("Punch with Left Mouse Button", 10, 10, 20, Color.Black);
            Raylib.DrawText("Slap with Right Mouse Button", 10, 40, 20, Color.Black);
            Raylib.DrawText("Punches: " + punches, 10, 70, 20, Color.Black);
            Raylib.DrawText("Slaps: " + slaps, 10, 100, 20, Color.Black);
            Raylib.DrawText("Total Damage: " + (int)totalDamage, 10, 130, 20, Color.Black);
        }
    }

}
