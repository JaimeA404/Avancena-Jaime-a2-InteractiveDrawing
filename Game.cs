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

        // custom colours for the face

        Color skinTone = new Color("#f7b46d");
        Color Smile = new Color("#fcb6b7");

        // damage tracking + display

        Vector2 facePos = new Vector2(400, 200);
        float faceRadius = 100f;

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

        // array of messages to display at different damage thresholds
        String[] messages = {
            "Please... I have a family...",
            "STOP!", 
            "WHAT DID I EVER DO TO YOU?!",
            "Make it stop..."
        };
        float[] messageThresholds = { 25f, 50f, 75f };


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
            
            void DrawFace(int x, int y)
            {
                Draw.FillColor = skinTone;
                Draw.Circle(x, y, 100); // head
            }

            void DrawEyeball(int x, int y)
            {
                int eyeballRadius = 20;
                Draw.FillColor = Color.White;
                Draw.Circle(x, y, eyeballRadius); // sclera 
                Draw.FillColor = Color.Black;
                Draw.Circle(x, y, eyeballRadius / 2); // pupil
            }

            void DrawSmile(int x, int y)
            {
                Draw.FillColor = Smile;
                Draw.Arc(x, y, 60, 30, 0, 180); // smile
            }

            void DrawLeftHurtEye(int x, int y)
            {
                int eyeballRadius = 20;
                Draw.FillColor = Color.White;
                Draw.Circle(x, y, eyeballRadius); // sclera
                Draw.FillColor = Color.Black;
                Draw.Circle(x, y, eyeballRadius / 2); // pupil
                Draw.Arc(x, y + 50, 50, 20, 0, 0);
            }

            void DrawRightHurtEye(int x, int y)
            {
                int eyeballRadius = 20;
                Draw.FillColor = Color.White;
                Draw.Circle(x, y, eyeballRadius); // sclera
                Draw.FillColor = Color.Black;
                Draw.Circle(x, y, eyeballRadius / 2); // pupil
                Draw.Arc(x, y - 50, 50, 20, 0, 0);
            }
            // the victim

            Draw.FillColor = skinTone;
            DrawFace(400, 200); // head
            DrawEyeball(360, 180); // left eye
            DrawEyeball(440, 180); // right eye
            DrawSmile(400, 240); // mouth

            //HUD displays information

            Raylib.DrawText("Punch with Left Mouse Button", 10, 10, 20, Color.Black);
            Raylib.DrawText("Slap with Right Mouse Button", 10, 40, 20, Color.Black);
            Raylib.DrawText("You can hold down the buttons to increase damage", 10, 70, 20, Color.Black);
            Raylib.DrawText("Punches: " + punches, 10, 100, 20, Color.Black);
            Raylib.DrawText("Slaps: " + slaps, 10, 130, 20, Color.Black);
            Raylib.DrawText("Total Damage: " + (int)totalDamage, 10, 160, 20, Color.Black);

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
                int mouseX = (int)Input.GetMouseX();
                int mouseY = (int)Input.GetMouseY();
                Vector2 mousePos = new Vector2(mouseX, mouseY);

                float distanceToFace = Vector2.Distance(mousePos, facePos);
                bool hitFace = distanceToFace <= faceRadius;

                if (hitFace)
                {


                    punches++;
                    totalDamage += punchPower;
                    int radius = (int)(punchPower * 15);
                    Draw.FillColor = Color.Red;
                    Draw.Circle(mouseX, mouseY, radius);
                }
                else // player missed
                {
                    Raylib.DrawText("Missed!", 10, 190, 20, Color.Black);
                }
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

            // first message at >=25 damage

            if (totalDamage >= 25f && totalDamage < 50 && !messageShown25)
            {
                messageShown25 = true;
                messageActive25 = true;
                messageStartTime = Time.SecondsElapsed;
            }
            if (messageActive25)
            {
                DrawLeftHurtEye(360, 180);
                DrawRightHurtEye(440, 180);
                if (Time.SecondsElapsed - messageStartTime < messageActivity)
                {
                    Raylib.DrawText((messages[0]), 450, 10, 20, Color.Black);

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
                    Raylib.DrawText((messages[1]), 450, 10, 20, Color.Black);
                    Raylib.DrawText((messages[2]), 450, 40, 20, Color.Black);
                    if (Time.SecondsElapsed == 2f)
                    {
                        messageActive50 = false;
                        messageShown50 = false;
                    }
                }
            }

            // third message at >=75 damage

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
                    Raylib.DrawText((messages[3]), 450, 10, 20, Color.Black);
                    if (Time.SecondsElapsed == 2f)
                    {
                        messageActive75 = false;
                        messageShown75 = false;
                    }
                }
            }

            // player wins at 100 damage

            if (totalDamage >= 100f)
            {
                guyBeatToDeath = true;
            }
            if (guyBeatToDeath)
            {
                Draw.FillColor = Color.White;
                Draw.Rectangle(0, 0, Window.Width, Window.Height);
                Draw.FillColor = Color.LightGray;
                Draw.Rectangle(360, 100, 80, 100);
                Draw.FillColor = Color.LightGray;
                Draw.Circle(400, 100, 40);
                Draw.FillColor = Color.LightGray;
                Draw.LineColor = Color.LightGray;
                Draw.Rectangle(361, 101, 78, 50);
                Raylib.DrawText("R.I.P", 368, 120, 30, Color.Black);
                Raylib.DrawText("You beat the guy to death!", 200, 300, 30, Color.Black);
                Raylib.DrawText("You better star running from the police", 180, 400, 30, Color.Black);
            }
        }
    }
}
