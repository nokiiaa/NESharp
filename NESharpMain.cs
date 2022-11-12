using NESharp.NES;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SFML.Window;
using SFML.Graphics;
using SFML.System;
using System.IO;

namespace NESharp
{
    public static class NESharpMain
    {
        public static long cycles;
        public static long cps;

        public static Game CurrentGame { get; private set; } = new Game();
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                MessageBox.Show("No input ROM specified!", "NE#: Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(-1);
            }

            CurrentGame.ROM = ROM.Parse(args[0]);
            CurrentGame.ROM.Mapper.Initialize();
            CurrentGame.OutputMode = OutputMode.NTSC;
            CPU.Initialize();
            PPU.Initialize();

            Texture screenTexture = new Texture(256, 240);
            RenderWindow gameWnd = new RenderWindow(VideoMode.DesktopMode, $"NE# v0.01 - {Path.GetFileName(CurrentGame.ROM.Filename)}")
            {
                Size = new Vector2u(640, 480)
            };
            SFML.Graphics.View camera = new SFML.Graphics.View(new Vector2f(0, 0), new Vector2f(280, 240));
            gameWnd.SetVisible(true);
            gameWnd.SetVerticalSyncEnabled(true);
            gameWnd.SetFramerateLimit(CurrentGame.OutputMode == OutputMode.NTSC ? 60u : 50u);
            gameWnd.Closed += (s, e) => gameWnd.Close();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (gameWnd.IsOpen)
            {
                gameWnd.SetView(camera);
                // Transfer control to the CPU until there is a VBlank interrupt
                while (!PPU.NewFrame)
                {
                    long old = CPU.TotalCycles;
                    for (int i = 0; 3 > i; i++)
                        PPU.Tick();
                    CPU.Step();
                    cycles += (CPU.TotalCycles - old);
                    CurrentGame.Controller.Tick();
                    if (CPU.TotalCycles % 1790 == 0)
                        Thread.Sleep(1);
                }
                // Update screen
                if (sw.ElapsedMilliseconds >= 1000)
                {
                    cps = cycles;
                    cycles = 0;
                    sw.Restart();
                }
                PPU.NewFrame = false;
                byte[] bytes = new byte[PPU.TextureData.Length * 4];
                Buffer.BlockCopy(PPU.TextureData, 0, bytes, 0, PPU.TextureData.Length * 4);
                screenTexture.Update(bytes, 256, 240, 0, 0);
                gameWnd.DispatchEvents();
                gameWnd.Clear(Color.Black);
                gameWnd.Draw(new Sprite(screenTexture) { Position = new Vector2f(-128, -120) });
                gameWnd.Display();
            }
        }
    }
}
