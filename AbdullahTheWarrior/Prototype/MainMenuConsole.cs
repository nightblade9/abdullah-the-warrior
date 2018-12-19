using System;
using System.Collections.Generic;
using System.Linq;
using DeenGames.AbdullahTheWarrior.Ecs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SadConsole;

namespace DeenGames.AbdullahTheWarrior.Prototype
{
    public class MainMenuConsole : SadConsole.Console
    {
        private bool isSelectingClass = false;

        public MainMenuConsole(int width, int height) : base(width, height)
        {
            int startX = width / 2;
            int startY = height / 2;
            this.Print(startX, startY, "[N]ew Game");
        }

        public override void Update(System.TimeSpan delta)
        {
            if (!isSelectingClass && Global.KeyboardState.IsKeyReleased(Keys.N))
            {
                this.isSelectingClass = true;
                this.ShowSpecializationSelections();
            }
            else if (isSelectingClass && Global.KeyboardState.KeysReleased.Any())
            {
                if (Global.KeyboardState.IsKeyReleased(Keys.A))
                {
                    this.StartGameAs(Specialization.Faris);
                }
                else if (Global.KeyboardState.IsKeyReleased(Keys.B))
                {
                    this.StartGameAs(Specialization.Stunhammer);
                }
                else if (Global.KeyboardState.IsKeyReleased(Keys.C))
                {
                    this.StartGameAs(Specialization.Ghazi);
                }
            }
        }

        private void StartGameAs(Specialization specialization)
        {
            int playerHealth = 40;
            int playerStrength = 5;
            int playerDefense = 3;
            int playerVision = 6;
            int numTurns = 1;
            int numAttacks = 1;

            if (specialization == Specialization.Faris)
            {
                numTurns = 2;
                numAttacks = 2;
                playerStrength = 7;
            }
            else if (specialization == Specialization.Stunhammer)
            {
                playerDefense = 4;
                playerHealth = 50;
            }
            else if (specialization == Specialization.Ghazi)
            {
                playerHealth = 45;
                playerStrength = 6;
                playerVision = 7;
            }
            
            Global.CurrentScreen.Children.Clear();
            Global.CurrentScreen.Children.Add(new PrototypeGameConsole(this.Width, this.Height, specialization, playerHealth, playerStrength, playerDefense, playerVision, numTurns, numAttacks));
        }

        private void ShowSpecializationSelections()
        {
            this.Clear();

            int startX = 8;
            int startY = this.Height / 2;

            this.Print(startX, startY, "[A] Faris (bow, multiple turns, multiple hits per attack)");
            this.Print(startX, startY + 1, "[B] Stunhammer (stuns and knocks back enemies)");
            this.Print(startX, startY + 2, "[C] Ghazi (moves across the map during attacks)");
        }
    }
}