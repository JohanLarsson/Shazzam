﻿namespace Shazzam.CodeGen
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.InteropServices;

    public class ShaderCompiler : INotifyPropertyChanged
    {
        private string? errorText;
        private bool isCompiled;

        public event PropertyChangedEventHandler? PropertyChanged;

        [Guid("8BA5FB08-5195-40e2-AC58-0D989C3A0102")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface ID3DXBuffer
        {
            [PreserveSig]
            IntPtr GetBufferPointer();

            [PreserveSig]
            int GetBufferSize();
        }

        public string? ErrorText
        {
            get => this.errorText;

            set
            {
                if (value == this.errorText)
                {
                    return;
                }

                this.errorText = value;
                this.RaiseNotifyChanged();
            }
        }

        public bool IsCompiled
        {
            get => this.isCompiled;

            set
            {
                if (value == this.isCompiled)
                {
                    return;
                }

                this.isCompiled = value;
                this.RaiseNotifyChanged();
            }
        }

        public void Compile(string codeText, ShaderProfile shaderProfile)
        {
            this.IsCompiled = false;
            var path = Properties.Settings.Default.FolderPath_Output;

            var defines = IntPtr.Zero;
            var includes = IntPtr.Zero;
            ID3DXBuffer? ppShader;
            ID3DXBuffer? ppErrorMsgs;

            var entryPoint = "main";

            var targetProfile = shaderProfile == ShaderProfile.PixelShader3
                ? "ps_3_0"
                : "ps_2_0";

            var useDx10 = false;

            var hr = 0;
            if (useDx10)
            {
                var pHr = 0;
                hr = D3DX10CompileFromMemory(
                    codeText,
                    codeText.Length,
                    string.Empty,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    entryPoint,
                    targetProfile,
                    0,
                    0,
                    IntPtr.Zero,
                    out ppShader,
                    out ppErrorMsgs,
                    ref pHr);
            }
            else
            {
                if (IntPtr.Size == 8)
                {
                    // 64 bit
                    hr = D3DXCompileShader64Bit(
                        codeText,
                        codeText.Length,
                        defines,
                        includes,
                        entryPoint,
                        targetProfile,
                        0,
                        out ppShader,
                        out ppErrorMsgs,
                        out _);
                }
                else
                {
                    // 32 bit
                    hr = D3DXCompileShader(
                        codeText,
                        codeText.Length,
                        defines,
                        includes,
                        entryPoint,
                        targetProfile,
                        0,
                        out ppShader,
                        out ppErrorMsgs,
                        out _);
                }
            }

            if (hr != 0)
            {
                var errors = ppErrorMsgs.GetBufferPointer();
                this.ErrorText = Marshal.PtrToStringAnsi(errors);
                this.IsCompiled = false;
                goto finished;
            }

            this.ErrorText = string.Empty;
            this.IsCompiled = true;
            var psPath = path + Constants.FileNames.TempShaderPs;
            var pCompiledPs = ppShader.GetBufferPointer();
            var compiledPsSize = ppShader.GetBufferSize();

            var compiledPs = new byte[compiledPsSize];
            Marshal.Copy(pCompiledPs, compiledPs, 0, compiledPs.Length);
            using (var psFile = File.Open(psPath, FileMode.Create, FileAccess.Write))
            {
                psFile.Write(compiledPs, 0, compiledPs.Length);
            }

finished:

            if (ppShader != null)
            {
                Marshal.ReleaseComObject(ppShader);
            }

            ppShader = null;

            if (ppErrorMsgs != null)
            {
                Marshal.ReleaseComObject(ppErrorMsgs);
            }

            ppErrorMsgs = null;
            this.CompileFinished();
            //// CreateFileCopies(path);
            //// throw new Exception("testing");
        }

        public void Reset()
        {
            this.ErrorText = "not compiled";
        }

        protected void RaiseNotifyChanged([System.Runtime.CompilerServices.CallerMemberName] string? propName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        [PreserveSig]
        [DllImport("D3DX9_40.dll", CharSet = CharSet.Auto)]
        private static extern int D3DXCompileShader(
            [MarshalAs(UnmanagedType.LPStr)] string pSrcData,
            int dataLen,
            IntPtr pDefines,
            IntPtr includes,
            [MarshalAs(UnmanagedType.LPStr)] string pFunctionName,
            [MarshalAs(UnmanagedType.LPStr)] string pTarget,
            int flags,
            out ID3DXBuffer ppShader,
            out ID3DXBuffer ppErrorMsgs,
            out IntPtr ppConstantTable);

        [PreserveSig]
        [DllImport("D3DX9_40_64bit.dll", CharSet = CharSet.Auto, EntryPoint = "D3DXCompileShader")]
        private static extern int D3DXCompileShader64Bit(
            [MarshalAs(UnmanagedType.LPStr)] string pSrcData,
            int dataLen,
            IntPtr pDefines,
            IntPtr includes,
            [MarshalAs(UnmanagedType.LPStr)] string pFunctionName,
            [MarshalAs(UnmanagedType.LPStr)] string pTarget,
            int flags,
            out ID3DXBuffer ppShader,
            out ID3DXBuffer ppErrorMsgs,
            out IntPtr ppConstantTable);

        [PreserveSig]
        [DllImport("d3dx10_43.dll", CharSet = CharSet.Auto)]
        private static extern int D3DX10CompileFromMemory(
            [MarshalAs(UnmanagedType.LPStr)] string pSrcData,
            int dataLen,
            [MarshalAs(UnmanagedType.LPStr)] string pFilename,
            IntPtr pDefines,
            IntPtr pInclude,
            [MarshalAs(UnmanagedType.LPStr)] string pFunctionName,
            [MarshalAs(UnmanagedType.LPStr)] string pProfile,
            int flags1,
            int flags2,
            IntPtr pPump,
            out ID3DXBuffer ppShader,
            out ID3DXBuffer ppErrorMsgs,
            ref int pHresult);

        private void CompileFinished()
        {
            // for instrumentation
        }
    }
}
