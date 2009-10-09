using System.Windows.Input;

namespace Shazzam.Commands {
	public static class AppCommands {

		public static readonly RoutedUICommand Exit = new RoutedUICommand("Exit", "Exit", typeof(AppCommands));

		public static readonly RoutedUICommand OpenImage = new RoutedUICommand("Open Image", "OpenImage",
			typeof(AppCommands), new InputGestureCollection { new KeyGesture(Key.I, ModifierKeys.Control) });

		public static readonly RoutedUICommand RemoveShader = new RoutedUICommand("Remove Shader", "RemoveShader",
			typeof(AppCommands), new InputGestureCollection { new KeyGesture(Key.F6) });

		public static readonly RoutedUICommand ApplyShader = new RoutedUICommand("Apply Shader", "ApplyShader",
			typeof(AppCommands), new InputGestureCollection { new KeyGesture(Key.F5) });

		public static readonly RoutedUICommand CompileShader = new RoutedUICommand("Compile Shader", "CompileShader",
			typeof(AppCommands), new InputGestureCollection { new KeyGesture(Key.F7) });

		public static readonly RoutedUICommand ExploreCompiledShaders = new RoutedUICommand("Explore Compiled Shaders",
			"ExploreCompiledShaders", typeof(AppCommands));

		public static readonly RoutedUICommand FullScreenImage = new RoutedUICommand("Full Screen Image", "FullScreenImage",
			typeof(AppCommands), new InputGestureCollection { new KeyGesture(Key.F9) });

		public static readonly RoutedUICommand FullScreenCode = new RoutedUICommand("Full Screen Code", "FullScreenCode",
			typeof(AppCommands), new InputGestureCollection { new KeyGesture(Key.F11) });

		public static readonly RoutedUICommand WhatsNew = new RoutedUICommand("What's New and User Guide", "WhatsNew",
			typeof(AppCommands), new InputGestureCollection { new KeyGesture(Key.F1) });


		public static void Initialize() {
			// Add a keyboard shortcut (Ctrl+Shift+S) to the "Save As" command.
			ApplicationCommands.SaveAs.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift));
		}
	}
}
