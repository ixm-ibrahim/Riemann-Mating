using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using System;
using System.Drawing;

namespace OpenTK_Reimann_Mating
{
    /* Problems:
     *      1. The closer 's' is to zero, the more infinite-points (represented as blue) there are - which obscures the outer 'q' Julia Set
     *          Note: the blue represtation can be commented out in shader.frag, but this will only replace the blue with some other color - causing the same problem with the rendering
     *      2. When s is zero, the entire sphere is somehow colored as if it's the outer 'q' Julia Set
     *      3. When the last n is being displayed (going through all the s for that n), the fractals have slightly different shapes - it's not a smooth transition, even if problems 1 and 2 were not there
     *          Note: this can be easily seen if the projection is zoomed in, so that q mates into p on the north pole (rather than the original p mating with q on the south pole)
     * 
     * 
     * Controls:
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
        Complex p = new Complex(-1, 0);
        Complex q = new Complex(-.123f, .745f);

        // Mating values
        int maxIterations = 100;
        double bailout = 100;
        int matingIterations = 20;      // Currently cannot exceed 25, or there will be problems with the shader
        int intermediateSteps = 20;

        // The higher, the more zoomed in on the Riemann Sphere
        float zoom = -4f;   // viewing as if the complex plane is flat, with p mating into q (viewed from the south pole)
        //float zoom = 0f;    // normal Riemann Sphere zoom
        //float zoom = 4f;    // viewing as if the complex plane is flat, with q mating into p (viewed from the north pole)

        // Starting position on the complex plane (centered at south pole of Riemann Sphere)
        float r = 0;
        float i = 0;

        const double R1 = 1e10;
        //const double R2 = 1e20;
        //const double R4 = 1e40;


        // MATING CODE


        // Note: these two need to be called in OnLoad()
        void InitializeMating()
        {
            frame = -1;

            t = new double[intermediateSteps];
            R = new double[intermediateSteps];

            x = new Complex[matingIterations * intermediateSteps];
            y = new Complex[matingIterations * intermediateSteps];

            ma = new Vector2[matingIterations * intermediateSteps];
            mb = new Vector2[matingIterations * intermediateSteps];
            mc = new Vector2[matingIterations * intermediateSteps];
            md = new Vector2[matingIterations * intermediateSteps];

            //var tmp = new Complex[intermediateSteps];

            for (int s = 0; s < intermediateSteps; s++)
            {
                t[s] = (s + .5) / intermediateSteps;
                R[s] = Math.Exp(Math.Pow(2, 1 - t[s]) * Math.Log(R1));

                // only used for speeding up calculations for the x_t and y_t arrays
                //tmp[s] = (1 + ((1 - t[s]) * q / R2)) / (1 + ((1 - t[s]) * p / R2));
            }

            var p_i = Complex.Zero;
            var q_i = Complex.Zero;

            for (int i = 0; i < matingIterations * intermediateSteps; i++)
            {
                int s = i % intermediateSteps;
                /*
                x[i] = Complex.Proj(tmp[s] * (p_i / R[s]) / (1 + ((1 - t[s]) * q / R4 * (p_i - p))));
                y[i] = Complex.Proj(tmp[s] * (R[s] / q_i) * (1 + ((1 - t[s]) * p / R4 * (q_i - q))));
                */

                x[i] = p_i / R[s];
                y[i] = R[s] / q_i;

                if (s == intermediateSteps - 1)
                {
                    p_i = (p_i ^ 2) + p;
                    q_i = (q_i ^ 2) + q;
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
                    frame += (float)e.Time * intermediateSteps / 2;
                else if (zoomMode == -1)
                    frame -= (float)e.Time * intermediateSteps / 2;

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
                    var z_x = new Complex[matingIterations - n];
                    var z_y = new Complex[matingIterations - n];

                    var tmp2 = (1 - y[first]) / (1 - x[first]);

                    for (int k = 0; k < matingIterations - n; k++)
                    {
                        int k_next = k + 1;
                        int next = intermediateSteps * k_next + s;
                        int prev = intermediateSteps * k + ((s + intermediateSteps - 1) % intermediateSteps);

                        z_x[k] = Complex.Sqrt(Complex.Proj(tmp2 * (x[next] - x[first]) / (x[next] - y[first])));
                        z_y[k] = Complex.Sqrt(Complex.Proj(tmp2 * (1 - (x[first] / y[next])) / (1 - (y[first] / y[next]))));

                        //Console.WriteLine(k + " (" + n + ")\n\t" + z_y[k] + "\n");
                        /**/
                        if ((-z_x[k] - x[prev]).RadiusSquared < (z_x[k] - x[prev]).RadiusSquared)
                            z_x[k] = -z_x[k];
                        if ((-z_y[k] - y[prev]).RadiusSquared < (z_y[k] - y[prev]).RadiusSquared)
                            z_y[k] = -z_y[k];
                    }

                    for (int k = 0; k < matingIterations - n; k++)
                    {
                        x[intermediateSteps * k + s] = z_x[k];
                        y[intermediateSteps * k + s] = z_y[k];
                    }
                }

