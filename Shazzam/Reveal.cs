namespace Shazzam.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Media.Animation;

    public class Reveal : System.Windows.Controls.Decorator
    {
        // Using a DependencyProperty as the backing store for IsExpanded.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register(
            "IsExpanded",
            typeof(bool),
            typeof(Reveal),
            new UIPropertyMetadata(false, OnIsExpandedChanged));

        // Using a DependencyProperty as the backing store for Duration.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DurationProperty = DependencyProperty.Register(
            "Duration",
            typeof(double),
            typeof(Reveal),
            new UIPropertyMetadata(250.0));

        // Using a DependencyProperty as the backing store for HorizontalReveal.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HorizontalRevealProperty = DependencyProperty.Register(
            "HorizontalReveal",
            typeof(HorizontalRevealMode),
            typeof(Reveal),
            new UIPropertyMetadata(HorizontalRevealMode.None));

        // Using a DependencyProperty as the backing store for VerticalReveal.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VerticalRevealProperty = DependencyProperty.Register(
            "VerticalReveal",
            typeof(VerticalRevealMode),
            typeof(Reveal),
            new UIPropertyMetadata(VerticalRevealMode.FromTopToBottom));

        // Using a DependencyProperty as the backing store for AnimationProgress.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AnimationProgressProperty = DependencyProperty.Register(
            "AnimationProgress",
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

        private static object CoerceAnimationProgress(DependencyObject d, object baseValue)
        {
            double num = (double)baseValue;
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

        protected override Size MeasureOverride(Size constraint)
        {
            UIElement child = this.Child;
            if (child != null)
            {
                child.Measure(constraint);

                double percent = this.AnimationProgress;
                double width = CalculateWidth(child.DesiredSize.Width, percent, this.HorizontalReveal);
                double height = CalculateHeight(child.DesiredSize.Height, percent, this.VerticalReveal);
                return new Size(width, height);
            }

            return default(Size);
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            UIElement child = this.Child;
            if (child != null)
            {
                double percent = this.AnimationProgress;
                HorizontalRevealMode horizontalReveal = this.HorizontalReveal;
                VerticalRevealMode verticalReveal = this.VerticalReveal;

                double childWidth = child.DesiredSize.Width;
                double childHeight = child.DesiredSize.Height;
                double x = CalculateLeft(childWidth, percent, horizontalReveal);
                double y = CalculateTop(childHeight, percent, verticalReveal);

                child.Arrange(new Rect(x, y, childWidth, childHeight));

                childWidth = child.RenderSize.Width;
                childHeight = child.RenderSize.Height;
                double width = CalculateWidth(childWidth, percent, horizontalReveal);
                double height = CalculateHeight(childHeight, percent, verticalReveal);
                return new Size(width, height);
            }

            return default(Size);
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
            double currentProgress = this.AnimationProgress;
            if (isExpanded)
            {
                currentProgress = 1.0 - currentProgress;
            }

            DoubleAnimation animation = new DoubleAnimation();
            animation.To = isExpanded ? 1.0 : 0.0;
            animation.Duration = TimeSpan.FromMilliseconds(this.Duration * currentProgress);
            animation.FillBehavior = FillBehavior.HoldEnd;

            this.BeginAnimation(AnimationProgressProperty, animation);
        }
    }

    public enum HorizontalRevealMode
    {
        /// <summary>
        ///     No horizontal reveal animation.
        /// </summary>
        None,

        /// <summary>
        ///     Reveal from the left to the right.
        /// </summary>
        FromLeftToRight,

        /// <summary>
        ///     Reveal from the right to the left.
        /// </summary>
        FromRightToLeft,

        /// <summary>
        ///     Reveal from the center to the bounding edge.
        /// </summary>
        FromCenterToEdge,
    }

    public enum VerticalRevealMode
    {
        /// <summary>
        ///     No vertical reveal animation.
        /// </summary>
        None,

        /// <summary>
        ///     Reveal from top to bottom.
        /// </summary>
        FromTopToBottom,

        /// <summary>
        ///     Reveal from bottom to top.
        /// </summary>
        FromBottomToTop,

        /// <summary>
        ///     Reveal from the center to the bounding edge.
        /// </summary>
        FromCenterToEdge,
    }
}
