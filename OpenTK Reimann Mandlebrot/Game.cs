using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using System;
using System.Drawing;
using System.Numerics;

namespace OpenTK_Reimann_Mating
{
    /* Controls:
     *      WASD, Space, L-Shift: camera movement
     *      Mouse: rotate camera in space
     *      Scroll: zoom
     *      N: toggle lock camera to center of the world (so that it appears that the sphere itself is rotating rather than the camera)
     *      B: change camera type from FPS to FREE or vice versa
     *      
     *      IJKL: movement along the projected complex plane (initially, the origin is centered at the south pole)
     *      O, U: zoom in and out on the projected complex plane
     *      
     *      Left, Right, Down Arrow Keys: decrease, increase, or stop changing the mating frame (NOTE: this only can be done after all the frames have been generated)
     *      
     *      F11: toggle fullscreen
     */



    class Game : GameWindow
    {
        // CHANGEABLE VALUES

        // Julia Sets to mate
        //Complex p = new Complex(-1, 0);             // basillica
        //Complex q = new Complex(-.123f, .745f);     // rabbit
        Complex p = new Complex(-.835f, -.2321f);
        Complex q = new Complex(.285f, .01f);

        // Mating values
        int maxIterations = 200;        // Increasing this will increase lag
        double bailout = 100;           // The higher, the smoother the colors will look
        int matingIterations = 43;      // Currently cannot exceed 50, or there will be problems with the shader
        int intermediateSteps = 16;     // Cannot be lower than 1

        // The higher, the more zoomed in on the Riemann Sphere
        //float zoom = -4f;   // viewing as if the complex plane is flat, with p mating into q (viewed from the south pole)
        float zoom = 0f;    // normal Riemann Sphere zoom
        //float zoom = 4f;    // viewing as if the complex plane is flat, with q mating into p (viewed from the north pole)

        // Starting position on the complex plane (centered at zero on the south pole of Riemann Sphere)
        float r = 0;
        float i = 0;

        // Set this to "true" if you want the camera to start by facing the south pole (best paired with the zoom = 4 option above)
        bool southPole = false;

        //float cameraStartDist = 11.5f;  // Looking at a small part of it (best paired with zoom = 4 option above)
        float cameraStartDist = 23.5f;  // Looking at the entire sphere (best paired with zoom = 0 option above)

        // This can be changed, but this holds the best results
        const double R1 = 1e10;


        // MATING CODE


