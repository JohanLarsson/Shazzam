namespace Shazzam.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Media.Animation;

    public class Reveal : System.Windows.Controls.Decorator
    {
        // Using a DependencyProperty as the backing store for IsExpanded.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register(
            nameof(IsExpanded),
            typeof(bool),
            typeof(Reveal),
            new UIPropertyMetadata(false, OnIsExpandedChanged));

        // Using a DependencyProperty as the backing store for Duration.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DurationProperty = DependencyProperty.Register(
            nameof(Duration),
            typeof(double),
            typeof(Reveal),
            new UIPropertyMetadata(250.0));

        // Using a DependencyProperty as the backing store for HorizontalReveal.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HorizontalRevealProperty = DependencyProperty.Register(
            nameof(HorizontalReveal),
            typeof(HorizontalRevealMode),
            typeof(Reveal),
            new UIPropertyMetadata(HorizontalRevealMode.None));

        // Using a DependencyProperty as the backing store for VerticalReveal.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VerticalRevealProperty = DependencyProperty.Register(
            nameof(VerticalReveal),
            typeof(VerticalRevealMode),
            typeof(Reveal),
            new UIPropertyMetadata(VerticalRevealMode.FromTopToBottom));

        // Using a DependencyProperty as the backing store for AnimationProgress.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AnimationProgressProperty = DependencyProperty.Register(
            nameof(AnimationProgress),
            typeof(double),
            typeof(Reveal),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure, null, CoerceAnimationProgress));

        static Reveal()
        {
            ClipToBoundsProperty.OverrideMetadata(typeof(Reveal), new FrameworkPropertyMetadata(true));
        }

        /// <summary>
        ///     Whether the child is expanded or not.
        ///     Note that an animation may be in progress when the value changes.
        ///     This is not meant to be used with AnimationProgress and can overwrite any
        ///     animation or values in that property.
        /// </summary>
        public bool IsExpanded
        {
            get => (bool)this.GetValue(IsExpandedProperty);
            set => this.SetValue(IsExpandedProperty, value);
        }

        /// <summary>
        ///     The duration in milliseconds of the reveal animation.
        ///     Will apply to the next animation that occurs (not to currently running animations).
        /// </summary>
        public double Duration
        {
            get => (double)this.GetValue(DurationProperty);
            set => this.SetValue(DurationProperty, value);
        }

        public HorizontalRevealMode HorizontalReveal
        {
            get => (HorizontalRevealMode)this.GetValue(HorizontalRevealProperty);
            set => this.SetValue(HorizontalRevealProperty, value);
        }

        public VerticalRevealMode VerticalReveal
        {
            get => (VerticalRevealMode)this.GetValue(VerticalRevealProperty);
            set => this.SetValue(VerticalRevealProperty, value);
        }

        /// <summary>
        ///     Value between 0 and 1 (inclusive) to move the reveal along.
        ///     This is not meant to be used with IsExpanded.
        /// </summary>
        public double AnimationProgress
        {
            get => (double)this.GetValue(AnimationProgressProperty);
            set => this.SetValue(AnimationProgressProperty, value);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            var child = this.Child;
            if (child != null)
            {
                child.Measure(constraint);

                var percent = this.AnimationProgress;
                var width = CalculateWidth(child.DesiredSize.Width, percent, this.HorizontalReveal);
                var height = CalculateHeight(child.DesiredSize.Height, percent, this.VerticalReveal);
                return new Size(width, height);
            }

            return default;
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            var child = this.Child;
            if (child != null)
            {
                var percent = this.AnimationProgress;
                var horizontalReveal = this.HorizontalReveal;
                var verticalReveal = this.VerticalReveal;

                var childWidth = child.DesiredSize.Width;
                var childHeight = child.DesiredSize.Height;
                var x = CalculateLeft(childWidth, percent, horizontalReveal);
                var y = CalculateTop(childHeight, percent, verticalReveal);

                child.Arrange(new Rect(x, y, childWidth, childHeight));

                childWidth = child.RenderSize.Width;
                childHeight = child.RenderSize.Height;
                var width = CalculateWidth(childWidth, percent, horizontalReveal);
                var height = CalculateHeight(childHeight, percent, verticalReveal);
                return new Size(width, height);
            }

            return default;
        }

        private static object CoerceAnimationProgress(DependencyObject d, object baseValue)
        {
            var num = (double)baseValue;
            if (num < 0.0)
            {
                return 0.0;
            }
            else if (num > 1.0)
            {
                return 1.0;
            }

            return baseValue;
        }

        private static void OnIsExpandedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ((Reveal)sender).SetupAnimation((bool)e.NewValue);
        }

        private static double CalculateLeft(double width, double percent, HorizontalRevealMode reveal)
        {
            if (reveal == HorizontalRevealMode.FromRightToLeft)
            {
                return (percent - 1.0) * width;
            }
            else if (reveal == HorizontalRevealMode.FromCenterToEdge)
            {
                return (percent - 1.0) * width * 0.5;
            }
            else
            {
                return 0.0;
            }
        }

        private static double CalculateTop(double height, double percent, VerticalRevealMode reveal)
        {
            if (reveal == VerticalRevealMode.FromBottomToTop)
            {
                return (percent - 1.0) * height;
            }
            else if (reveal == VerticalRevealMode.FromCenterToEdge)
            {
                return (percent - 1.0) * height * 0.5;
            }
            else
            {
                return 0.0;
            }
        }

        private static double CalculateWidth(double originalWidth, double percent, HorizontalRevealMode reveal)
        {
            if (reveal == HorizontalRevealMode.None)
            {
                return originalWidth;
            }
            else
            {
                return originalWidth * percent;
            }
        }

        private static double CalculateHeight(double originalHeight, double percent, VerticalRevealMode reveal)
        {
            if (reveal == VerticalRevealMode.None)
            {
                return originalHeight;
            }
            else
            {
                return originalHeight * percent;
            }
        }

        private void SetupAnimation(bool isExpanded)
        {
            // Adjust the time if the animation is already in progress
            var currentProgress = this.AnimationProgress;
            if (isExpanded)
            {
                currentProgress = 1.0 - currentProgress;
            }

            var animation = new DoubleAnimation
                                {
                                    To = isExpanded
                                             ? 1.0
                                             : 0.0,
                                    Duration = TimeSpan.FromMilliseconds(this.Duration * currentProgress),
                                    FillBehavior = FillBehavior.HoldEnd,
                                };

            this.BeginAnimation(AnimationProgressProperty, animation);
        }
    }
}
