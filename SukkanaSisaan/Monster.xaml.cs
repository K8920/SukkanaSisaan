﻿using System;
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
        private double speed = 10;
        // Monster locations
        public double LocationX;
        public double LocationY;
        public int move = 0;
        public Monster()
        {
            this.InitializeComponent();
        }
        // COLLISION
        // MOVEMENT
        // test movement
        public void MovePattern1()
        {
            if (move == 0)
               {
                LocationX = LocationX + speed;
                if (LocationX > 400)
                    move = 1;
               }

            if (move == 1)
            {
                LocationX = LocationX - speed;
                if (LocationX < 100)
                    move = 0;
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