        // Note: these two need to be called in OnLoad()
        void InitializeMating()
        {
            completed = false;
            frame = -1;

            t = new double[intermediateSteps];
            R = new double[intermediateSteps];

            x = new ComplexFix[matingIterations * intermediateSteps];
            y = new ComplexFix[matingIterations * intermediateSteps];

            ma = new OpenTK.Vector2[matingIterations * intermediateSteps];
            mb = new OpenTK.Vector2[matingIterations * intermediateSteps];
            mc = new OpenTK.Vector2[matingIterations * intermediateSteps];
            md = new OpenTK.Vector2[matingIterations * intermediateSteps];

            var tmp = new Complex[intermediateSteps];
            const double R2 = 1e20;
            const double R4 = 1e40;

            for (int s = 0; s < intermediateSteps; s++)
            {
                t[s] = (s + .5) / intermediateSteps;
                R[s] = Math.Exp(Math.Pow(2, 1 - t[s]) * Math.Log(R1));

                // only used for speeding up calculations for the x_t and y_t arrays
                tmp[s] = (1 + ((1 - t[s]) * q / R2)) / (1 + ((1 - t[s]) * p / R2));
            }

            var p_i = ComplexFix.Zero;
            var q_i = ComplexFix.Zero;

            for (int i = 0; i < matingIterations * intermediateSteps; i++)
            {
                int s = i % intermediateSteps;
                /*
                x[i] = Complex.Proj(tmp[s] * (p_i / R[s]) / (1 + ((1 - t[s]) * q / R4 * (p_i - p))));
                y[i] = Complex.Proj(tmp[s] * (R[s] / q_i) * (1 + ((1 - t[s]) * p / R4 * (q_i - q))));
                */

                x[i] = ComplexFix.Zero;
                y[i] = ComplexFix.Zero;

                if (p_i.big)
                {
                    x[i].big = true;
                    x[i].bigC = p_i.bigC / (int)R[s];
                    x[i].c = x[i].bigC.toComplex();
                }
                else
                    x[i].c = p_i.c / R[s];

                if (q_i.big)
                {
                    y[i].big = true;
                    y[i].bigC = (int)R[s] / q_i.bigC;
                    y[i].c = y[i].bigC.toComplex();
                }
                else
                    y[i].c = R[s] / q_i.c;


                if (s == intermediateSteps - 1)
                {
                    var p_test = (p_i.c ^ 2) + p;
                    var q_test = (q_i.c ^ 2) + q;

                    if (!p_i.big && Complex.IsInfinity(p_test))
                    {
                        p_i.big = true;
                        p_i.bigC = new BigComplex(p_i.c);
                    }
                    if (!q_i.big && Complex.IsInfinity(q_test))
                    {
                        q_i.big = true;
                        q_i.bigC = new BigComplex(q_i.c);
                    }

                    if (p_i.big)
                        p_i.bigC = (p_i.bigC ^ 2) + new BigComplex(p);
                    else
                        p_i.c = p_test;

                    if (q_i.big)
                        q_i.bigC = (q_i.bigC ^ 2) + new BigComplex(q);
                    else
                        q_i.c = q_test;

                }
            }
        }

        void UpdateMating(FrameEventArgs e)
        {
            if (!completed)
                frame++;
            else
            {
                if (zoomMode == 1)
                    frame += (float)e.Time * intermediateSteps/1;
                else if (zoomMode == -1)
                    frame -= (float)e.Time * intermediateSteps/1;

                if (frame < 0)
                    frame = 0;
                else if (frame >= matingIterations * intermediateSteps)
                    frame = matingIterations * intermediateSteps - 1;
            }

            if (frame >= matingIterations * intermediateSteps)
                frame = matingIterations * intermediateSteps - 1;

            // frame = intermediate_steps * n + s
            // for each n
            //      for each s
            int s = (int)frame % intermediateSteps;
            int n = ((int)frame - s) / intermediateSteps;

            // Only perform these calculations if all the frames have not been generated
            if (!completed)
            {
                if (frame == matingIterations * intermediateSteps - 1)
                    completed = true;

                int first = intermediateSteps + s;

                if (n > 0)
                {
                    var z_x = new ComplexFix[matingIterations - n];
                    var z_y = new ComplexFix[matingIterations - n];

                    var tmp = (1 - y[first].c) / (1 - x[first].c);
                    var bigTmp = (1 - y[first].bigC) / (1 - x[first].bigC);

                    for (int k = 0; k < matingIterations - n; k++)
                    {
                        int k_next = k + 1;
                        int next = intermediateSteps * k_next + s;
                        int prev = intermediateSteps * k + ((s + intermediateSteps - 1) % intermediateSteps);

                        z_x[k] = ComplexFix.Zero;
                        z_y[k] = ComplexFix.Zero;

                        if (x[next].big || x[first].big || y[first].big)
                        {
                            z_x[k].bigC = BigComplex.Sqrt(bigTmp * (x[next].bigC - x[first].bigC) / (x[next].bigC - y[first].bigC));
                            z_x[k].c = z_x[k].bigC.toComplex();

                            if (Complex.IsInfinity(z_x[k].c))
                                z_x[k].big = true;

                            if ((-z_x[k].bigC - x[prev].bigC).RadiusSquared < (z_x[k].bigC - x[prev].bigC).RadiusSquared)
                                z_x[k] = -z_x[k];
                        }
                        else
                        {
                            z_x[k].c = Complex.Sqrt(Complex.Proj(tmp * (x[next].c - x[first].c) / (x[next].c - y[first].c)));

                            if ((-z_x[k].c - x[prev].c).RadiusSquared < (z_x[k].c - x[prev].c).RadiusSquared)
                                z_x[k] = -z_x[k];
                        }

                        if (y[next].big || x[first].big || y[first].big)
                        {
                            z_y[k].bigC = BigComplex.Sqrt(bigTmp * (1 - (x[first].bigC / y[next].bigC)) / (1 - (y[first].bigC / y[next].bigC)));
                            z_y[k].c = z_y[k].bigC.toComplex();

                            if (Complex.IsInfinity(z_y[k].c))
                                z_y[k].big = true;

                            if ((-z_y[k].bigC - y[prev].bigC).RadiusSquared < (z_y[k].bigC - y[prev].bigC).RadiusSquared)
                                z_y[k] = -z_y[k];
                        }
                        else
                        {
                            z_y[k].c = Complex.Sqrt(Complex.Proj(tmp * (1 - (x[first].c / y[next].c)) / (1 - (y[first].c / y[next].c))));

                            if ((-z_y[k].c - y[prev].c).RadiusSquared < (z_y[k].c - y[prev].c).RadiusSquared)
                                z_y[k] = -z_y[k];
                        }
                    }

                    for (int k = 0; k < matingIterations - n; k++)
                    {
                        x[intermediateSteps * k + s] = z_x[k];
                        y[intermediateSteps * k + s] = z_y[k];
                    }
                }

                var d = y[first].c - 1;
                var c = 1 - x[first].c;
                var b = x[first].c * d;
                var a = y[first].c * c;

                ma[(int)frame] = new OpenTK.Vector2((float)a.R, (float)a.I);
                mb[(int)frame] = new OpenTK.Vector2((float)b.R, (float)b.I);
                mc[(int)frame] = new OpenTK.Vector2((float)c.R, (float)c.I);
                md[(int)frame] = new OpenTK.Vector2((float)d.R, (float)d.I);
                /**/
                Console.WriteLine("\n" + frame + ": " + n + " -> " + s);
                Console.WriteLine("\tma: " + ma[(int)frame]);
                Console.WriteLine("\tmb: " + mb[(int)frame]);
                Console.WriteLine("\tmc: " + mc[(int)frame]);
                Console.WriteLine("\tmd: " + md[(int)frame]);
                
            }
        }


