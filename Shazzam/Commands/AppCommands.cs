﻿namespace Shazzam.Commands
{
    using System.Windows.Input;

    public static class AppCommands
    {
        public static readonly RoutedUICommand Exit = new(
            "Exit",
            "Exit",
            typeof(AppCommands));

        public static readonly RoutedUICommand OpenImage = new(
            "Open Image",
            "OpenImage",
            typeof(AppCommands),
            new InputGestureCollection { new KeyGesture(Key.I, ModifierKeys.Control) });

        public static readonly RoutedUICommand OpenMedia = new(
            "Open Media",
            "OpenMedia",
            typeof(AppCommands),
            new InputGestureCollection { new KeyGesture(Key.M, ModifierKeys.Control) });

        public static readonly RoutedUICommand RemoveShader = new(
            "Remove Shader",
            "RemoveShader",
            typeof(AppCommands),
            new InputGestureCollection { new KeyGesture(Key.F6) });

        public static readonly RoutedUICommand ApplyShader = new(
            "Apply Shader",
            "ApplyShader",
            typeof(AppCommands),
            new InputGestureCollection { new KeyGesture(Key.F5) });

        public static readonly RoutedUICommand CompileShader = new(
            "Compile Shader",
            "CompileShader",
            typeof(AppCommands),
            new InputGestureCollection { new KeyGesture(Key.F7) });

        public static void Initialize()
        {
            // Add a keyboard shortcut (Ctrl+Shift+S) to the "Save As" command.
            ApplicationCommands.SaveAs.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift));
        }
    }
}
