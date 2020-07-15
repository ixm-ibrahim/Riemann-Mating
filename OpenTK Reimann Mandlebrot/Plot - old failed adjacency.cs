using OpenTK;
using System;
using System.Linq;
using System.Collections.Generic;

namespace OpenTK_Reimann_Mandlebrot
{

    public struct TexturedVertex
    {
        public const int Size = (3 + 3 + 2) * sizeof(float); // size of struct in bytes
        // public const int Size = sizeof(Vector2) + sizeof(Vector3); // size of struct in bytes

        public readonly Vector3 position;
        public readonly Vector3 normal;
        public readonly Vector2 textureCoordinate;

        public TexturedVertex(Vector3 position, Vector3 normal, Vector2 textureCoordinate)
        {
            this.position = position;
            this.normal = normal;
            this.textureCoordinate = textureCoordinate;
        }
    }

    public class TriangleFace
    {
        public Vector3[] verticies;
        public Vector3 normal;

        public TriangleFace(Vector3 a, Vector3 b, Vector3 c)
        {
            verticies = new Vector3[3];

            verticies[0] = a;
            verticies[1] = b;
            verticies[2] = c;

            normal = GetNormal();
        }

        public TriangleFace(Vector3 a, Vector3 b, Vector3 c, Vector3 norm)
        {
            verticies = new Vector3[3];

            verticies[0] = a;
            verticies[1] = b;
            verticies[2] = c;

            normal = norm;
        }

        public Vector3 this[int index]
        {
            get { return verticies[index]; }

            set
            {
                verticies[index] = value;

                normal = GetNormal();
            }
        }

        public static bool operator ==(TriangleFace a, TriangleFace b)
        {
            return a.verticies.All(b.verticies.Contains) && a.normal == b.normal;
        }
        public static bool operator !=(TriangleFace a, TriangleFace b)
        {
            return !(a == b);
        }
        Vector3 GetNormal()
        {
            return Vector3.Cross(verticies[1] - verticies[0], verticies[2] - verticies[0]).Normalized();
        }
    }

    // http://sol.gfxile.net/sphere/index.html
    public class IcoSphere
    {
        private List<Vector3> points;
        private int index;
        private Dictionary<long, int> middlePointIndexCache;

        float maxR;
        float maxI;
        float minR;
        float minI;

