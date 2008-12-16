using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Shazzam
{
  class InstantShaderEffect : ShaderEffect
  {

    public InstantShaderEffect(PixelShader shader)
    {
      this.PixelShader = shader;
      UpdateShaderValue(InputProperty);
    }
    public Brush Input
    {
      get { return (Brush)GetValue(InputProperty); }
      set { SetValue(InputProperty, value); }
    }

    public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(InstantShaderEffect), 0);



  }
}
