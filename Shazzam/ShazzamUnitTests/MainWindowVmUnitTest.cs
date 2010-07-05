using System.Windows;
using System.Windows.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shazzam.ViewModels;
using TypeMock.ArrangeActAssert;
using System;
using System.Diagnostics;
using TypeMock;

namespace ShazzamUnitTests
{
  [TestClass, Isolated]
  public class MainWindowVmUnitTest
  {

    #region Additional test attributes
    //
    // You can use the following additional attributes as you write your tests:
    //
    // Use ClassInitialize to run code before running the first test in the class
    // [ClassInitialize()]
    // public static void MyClassInitialize(TestContext testContext) { }
    //
    // Use ClassCleanup to run code after all tests in a class have run
    // [ClassCleanup()]
    // public static void MyClassCleanup() { }
    //
    // Use TestInitialize to run code before running each test
    // [TestInitialize()]
    // public void MyTestInitialize() { }
    //
    // Use TestCleanup to run code after each test has run
    // [TestCleanup()]
    // public void MyTestCleanup() { }
    //
    #endregion
    //  http://software.intel.com/en-us/blogs/2009/12/11/adventures-with-typemock-isolator-and-mock-objects/
   // http://www.typemock.com/community/viewtopic.php?p=3598&sid=23814fc24469d7023c8715bbf1066a7b 

    [TestMethod]
    public void MockSample_TestMessageBox()
    {
      // create a mock instance of th System.Diagnostics.Process class
      //    var process = Isolate.Fake.Instance<Process>();
      TypeMock.MockObject mock = MockManager.MockObject(typeof(Process));
      mock.CallStatic.ExpectAndReturn("Start", mock.Object);

    }
    [TestMethod]
    public void CodeAndImageRows_CheckDefault_AreBothSameHeight()
    {
      var vm = new MainWindowViewModel();
      var rowHeight = new GridLength(5, GridUnitType.Star);
      Assert.AreEqual(vm.CodeRowHeight, rowHeight);
      Assert.AreEqual(vm.ImageRowHeight, rowHeight);
    }

    [TestMethod]
    public void FullScreenCodeCommand_RunCommand_ImageRowSetToZero()
    {
      var vm = new MainWindowViewModel();

      vm.FullScreenCodeCommand.Execute(this);
      Assert.AreEqual(vm.ImageRowHeight, new GridLength(0));
    }

    [TestMethod]
    public void FullScreenImageCommand_RunCommand_CodeRowSetToZero()
    {
      var vm = new MainWindowViewModel();

      vm.FullScreenImageCommand.Execute(this);
      Assert.AreEqual(vm.CodeRowHeight, new GridLength(0));
    }
    [TestMethod]
    public void ImageStretchCommand_CheckDefault_IsUniform()
    {
      var vm = new MainWindowViewModel();

      Assert.AreEqual(vm.ImageStretch, Stretch.Uniform);
    }
    [TestMethod]
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