                var d = y[first] - 1;
                var c = 1 - x[first];
                var b = x[first] * d;
                var a = y[first] * c;

                ma[(int)frame] = new Vector2((float)a.R, (float)a.I);
                mb[(int)frame] = new Vector2((float)b.R, (float)b.I);
                mc[(int)frame] = new Vector2((float)c.R, (float)c.I);
                md[(int)frame] = new Vector2((float)d.R, (float)d.I);
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

            // Check to see if the window is focused
            if (!Focused)
                return;

            if (fullscreen)
                WindowState = WindowState.Fullscreen;
            else
                WindowState = WindowState.Normal;

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

            Console.WriteLine("frame: " + frame + " / " + (matingIterations*intermediateSteps) + "\n\tn: " + n + "\n\ts: " + s + "\n");

            // This deals with the 2 frames where the shader's float precision is too little
            if (completed && s < 2)
            {
                if (zoomMode == 1)
                {
                    frame += 2 - s;
                    s = 2;
                }
                else if (zoomMode == -1)
                {
                    frame -= s + 1;
                    s = intermediateSteps - 1;
                    n--;
                }
            }


            shader.SetInt("maxIterations", maxIterations);
            shader.SetFloat("bailout", (float) bailout);
            shader.SetVector2("p", new Vector2((float) p.R, (float) p.I));
            shader.SetVector2("q", new Vector2((float) q.R, (float) q.I));
            shader.SetDouble("R_t", (float) R[s]);
            shader.SetInt("currentMatingIteration", n);
            
            shader.SetFloat("time", (float) time);
            shader.SetFloat("zoom", zoom);
            shader.SetFloat("rPos", r);
            shader.SetFloat("iPos", i);
            
            var ma_frame = new Vector2[n + 1];
            var mb_frame = new Vector2[n + 1];
            var mc_frame = new Vector2[n + 1];
            var md_frame = new Vector2[n + 1];

            for (int k = 0; k <= n; k++)
            {
                ma_frame[k] = ma[intermediateSteps * k + s];
                mb_frame[k] = mb[intermediateSteps * k + s];
                mc_frame[k] = mc[intermediateSteps * k + s];
                md_frame[k] = md[intermediateSteps * k + s];
                /*
                if (!completed)
                {
                    Console.WriteLine("k: " + k);
                    Console.WriteLine("\tma: " + ma_frame[k]);
                    Console.WriteLine("\tmb: " + mb_frame[k]);
                    Console.WriteLine("\tmc: " + mc_frame[k]);
                    Console.WriteLine("\tmd: " + md_frame[k]);
                    Console.WriteLine();
                }*/
            }


            shader.SetVector2Array("ma", ma_frame);
            shader.SetVector2Array("mb", mb_frame);
            shader.SetVector2Array("mc", mc_frame);
            shader.SetVector2Array("md", md_frame);





            shader.SetVector3("cPos", camera.Position);
            shader.SetVector3("pCenter", Vector3.Zero);
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
        Vector3 start = new Vector3(0, 0, 17.01f);
        Vector3 target = new Vector3(0, 0, 10.1f);

        // Handles to OpenGL objects
        int VertexBufferObject;
        int VertexArrayObject_Model;

        Shader shader;

        int zoomMode = 0;

        // intermediate_steps
        double[] t = new double[16];
        double[] R = new double[16];

        // matingIterations * intermediateSteps
        Complex[] x;
        Complex[] y;

        // matingIterations * intermediateSteps
        Vector2[] ma;
        Vector2[] mb;
        Vector2[] mc;
        Vector2[] md;

        // Once all the frames of the mating have been calculated, the user is free to go back and forth using the arrow keys
        bool completed = false;


        public Game (int width, int height, string title) : base(width, height, GraphicsMode.Default, title) { }

        void ResetCamera()
        {
            camera = new Camera(Vector3.UnitZ * 3, Width / (float)Height);

            camera.Position = start;
            camera.target = target;

            // start by facing the south pole
            camera.ArcBallPitch(45, Camera.Type.FREE, false, true);
        }

        //@Fix error where, if you move the mouse fast enough, the cursor leaves the screen
        void ResetCursor()
        {
            if (Focused)
            {
                Mouse.SetPosition(X + Width / 2f, Y + Height / 2f);
                camera.lastMousePos = new Vector2(Mouse.GetCursorState().X, Mouse.GetCursorState().Y);
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