        // How the mating variables interact with the window
        void UpdateFractal(FrameEventArgs e)
        {
            UpdateMating(e);

            KeyboardState k = Keyboard.GetState();

            // Movement on the projected complex plane, using IJKL (O and U for zooming in and out)

            if (k.IsKeyDown(Key.I))
                i += (float)(e.Time / Math.Pow(2, zoom));
            if (k.IsKeyDown(Key.K))
                i -= (float)(e.Time / Math.Pow(2, zoom));
            if (k.IsKeyDown(Key.L))
                r += (float)(e.Time / Math.Pow(2, zoom));
            if (k.IsKeyDown(Key.J))
                r -= (float)(e.Time / Math.Pow(2, zoom));
            if (k.IsKeyDown(Key.O))
                zoom += (float)e.Time * 2;
            if (k.IsKeyDown(Key.U))
                zoom -= (float)e.Time * 2;
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            
            // We add the time elapsed since last frame to the total amount of time passed.
            time += e.Time;

            if (fullscreen)
                WindowState = WindowState.Fullscreen;
            else
                WindowState = WindowState.Normal;

            if (Focused)
                camera.Input((float)e.Time);

            UpdateFractal(e);
            
            ResetCursor();
        }

        // Render loop
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);


            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.BindVertexArray(VertexArrayObject_Model);

            shader.Use();
            
            // frame = intermediate_steps * n + s
            // for each n
            //      for each s
            int s = (int) frame % intermediateSteps;
            int n = ((int) frame - s) / intermediateSteps;

            // Comment this out to remove console updatess
            Console.WriteLine("frame: " + (int)frame + " / " + (matingIterations*intermediateSteps) + "\n\tn: " + n + "\n\ts: " + s + "\n");

