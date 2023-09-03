//Developed by Halil Emre Yildiz - @Jahn_Star (All right reserved. 2020)
//https://jahnstar.github.io/donate/
// Last Edit: 18.10.2020
using UnityEngine;
namespace JahnStar.HeyTriangulator
{
    public class Generator
    {
        int by_JahnStar;
        public enum RandomType { fair, equal };
        public static Mesh PointsToMesh(Vector3[] vertices)
        {
            Vector2[] vertices2D = new Vector2[vertices.Length];
            for (int i = 0; i < vertices.Length; i++) vertices2D[i] = new Vector2(vertices[i].x, vertices[i].z);

            // Use the triangulator to get indices for creating triangles
            Triangulator tr = new Triangulator(vertices2D);
            int[] indices = tr.Triangulate();

            // Create new mesh
            Mesh newMesh = new Mesh();
            newMesh.vertices = vertices;
            newMesh.triangles = indices;
            newMesh.RecalculateNormals();
            newMesh.RecalculateBounds();

            return newMesh;
        }
        public static Vector3[] PickRandomLocations(Mesh mesh, RandomType type = RandomType.fair, int count = 1)
        {
            Vector3[] points = new Vector3[count];
            int equal_order = 0;
            for (int i = 0; i < count; i++)
            {
                Triangle tri = PickRandomTriangle(mesh, type, ref equal_order);
                Vector2 randPos = RandomWithinTriangle(tri);
                points[i] = new Vector3(randPos.x, 0f, randPos.y);
            }
            return points;
        }
        /// <summary>
        /// Returns a 'Triangle' if <param name="type">'type'</param> is 'RandomType.fail'
        /// </summary>
        public static Triangle PickRandomTriangle(Mesh mesh, RandomType type, ref int equal_triOrder)
        {
            switch (type)
            {
                case RandomType.fair:
                    {
                        Vector3[] verts = mesh.vertices;
                        int[] tris = mesh.triangles;

                        int totalArea = 0;
                        int[] trisArea = new int[tris.Length];
                        for (int i = 0; i < tris.Length; i += 3)
                        {
                            trisArea[i] = (int)TriArea(new Triangle(verts[tris[i]], verts[tris[i + 1]], verts[tris[i + 2]]));
                            totalArea += trisArea[i];
                        }

                        var rng = Random.Range(0f, (float)totalArea);
                        for (int i = 0; i < tris.Length; i += 3)
                        {
                            Triangle triangle = new Triangle(verts[tris[i]], verts[tris[i + 1]], verts[tris[i + 2]]);
                            if (rng < TriArea(triangle)) return triangle;
                            rng -= TriArea(triangle);
                        }
                    }
                    break;
                case RandomType.equal:
                    {
                        Vector3[] verts = mesh.vertices;
                        int[] tris = mesh.triangles;

                        if (equal_triOrder >= tris.Length) equal_triOrder = 0;
                        Triangle triangle = new Triangle(verts[tris[equal_triOrder]], verts[tris[equal_triOrder + 1]], verts[tris[equal_triOrder + 2]]);
                        equal_triOrder += 3;
                        return triangle;
                    }
            }
            throw new System.Exception("Buraya gelemez");
        }
        public static Vector2 RandomWithinTriangle(Triangle triangle)
        {
            var r1 = Mathf.Sqrt(Random.Range(0f, 1f));
            var r2 = Random.Range(0f, 1f);
            var m1 = 1 - r1;
            var m2 = r1 * (1 - r2);
            var m3 = r2 * r1;

            var p1 = triangle.vertex1_vector2;
            var p2 = triangle.vertex2_vector2;
            var p3 = triangle.vertex3_vector2;
            return (m1 * p1) + (m2 * p2) + (m3 * p3);
        }
        public static float TriArea(Triangle triangle)
        {
            var p1 = triangle.vertex1_vector2;
            var p2 = triangle.vertex2_vector2;
            var p3 = triangle.vertex3_vector2;
            Vector3 V = Vector3.Cross(p1 - p2, p1 - p3);
            return V.magnitude * 0.5f;
        }

        public static bool ContainsPoint(Vector2[] polyPoints, Vector2 checkPoint)
        {
            var p = checkPoint;
            var j = polyPoints.Length - 1;
            var inside = false;
            for (int i = 0; i < polyPoints.Length; j = i++)
            {
                var pi = polyPoints[i];
                var pj = polyPoints[j];
                if (((pi.y <= p.y && p.y < pj.y) || (pj.y <= p.y && p.y < pi.y)) && (p.x < (pj.x - pi.x) * (p.y - pi.y) / (pj.y - pi.y) + pi.x))
                    inside = !inside;
            }
            return inside;
        }
    }
    public class Triangle
    {
        public Vector2 vertex1_vector2, vertex2_vector2, vertex3_vector2;
        public Vector3 vertex1_vector3, vertex2_vector3, vertex3_vector3;
        public Triangle(Vector3 vertex1, Vector3 vertex2, Vector3 vertex3)
        {
            vertex1_vector3 = vertex1;
            vertex2_vector3 = vertex2;
            vertex3_vector3 = vertex3;

            vertex1_vector2 = new Vector2(vertex1.x, vertex1.z);
            vertex2_vector2 = new Vector2(vertex2.x, vertex2.z);
            vertex3_vector2 = new Vector2(vertex3.x, vertex3.z);
        }
    }

}