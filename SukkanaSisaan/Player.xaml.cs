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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SukkanaSisaan
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Player : Page
    {
        // offset
        // private int currentFrame = 0
        // private int frameheight = X

        // private DispatcherTimer timer;

        // private double 
        private double speed = 10;

        public double LocationX { get; set; }
        public double LocationY { get; set; }
        public double getposX;
        public double getposY;

        public Player()
        {
            this.InitializeComponent();
        }

        // MOVEMENTS
        public void MoveUp()
        {
            if (LocationY > 0)
            {
                LocationY = LocationY - speed;
            }
        }

        public void MoveDown()
        {
            if (LocationY < 960 - 50)
            {
                LocationY = LocationY + speed;
            }
        }

        public void MoveLeft()
        {
            if (LocationX > 0)
            {
                LocationX = LocationX - speed;
            }
        }

        public void MoveRight()
        {
            if (LocationX < 1280 - 50)
            {
                LocationX = LocationX + speed;
            }
        }

        public void UpdatePlayer()
        {
            SetValue(Canvas.LeftProperty, LocationX);
            SetValue(Canvas.TopProperty, LocationY);
        }
        public void GetPos()
        {
            getposX = LocationX;
            getposY = LocationY;
        }
    }
}
