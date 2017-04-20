namespace Shazzam
{
    using System.Collections.Generic;

    public class ShaderModel
    {
        public string ShaderFileName { get; set; }

        public string GeneratedClassName { get; set; }

        public string GeneratedNamespace { get; set; }

        public string Description { get; set; }

        public TargetFramework TargetFramework { get; set; }

        public List<ShaderModelConstantRegister> Registers { get; set; }
    }
}
