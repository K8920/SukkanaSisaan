using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SukkanaSisaan
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Game_map_01 : Page
    {
        // monster
        private Monster monster;

        // player
        private Player player;
        private Rock rock;
        private bool CollisionHappen = false;

        // canvas width and height
        private double CanvasWidth;
        private double CanvasHeight;

        private bool UpPressed;
        private bool DownPressed;
        private bool LeftPressed;
        private bool RightPressed;

        private DispatcherTimer timer;

        public Game_map_01()
        {
            this.InitializeComponent();
            CanvasWidth = GameCanvas.Width;
            CanvasHeight = GameCanvas.Height;

            // monster location
            monster = new Monster
            {
                LocationX = 300,
                LocationY = 400
            };
           
            // add monster to the canvas
            GameCanvas.Children.Add(monster);

            // player location
            player = new Player
            {
                LocationX = GameCanvas.Width / 2,
                LocationY = GameCanvas.Height / 2
             };

            // solid object location
            rock = new Rock
            {
                LocationX = GameCanvas.Width / 3,
                LocationY = GameCanvas.Height / 3
            };
           
          

            // add player to the canvas
            GameCanvas.Children.Add(player);

            // add test solid dwayne the rock johnson
            GameCanvas.Children.Add(rock);

            // key listeners
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            Window.Current.CoreWindow.KeyUp += CoreWindow_KeyUp;

            // game loop
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1000 / 60);
            timer.Tick += Timer_Tick;
            timer.Start();

            // update player position
            player.UpdateLocation();
            rock.UpdateLocation();
            monster.UpdateMonster();
        }

        private void Timer_Tick(object sender, object e)
        {
            // moving
            CheckCollision();
            player.UpdateLocation();

            if (UpPressed)
            {
                if (CollisionHappen == false)
                    player.MoveUp();
                else if (CollisionHappen == true)
                    player.LocationY = player.LocationY + 20;
            }
            if (DownPressed)
            {
                if (CollisionHappen == false)
                    player.MoveDown();
                else if (CollisionHappen == true)
                    player.LocationY = player.LocationY - 20;
            }
            if (LeftPressed)
            {
                if (CollisionHappen == false)
                    player.MoveLeft();
                else if (CollisionHappen == true)
                    player.LocationX = player.LocationX + 20;
            }
            if (RightPressed)
            {
                if (CollisionHappen == false)
                    player.MoveRight();
                else if (CollisionHappen == true)
                    player.LocationX = player.LocationX - 20;
            }

            monster.MovePattern1();
            player.UpdateLocation();
            monster.UpdateMonster();
           
            /* if (CollisionHappen == true)
             {
                 UpPressed = false;
                 DownPressed = false;
                 LeftPressed = false;
                 RightPressed = false;


            // POISTA
             if (UpPressed) player.MoveUp();
                 if (DownPressed) player.MoveDown();
                 if (LeftPressed) player.MoveLeft();
                 if (RightPressed) player.MoveRight();
             }
             */


        }
        private void CheckCollision()
        {
            // player
            Rect r1 = new Rect(player.LocationX, player.LocationY, player.ActualHeight, player.ActualWidth);
            // rock
            Rect r2 = new Rect(rock.LocationX, rock.LocationY, rock.ActualHeight, rock.ActualWidth);
            r1.Intersect(r2);
            if (!r1.IsEmpty)
            {
                CollisionHappen = true;
            }
            else if (r1.IsEmpty)
            {
                CollisionHappen = false;
            }
        }
        private void CoreWindow_KeyUp(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case VirtualKey.Up:
                    UpPressed = false;
                    break;
                case VirtualKey.Down:
                    DownPressed = false;
                    break;
                case VirtualKey.Left:
                    LeftPressed = false;
                    break;
                case VirtualKey.Right:
                    RightPressed = false;
                    break;
            }
        }

        // lul lul
        private void CoreWindow_KeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
           
                switch (args.VirtualKey)
                {
                    case VirtualKey.Up:
                        UpPressed = true;
                        break;
                    case VirtualKey.Down:
                        DownPressed = true;
                        break;
                    case VirtualKey.Left:
                        LeftPressed = true;
                        break;
                    case VirtualKey.Right:
                        RightPressed = true;
                        break;
                }
            
        }
    }
}
