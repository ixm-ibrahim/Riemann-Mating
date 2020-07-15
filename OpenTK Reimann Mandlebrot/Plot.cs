using OpenTK;
using System;
using System.Linq;
using System.Collections.Generic;

namespace OpenTK_Reimann_Mating
{
    public struct TexturedVertex
    {
        public const int Size = (3 + 3 + 2) * sizeof(float); // size of struct in bytes
        // public const int Size = sizeof(Vector2) + sizeof(Vector3); // size of struct in bytes

        public Vector3 position;
        public Vector3 normal;
        public Vector2 textureCoordinate;

        public TexturedVertex(Vector3 position)
        {
            this.position = position;
            normal = Vector3.Zero;
            textureCoordinate = Vector2.Zero;
        }

        public TexturedVertex(Vector3 position, Vector3 normal)
        {
            this.position = position;
            this.normal = normal;
            textureCoordinate = Vector2.Zero;
        }

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
        public Vector3 center;
        public Vector3 adjacent_01;
        public Vector3 adjacent_12;
        public Vector3 adjacent_20;

        public bool reversed = false;

        public int subdivision = 0;

        public TriangleFace(Vector3 a, Vector3 b, Vector3 c, bool reversed = false, int subdivision = 0)
        {
            verticies = new Vector3[3];

            verticies[0] = a;
            verticies[1] = b;
            verticies[2] = c;

            normal = GetNormal();
            center = GetCenter();

            this.reversed = reversed;
            this.subdivision = subdivision;
        }

        public TriangleFace(Vector3 a, Vector3 b, Vector3 c, Vector3 norm)
        {
            verticies = new Vector3[3];

            verticies[0] = a;
            verticies[1] = b;
            verticies[2] = c;

            normal = norm;
            center = GetCenter();
        }