        public TexturedVertex[] Create(int recursionLevel)
        {
            middlePointIndexCache = new Dictionary<long, int>();
            points = new List<Vector3>();
            index = 0;
            var t = (float)((1.0 + Math.Sqrt(5.0)) / 2.0);
            var s = 1;

            AddVertex(new Vector3(-s, t, 0));
            AddVertex(new Vector3(s, t, 0));
            AddVertex(new Vector3(-s, -t, 0));
            AddVertex(new Vector3(s, -t, 0));

            AddVertex(new Vector3(0, -s, t));
            AddVertex(new Vector3(0, s, t));
            AddVertex(new Vector3(0, -s, -t));
            AddVertex(new Vector3(0, s, -t));

            AddVertex(new Vector3(t, 0, -s));
            AddVertex(new Vector3(t, 0, s));
            AddVertex(new Vector3(-t, 0, -s));
            AddVertex(new Vector3(-t, 0, s));

            var faces = new List<TriangleFace>();

            // 5 faces around point 0
            faces.Add(new TriangleFace(points[0], points[11], points[5]));
            faces.Add(new TriangleFace(points[0], points[5], points[1]));
            faces.Add(new TriangleFace(points[0], points[1], points[7]));
            faces.Add(new TriangleFace(points[0], points[7], points[10]));
            faces.Add(new TriangleFace(points[0], points[10], points[11]));

            // 5 adjacent faces 
            faces.Add(new TriangleFace(points[1], points[5], points[9]));
            faces.Add(new TriangleFace(points[5], points[11], points[4]));
            faces.Add(new TriangleFace(points[11], points[10], points[2]));
            faces.Add(new TriangleFace(points[10], points[7], points[6]));
            faces.Add(new TriangleFace(points[7], points[1], points[8]));

            // 5 faces around point 3
            faces.Add(new TriangleFace(points[3], points[9], points[4]));
            faces.Add(new TriangleFace(points[3], points[4], points[2]));
            faces.Add(new TriangleFace(points[3], points[2], points[6]));
            faces.Add(new TriangleFace(points[3], points[6], points[8]));
            faces.Add(new TriangleFace(points[3], points[8], points[9]));

            // 5 adjacent faces 
            faces.Add(new TriangleFace(points[4], points[9], points[5]));
            faces.Add(new TriangleFace(points[2], points[4], points[11]));
            faces.Add(new TriangleFace(points[6], points[2], points[10]));
            faces.Add(new TriangleFace(points[8], points[6], points[7]));
            faces.Add(new TriangleFace(points[9], points[8], points[1]));


            // subdivide triangles
            for (int i = 0; i < recursionLevel; i++)
            {
                var faces2 = new List<TriangleFace>();

                foreach (var tri in faces)
                {
                    // replace triangle by 4 triangles
                    int a = GetMiddlePoint(tri[0], tri[1]);
                    int b = GetMiddlePoint(tri[1], tri[2]);
                    int c = GetMiddlePoint(tri[2], tri[0]);

                    faces2.Add(new TriangleFace(tri[0], points[a], points[c]));
                    faces2.Add(new TriangleFace(tri[1], points[b], points[a]));
                    faces2.Add(new TriangleFace(tri[2], points[c], points[b]));
                    faces2.Add(new TriangleFace(points[a], points[b], points[c]));
                }

                faces = faces2;
            }
            Console.WriteLine(faces.Count);
            // done, now add triangles to mesh
            var vertices = new List<TexturedVertex>();

            foreach (var tri in faces)
            {
                foreach (var v in tri.verticies)
                {
                    var tmp = v.Z == 1 ? float.MaxValue : (1 + (v.Z + 1) / (1 - v.Z)) / 2f;
                    var r = v.X * tmp;
                    var i = v.Y * tmp;

                    maxR = Math.Max(r, maxR);
                    minR = Math.Min(r, minR);
                    maxI = Math.Max(i, maxI);
                    minI = Math.Min(i, minI);
                }
            }

            foreach (var tri in faces)
            {
                /*
                var uv1 = GetSphereCoord(tri[0]);
                var uv2 = GetSphereCoord(tri[1]);
                var uv3 = GetSphereCoord(tri[2]);
                */
/*
                var uv1 = GetSphereCoord2(tri[0]);
                var uv2 = GetSphereCoord2(tri[1]);
                var uv3 = GetSphereCoord2(tri[2]);
*/
                
                var uv1 = GetReimannCoord(tri[0]);
                var uv2 = GetReimannCoord(tri[1]);
                var uv3 = GetReimannCoord(tri[2]);
                

                vertices.Add(new TexturedVertex(tri[0], tri.normal, uv1));
                vertices.Add(new TexturedVertex(tri[1], tri.normal, uv2));
                vertices.Add(new TexturedVertex(tri[2], tri.normal, uv3));
            }

            return vertices.ToArray();
        }

        // https://dreamstatecoding.blogspot.com/search?q=texturedvertex
        public static Vector2 GetSphereCoord(Vector3 v)
        {
            Vector2 uv;
            var len = v.Length;

            uv.Y = (float)(Math.Acos(v.Y / len) / Math.PI);
            uv.X = -(float)((Math.Atan2(v.Z, v.X) / Math.PI + 1.0f) * 0.5f);

            return uv;
        }

        public static Vector2 GetSphereCoord2(Vector3 v)
        {
            Vector2 uv;
            var r = v.Length;

            var theta = Math.Atan2(v.Z, v.X);
            var phi = Math.Acos(-v.Y / r);

            phi /= Math.Abs(Math.Cos(theta % Math.PI / 4));

            uv.X = (float) (phi * Math.Cos(theta));
            uv.Y = (float) (phi * Math.Sin(theta));

            uv.X = (float)((uv.X - -Math.PI) / (Math.PI - -Math.PI) * (1 - 0) + 0);
            uv.Y = (float)((uv.Y - -Math.PI) / (Math.PI - -Math.PI) * (1 - 0) + 0);

            return uv;
        }

