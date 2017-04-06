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
        private List<Heart> hearts;
        private Hits hits, hits2, hits3;
        private Projectile projectile;
        private Rock rock;
        private Woods_1 woods_1;
        private bool CollisionHappen = false;

        // canvas width and height
        private double CanvasWidth;
        private double CanvasHeight;

        private bool UpPressed;
        private bool DownPressed;
        private bool LeftPressed;
        private bool RightPressed;
        private bool ZPressed;
        private bool ProjectileActive = false;

        private DispatcherTimer timer;
        private DispatcherTimer attTimer;

        public Game_map_01()
        {
            this.InitializeComponent();
            CanvasWidth = GameCanvas.Width;
            CanvasHeight = GameCanvas.Height;

            hearts = new List<Heart>();

            int heartsCount = 3;
            int xStartPos = 70;
            int yStartPos = 30;
            int step = 10;
            for (int i = 0; i < heartsCount; i++)
            {
                int x = (50 + i * 30 + step) + xStartPos;
                int y = (20 + step) + yStartPos;
                Heart heart = new Heart
                {
                    LocationX = x,
                    LocationY = y
                };
                hearts.Add(heart);
                GameCanvas.Children.Add(heart);
                heart.SetLocation();
            }

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
            player.UpdatePlayer();

            // solid object location
            rock = new Rock
            {
                LocationX = 100,
                LocationY = 100
            };
            GameCanvas.Children.Add(rock);

            woods_1 = new Woods_1
            {
                LeftLocationX = 0,
                LeftLocationY = 0,
                TopLocationX = 0,
                TopLocationY = 0,
                BottomLocationX = 0,
                BottomLocationY = 0,
                RightLocationX = 0,
                RightLocationY = 0
            };
            GameCanvas.Children.Add(woods_1);
            //GameCanvas.Children.Add(woods_1);
            // add player to the canvas
            GameCanvas.Children.Add(player);
            

            // key listeners
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            Window.Current.CoreWindow.KeyUp += CoreWindow_KeyUp;

            // game loop
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1000 / 60);
            timer.Tick += Timer_Tick;
            timer.Start();

            // attack timer
            attTimer = new DispatcherTimer();
            attTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            attTimer.Tick += attTimer_Tick;

            // update player position
            player.UpdatePlayer();
            rock.UpdateLocation();
            woods_1.UpdateLocation();
            monster.UpdateMonster();
        }

        private void attTimer_Tick(object sender, object e)
        {
            GameCanvas.Children.Remove(projectile);
            attTimer.Stop();
            ProjectileActive = false;
        }

        private void Timer_Tick(object sender, object e)
        {
            // moving
            CheckCollision();
            player.UpdatePlayer();

            if (UpPressed && ProjectileActive == false)
            {
                player.PlayerFacing = 0;
                if (CollisionHappen == false)
                    player.MoveUp();
                else if (CollisionHappen == true)
                    player.LocationY = player.LocationY + 20;
            }
            if (DownPressed && ProjectileActive == false)
            {
                player.PlayerFacing = 2;
                if (CollisionHappen == false)
                    player.MoveDown();
                else if (CollisionHappen == true)
                    player.LocationY = player.LocationY - 20;
            }
            if (LeftPressed && ProjectileActive == false)
            {
                player.PlayerFacing = 3;
                if (CollisionHappen == false)
                    player.MoveLeft();
                else if (CollisionHappen == true)
                    player.LocationX = player.LocationX + 20;
            }
            if (RightPressed && ProjectileActive == false)
            {
                player.PlayerFacing = 1;
                if (CollisionHappen == false)
                    player.MoveRight();
                else if (CollisionHappen == true)
                    player.LocationX = player.LocationX - 20;
            }
            // Z KEY
            if (ZPressed)
            {
                if (ProjectileActive == false)
                {
                    player.health--;
                    if (player.PlayerFacing == 0)
                    {
                        projectile = new Projectile
                        {
                            LocationX = player.LocationX,
                            LocationY = player.LocationY - player.Height
                        };
                        ProjectileActive = true;
                    }

                    else if (player.PlayerFacing == 1)
                    {
                        projectile = new Projectile
                        {
                            LocationX = player.LocationX + player.Width,
                            LocationY = player.LocationY
                        };
                        ProjectileActive = true;
                    }

                    else if (player.PlayerFacing == 2)
                    {
                        projectile = new Projectile
                        {
                            LocationX = player.LocationX,
                            LocationY = player.LocationY + player.Height
                        };
                        ProjectileActive = true;
                    }

                    else if (player.PlayerFacing == 3)
                    {
                        projectile = new Projectile
                        {
                            LocationX = player.LocationX - player.Width,
                            LocationY = player.LocationY
                        };
                        ProjectileActive = true;
                    }
                    GameCanvas.Children.Add(projectile);
                    attTimer.Start();
                }
            }
            monster.MovePattern1();
            player.UpdatePlayer();
            monster.UpdateMonster();
            if (ProjectileActive) projectile.UpdateProjectile();

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
            Rect r1 = player.GetRect();
            r1.Intersect(rock.GetRect());
            // rock
           //Rect r2 = new Rect(rock.LocationX, rock.LocationY, rock.ActualHeight, rock.ActualWidth);
           // Rect woodsleft_1 = new Rect(woods_1.LocationX, woods_1.LocationY, woods_1.ActualHeight, woods_1.ActualWidth);
               //Rect woodsleft_1 = new Rect
               //Rect woodsleft_1 = new Rect
               //Rect woodsleft_1 = new Rect
            //r1.Intersect(r2);
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
                case VirtualKey.Z:
                    ZPressed = false;
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
                    case VirtualKey.Z:
                        ZPressed = true;
                        break;
            }
            
        }
    }
}
