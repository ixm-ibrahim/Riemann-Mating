using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OpenTK_Riemann_Mating
{
    class Shader
    {
        // Represents the location of the final shader program after it's finished being compiled
        public int Handle;

        // 
        bool disposedValue = false;

        public readonly Dictionary<string, int> uniformLocations;

        public Shader(string vertexPath, string fragmentPath)
        {
            // Handles for individual shaders
            int VertexShader;
            int FragmentShader;

            // Get the source code for the shaders
            string vertexShaderSource = LoadShaderSource(vertexPath);
            string fragmentShaderSource = LoadShaderSource(fragmentPath);

            // Generate shaders and bind them with source code
            VertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(VertexShader, vertexShaderSource);
            FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentShader, fragmentShaderSource);

            // Compile shaders and check for errors
            Console.Write(vertexPath + "... ");
            string infoLogVert = CompileShader(VertexShader);
            Console.Write(" compiled successfully.\n\n" + fragmentPath + "...");
            string infoLogFrag = CompileShader(FragmentShader);
            Console.WriteLine(" compiled successfully.\n");

            // Link shaders to a program that can be run by the GPU
            Handle = GL.CreateProgram();

            // Attach both shaders...
            GL.AttachShader(Handle, VertexShader);
            GL.AttachShader(Handle, FragmentShader);

            // And then link them together.
            LinkProgram(Handle);

            // The individual shaders will not be needed after the full shader program is finished
            GL.DetachShader(Handle, VertexShader);
            GL.DetachShader(Handle, FragmentShader);
            GL.DeleteShader(VertexShader);
            GL.DeleteShader(FragmentShader);

            // The shader is now ready to go, but first, we're going to cache all the shader uniform locations.
            // Querying this from the shader is very slow, so we do it once on initialization and reuse those values
            // later.

            // First, we have to get the number of active uniforms in the shader.
            GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out var numberOfUniforms);

            // Next, allocate the dictionary to hold the locations.
            uniformLocations = new Dictionary<string, int>();

            //Console.WriteLine("HERE - " + numberOfUniforms);

            // Loop over all the uniforms,
            for (var i = 0; i < numberOfUniforms; i++)
            {
                // get the name of this uniform,
                var key = GL.GetActiveUniform(Handle, i, out _, out _);

                //Console.WriteLine(key);

                // get the location,
                var location = GL.GetUniformLocation(Handle, key);

                // and then add it to the dictionary.
                uniformLocations.Add(key, location);
            }
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteProgram(Handle);

                disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        ~Shader()
        {
            GL.DeleteProgram(Handle);
        }

        string LoadShaderSource(string path)
        {
            string shaderSource;

            using (StreamReader reader = new StreamReader(path, Encoding.UTF8))
            {
                shaderSource = reader.ReadToEnd();
            }
            //System.Console.WriteLine(shaderSource);
            return shaderSource;
        }

        string CompileShader(int shader)
        {
            GL.CompileShader(shader);

            string infoLog = GL.GetShaderInfoLog(shader);

            if (!infoLog.StartsWith("WARNING") && infoLog != String.Empty)
                throw new Exception(infoLog);

            return infoLog;
        }

        void LinkProgram(int program)
        {
            GL.LinkProgram(Handle);

            // Check for linking errors
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out var code);

            if (code != (int)All.True)
            {
                // We can use 'GL.GetProgramInfoLog(program)' to get information about the error.
                //throw new Exception($"Error occurred whilst linking Program({program})");
            }
        }

        public void Use()
        {
            GL.UseProgram(Handle);
        }

        public int GetAttribLocation(string attribName)
        {
            return GL.GetAttribLocation(Handle, attribName);
        }

        public int GetInt(string name)
        {
            int[] i = new int[1];
            GL.GetUniform(Handle, GL.GetUniformLocation(Handle, name), i);

            return i[0];
        }

        public float GetFloat(string name)
        {
            float[] f = new float[1];
            GL.GetUniform(Handle, GL.GetUniformLocation(Handle, name), f);

            return f[0];
        }

        public Vector2 GetVector2(string name)
        {
            float[] v2 = new float[2];
            GL.GetUniform(Handle, GL.GetUniformLocation(Handle, name), v2);

            return new Vector2(v2[0], v2[1]);
        }

        public Vector2[] GetVector2Array(string name, int n)
        {
            float[] a = new float[n * 2];
            GL.GetUniform(Handle, GL.GetUniformLocation(Handle, name), a);

            var v2 = new Vector2[n];

            for (int i = 0; i < n * 2; i += 2)
                v2[i / 2] = new Vector2(a[i], a[i + 1]);

            return v2;
        }

        // Uniform setters
        // Uniforms are variables that can be set by user code, instead of reading them from the VBO.
        // You use VBOs for vertex-related data, and uniforms for almost everything else.

        // Setting a uniform is almost always the exact same, so I'll explain it here once, instead of in every method:
        //     1. Bind the program you want to set the uniform on
        //     2. Get a handle to the location of the uniform with GL.GetUniformLocation.
        //     3. Use the appropriate GL.Uniform* function to set the uniform.

        // Set a uniform int on the shader
        public void SetInt(string name, int data)
        {
            GL.UseProgram(Handle);
            GL.Uniform1(uniformLocations[name], data);
        }

        // Set a uniform float on the shader
        public void SetFloat(string name, float data)
        {
            GL.UseProgram(Handle);
            GL.Uniform1(uniformLocations[name], data);
        }

        // Set a uniform double on the shader
        public void SetDouble(string name, double data)
        {
            GL.UseProgram(Handle);
            GL.Uniform1(uniformLocations[name], data);
        }

        // Set a uniform Vector2 on the shader
        public void SetVector2(string name, Vector2 data)
        {
            GL.UseProgram(Handle);
            GL.Uniform2(uniformLocations[name], data);
        }

        // Set a uniform Vector2 on the shader
        public void SetVector2d(string name, Vector2d data)
        {
            double[] d = new double[]
            {
                data.X,
                data.Y,
            };

            GL.UseProgram(Handle);
            GL.Uniform2(uniformLocations[name], 2, d);
        }

        // Set a uniform Vector2 array on the shader
        public void SetVector2Array(string name, Vector2[] data)
        {/*
            var floats = new float[data.Length * 2];

            for (int i = 0; i < data.Length * 2; i += 2)
            {
                floats[i] = data[i / 2].X;
                floats[i + 1] = data[i / 2].Y;
            }

            GL.UseProgram(Handle);
            GL.Uniform2(uniformLocations[name + "[0]"], data.Length, floats);*/

            GL.UseProgram(Handle);

            for (int i = 0; i < data.Length; i++)
            {
                GL.Uniform2(uniformLocations[name + i], data[i]);
            }
        }

        // Set a uniform Vector2 array on the shader
        public void SetVector2dArray(string name, Vector2d[] data)
        {/*
            var floats = new float[data.Length * 2];

            for (int i = 0; i < data.Length * 2; i += 2)
            {
                floats[i] = data[i / 2].X;
                floats[i + 1] = data[i / 2].Y;
            }

            GL.UseProgram(Handle);
            GL.Uniform2(uniformLocations[name + "[0]"], data.Length, floats);*/

            GL.UseProgram(Handle);

            /*
            for (int i = 0; i < data.Length; i++)
            {
                double[] d = new double[]
                {
                    data[i].X,
                    data[i].Y,
                };

                GL.Uniform2(uniformLocations[name + i], 2, d);
            }
            */

            for (int i = 0; i < data.Length; i++)
            {
                GL.Uniform2(uniformLocations[name + i], new Vector2((float)data[i].X, (float)data[i].Y));
            }
        }

        // Set a uniform Vector3 on the shader
        public void SetVector3(string name, Vector3 data)
        {
            GL.UseProgram(Handle);
            GL.Uniform3(uniformLocations[name], data);
        }

        // Set a uniform Matrix4 on the shader
        //      The matrix is transposed before being sent to the shader
        public void SetMatrix4(string name, Matrix4 data)
        {
            GL.UseProgram(Handle);
            GL.UniformMatrix4(uniformLocations[name], true, ref data);
        }

    }
}