        private Vector2 GetReimannCoord(Vector3 v)
        {
            
            if (v.Y > .99f)
                v.Y = .99f;
            /**/
            var tmp = (1 + (v.Y + 1) / (1 - v.Y)) / 2f;
            var r = v.X * tmp;
            var i = v.Z * tmp;

            //r = (r - minR) / (maxR - minR) * (1 - 0) + 0;
            //i = (i - minI) / (maxI - minI) * (1 - 0) + 0;

            //r = (float)(1 / (1 + Math.Exp(-r * Math.Log(.001) / maxR)));
            //i = (float)(1 / (1 + Math.Exp(-i * Math.Log(.001) / maxI)));

            //r = (float)(Math.Sin(.5 * r * Math.PI / maxR) * (1 - .5) + .5);
            //i = (float)(Math.Sin(.5 * i * Math.PI / maxI) * (1 - .5) + .5);
            /*
            if (r >= 0)
                r = (float)(Math.Sqrt(maxR*maxR - (r - maxR)*(r - maxR)) / (2 * maxR) + .5);
            else
                r = (float)(-Math.Sqrt(maxR * maxR - (-r - maxR) * (-r - maxR)) / (2 * maxR) + .5);

            if (i >= 0)
                i = (float)(Math.Sqrt(maxI * maxI - (i - maxI) * (i - maxI)) / (2 * maxI) + .5);
            else
                i = (float)(-Math.Sqrt(maxI * maxI - (-i - maxI) * (-i - maxI)) / (2 * maxI) + .5);
            */
            /*
            var er = .5 * (Math.Sqrt(maxR * maxR + 8) - maxR);
            var ei = .5 * (Math.Sqrt(maxI * maxI + 8) - maxI);
            if (r >= 0)
                r = (float)(1 - 1 / (maxR * (r + er)) + 1 / (maxR * (maxR + er)));
            else
                r = (float)(1 / (maxR * (-r + er)) - 1 / (maxR * (maxR + er)));

            if (i >= 0)
                i = (float)(1 - 1 / (maxI * (i + ei)) + 1 / (maxI * (maxI + ei)));
            else
                i = (float)(1 / (maxI * (-i + ei)) - 1 / (maxI * (maxI + ei)));
            */
            /*
            var a = .5;
            var br = .5 * (Math.Sqrt(maxR * (maxR + 8 * a)) - 3 * maxR);
            var bi = .5 * (Math.Sqrt(maxI * (maxI + 8 * a)) - 3 * maxI);
            if (r >= 0)
                r = (float)(1 - a / (maxR + r + br) + a / (2 * maxR + br));
            else
                r = (float)(a / (maxR - r + br) - a / (2 * maxR + br));

            if (i >= 0)
                i = (float)(1 - a / (maxI + i + bi) + a / (2 * maxI + bi));
            else
                i = (float)(a / (maxI - i + bi) - a / (2 * maxI + bi));
                */
            /*
            if (r >= 0)
                r = -(r-maxR)*(r-maxR)/(2*maxR*maxR) + 1;
            else
                r = (-r - maxR) * (-r - maxR) / (2 * maxR * maxR);

            if (i >= 0)
                i = -(i - maxI) * (i - maxI) / (2 * maxI * maxI) + 1;
            else
                i = (-i - maxI) * (-i - maxI) / (2 * maxI * maxI);
                */
            /**/
            float a = 1;
            if (r >= 0)
                r = (float)(Math.Log(1 + a * r - 0) / Math.Log(1 + a * maxR - 0) * (1 - .5) + .5);
            else
                r = (float)(-Math.Log(1 + -a * r - 0) / Math.Log(1 + a * maxR - 0) * (1 - .5) + .5);

            if (i >= 0)
                i = (float)(Math.Log(1 + a * i - 0) / Math.Log(1 + a * maxI - 0) * (1 - .5) + .5);
            else
                i = (float)(-Math.Log(1 + -a * i - 0) / Math.Log(1 + a * maxI - 0) * (1 - .5) + .5);
            


            return new Vector2(r, i);
        }

        private int AddVertex(Vector3 p)
        {
            points.Add(p.Normalized());

            return index++;
        }

        // return index of point in the middle of p1 and p2
        private int GetMiddlePoint(Vector3 point1, Vector3 point2)
        {
            long i1 = points.IndexOf(point1);
            long i2 = points.IndexOf(point2);

            // first check if we have it already
            var firstIsSmaller = i1 < i2;
            long smallerIndex = firstIsSmaller ? i1 : i2;
            long greaterIndex = firstIsSmaller ? i2 : i1;
            long key = (smallerIndex << 32) + greaterIndex;

            if (middlePointIndexCache.TryGetValue(key, out int ret))
                return ret;

            // not in cache, calculate it

            var middle = new Vector3(
                (point1.X + point2.X) / 2.0f,
                (point1.Y + point2.Y) / 2.0f,
                (point1.Z + point2.Z) / 2.0f);

            // add vertex makes sure point is on unit sphere
            int i = AddVertex(middle);

            // store it, return index
            middlePointIndexCache.Add(key, i);
            return i;
        }
    }

    // http://www.songho.ca/opengl/gl_sphere.html#cubesphere
    public class CubeSphere
    {
        public class QuadFace
        {
            public enum Side
            {
                NONE = -1, BOTTOM = 0, FRONT, RIGHT, BACK, LEFT, TOP
            };

            public Vector3[] vertices;
            public Vector3 center;