            shader.SetInt("maxIterations", maxIterations);
            shader.SetFloat("bailout", (float) bailout);
            shader.SetVector2("p", new OpenTK.Vector2((float) p.R, (float) p.I));
            shader.SetVector2("q", new OpenTK.Vector2((float) q.R, (float) q.I));
            shader.SetDouble("R_t", (float) R[s]);
            shader.SetInt("currentMatingIteration", n);
            
            shader.SetFloat("time", (float) time);
            shader.SetFloat("zoom", zoom);
            shader.SetFloat("rPos", r);
            shader.SetFloat("iPos", i);
            
            var ma_frame = new OpenTK.Vector2[n + 1];
            var mb_frame = new OpenTK.Vector2[n + 1];
            var mc_frame = new OpenTK.Vector2[n + 1];
            var md_frame = new OpenTK.Vector2[n + 1];

            for (int k = 0; k <= n; k++)
            {
                ma_frame[k] = ma[intermediateSteps * k + s];
                mb_frame[k] = mb[intermediateSteps * k + s];
                mc_frame[k] = mc[intermediateSteps * k + s];
                md_frame[k] = md[intermediateSteps * k + s];
            }


            shader.SetVector2Array("ma", ma_frame);
            shader.SetVector2Array("mb", mb_frame);
            shader.SetVector2Array("mc", mc_frame);
            shader.SetVector2Array("md", md_frame);





            shader.SetVector3("cPos", camera.Position);
            shader.SetVector3("pCenter", OpenTK.Vector3.Zero);
            shader.SetFloat("sideLength", sideLength);
            shader.SetFloat("n", 4);
            shader.SetFloat("minR", sideLength);
            shader.SetFloat("maxR", sideLength + (scale * sideLength));

            // Matrix4.Identity is used as the matrix, since we just want to draw it at 0, 0, 0
            var model = Matrix4.Identity;
            //model *= Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(5 * time));
            shader.SetMatrix4("model", model);
            shader.SetMatrix4("view", camera.GetViewMatrix());
            shader.SetMatrix4("projection", camera.GetProjectionMatrix());

