namespace Shazzam
{
    using System.Collections.Generic;

    public class ShaderModel
    {
        public ShaderModel(
            string shaderFileName,
            string generatedClassName,
            string generatedNamespace,
            string description,
            TargetFramework targetFramework,
            List<ShaderModelConstantRegister> registers)
        {
            this.ShaderFileName = shaderFileName;
            this.GeneratedClassName = generatedClassName;
            this.GeneratedNamespace = generatedNamespace;
            this.Description = description;
            this.TargetFramework = targetFramework;
            this.Registers = registers;
        }

        public string ShaderFileName { get; }

        public string GeneratedClassName { get; }

        public string GeneratedNamespace { get; }

        public string Description { get; }

        public TargetFramework TargetFramework { get; }

        public List<ShaderModelConstantRegister> Registers { get; }
    }
}