            public QuadFace nextA;  // bottom link
            public QuadFace nextB;  // right link
            public QuadFace nextC;  // top link
            public QuadFace nextD;  // left link

            public QuadFace subdivisonA;    // bottom-left subdivison
            public QuadFace subdivisonB;    // bottom-right subdivision
            public QuadFace subdivisonC;    // top-right subdivision
            public QuadFace subdivisonD;    // top-left subdivision

            public Side side;
            public int subdivion = 0;

            public QuadFace(Vector3 a, Vector3 b, Vector3 c, Vector3 d, Side side = Side.NONE)
            {
                vertices = new Vector3[4];

                vertices[0] = a;
                vertices[1] = b;
                vertices[2] = c;
                vertices[3] = d;

                this.side = side;

                center = (vertices[0] + vertices[1] + vertices[2] + vertices[3]) / 4;
            }

            public Vector3 this[int index]
            {
                get { return vertices[index]; }

                set
                {
                    vertices[index] = value;

                    center = (vertices[0] + vertices[1] + vertices[2] + vertices[3]) / 4;
                }
            }
        
            public TriangleFace triangleDAB => new TriangleFace(vertices[3], vertices[0], vertices[1]);

            public TriangleFace triangleABC => new TriangleFace(vertices[0], vertices[1], vertices[2]);

            public TriangleFace triangleBCD => new TriangleFace(vertices[1], vertices[2], vertices[3]);

            public TriangleFace triangleCDA => new TriangleFace(vertices[2], vertices[3], vertices[0]);
            public void Subdivide()
            {
                // replace quad by 4 quads
                var a = (vertices[0] + vertices[1]) / 2;   // between 0 and 1
                var b = (vertices[1] + vertices[2]) / 2;   // between 1 and 2
                var c = (vertices[2] + vertices[3]) / 2;   // between 2 and 3
                var d = (vertices[3] + vertices[0]) / 2;   // between 3 and 0

                subdivisonA = new QuadFace(vertices[0], a, center, d, side);    // bottom-left
                subdivisonB = new QuadFace(vertices[1], b, center, a, side);    // bottom-right
                subdivisonC = new QuadFace(vertices[2], c, center, b, side);    // top-right
                subdivisonD = new QuadFace(vertices[3], d, center, c, side);    // top-left

                subdivion++;
            }

            public void SphericalSubdivide(float radius = 1)
            {
                // Get midpoints and center
                var a = ((vertices[0] + vertices[1]) / 2).Normalized();   // bottom midpoint
                var b = ((vertices[1] + vertices[2]) / 2).Normalized();   // right midpoint
                var c = ((vertices[2] + vertices[3]) / 2).Normalized();   // top midpoint
                var d = ((vertices[3] + vertices[0]) / 2).Normalized();   // left midpoint
                var center = radius * this.center.Normalized();    // center of quad\

                // Create subdivisons
                subdivisonA = new QuadFace(vertices[0], a, center, d, side);    // bottom-left corner
                subdivisonB = new QuadFace(a, vertices[1], b, center, side);    // bottom-right corner
                subdivisonC = new QuadFace(center, b, vertices[2], c, side);    // top-right corner
                subdivisonD = new QuadFace(d, center, c, vertices[3], side);    // top-left corner

                // Update links
                subdivisonA.nextA = nextA;          // bottom link to original bottom link
                subdivisonA.nextB = subdivisonB;    // right link to bottom-right subdivision
                subdivisonA.nextC = subdivisonD;    // top link to top-left subdivison
                subdivisonA.nextD = nextD;          // left link to original left link

                subdivisonB.nextA = nextA;          // bottom link to original bottom link
                subdivisonB.nextB = nextB;          // right link to original right link
                subdivisonB.nextC = subdivisonC;    // top link to top-right subdivison
                subdivisonB.nextD = subdivisonA;    // left link to bottom-left subdivison

                subdivisonC.nextA = subdivisonB;    // bottom link to bottom-right subdivision
                subdivisonC.nextB = nextB;          // right link to original right link
                subdivisonC.nextC = nextC;          // top link to original top link
                subdivisonC.nextD = subdivisonD;    // left link to top-left subdivision

                subdivisonD.nextA = subdivisonA;    // bottom link to bottom-left subdivision
                subdivisonD.nextB = subdivisonC;    // right link to top-right subdivision
                subdivisonD.nextC = nextC;          // top link to original top link
                subdivisonD.nextD = nextD;          // left link to original left link

                subdivion++;
            }
        }

        private List<Vector3> points;
        int index = 0;

        float maxR;
        float maxI;
        float minR;
        float minI;