        public Vector3 this[int index]
        {
            get { return verticies[index]; }

            set
            {
                verticies[index] = value;

                normal = GetNormal();
                center = GetCenter();
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

        Vector3 GetCenter()
        {
            return ((verticies[0] + verticies[1] + verticies[2]) / 3).Normalized();
        }
    }

    public class QuadFace
    {
        public Vector3[] vertices;
        public Vector3 center;

        public QuadFace subdivisonA;    // bottom-left subdivison
        public QuadFace subdivisonB;    // bottom-right subdivision
        public QuadFace subdivisonC;    // top-right subdivision
        public QuadFace subdivisonD;    // top-left subdivision

        public int subdivion = 0;

        public QuadFace(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
        {
            vertices = new Vector3[4];

            vertices[0] = a;
            vertices[1] = b;
            vertices[2] = c;
            vertices[3] = d;

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

            subdivisonA = new QuadFace(vertices[0], a, center, d);    // bottom-left
            subdivisonB = new QuadFace(a, vertices[1], b, center);    // bottom-right
            subdivisonC = new QuadFace(center, b, vertices[2], c);    // top-right
            subdivisonD = new QuadFace(d, center, c, vertices[3]);    // top-left

            subdivion++;
        }

        public void SphericalSubdivide(float radius = 1)
        {
            // Get midpoints and center
            var a = radius * ((vertices[0] + vertices[1]) / 2).Normalized();   // bottom midpoint
            var b = radius * ((vertices[1] + vertices[2]) / 2).Normalized();   // right midpoint
            var c = radius * ((vertices[2] + vertices[3]) / 2).Normalized();   // top midpoint
            var d = radius * ((vertices[3] + vertices[0]) / 2).Normalized();   // left midpoint
            var center = radius * this.center.Normalized();    // center of quad\

            // Create subdivisons
            subdivisonA = new QuadFace(vertices[0], a, center, d);    // bottom-left corner
            subdivisonB = new QuadFace(a, vertices[1], b, center);    // bottom-right corner
            subdivisonC = new QuadFace(center, b, vertices[2], c);    // top-right corner
            subdivisonD = new QuadFace(d, center, c, vertices[3]);    // top-left corner

            subdivion++;
        }
    }

    public class SubdividedQuad
    {
        public TexturedVertex[] vertices;
        float sideLength;

        public SubdividedQuad(float sideLength, int subdivisions, bool planet = false)
        {
            this.sideLength = sideLength;

            Create(subdivisions);

            if (planet)
                PlanetGround(Vector3.Zero, sideLength, sideLength, new Camera(new Vector3(0,0,13.0f), 1));
        }

        public void Create(int subdivisions)
        {
            var s = sideLength / 2;
            var a = new Vector3(s, s, 0);     // 0
            var b = new Vector3(-s, s, 0);    // 1
            var c = new Vector3(-s, -s, 0);   // 2
            var d = new Vector3(s, -s, 0);    // 3

            Console.WriteLine(a);
            Console.WriteLine(b);
            Console.WriteLine(c);
            Console.WriteLine(d);

            var faces = new List<QuadFace>
            {
                new QuadFace(a, b, c, d)
            };
            
            // Subdivide cube
            for (int i = 0; i < subdivisions; i++)
            {
                var subdividedFaces = new List<QuadFace>();

                // Subdivide each quad
                for (int j = 0; j < faces.Count; j++)
                {
                    faces[j].Subdivide();

                    // Add subdivided faces
                    subdividedFaces.Add(faces[j].subdivisonA);
                    subdividedFaces.Add(faces[j].subdivisonB);
                    subdividedFaces.Add(faces[j].subdivisonC);
                    subdividedFaces.Add(faces[j].subdivisonD);
                }

                faces = subdividedFaces;
            }

            // done, now add triangles to mesh
            var vertices = new List<TexturedVertex>();

            // Add the vertices for 2 triangles for each quad (DAB and BCD)
            for (int i = faces.Count - 1; faces.Count > 0; i--)
            {
                Vector2 uvA = GetSphereCoord(faces[i][0]);
                Vector2 uvB = GetSphereCoord(faces[i][1]);
                Vector2 uvC = GetSphereCoord(faces[i][2]);
                Vector2 uvD = GetSphereCoord(faces[i][3]);

                // Create vertices for triangle DAB
                vertices.Add(new TexturedVertex(faces[i][3], Vector3.UnitZ, uvD));  // Point D
                vertices.Add(new TexturedVertex(faces[i][0], Vector3.UnitZ, uvA));  // Point A
                vertices.Add(new TexturedVertex(faces[i][1], Vector3.UnitZ, uvB));  // Point B

                // Create vertices for triangle BCD
                vertices.Add(new TexturedVertex(faces[i][1], Vector3.UnitZ, uvB));  // Point B
                vertices.Add(new TexturedVertex(faces[i][2], Vector3.UnitZ, uvC));  // Point C
                vertices.Add(new TexturedVertex(faces[i][3], Vector3.UnitZ, uvD));  // Point D
                /*
                // Create vertices for triangle DAB
                vertices.Add(new TexturedVertex(faces[i][3], new Vector3((float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble()), uvD));  // Point D
                vertices.Add(new TexturedVertex(faces[i][0], new Vector3((float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble()), uvA));  // Point A
                vertices.Add(new TexturedVertex(faces[i][1], new Vector3((float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble()), uvB));  // Point B

                // Create vertices for triangle BCD
                vertices.Add(new TexturedVertex(faces[i][1], new Vector3((float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble()), uvB));  // Point B
                vertices.Add(new TexturedVertex(faces[i][2], new Vector3((float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble()), uvC));  // Point C
                vertices.Add(new TexturedVertex(faces[i][3], new Vector3((float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble()), uvD));  // Point D
                */
                faces.RemoveAt(i);
            }

            this.vertices = vertices.ToArray();
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

        public void OffsetZ(float zs)
        {
            for (int i = 0; i < vertices.Length; i++)
                vertices[i].position.Z += zs;
        }

        public void SphericalProjection(float radius)
        {
            Random r = new Random();

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].normal = vertices[i].position.Normalized();

                vertices[i].position = vertices[i].normal * radius;

                vertices[i].normal = new Vector3((float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble());
            }
        }

        public void HemisphereApproximation(float n)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].position /= sideLength / 2;

                vertices[i].position.Z = (float) ((1 - Math.Pow(vertices[i].position.X, n)) * (1 - Math.Pow(vertices[i].position.Y, n)));

                vertices[i].position *= sideLength / 2;
            }
                
        }

        public void PlanetGround(Vector3 position, float minRadius, float maxRadius, Camera c, float n = 4)
        {
            HemisphereApproximation(4);

            float d = c.Position.Length - position.Length;
            float h = (float) Math.Sqrt(d * d - minRadius * minRadius);
            float s = (float) Math.Sqrt(maxRadius * maxRadius - minRadius * minRadius);
            float zs = (maxRadius * maxRadius + d * d - (h + s) * (h + s)) / (2 * minRadius * (h + s));

            OffsetZ(zs);

            SphericalProjection(minRadius);
        }
    }
}