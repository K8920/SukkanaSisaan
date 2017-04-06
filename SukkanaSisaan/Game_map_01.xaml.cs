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
        private Monster monster2;
        // player
        private Player player;
        private Projectile projectile;
        private Rock rock;

        // canvas width and height
        private double CanvasWidth;
        private double CanvasHeight;

        private bool UpPressed;
        private bool DownPressed;
        private bool LeftPressed;
        private bool RightPressed;
        private bool ZPressed;
        private bool ProjectileActive = false;

        private bool UpHit = false;
        private bool DnHit = false;
        private bool LeHit = false;
        private bool RiHit = false;

        private DispatcherTimer timer;
        private DispatcherTimer attTimer;
        private DispatcherTimer monstertimer1;
        private DispatcherTimer monstertimer2;
        private DispatcherTimer randnumtimer;
        private DispatcherTimer randnumtimer2;

        public Game_map_01()
        {
            this.InitializeComponent();
            CanvasWidth = GameCanvas.Width;
            CanvasHeight = GameCanvas.Height;

            // monster starting location
            monster = new Monster
            {
                LocationX = 300,
                LocationY = 400
            };

            monster2 = new Monster
            {
                LocationX = 1000,
                LocationY = 400
            };
           
            // add monster to the canvas
            GameCanvas.Children.Add(monster);
            GameCanvas.Children.Add(monster2);

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

            // attack timer
            attTimer = new DispatcherTimer();
            attTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            attTimer.Tick += attTimer_Tick;

            // randnum timer
            randnumtimer = new DispatcherTimer();
            randnumtimer.Interval = new TimeSpan(0, 0, 0, 0, 1000 / 3);
            randnumtimer.Tick += randnumtimer_Tick;
            randnumtimer.Start();

            //timer 2
            // randnum timer
            randnumtimer2 = new DispatcherTimer();
            randnumtimer2.Interval = new TimeSpan(0, 0, 0, 0, 1000 / 4);
            randnumtimer2.Tick += randnumtimer2_Tick;
            randnumtimer2.Start();

            // monster timer
            monstertimer1 = new DispatcherTimer();
            monstertimer1.Interval = new TimeSpan(0, 0, 0, 0, 1000 / 60);
            monstertimer1.Tick += monstertimer1_Tick;
            monstertimer1.Start();

            // monster timer 2
            monstertimer2 = new DispatcherTimer();
            monstertimer2.Interval = new TimeSpan(0, 0, 0, 0, 1000 / 60);
            monstertimer2.Tick += monstertimer2_Tick;
            monstertimer2.Start();

            // update position
            player.UpdatePlayer();
            rock.UpdateLocation();
            monster.UpdateMonster();
            monster2.UpdateMonster();
        }
        // random number generated for monster movement
        private void randnumtimer_Tick(object sender, object e)
        {
            monster.GenerateNumber();
        }
        private void randnumtimer2_Tick(object sender, object e)
        {
            monster2.GenerateNumber2();
            
        }
        // monster random movement
        private void monstertimer1_Tick(object sender, object e)
        {
            monster.MovePattern2();
        }
        private void monstertimer2_Tick(object sender, object e)
        {
            monster2.MovePattern2();
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

            if (UpPressed && DownPressed == false)
            {
          
                player.MoveUp();
            
            }
            if (DownPressed && UpPressed == false)
            {
             
                player.MoveDown();

            }
            if (LeftPressed && RightPressed == false)
            {
              
                player.MoveLeft();
 
            }
            if (RightPressed && LeftPressed == false)
            {
               
                player.MoveRight();
 
            }
            // Z KEY
            if (ZPressed)
            {
                if (ProjectileActive == false)
                {
                    projectile = new Projectile
                    {
                        LocationX = player.LocationX,
                        LocationY = player.LocationY - 30
                    };
                    ProjectileActive = true;
                    GameCanvas.Children.Add(projectile);
                    attTimer.Start();
                }
            }
            player.UpdatePlayer();
            monster.UpdateMonster();
            monster2.UpdateMonster();
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
            Rect r1 = new Rect(player.LocationX, player.LocationY, player.ActualHeight, player.ActualWidth);
            // rock
            Rect r2 = new Rect(rock.LocationX, rock.LocationY, rock.ActualHeight, rock.ActualWidth);
            r1.Intersect(r2);
            if (!r1.IsEmpty && UpPressed == true && DnHit == false && LeHit == false && RiHit == false)
            {
                UpPressed = false;
                UpHit = true;
            }
            if (!r1.IsEmpty && DownPressed == true && UpHit == false && LeHit == false && RiHit == false)
            {
                DownPressed = false;
                DnHit = true;
            }
            if (!r1.IsEmpty && LeftPressed == true && UpHit == false && DnHit == false && RiHit == false)
            {
                LeftPressed = false;
                LeHit = true;
             
            }
            if (!r1.IsEmpty && RightPressed == true && UpHit == false && DnHit == false && LeHit == false)
            {
                RightPressed = false;
                RiHit = true;
              
            }
                    
            if (r1.IsEmpty)
            {
                UpHit = false;
                DnHit = false;
                LeHit = false;
                RiHit = false;
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