        public TexturedVertex[] Create(int subdivisions, double radius, bool riemannTexturing = false)
        {
            points = new List<Vector3>();
            var s = (float)(radius / Math.Sqrt(3.0));  // normalized magnitude of cube vertex on sphere with radius

            // Bottom vertices
            AddVertex(new Vector3(-s, -s, -s)); // 0
            AddVertex(new Vector3(s, -s, -s));  // 1
            AddVertex(new Vector3(s, s, -s));   // 2
            AddVertex(new Vector3(-s, s, -s));  // 3
            // Top vertices
            AddVertex(new Vector3(-s, -s, s));  // 4
            AddVertex(new Vector3(s, -s, s));   // 5
            AddVertex(new Vector3(s, s, s));    // 6
            AddVertex(new Vector3(-s, s, s));   // 7


            /* All faces are generated counter-clockwise, except for the top and bottom, which are flipped accross the vertical axis (B and D are switched):
             * 
             *             7------6
             *             |  5   |                             
             *             |  T   |                             C
             *      7------4------5------6------7               ^
             *      |  4   |  1   |  2   |  3   |               |
             *      |  L   |  F   |  R   |  Ba  |         D <---+---> B
             *      3------0------1------2------3               |
             *             |  0   |                             V
             *             |  Bo  |                             A
             *             3------2                             
             */
            
            var faces = new List<QuadFace>
            {
                new QuadFace(points[3], points[2], points[1], points[0], QuadFace.Side.BOTTOM),     // bottom
                new QuadFace(points[0], points[1], points[5], points[4], QuadFace.Side.FRONT),      // front
                new QuadFace(points[1], points[2], points[6], points[5], QuadFace.Side.RIGHT),      // right
                new QuadFace(points[2], points[3], points[7], points[6], QuadFace.Side.BACK),       // back
                new QuadFace(points[3], points[0], points[4], points[7], QuadFace.Side.LEFT),       // left
                new QuadFace(points[4], points[5], points[6], points[7], QuadFace.Side.TOP)         // top
            };

            // bottom links
            faces[0].nextA = faces[3];  // back
            faces[0].nextB = faces[2];  // right
            faces[0].nextC = faces[1];  // front
            faces[0].nextD = faces[4];  // left
            // front links
            faces[1].nextA = faces[0];  // bottom
            faces[1].nextB = faces[2];  // right
            faces[1].nextC = faces[5];  // top
            faces[1].nextD = faces[4];  // left
            // right links
            faces[2].nextA = faces[0];  // bottom
            faces[2].nextB = faces[3];  // back
            faces[2].nextC = faces[5];  // top
            faces[2].nextD = faces[1];  // front
            // back links
            faces[3].nextA = faces[0];  // bottom
            faces[3].nextB = faces[4];  // left
            faces[3].nextC = faces[5];  // top
            faces[3].nextD = faces[2];  // right
            // left links
            faces[4].nextA = faces[0];  // bottom
            faces[4].nextB = faces[1];  // front
            faces[4].nextC = faces[5];  // top
            faces[4].nextD = faces[3];  // back
            // top links
            faces[5].nextA = faces[1];  // front
            faces[5].nextB = faces[2];  // right
            faces[5].nextC = faces[3];  // back
            faces[5].nextD = faces[4];  // left

            // Subdivide cube
            for (int i = 0; i < subdivisions; i++)
            {
                var subdividedFaces = new List<QuadFace>();

                // Subdivide each quad
                for (int e = 0; e < faces.Count; e++)
                    faces[e].SphericalSubdivide();

                // Update and add subdivided quads to list of total faces
                for (int e = 0; e < faces.Count; e++)
                {
                    // Update links that are linked to original quad instead of recently created subdivisons
                    // bottom-left
                    faces[e].subdivisonA.nextA = faces[e].nextA.subdivisonD;          // bottom link to original bottom link
                    faces[e].subdivisonA.nextD = faces[e].nextD.subdivisonB;          // left link to original left link
                    // bottom-right
                    faces[e].subdivisonB.nextA = faces[e].nextA.subdivisonC;          // bottom link to original bottom link
                    faces[e].subdivisonB.nextB = faces[e].nextB.subdivisonA;          // right link to original right link
                    // top-right
                    faces[e].subdivisonC.nextB = faces[e].nextB.subdivisonD;          // right link to original right link
                    faces[e].subdivisonC.nextC = faces[e].nextC.subdivisonB;          // top link to original top link
                    // top-left
                    faces[e].subdivisonD.nextC = faces[e].nextC.subdivisonA;          // top link to original top link
                    faces[e].subdivisonD.nextD = faces[e].nextD.subdivisonC;          // left link to original left link
                    

                    // Add subdivided faces
                    subdividedFaces.Add(faces[e].subdivisonA);
                    subdividedFaces.Add(faces[e].subdivisonB);
                    subdividedFaces.Add(faces[e].subdivisonC);
                    subdividedFaces.Add(faces[e].subdivisonD);
                }

                faces = subdividedFaces;
            }
            
            // done, now add triangles to mesh
            var vertices = new List<TexturedVertex>();

            foreach (var quad in faces)
            {
                foreach (var v in quad.vertices)
                {
                    var tmp = v.Z == 1 ? float.MaxValue : (1 + (v.Z + 1) / (1 - v.Z)) / 2f;
                    var r = v.X * tmp;
                    var i = v.Y * tmp;

                    maxR = Math.Max(r, maxR);
                    minR = Math.Min(r, minR);
                    maxI = Math.Max(i, maxI);
                    minI = Math.Min(i, minI);
                }
            }

            // Add the vertices for 2 triangles for each quad (DAB and BCD)
            foreach (var quad in faces)
            {
                Vector3 normA;
                Vector3 normB;
                Vector3 normC;
                Vector3 normD;
                Vector2 uvA;
                Vector2 uvB;
                Vector2 uvC;
                Vector2 uvD;

                // Get texture coordinates
                if (riemannTexturing)
                {
                    /*
                    uvA = GetReimannCoord(quad[0]);
                    uvB = GetReimannCoord(quad[1]);
                    uvC = GetReimannCoord(quad[2]);
                    uvD = GetReimannCoord(quad[3]);
                    */
                    uvA = GetSphereCoord2(quad[0]);
                    uvB = GetSphereCoord2(quad[1]);
                    uvC = GetSphereCoord2(quad[2]);
                    uvD = GetSphereCoord2(quad[3]);
                }
                else
                {
                    uvA = GetSphereCoord(quad[0]);
                    uvB = GetSphereCoord(quad[1]);
                    uvC = GetSphereCoord(quad[2]);
                    uvD = GetSphereCoord(quad[3]);
                }

                /*      
                 *      D------C    D------C
                 *      |  D   |    |  C   |
                 *      |      |    |      |
                 *      A------B    A------B
                 *      
                 *      D------C    D------C
                 *      |  A   |    |  B   |
                 *      |      |    |      |
                 *      A------B    A------B
                 */

                // Get average normals for each point
                normA = (quad.triangleDAB.normal + quad.nextD.triangleABC.normal + quad.nextD.nextA.triangleBCD.normal + quad.nextA.triangleCDA.normal) / 4;    // Point A
                normB = (quad.triangleABC.normal + quad.nextA.triangleBCD.normal + quad.nextA.nextB.triangleCDA.normal + quad.nextB.triangleDAB.normal) / 4;    // Point B
                normC = (quad.triangleBCD.normal + quad.nextB.triangleCDA.normal + quad.nextB.nextC.triangleDAB.normal + quad.nextC.triangleABC.normal) / 4;    // Point C
                normD = (quad.triangleCDA.normal + quad.nextC.triangleDAB.normal + quad.nextC.nextD.triangleABC.normal + quad.nextD.triangleBCD.normal) / 4;    // Point D



                // Handle faces that are corners (where there are only 3 adjacent faces rather than the usual 4)

                /* BOTTOM (note: the edge between bottom and front is already perfect, so no adjustment is needed there):
                 * 
                 *                                          Front
                 *                                            C
                 *                                            ^
                 *                                     D------+------C
                 *                                     |  D   |  C   |
                 *                                     |      |      |
                 *                                  D<-+------+------+->B
                 *                                     |  A   |  B   |
                 *                                     |      |      |
                 *                                     A------+------B
                 *                                            V
                 *                                            A
                 *      
                 *               Left                       Bottom                      Right
                 *                 B                          C                           D
                 *                 ^                          ^                           ^
                 *          C------+------B            D------+------C             A------+------D
                 *          |  C   |  B   |            |  D   |  C   |             |  A   |  D   |
                 *          |      |      |            |      |      |             |      |      |
                 *      C <-+------+------+->A     D <-+------+------+-> B     A <-+------+------+->C
                 *          |  D   |  A   |            |  A   |  B   |             |  B   |  C   |
                 *          |      |      |            |      |      |             |      |      |
                 *          D------+------A            A------+------B             B------+------C
                 *                 V                          V                           V
                 *                 D                          A                           B
                 *      
                 *                                          Back
                 *                                            A
                 *                                            ^
                 *                                     B------+------A
                 *                                     |  B   |  A   |
                 *                                     |      |      |
                 *                                  B<-+------+------+->D
                 *                                     |  C   |  D   |
                 *                                     |      |      |
                 *                                     C------+------D
                 *                                            V
                 *                                            C
                 */
                if (quad.side == QuadFace.Side.BOTTOM)
                {
                    // Edge with back (A to B)
                    if (quad.nextA.side == QuadFace.Side.BACK)
                    {
                        // Update normals for A and B
                        normA = (quad.triangleDAB.normal + quad.nextA.triangleABC.normal + quad.nextA.nextB.triangleDAB.normal + quad.nextD.triangleABC.normal) / 4;
                        normB = (quad.triangleABC.normal + quad.nextB.triangleDAB.normal + quad.nextB.nextA.triangleABC.normal + quad.nextA.triangleDAB.normal) / 4;

                        // Test for corner (around point A) with left by testing left edge (A to D)
                        if (quad.nextD.side == QuadFace.Side.LEFT)
                        {
                            Console.WriteLine("LEFT");
                            normA = (quad.triangleDAB.normal + quad.nextA.triangleABC.normal + quad.nextD.triangleDAB.normal) / 3;
                            normD = (quad.triangleCDA.normal + quad.nextD.triangleABC.normal + quad.nextD.nextB.triangleDAB.normal + quad.nextC.triangleDAB.normal) / 4;
                        }
                        // Test for corner (around point B) with right by testing right edge (B to C)
                        else if (quad.nextB.side == QuadFace.Side.RIGHT)
                        {
                            Console.WriteLine("RIGHT");
                            normB = (quad.triangleABC.normal + quad.nextB.triangleABC.normal + quad.nextA.triangleDAB.normal) / 3;
                            normC = (quad.triangleBCD.normal + quad.nextC.triangleABC.normal + quad.nextC.nextB.triangleABC.normal + quad.nextB.triangleDAB.normal) / 4;
                        }
                        else
                            Console.WriteLine("EDGE");
                    }
                }

                /* BACK (note: the horizontal edges are already perfect, so no adjustment is needed there):
                 * 
                 *                                           Top
                 *                                            A
                 *                                            ^
                 *                                     B------+------A
                 *                                     |  B   |  A   |
                 *                                     |      |      |
                 *                                  B<-+------+------+->D
                 *                                     |  C   |  D   |
                 *                                     |      |      |
                 *                                     C------+------D
                 *                                            V
                 *                                            C
                 *      
                 *               Right                       Back                       Left
                 *                 C                          C                           C
                 *                 ^                          ^                           ^
                 *          D------+------C            D------+------C             D------+------C
                 *          |  D   |  C   |            |  D   |  C   |             |  D   |  C   |
                 *          |      |      |            |      |      |             |      |      |
                 *      D <-+------+------+->B     D <-+------+------+-> B     D <-+------+------+->B
                 *          |  A   |  B   |            |  A   |  B   |             |  A   |  B   |
                 *          |      |      |            |      |      |             |      |      |
                 *          A------+------B            A------+------B             A------+------B
                 *                 V                          V                           V
                 *                 A                          A                           A
                 *      
                 *                                         Bottom
                 *                                            A
                 *                                            ^
                 *                                     B------+------A
                 *                                     |  B   |  A   |
                 *                                     |      |      |
                 *                                  B<-+------+------+->D
                 *                                     |  C   |  D   |
                 *                                     |      |      |
                 *                                     C------+------D
                 *                                            V
                 *                                            C
                 */
                if (quad.side == QuadFace.Side.BACK)
                {
                    // Edge with bottom (A to B)
                    if (quad.nextA.side == QuadFace.Side.BOTTOM)
                    {
                        // Update normals for A and B
                        normA = (quad.triangleDAB.normal + quad.nextA.triangleABC.normal + quad.nextA.nextB.triangleDAB.normal + quad.nextD.triangleABC.normal) / 4;
                        normB = (quad.triangleABC.normal + quad.nextB.triangleDAB.normal + quad.nextB.nextA.triangleABC.normal + quad.nextA.triangleDAB.normal) / 4;

                    }
                }
                /*
                // Create vertices for triangle DAB
                vertices.Add(new TexturedVertex(quad[3], normD, uvD));  // Point D
                vertices.Add(new TexturedVertex(quad[0], normA, uvA));  // Point A
                vertices.Add(new TexturedVertex(quad[1], normB, uvB));  // Point B

                // Create vertices for triangle BCD
                vertices.Add(new TexturedVertex(quad[1], normB, uvB));  // Point B
                vertices.Add(new TexturedVertex(quad[2], normC, uvC));  // Point C
                vertices.Add(new TexturedVertex(quad[3], normD, uvD));  // Point D
                */
                // Create vertices for triangle DAB
                vertices.Add(new TexturedVertex(quad[3], quad[3].Normalized(), uvD));  // Point D
                vertices.Add(new TexturedVertex(quad[0], quad[0].Normalized(), uvA));  // Point A
                vertices.Add(new TexturedVertex(quad[1], quad[1].Normalized(), uvB));  // Point B

                // Create vertices for triangle BCD
                vertices.Add(new TexturedVertex(quad[1], quad[1].Normalized(), uvB));  // Point B
                vertices.Add(new TexturedVertex(quad[2], quad[2].Normalized(), uvC));  // Point C
                vertices.Add(new TexturedVertex(quad[3], quad[3].Normalized(), uvD));  // Point D
                /*
                
                // Create vertices for triangle DAB
                vertices.Add(new TexturedVertex(quad[3], quad.triangleDAB.normal, uvD));  // Point D
                vertices.Add(new TexturedVertex(quad[0], quad.triangleDAB.normal, uvA));  // Point A
                vertices.Add(new TexturedVertex(quad[1], quad.triangleDAB.normal, uvB));  // Point B

                // Create vertices for triangle BCD
                vertices.Add(new TexturedVertex(quad[1], quad.triangleBCD.normal, uvB));  // Point B
                vertices.Add(new TexturedVertex(quad[2], quad.triangleBCD.normal, uvC));  // Point C
                vertices.Add(new TexturedVertex(quad[3], quad.triangleBCD.normal, uvD));  // Point D
                */
            }

            return vertices.ToArray();
        }

