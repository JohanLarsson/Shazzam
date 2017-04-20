using System.Windows;
using System.Windows.Media;
using Shazzam.ViewModels;

namespace ShazzamUnitTests
{
    using NUnit.Framework;

    public class MainWindowVmUnitTest
  {
    [Test]
    public void CodeAndImageRows_CheckDefault_AreBothSameHeight()
    {
      var vm = new MainWindowViewModel();
      var rowHeight = new GridLength(5, GridUnitType.Star);
      Assert.AreEqual(vm.CodeRowHeight, rowHeight);
      Assert.AreEqual(vm.ImageRowHeight, rowHeight);
    }

    [Test]
    public void FullScreenCodeCommand_RunCommand_ImageRowSetToZero()
    {
      var vm = new MainWindowViewModel();

      vm.FullScreenCodeCommand.Execute(this);
      Assert.AreEqual(vm.ImageRowHeight, new GridLength(0));
    }

    [Test]
    public void FullScreenImageCommand_RunCommand_CodeRowSetToZero()
    {
      var vm = new MainWindowViewModel();

      vm.FullScreenImageCommand.Execute(this);
      Assert.AreEqual(vm.CodeRowHeight, new GridLength(0));
    }
    [Test]
    public void ImageStretchCommand_CheckDefault_IsUniform()
    {
      var vm = new MainWindowViewModel();

      Assert.AreEqual(vm.ImageStretch, Stretch.Uniform);
    }
    [Test]
    public void ImageStretchCommand_RunCommand_StretchValueCorrect()
    {
      var vm = new MainWindowViewModel();

      vm.ImageStretchCommand.Execute("none");
      Assert.AreEqual(vm.ImageStretch, Stretch.None);
      vm.ImageStretchCommand.Execute("fill");
      Assert.AreEqual(vm.ImageStretch, Stretch.Fill);
      vm.ImageStretchCommand.Execute("uniform");
      Assert.AreEqual(vm.ImageStretch, Stretch.Uniform);

      vm.ImageStretchCommand.Execute("uniformtofill");
      Assert.AreEqual(vm.ImageStretch, Stretch.UniformToFill);
    }
  }
}
