using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Zooming.Scaling
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnAnimate_Click(object sender, RoutedEventArgs e)
        {
            // We may have already set the LayoutTransform to a ScaleTransform.
            // If not, do so now.


            if (!(mainPanel.LayoutTransform is ScaleTransform scaler))
            {
                scaler = new ScaleTransform(1.0, 1.0);
                mainPanel.LayoutTransform = scaler;
            }

            // We'll need a DoubleAnimation object to drive 
            // the ScaleX and ScaleY properties.

            DoubleAnimation animator = new DoubleAnimation()
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(600)),
            };

            // Toggle the scale between 1.0 and 1.5.

            if (scaler.ScaleX == 1.0)
            {
                animator.To = 1.5;
            }
            else
            {
                animator.To = 1.0;
            }

            scaler.BeginAnimation(ScaleTransform.ScaleXProperty, animator);
            scaler.BeginAnimation(ScaleTransform.ScaleYProperty, animator);
        }

        private void Slider1_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            // The user is clicking on the slider, probably about to drag it.

            if (mainPanel.LayoutTransform is ScaleTransform scaler && scaler.HasAnimatedProperties)
            {
                // This means the current ScaleX and ScaleY properties were set via
                // animation, which has a higher value precedence than a locally set
                // value, so we need to remove the animation by setting a null 
                // AnimationTimeline before we can set a local value when the user
                // drags the slider (in slider1_ValueChanged).

                scaler.ScaleX = scaler.ScaleX;
                scaler.ScaleY = scaler.ScaleY;

                // Remove the animation, causing the local values (set above) to apply.

                scaler.BeginAnimation(ScaleTransform.ScaleXProperty, null);
                scaler.BeginAnimation(ScaleTransform.ScaleYProperty, null);
            }
        }

        private void Slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!(mainPanel.LayoutTransform is ScaleTransform scaler))
            {
                mainPanel.LayoutTransform = new ScaleTransform(slider1.Value, slider1.Value);
            }
            else if (scaler.HasAnimatedProperties)
            {
                // Do nothing because the value is being changed by animation.
                // Setting scaler.ScaleX will cause infinite recursion due to the
                // binding specified in the XAML.
            }
            else
            {
                scaler.ScaleX = slider1.Value;
                scaler.ScaleY = slider1.Value;
            }
        }

        private void BtnInstant_Click(object sender, RoutedEventArgs e)
        {

            if (!(mainPanel.LayoutTransform is ScaleTransform scaler))
            {
                // Currently no zoom, so go instantly to max zoom.
                mainPanel.LayoutTransform = new ScaleTransform(1.5, 1.5);
            }
            else
            {
                double curZoomFactor = scaler.ScaleX;

                // If the current ScaleX and ScaleY properties were set by animation,
                // we'll have to remove the animation before we can explicitly set
                // them to "local" values.

                if (scaler.HasAnimatedProperties)
                {
                    // Remove the animation by assigning a null 
                    // AnimationTimeline to the properties.
                    // Note that this causes them to revert to 
                    // their most recently assigned "local" values.

                    scaler.BeginAnimation(ScaleTransform.ScaleXProperty, null);
                    scaler.BeginAnimation(ScaleTransform.ScaleYProperty, null);
                }

                if (curZoomFactor == 1.0)
                {
                    scaler.ScaleX = 1.5;
                    scaler.ScaleY = 1.5;
                }
                else
                {
                    scaler.ScaleX = 1.0;
                    scaler.ScaleY = 1.0;
                }
            }
        }
    }
}
