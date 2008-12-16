using System.Windows.Input;

namespace Shazzam.Commands {
	public static class AppCommands {

		public static readonly RoutedUICommand FullScreen =
			new RoutedUICommand("Full Screen", "FullScreen",
				typeof(AppCommands),
					new InputGestureCollection(
					new InputGesture[]{
          new KeyGesture(Key.F9,ModifierKeys.None)}));

		public static readonly RoutedUICommand DockLeft =
			new RoutedUICommand("Dock Left", "DockLeft",
				typeof(AppCommands),
					new InputGestureCollection(
					new InputGesture[]{
          new KeyGesture(Key.F10,ModifierKeys.None)}));

		public static readonly RoutedUICommand FullScreenCode =
			new RoutedUICommand("Full Screen Code", "FullScreenCode",
				typeof(AppCommands),
					new InputGestureCollection(
					new InputGesture[]{
         new KeyGesture(Key.F11,ModifierKeys.None)}));

		public static readonly RoutedUICommand OpenImage =
			new RoutedUICommand("Open Image", "OpenImage",
				typeof(AppCommands),
					new InputGestureCollection(
					new InputGesture[]{
          new KeyGesture(Key.I, ModifierKeys.Control)}));

		public static readonly RoutedUICommand RemoveShader =
		new RoutedUICommand("_Remove Shader", "RemoveShader",
						typeof(AppCommands),
							new InputGestureCollection(
							new InputGesture[]{
								new KeyGesture(Key.F6, ModifierKeys.None)}));

		public static readonly RoutedUICommand ApplyShader =
						new RoutedUICommand("_Apply Shader", "ApplyShader",
						typeof(AppCommands),
							new InputGestureCollection(
							new InputGesture[]{
									new KeyGesture(Key.F5, ModifierKeys.None)}));

		public static readonly RoutedUICommand CompileShader =
					new RoutedUICommand("_Compile Shader", "CompileShader",
					typeof(AppCommands),
						new InputGestureCollection(
						new InputGesture[]{
									new KeyGesture(Key.F7, ModifierKeys.None)}));

	}
}
