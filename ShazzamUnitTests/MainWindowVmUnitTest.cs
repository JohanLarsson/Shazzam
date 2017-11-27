namespace ShazzamUnitTests
{
    using System.Windows;
    using System.Windows.Media;
    using NUnit.Framework;

    using Shazzam;

    public class MainWindowVmUnitTest
    {
        [Test]
        public void CodeAndImageRows_CheckDefault_AreBothSameHeight()
        {
            var vm = MainWindowViewModel.Instance;
            var rowHeight = new GridLength(5, GridUnitType.Star);
            Assert.AreEqual(vm.CodeRowHeight, rowHeight);
            Assert.AreEqual(vm.ImageRowHeight, rowHeight);
        }

        [Test]
        public void FullScreenCodeCommand_RunCommand_ImageRowSetToZero()
        {
            var vm = MainWindowViewModel.Instance;
            vm.FullScreenCodeCommand.Execute(this);
            Assert.AreEqual(vm.ImageRowHeight, new GridLength(0));
        }

        [Test]
        public void FullScreenImageCommand_RunCommand_CodeRowSetToZero()
        {
            var vm = MainWindowViewModel.Instance;
            vm.FullScreenImageCommand.Execute(this);
            Assert.AreEqual(vm.CodeRowHeight, new GridLength(0));
        }

        [Test]
        public void ImageStretchCommand_CheckDefault_IsUniform()
        {
            var vm = MainWindowViewModel.Instance;
            Assert.AreEqual(vm.ImageStretch, Stretch.Uniform);
        }

        [Test]
        public void ImageStretchCommand_RunCommand_StretchValueCorrect()
        {
            var vm = MainWindowViewModel.Instance;
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
