using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;
using Microsoft.CSharp;

namespace Shazzam.CodeGen {
	class CreatePixelShaderClass {

		public static Assembly CompileInMemory(string code) {
			var provider = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v3.5" } });

			CompilerParameters options = new CompilerParameters();
			options.ReferencedAssemblies.Add("System.Core.dll");
			options.ReferencedAssemblies.Add("WindowsBase.dll");
			options.ReferencedAssemblies.Add("PresentationFramework.dll");
			options.ReferencedAssemblies.Add("PresentationCore.dll");
			options.IncludeDebugInformation = false;
			options.GenerateExecutable = false;
			options.GenerateInMemory = true;
			CompilerResults results =
				 provider.CompileAssemblyFromSource(options, code);
			provider.Dispose();
			Assembly generatedAssembly = null;
			if (results.Errors.Count == 0)
			{
				generatedAssembly = results.CompiledAssembly;
			}
			return generatedAssembly;
		}
		// Build WPF class

		public static CodeCompileUnit BuildPixelShaderGraph(List<ShaderModelConstantRegister> rawShaderMembers) {
			// Create a new CodeCompileUnit to contain
			// the program graph.
			var codeGraph = new CodeCompileUnit();

			CodeNamespace namespaces = AssignNamespacesToGraph(codeGraph);

			// Declare a new type
			CodeTypeDeclaration shader = new CodeTypeDeclaration("AutoGenShaderEffect");
			shader.BaseTypes.Add(new CodeTypeReference("ShaderEffect"));
			// Add the new type to the namespace type collection.
			namespaces.Types.Add(shader);

			CodeConstructor constructor = CreateConstructor(rawShaderMembers);
			shader.Members.Add(constructor);
			//	constructor = CreateStaticConstructor();
			//		shader.Members.Add(constructor);

			var field = GeneratePixelShaderRegister("input");
			shader.Members.Add(field);
			CodeMemberProperty prop = GenerateNormalDependencyProperty("input", "System.Windows.Media.Brush");
			shader.Members.Add(prop);

			foreach (var item in rawShaderMembers)
			{
				field = GenerateCRegisterRegister(item.VariableName, item.VariableType, item.ConstantRegister);
				shader.Members.Add(field);
				prop = GenerateNormalDependencyProperty(item.VariableName, item.VariableType.FullName);
				shader.Members.Add(prop);
			}

			return codeGraph;
		}
		private static CodeMemberField GeneratePixelShaderRegister(string propertyName) {

			propertyName = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(propertyName);
			// create the dependency property
			var field = new CodeMemberField("DependencyProperty", propertyName + "Property");
			field.Attributes = MemberAttributes.Public | MemberAttributes.Static;

			CodeTypeReferenceExpression shaderType = new CodeTypeReferenceExpression("ShaderEffect");
			var method = new CodeMethodInvokeExpression(shaderType, "RegisterPixelShaderSamplerProperty");
			// method.Method = new CodeMethodReferenceExpression(, "SetValue");
			field.InitExpression = method;
			method.Parameters.Add(new CodePrimitiveExpression(propertyName));
			CodeTypeOfExpression typeOfExp = new CodeTypeOfExpression("AutoGenShaderEffect");
			method.Parameters.Add(typeOfExp);
			method.Parameters.Add(new CodePrimitiveExpression(0));
			return field;
		}

		private static CodeMemberField GenerateCRegisterRegister(string propertyName, Type propertyType, Int32 registerNumber) {

			propertyName = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(propertyName);
			// create the dependency property
			var field = new CodeMemberField("DependencyProperty", propertyName + "Property");
			field.Attributes = MemberAttributes.Public | MemberAttributes.Static;

			CodeTypeReferenceExpression shaderType = new CodeTypeReferenceExpression("DependencyProperty");
			var method = new CodeMethodInvokeExpression(shaderType, "Register");

			field.InitExpression = method;

			method.Parameters.Add(new CodePrimitiveExpression(propertyName));

			CodeTypeOfExpression typeOfExp = new CodeTypeOfExpression(propertyType);

			method.Parameters.Add(typeOfExp);
			typeOfExp = new CodeTypeOfExpression("AutoGenShaderEffect");
			method.Parameters.Add(typeOfExp);

			CodeExpression[] args = new CodeExpression[2];
			args[0] = new CodeObjectCreateExpression(propertyType);
			var arg = new CodePrimitiveExpression(registerNumber);
			args[1] = new CodeMethodInvokeExpression(null, "PixelShaderConstantCallback", arg);
			CodeObjectCreateExpression codeEx = new CodeObjectCreateExpression(typeof(UIPropertyMetadata), args);

			method.Parameters.Add(codeEx);
			return field;
		}

		private static CodeMemberProperty GenerateNormalDependencyProperty(string propertyName, string typeName) {
			propertyName = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(propertyName);

			var prop = new CodeMemberProperty();
			prop.Name = propertyName;
			prop.Type = new CodeTypeReference(typeName);
			prop.Attributes = MemberAttributes.Public;
			prop.HasGet = true;
			prop.HasSet = true;

			// create the getter
			CodeMethodInvokeExpression m1;
			m1 = new CodeMethodInvokeExpression();
			m1.Method = new CodeMethodReferenceExpression(null, "GetValue");
			m1.Parameters.Add(new CodeVariableReferenceExpression(propertyName + "Property"));

			CodeExpression exp = new CodeCastExpression(typeName, m1);
			prop.GetStatements.Add(new CodeMethodReturnStatement(exp));

			// create the setter
			CodeMethodInvokeExpression m2 = new CodeMethodInvokeExpression();
			m2.Method = new CodeMethodReferenceExpression(null, "SetValue");
			m2.Parameters.Add(new CodeVariableReferenceExpression(propertyName + "Property"));
			m2.Parameters.Add(new CodeVariableReferenceExpression("value"));
			prop.SetStatements.Add(m2);
			return prop;
		}