            // Draw "sphere"
            GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Length);


            // Swap the two buffers
            //  1. buffer that is being displayed
            //  2. buffer that is being rendered to
            Context.SwapBuffers();
        }

        // END MATING CODE


        // DO NOT CHANGE

        // How much time has passed since the program began
        double time = 0;

        // Current mating frame
        float frame = 0;

        Camera camera;
        bool fullscreen = false;

        TexturedVertex[] vertices = new SubdividedQuad(10, 8, false).vertices;

        float sideLength = 10f;
        float scale = 0f;

        // Handles to OpenGL objects
        int VertexBufferObject;
        int VertexArrayObject_Model;

        Shader shader;

        int zoomMode = 0;

        // intermediate_steps
        double[] t = new double[16];
        double[] R = new double[16];

        // matingIterations * intermediateSteps
        ComplexFix[] x;
        ComplexFix[] y;

        // matingIterations * intermediateSteps
        OpenTK.Vector2[] ma;
        OpenTK.Vector2[] mb;
        OpenTK.Vector2[] mc;
        OpenTK.Vector2[] md;

        // Once all the frames of the mating have been calculated, the user is free to go back and forth using the arrow keys
        bool completed = false;


        public Game (int width, int height, string title) : base(width, height, GraphicsMode.Default, title) { }

        void ResetCamera()
        {
            camera = new Camera(OpenTK.Vector3.UnitZ * 3, Width / (float)Height);

            camera.Position = new OpenTK.Vector3(0, 0, cameraStartDist);
            camera.target = new OpenTK.Vector3(0, 0, 10.1f);

            camera.cameraLock = true;

            // start by facing the south pole
            if (southPole)
                camera.ArcBallPitch(45, Camera.Type.FREE, false, true);
        }

        //@Fix error where, if you move the mouse fast enough, the cursor leaves the screen
        void ResetCursor()
        {
            if (Focused)
            {
                Mouse.SetPosition(X + Width / 2f, Y + Height / 2f);
                camera.lastMousePos = new OpenTK.Vector2(Mouse.GetCursorState().X, Mouse.GetCursorState().Y);
            }
        }

        // This function runs one time, when the window first opens
        // Any initialization-related code should go here
        protected override void OnLoad(EventArgs e)
        {
            InitializeMating();
            UpdateMating(null);

            GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            
            // Generates buffer ID
            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, TexturedVertex.Size * vertices.Length, vertices, BufferUsageHint.StaticDraw);

            // Create and enable shaders
            shader = new Shader("../../Shaders/shader.vert", "../../Shaders/shader.frag");

            VertexArrayObject_Model = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject_Model);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);

            // Enable variable "aPosition" in the vertex shader
            var vertexLocation = shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);

            // We now need to define the layout of the normal so the shader can use it
            var normalLocation = shader.GetAttribLocation("aNormal");
            GL.EnableVertexAttribArray(normalLocation);
            GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));

            // The texture coords have now been added
            var texCoordLocation = shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));

            // Camera initialization
            ResetCamera();
            ResetCursor();

            // We make the mouse cursor invisible so we can have proper FPS-camera movement
            CursorVisible = false;
        }

        // Used to fix camera jerk upon window focus
        protected override void OnFocusedChanged(EventArgs e)
        {
            base.OnFocusedChanged(e);

            if (Focused)
            {
                ResetCursor();
            }

        }

        // This function runs every time the window get resized
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // Maps the Normalized Device Coordinates (NDC) to the window's screen-space coordinates
            // If this doesn't happen, the NDC will no longer be correct.
            GL.Viewport(0, 0, Width, Height);

            camera.AspectRatio = Width / (float)Height;
        }

        // After program ends, clean up buffers
        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);

            // Unbind all resources by binding the targets to NULL
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            // Delete all the resources
            GL.DeleteBuffer(VertexBufferObject);
            GL.DeleteVertexArray(VertexArrayObject_Model);

            // Dispose of the shader and textures
            shader.Dispose();
        }

        // Occurs whenever a keyboard key is pressed
        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Key == Key.Escape)                    // <Esc>
            {
                Exit();
            }
            if (e.Key == camera.KeyMap["fullscreen"])   // F11
            {
                fullscreen = !fullscreen;
            }
            if (e.Key == camera.KeyMap["cameraMode"])   // B
            {
                if (camera.type == Camera.Type.FPS)
                    camera.type = Camera.Type.FREE;
                else
                    camera.type = Camera.Type.FPS;
            }
            if (e.Key == camera.KeyMap["cameraLock"])   // N
            {
                camera.cameraLock = !camera.cameraLock;
            }
            if (e.Key == camera.KeyMap["displayInfo"])  // /
            {
                Console.WriteLine(camera);
                Console.WriteLine("FRACTAL: " + r + ", " + i + " with zoom " + zoom);

            }
            if (e.Key == camera.KeyMap["right"])        // Right arrow key
            {
                zoomMode = 1;
            }
            if (e.Key == camera.KeyMap["down"])         // Down arrow key
            {
                zoomMode = 0;
            }
            if (e.Key == camera.KeyMap["left"])         // Left arrow key
            {
                zoomMode = -1;
            }
        }

        public Bitmap TakeScreenshot()
        {
            if (GraphicsContext.CurrentContext == null)
                throw new GraphicsContextMissingException();

            int w = ClientSize.Width;
            int h = ClientSize.Height;
            Bitmap bmp = new Bitmap(w, h);

            System.Drawing.Imaging.BitmapData data = bmp.LockBits(ClientRectangle, System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            GL.ReadPixels(0, 0, w, h, PixelFormat.Bgr, PixelType.UnsignedByte, data.Scan0);
            bmp.UnlockBits(data);

            return bmp;
        }

    }
}