        // https://dreamstatecoding.blogspot.com/search?q=texturedvertex
        public static Vector2 GetSphereCoord(Vector3 v)
        {
            Vector2 uv;
            var len = v.Length;

            uv.Y = (float)(Math.Acos(v.Y / len) / Math.PI);
            uv.X = -(float)((Math.Atan2(v.Z, v.X) / Math.PI + 1.0f) * 0.5f);

            return uv;
        }

        public static Vector2 GetSphereCoord2(Vector3 v)
        {
            Vector2 uv;
            var r = v.Length;

            var theta = Math.Atan2(v.Z, v.X);
            var phi = Math.Acos(-v.Y / r);

            phi /= Math.Abs(Math.Cos(theta % Math.PI / 4));

            uv.X = (float)(phi * Math.Cos(theta));
            uv.Y = (float)(phi * Math.Sin(theta));

            uv.X = (float)((uv.X - -Math.PI) / (Math.PI - -Math.PI) * (1 - 0) + 0);
            uv.Y = (float)((uv.Y - -Math.PI) / (Math.PI - -Math.PI) * (1 - 0) + 0);

            return uv;
        }

        private Vector2 GetReimannCoord(Vector3 v)
        {
            //var tmp = (1 + (v.Y + 1) / (1 - v.Y)) / 2f;
            //var r = v.X * tmp;
            //var i = v.Z * tmp;
            if (v.Y >= 1)
                v.Y = .99f;
            var tmp = (1 + (v.Y + 1) / (1 - v.Y)) / 2f;
            var r = v.X * tmp;
            var i = v.Z * tmp;

            //r = (r - minR) / (maxR - minR) * (1 - 0) + 0;
            //i = (i - minI) / (maxI - minI) * (1 - 0) + 0;

            //r = (float)(1 / (1 + Math.Exp(-r)));
            //i = (float)(1 / (1 + Math.Exp(-i)));

            r = (float)(Math.Sin(.5 * r * Math.PI / maxR) * (1 - .5) + .5);
            i = (float)(Math.Sin(.5 * i * Math.PI / maxI) * (1 - .5) + .5);
            /*
            if (r >= 0)
                r = (float)(Math.Log(1 + r - 0) / Math.Log(1 + maxR - 0) * (1 - .5) + .5);
            else
                r = (float)(-Math.Log(1 + -r - 0) / Math.Log(1 + maxR - 0) * (1 - .5) + .5);

            if (i >= 0)
                i = (float)(Math.Log(1 + i - 0) / Math.Log(1 + maxI - 0) * (1 - .5) + .5);
            else
                i = (float)(-Math.Log(1 + -i - 0) / Math.Log(1 + maxI - 0) * (1 - .5) + .5);
            */
            return new Vector2(r, i);
        }

        private int AddVertex(Vector3 p)
        {
            points.Add(p.Normalized());

            return index++;
        }
    }
}