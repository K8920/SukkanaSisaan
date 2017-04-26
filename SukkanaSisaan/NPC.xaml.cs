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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SukkanaSisaan
{
    public sealed partial class NPC : UserControl
    {
        public double LocationX;
        public double LocationY;
        public NPC()
        {
            this.InitializeComponent();
        }

        // Collision
        // Dialogue
        public void Dialogue()
        {
            NpcTextBlock.Text = "Hello stranger";
        }
        public void EmptyDialogue()
        {
            NpcTextBlock.Text = "";
        }
        public void UpdateNPC()
        {
            SetValue(Canvas.LeftProperty, LocationX);
            SetValue(Canvas.TopProperty, LocationY);
        }
    }
}
