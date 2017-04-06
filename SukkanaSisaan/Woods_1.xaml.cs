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
    public sealed partial class Woods_1 : Page
    {
        public double LeftLocationX { get; set; }
        public double LeftLocationY { get; set; }
        public double RightLocationX { get; set; }
        public double RightLocationY { get; set; }
        public double TopLocationX { get; set; }
        public double TopLocationY { get; set; }
        public double BottomLocationX { get; set; }
        public double BottomLocationY { get; set; }
        public Woods_1()
        {
            this.InitializeComponent();
        }
        public void UpdateLocation()
        {
            SetValue(Canvas.LeftProperty, LeftLocationX);
            SetValue(Canvas.TopProperty, LeftLocationY);

            SetValue(Canvas.LeftProperty, RightLocationX);
            SetValue(Canvas.TopProperty, RightLocationY);

            SetValue(Canvas.LeftProperty, TopLocationX);
            SetValue(Canvas.TopProperty, TopLocationY);

            SetValue(Canvas.LeftProperty, BottomLocationX);
            SetValue(Canvas.TopProperty, BottomLocationY);
        }
    }
}
