using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
        private NPC npc1;
        // player
        private Player player;
        private List<Heart> hearts;
        public List<Monster> monsters = new List<Monster>();
        private Hits hits, hits2, hits3;
        private Projectile projectile;
        private Rock rock;

        // canvas width and height
        private double CanvasWidth;
        private double CanvasHeight;
        public double PosX;
        public double PosY;

        private bool UpPressed;
        private bool DownPressed;
        private bool LeftPressed;
        private bool RightPressed;
        private bool ZPressed;
        private bool XPressed;
        private bool ProjectileActive = false;

        private bool UpHit = false;
        private bool DnHit = false;
        private bool LeHit = false;
        private bool RiHit = false;

        private DispatcherTimer timer;
        private DispatcherTimer attTimer;
        private DispatcherTimer invTimer;
        private DispatcherTimer monstertimer1;
        private DispatcherTimer monstertimer2;
        private DispatcherTimer randnumtimer;
        private DispatcherTimer randnumtimer2;

        private MediaElement mediaElement;
        private MediaElement mediaElement_2;


        public Game_map_01()
        {
            this.InitializeComponent();
            CanvasWidth = GameCanvas.Width;
            CanvasHeight = GameCanvas.Height;
            InitAudio();
            hearts = new List<Heart>();

            int heartsCount = 3;
            int xStartPos = 0;
            int yStartPos = 30;
            int step = 10;
            for (int i = 0; i < heartsCount; i++)
            {
                int x = (50 + i * 71 + step) + xStartPos;
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

            // List of monsters
            monsters.Add(new Monster() { LocationX = 100, LocationY = 400 });
            monsters.Add(new Monster() { LocationX = 200, LocationY = 50 });
            monsters.Add(new Monster() { LocationX = 300, LocationY = 0 });
            monsters.Add(new Monster() { LocationX = 12000, LocationY = 600 });
            monsters.Add(new Monster() { LocationX = 500, LocationY = 150 });
            monsters.Add(new Monster() { LocationX = 1000, LocationY = 235 });
            monsters.Add(new Monster() { LocationX = 700, LocationY = 120 });

            foreach (Monster monster in monsters)
            {
                GameCanvas.Children.Add(monster);
            }
       
           // Monster monster = monsters.ElementAt(0);
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
                LocationX = 200,
                LocationY = 200
            };

            // npc location
            npc1 = new NPC
            {
                LocationX = 1000,
                LocationY = 700
            };
            npc1.UpdateNPC();
            GameCanvas.Children.Add(npc1);

            GameCanvas.Children.Add(rock);
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
            attTimer.Interval = new TimeSpan(0, 0, 0, 0, 250);
            attTimer.Tick += attTimer_Tick;

            //invulnerability timer
            invTimer = new DispatcherTimer();
            invTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            invTimer.Tick += invTimer_Tick;

            // randnum timer
            randnumtimer = new DispatcherTimer();
            randnumtimer.Interval = new TimeSpan(0, 0, 0, 0, 1000 / 3);
            randnumtimer.Tick += randnumtimer_Tick;
            randnumtimer.Start();

            // monster timer
            monstertimer1 = new DispatcherTimer();
            monstertimer1.Interval = new TimeSpan(0, 0, 0, 0, 1000 / 60);
            monstertimer1.Tick += monstertimer1_Tick;
            monstertimer1.Start();

            // update position
            player.UpdatePlayer();
            rock.UpdateLocation();
           /* hearts = new List<Heart>();

            int heartsCount = 3;
            int xStartPos = 70;
            int yStartPos = 30;
            int step = 10;
            for (int i = 0; i < heartsCount; i++)
            {
                int x = (50 + i * 71 + step) + xStartPos;
                int y = (20 + step) + yStartPos;
                Heart heart = new Heart
                {
                    LocationX = x,
                    LocationY = y
                };
                hearts.Add(heart);
                GameCanvas.Children.Add(heart);
                heart.SetLocation();
            }*/
        }
        // random number generated for monster movement

        private void randnumtimer_Tick(object sender, object e)
        {
            foreach (Monster monster in monsters)
            {
                monster.GenerateNumber();
            }
        }

        // monster random movement
        private void monstertimer1_Tick(object sender, object e)
        {
            foreach (Monster monster in monsters)
            {
                monster.MovePattern2();
            }
        }

        private void attTimer_Tick(object sender, object e)
        {
            GameCanvas.Children.Remove(projectile);
            attTimer.Stop();
            ProjectileActive = false;
        }

        private void invTimer_Tick(object sender, object e)
        {
            invTimer.Stop();
            player.Invulnerable = false;
        }

        private void Timer_Tick(object sender, object e)
        {
            // moving
            CheckCollision();
            player.UpdatePlayer();

            if (UpPressed && ProjectileActive == false)
            {
                player.PlayerFacing = 0;
                player.MoveUp();
            
            }
            if (DownPressed && ProjectileActive == false)
            {
                player.PlayerFacing = 2;
                player.MoveDown();

            }
            if (LeftPressed && ProjectileActive == false)
            {
                player.PlayerFacing = 3;
                player.MoveLeft();
 
            }
            if (RightPressed && ProjectileActive == false)
            {
                player.PlayerFacing = 1;
                player.MoveRight();
 
            }
            // Z KEY
            if (ZPressed)
            {
                if (ProjectileActive == false)
                {
                    if (player.PlayerFacing == 0)
                    {
                        projectile = new Projectile
                        {
                            LocationX = player.LocationX-25,
                            LocationY = player.LocationY - player.Height - 25
                        };
                        projectile.Rotate(270);
                        ProjectileActive = true;
                    }

                    else if (player.PlayerFacing == 1)
                    {
                        projectile = new Projectile
                        {
                            LocationX = player.LocationX + player.Width,
                            LocationY = player.LocationY
                        };
                        projectile.Rotate(0);
                        ProjectileActive = true;
                    }

                    else if (player.PlayerFacing == 2)
                    {
                        projectile = new Projectile
                        {
                            LocationX = player.LocationX - 25,
                            LocationY = player.LocationY + player.Height + 25
                        };
                        projectile.Rotate(90);
                        ProjectileActive = true;
                    }

                    else if (player.PlayerFacing == 3)
                    {
                        projectile = new Projectile
                        {
                            LocationX = player.LocationX - player.Width*2,
                            LocationY = player.LocationY
                        };
                        projectile.Rotate(180);
                        ProjectileActive = true;
                    }
                    GameCanvas.Children.Add(projectile);
                    attTimer.Start();
                }
            }
            
        player.UpdatePlayer();
            foreach (Monster monster in monsters)
            {
                monster.UpdateMonster();
            }
            if (ProjectileActive) projectile.UpdateProjectile();

            // NPC dialogue
            if (player.LocationX >= npc1.LocationX + 5 && player.LocationY >= npc1.LocationY + 5 || player.LocationX >= npc1.LocationX - 5 && player.LocationY >= npc1.LocationY - 5)
            {
                npc1.Dialogue();
            }
            else
            {
                npc1.EmptyDialogue();

            }

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

        private async void InitAudio()
        {
            // audios
            mediaElement = new MediaElement();
            StorageFolder folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
            StorageFile file = await folder.GetFileAsync("grass.mp3");
            var stream = await file.OpenAsync(FileAccessMode.Read);
            mediaElement.AutoPlay = true;
            mediaElement.SetSource(stream, file.ContentType);
        }

        private void CheckCollision()
        {
            // player
          Rect r1 = player.GetRect();
        //  Rect rMon1 = new Rect(monster.LocationX, monster.LocationY, monster.Width, monster.Height);
          r1.Intersect(rock.GetRect());
          // rock
         //Rect r2 = new Rect(rock.LocationX, rock.LocationY, rock.ActualHeight, rock.ActualWidth);
         // Rect woodsleft_1 = new Rect(woods_1.LocationX, woods_1.LocationY, woods_1.ActualHeight, woods_1.ActualWidth);
          if (!r1.IsEmpty && UpPressed == true && DnHit == false && LeHit == false && RiHit == false)
             //Rect woodsleft_1 = new Rect
             //Rect woodsleft_1 = new Rect
          //r1.Intersect(r2);
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

            Rect r2 = player.GetRect();
            if (r2.IsEmpty)
            {
                if (player.Invulnerable == false)
                {
                    player.DamagePlayer();
                    invTimer.Start();
                    player.Invulnerable = true;
                    if (hearts.Count >= 1)
                    {
                        hearts.RemoveAt(hearts.Count - 1);
                        GameCanvas.Children.RemoveAt(hearts.Count);
                    };
                    if (player.health == 0)

                    {
                        mediaElement.Stop();
                        Frame.Navigate(typeof(MainPage));
                    }
                }
            }

            Rect r3 = player.GetRect();
            if (r3.IsEmpty)
            {
                if (player.Invulnerable == false)
                {
                    player.DamagePlayer();
                    invTimer.Start();
                    player.Invulnerable = true;
                    if (hearts.Count >= 1)
                    {
                        hearts.RemoveAt(hearts.Count - 1);
                        GameCanvas.Children.RemoveAt(hearts.Count);
                    };
                    if (player.health == 0)

                    {
                        mediaElement.Stop();
                        Frame.Navigate(typeof(MainPage));
                    }
                }
            }
            if(ProjectileActive == true)
            {
            Rect rSword1 = projectile.GetRect();
                foreach (Monster monster in monsters)
                {
                    Rect skull = new Rect(monster.LocationX, monster.LocationY, monster.Width, monster.Height);
                    rSword1.Intersect(skull);
                    if (!rSword1.IsEmpty)
                    {
                        monsters.Remove(monster);
                        GameCanvas.Children.Remove(monster);
                    }
                }
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
                case VirtualKey.X:
                    XPressed = false;
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
                case VirtualKey.X:
                    if (XPressed == false)
                    {
                        player.DamagePlayer();
                        if (hearts.Count >= 1)
                        {
                            hearts.RemoveAt(hearts.Count - 1);
                            GameCanvas.Children.RemoveAt(hearts.Count);
                        };
                        if (player.health == 0)

                        {
                            Frame.Navigate(typeof(MainPage));
                        }
                    }
                    XPressed = true;
                    break;
            }
        }
    }
}