		private static CodeConstructor CreateStaticConstructor() {
			CodeConstructor constructor = new CodeConstructor();
			//	CodeMethodInvokeExpression method = new CodeMethodInvokeExpression();
			constructor.Attributes = MemberAttributes.Static | MemberAttributes.Public;
			return constructor;
		}
		private static CodeConstructor CreateConstructor(List<ShaderModelConstantRegister> rawShaderMembers) {

			CodeConstructor constructor = new CodeConstructor();
			CodeMethodInvokeExpression method = new CodeMethodInvokeExpression();
			constructor.Attributes = MemberAttributes.Public;
			var parm = new CodeParameterDeclarationExpression("PixelShader", "shader");
			constructor.Parameters.Add(parm);
			// Declare a new code entry point method.
			CodeEntryPointMethod start = new CodeEntryPointMethod();
			constructor.Statements.Add(new CodeCommentStatement("Note: for your project you must decide how to use the generated ShaderEffect class (Choose A or B below)."));
			constructor.Statements.Add(new CodeCommentStatement("A: Comment out the following line if you are not passing in the shader and remove the shader parameter from the constructor"));
			constructor.Statements.Add(new CodeSnippetStatement(""));

			var statement = new CodeAssignStatement();
			statement.Left = new CodeVariableReferenceExpression("PixelShader");
			statement.Right = new CodeArgumentReferenceExpression("shader");
			constructor.Statements.Add(statement);
			constructor.Statements.Add(new CodeSnippetStatement(""));
			constructor.Statements.Add(new CodeCommentStatement("B: Uncomment the following two lines - which load the *.ps file"));
			constructor.Statements.Add(new CodeCommentStatement("Uri u = new Uri(@\"pack://application:,,,/bandedswirl.ps\");")); constructor.Statements.Add(new CodeCommentStatement("PixelShader = new PixelShader() { UriSource = u };"));

			constructor.Statements.Add(new CodeSnippetStatement(""));
			constructor.Statements.Add(new CodeCommentStatement("Must initialize each DependencyProperty that's affliated with a shader register"));
			constructor.Statements.Add(new CodeCommentStatement("Ensures the shader initializes to the proper default value."));

			constructor.Statements.Add(CreateUpdateMethod("Input"));
			foreach (var item in rawShaderMembers)
			{
				constructor.Statements.Add(CreateUpdateMethod(item.VariableName));
			}
			return constructor;
		}

		private static CodeMethodInvokeExpression CreateUpdateMethod(string propertyName) {

			var method = new CodeMethodInvokeExpression();

			var thisType = new CodeThisReferenceExpression();
			method = new CodeMethodInvokeExpression();
			method.Method = new CodeMethodReferenceExpression(thisType, "UpdateShaderValue");
			method.Parameters.Add(new CodeVariableReferenceExpression(propertyName + "Property"));
			return method;
		}

		private static CodeNamespace AssignNamespacesToGraph(CodeCompileUnit codeGraph) {
			CodeNamespace ns = new CodeNamespace("Shazzam.Shaders");
			codeGraph.Namespaces.Add(ns);

			// using/Imports directives
			ns.Imports.Add(new CodeNamespaceImport("System.Windows"));
			ns.Imports.Add(new CodeNamespaceImport("System.Windows.Media"));
			ns.Imports.Add(new CodeNamespaceImport("System.Windows.Media.Effects"));
			return ns;
		}

		public static string GetSourceText(CodeDomProvider currentProvider, List<ShaderModelConstantRegister> rawShaderMembers) {

			GenerateCode(currentProvider, BuildPixelShaderGraph(rawShaderMembers));

			// Build the source file name with the appropriate
			// language extension.
			String sourceFile;
			if (currentProvider.FileExtension[0] == '.')
			{
				sourceFile = "TestGraph" + currentProvider.FileExtension;
			}
			else
			{
				sourceFile = "TestGraph." + currentProvider.FileExtension;
			}

			// Read in the generated source file and
			// display the source text.
			StreamReader sr = new StreamReader(sourceFile);
			string x = sr.ReadToEnd();
			sr.Close();
			return x;
		}
		public static void GenerateCode(CodeDomProvider provider,  CodeCompileUnit compileunit) {
			// Build the source file name with the appropriate
			// language extension.
			String sourceFile;
			if (provider.FileExtension[0] == '.')
			{
				sourceFile = "TestGraph" + provider.FileExtension;
			}
			else
			{
				sourceFile = "TestGraph." + provider.FileExtension;
			}

			// Create an IndentedTextWriter, constructed with
			// a StreamWriter to the source file.
			IndentedTextWriter tw = new IndentedTextWriter(new StreamWriter(sourceFile, false), "    ");
			// Generate source code using the code generator.
			provider.GenerateCodeFromCompileUnit(compileunit, tw, new CodeGeneratorOptions());
			tw.Close();
		}
	}
}
