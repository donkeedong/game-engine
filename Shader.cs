using System;
using System.Text;
using System.IO;
using OpenToolkit.Graphics.OpenGL4;

public class Shader : IDisposable
{
    int Handle;

    public Shader(string vertexPath, string fragmentPath)
    {
        string VertexShaderSource;

        using (StreamReader reader = new StreamReader(vertexPath, Encoding.UTF8))
        {
            VertexShaderSource = reader.ReadToEnd();
        }

        string FragmentShaderSource;

        using (StreamReader reader = new StreamReader(fragmentPath, Encoding.UTF8))
        {
            FragmentShaderSource = reader.ReadToEnd();
        }

        int VertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(VertexShader, VertexShaderSource);

        int FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(FragmentShader, FragmentShaderSource);

        GL.CompileShader(VertexShader);

        string infoLogVert = GL.GetShaderInfoLog(VertexShader);
        if (infoLogVert != System.String.Empty)
            System.Console.WriteLine(infoLogVert);

        GL.CompileShader(FragmentShader);

        string infoLogFrag = GL.GetShaderInfoLog(FragmentShader);

        if (infoLogFrag != System.String.Empty)
            System.Console.WriteLine(infoLogFrag);

        Handle = GL.CreateProgram();

        GL.AttachShader(Handle, VertexShader);
        GL.AttachShader(Handle, FragmentShader);

        GL.LinkProgram(Handle);

        GL.DetachShader(Handle, VertexShader);
        GL.DetachShader(Handle, FragmentShader);
        GL.DeleteShader(FragmentShader);
        GL.DeleteShader(VertexShader);
    }

    public void Use()
    {
        GL.UseProgram(Handle);
    }

    private bool disposedValue = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            GL.DeleteProgram(Handle);

            disposedValue = true;
        }
    }

    ~Shader()
    {
        GL.DeleteProgram(Handle);
    }


    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}