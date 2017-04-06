using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace SukkanaSisaan
{
    public sealed partial class Monster : UserControl
    {
        // Monster current health
        public int health { get; set; }
        // Monster attack
        public int attack { get; set; }
        // Monster maxhealth
        private int healthMax;
        // Monster speed
        public double speed = 10;
        // Monster locations
        public double LocationX;
        public double LocationY;
        public int move = 0;
        Random random = new Random();
        int randomnumber;
        public Monster()
        {
            this.InitializeComponent();
        }
        // COLLISION
        // MOVEMENT
      //  public void MovePattern1()
      //  {
      //     if (move == 0)
      //     {
      //         LocationX = LocationX + speed;
      //         if (LocationX > 400)
      //         move = 1;
      //     }
      // 
      //         if (move == 1)
      //     {
      //         LocationX = LocationX - speed;
      //         if (LocationX < 100)
      //         move = 0;
      //     }
      //  }
        // GENERATE RANDOM NUMBER
        public void GenerateNumber()
        {
           randomnumber = random.Next(1, 5);
        }

        public void GenerateNumber2()
        {
            randomnumber = random.Next(1, 5);
        }
        // RANDOM MOVE PATTERN FOR MONSTER
        public void MovePattern2()
        {
            if (LocationY > 0)
            {
                if (randomnumber == 1)  // UP
                {
                    LocationY = LocationY - speed;
                }
            }
            if (LocationX < 1280 - 66)
            {
                if (randomnumber == 2)  // RIGHT
                {
                    LocationX = LocationX + speed;
                }
            }
            if (LocationY < 960 - 66)
            {
                if (randomnumber == 3)  // DOWN
                {
                    LocationY = LocationY + speed;
                }
            }
            if (LocationX > 0)
            {
                if (randomnumber == 4)  // LEFT
                {
                    LocationX = LocationX - speed;
                }
            }
        }
        // DETECT
        // CHASE
        // UPDATE MONSTER LOCATION
        public void UpdateMonster()
        {
            SetValue(Canvas.LeftProperty, LocationX);
            SetValue(Canvas.TopProperty, LocationY);
        }
    }
}
