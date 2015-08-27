using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotGame.Utils;
using DotGame.Assets;
using DotGame.Graphics;
using System.Numerics;

namespace DotGame.Geometry
{
    /// <summary>
    /// Stellt Funktionen bereit um Mesh-Daten zu generieren.
    /// </summary>
    public class MeshBuilder
    {
        public Engine Engine { get; private set; }

        private Vector3[] positions;
        private Vector2[] texCoords;
        private Vector4[] colors;
        private Vector3[] normals;
        private int[] indices;
        private int vertexCount = 0;
        private int indexCount = 0;

        /// <summary>
        /// Gibt die Anzahl der Vertices die generiert wurden an.
        /// </summary>
        public int VertexCount { get { return vertexCount; } }

        /// <summary>
        /// Gibt die Anzahl der Indices die generiert wurden an.
        /// </summary>
        public int IndexCount { get { return indexCount; } }

        /// <summary>
        /// Gibt an ob Indices generiert wurden.
        /// </summary>
        public bool HasIndices { get { return indices != null; } }

        /// <summary>
        /// Gibt an ob Texturekoordinaten generiert wurden.
        /// </summary>
        public bool HasTexCoords { get { return texCoords != null; } }

        /// <summary>
        /// Gibt an ob Normals generiert wurden.
        /// </summary>
        public bool HasNormals { get { return normals != null; } }

        /// <summary>
        /// Gibt an ob Colors generiert wurden.
        /// </summary>
        public bool HasColors { get { return colors != null; } }

        public MeshBuilder(Engine engine)
        {
            this.Engine = engine;

            Reset();
        }

        /// <summary>
        /// Erstellt die VertexDescription die für die derzeit generierten Daten passt. 
        /// </summary>
        /// <returns></returns>
        private VertexDescription CreateDescription()
        {
            List<VertexElement> elements = new List<VertexElement>();
            elements.Add(new VertexElement(VertexElementUsage.Position, 0, VertexElementType.Vector3));
            if (HasTexCoords)
                elements.Add(new VertexElement(VertexElementUsage.TexCoord, 0, VertexElementType.Vector2));
            if (HasNormals)
                elements.Add(new VertexElement(VertexElementUsage.Normal, 0, VertexElementType.Vector3));
            if (HasColors)
                elements.Add(new VertexElement(VertexElementUsage.Color, 0, VertexElementType.Vector4));
            return new VertexDescription(elements.ToArray());
        }

        /// <summary>
        /// Sorgt dafür, dass die Buffer die richtige Größe haben und erstellt wurden.
        /// </summary>
        /// <param name="needUV"></param>
        /// <param name="needNormal"></param>
        /// <param name="needColor"></param>
        private void EnsureBufferVertex(bool needUV, bool needNormal, bool needColor)
        {
            int bufferSize = Math.Max(vertexCount * 2, positions.Length * 2);
            if (vertexCount >= positions.Length)
                Array.Resize(ref positions, bufferSize);

            if (needUV && !HasTexCoords)
                texCoords = new Vector2[bufferSize];
            else if (HasTexCoords && vertexCount >= texCoords.Length)
                Array.Resize(ref texCoords, bufferSize);

            if (needNormal && !HasNormals)
                normals = new Vector3[bufferSize];
            else if (HasNormals && vertexCount >= normals.Length)
                Array.Resize(ref normals, bufferSize);

            if (needColor && !HasColors)
                colors = new Vector4[bufferSize];
            else if (HasColors && vertexCount >= colors.Length)
                Array.Resize(ref colors, bufferSize);
        }

        private void EnsureBufferIndex()
        {
            if (!HasIndices)
            {
                indices = new int[64];
                return;
            }

            int bufferSize = Math.Max(indexCount * 2, indices.Length * 2);
            if (indexCount >= indices.Length)
                Array.Resize(ref indices, bufferSize);
        }

        public void Reset()
        {
            vertexCount = 0;
            indexCount = 0;

            positions = new Vector3[64];
            texCoords = null;
            normals = null;
            colors = null;
        }

        public void PushVertex(Vector3 position)
        {
            EnsureBufferVertex(false, false, false);
            positions[vertexCount++] = position;
        }

        public void PushVertex(Vector3 position, Vector2 uv)
        {
            EnsureBufferVertex(true, false, false);

            positions[vertexCount] = position;
            texCoords[vertexCount] = uv;
            vertexCount++;
        }

        public void PushVertex(Vector3 position, Vector2 uv, Vector3 normal)
        {
            EnsureBufferVertex(true, true, false);
            positions[vertexCount] = position;
            texCoords[vertexCount] = uv;
            normals[vertexCount] = normal;

            vertexCount++;
        }

        public void PushVertex(Vector3 position, Vector2 uv, Vector3 normal, Vector4 color)
        {
            EnsureBufferVertex(true, true, true);
            positions[vertexCount] = position;
            texCoords[vertexCount] = uv;
            colors[vertexCount] = color;
            normals[vertexCount] = normal;

            vertexCount++;
        }

        public void PushIndex(params int[] indices)
        {
            for (int i = 0; i < indices.Length; i++)
                PushIndex(indices[i]);
        }

        public void PushIndex(int index)
        {
            EnsureBufferIndex();
            indices[indexCount++] = index;
        }

        public void PushQuad(Vector3 start, Vector3 end)
        {
            Vector3 min = Vector3.Min(start, end);
            Vector3 max = Vector3.Max(start, end);

            if (HasIndices)
                PushIndex(vertexCount, vertexCount + 1, vertexCount + 2, vertexCount + 3, vertexCount + 4, vertexCount + 5);
            PushVertex(new Vector3(min.X, max.Y, min.Z));
            PushVertex(new Vector3(min.X, max.Y, max.Z));
            PushVertex(new Vector3(max.X, max.Y, max.Z));
            PushVertex(new Vector3(min.X, max.Y, min.Z));
            PushVertex(new Vector3(max.X, max.Y, max.Z));
            PushVertex(new Vector3(max.X, max.Y, min.Z));
        }
        public void PushQuad(Vector3 start, Vector3 end, float textureRepeatU, float textureRepeatV)
        {
            Vector3 min = Vector3.Min(start, end);
            Vector3 max = Vector3.Max(start, end);

            if (HasIndices)
                PushIndex(vertexCount, vertexCount + 1, vertexCount + 2, vertexCount + 3, vertexCount + 4, vertexCount + 5);
            PushVertex(new Vector3(min.X, max.Y, min.Z), new Vector2(0.0f, textureRepeatV), new Vector3(0, 1, 0));
            PushVertex(new Vector3(min.X, max.Y, max.Z), new Vector2(0.0f, 0.0f), new Vector3(0, 1, 0));
            PushVertex(new Vector3(max.X, max.Y, max.Z), new Vector2(textureRepeatU, 0.0f), new Vector3(0, 1, 0));
            PushVertex(new Vector3(min.X, max.Y, min.Z), new Vector2(0.0f, textureRepeatV), new Vector3(0, 1, 0));
            PushVertex(new Vector3(max.X, max.Y, max.Z), new Vector2(textureRepeatU, 0.0f), new Vector3(0, 1, 0));
            PushVertex(new Vector3(max.X, max.Y, min.Z), new Vector2(textureRepeatU, textureRepeatV), new Vector3(0, 1, 0));
        }

        /// <summary>
        /// Fügt einen Würfel zu den Vertices zu (mit TexCoords und Normals, sowie Indices wenn schon welche vorhanden sind)
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="TextureRepeatU"></param>
        /// <param name="TextureRepeatV"></param>
        public void PushCube(Vector3 start, Vector3 end, Vector2 uvStart, Vector2 uvEnd)
        {
            Vector3 min = Vector3.Min(start, end);
            Vector3 max = Vector3.Max(start, end);

            // Front
            if (HasIndices)
                PushIndex(vertexCount, vertexCount + 1, vertexCount + 2, vertexCount + 3, vertexCount + 4, vertexCount + 5);
            PushVertex(new Vector3(min.X, min.Y, min.Z), new Vector2(uvStart.X, uvEnd.Y), new Vector3(0f, 0f, 1f));
            PushVertex(new Vector3(min.X, max.Y, min.Z), new Vector2(uvStart.X, uvStart.Y), new Vector3(0f, 0f, 1f));
            PushVertex(new Vector3(max.X, max.Y, min.Z), new Vector2(uvEnd.X, uvStart.Y), new Vector3(0f, 0f, 1f));

            PushVertex(new Vector3(min.X, min.Y, min.Z), new Vector2(uvStart.X, uvEnd.Y), new Vector3(0f, 0f, 1f));
            PushVertex(new Vector3(max.X, max.Y, min.Z), new Vector2(uvEnd.X, uvStart.Y), new Vector3(0f, 0f, 1f));
            PushVertex(new Vector3(max.X, min.Y, min.Z), new Vector2(uvEnd.X, uvEnd.Y), new Vector3(0f, 0f, 1f));

            // Back
            if (HasIndices)
                PushIndex(vertexCount, vertexCount + 1, vertexCount + 2, vertexCount + 3, vertexCount + 4, vertexCount + 5);
            PushVertex(new Vector3(min.X, min.Y, max.Z), new Vector2(uvEnd.X, uvStart.Y), new Vector3(0f, 0f, -1f));
            PushVertex(new Vector3(max.X, max.Y, max.Z), new Vector2(uvStart.X, uvEnd.Y), new Vector3(0f, 0f, -1f));
            PushVertex(new Vector3(min.X, max.Y, max.Z), new Vector2(uvEnd.X, uvEnd.Y), new Vector3(0f, 0f, -1f));

            PushVertex(new Vector3(min.X, min.Y, max.Z), new Vector2(uvEnd.X, uvStart.Y), new Vector3(0f, 0f, -1f));
            PushVertex(new Vector3(max.X, min.Y, max.Z), new Vector2(uvStart.X, uvStart.Y), new Vector3(0f, 0f, -1f));
            PushVertex(new Vector3(max.X, max.Y, max.Z), new Vector2(uvStart.X, uvEnd.Y), new Vector3(0f, 0f, -1f));

            // Top
            if (HasIndices)
                PushIndex(vertexCount, vertexCount + 1, vertexCount + 2, vertexCount + 3, vertexCount + 4, vertexCount + 5);
            PushVertex(new Vector3(min.X, max.Y, min.Z), new Vector2(uvStart.X, uvEnd.Y), new Vector3(0, 1, 0));
            PushVertex(new Vector3(min.X, max.Y, max.Z), new Vector2(uvStart.X, uvStart.Y), new Vector3(0, 1, 0));
            PushVertex(new Vector3(max.X, max.Y, max.Z), new Vector2(uvEnd.X, uvStart.Y), new Vector3(0, 1, 0));

            PushVertex(new Vector3(min.X, max.Y, min.Z), new Vector2(uvStart.X, uvEnd.Y), new Vector3(0, 1, 0));
            PushVertex(new Vector3(max.X, max.Y, max.Z), new Vector2(uvEnd.X, uvStart.Y), new Vector3(0, 1, 0));
            PushVertex(new Vector3(max.X, max.Y, min.Z), new Vector2(uvEnd.X, uvEnd.Y), new Vector3(0, 1, 0));

            // Bottom
            if (HasIndices)
                PushIndex(vertexCount, vertexCount + 1, vertexCount + 2, vertexCount + 3, vertexCount + 4, vertexCount + 5);
            PushVertex(new Vector3(min.X, min.Y, min.Z), new Vector2(uvEnd.X, uvStart.Y), new Vector3(0, -1, 0));
            PushVertex(new Vector3(max.X, min.Y, max.Z), new Vector2(uvStart.X, uvEnd.Y), new Vector3(0, -1, 0));
            PushVertex(new Vector3(min.X, min.Y, max.Z), new Vector2(uvEnd.X, uvEnd.Y), new Vector3(0, -1, 0));

            PushVertex(new Vector3(min.X, min.Y, min.Z), new Vector2(uvEnd.X, uvStart.Y), new Vector3(0, -1, 0));
            PushVertex(new Vector3(max.X, min.Y, min.Z), new Vector2(uvStart.X, uvStart.Y), new Vector3(0, -1, 0));
            PushVertex(new Vector3(max.X, min.Y, max.Z), new Vector2(uvStart.X, uvEnd.Y), new Vector3(0, -1, 0));

            // Left
            if (HasIndices)
                PushIndex(vertexCount, vertexCount + 1, vertexCount + 2, vertexCount + 3, vertexCount + 4, vertexCount + 5);
            PushVertex(new Vector3(min.X, min.Y, min.Z), new Vector2(uvStart.X, uvStart.Y), new Vector3(-1, 0, 0));
            PushVertex(new Vector3(min.X, min.Y, max.Z), new Vector2(uvEnd.X, uvStart.Y), new Vector3(-1, 0, 0));
            PushVertex(new Vector3(min.X, max.Y, max.Z), new Vector2(uvEnd.X, uvEnd.Y), new Vector3(-1, 0, 0));

            PushVertex(new Vector3(min.X, min.Y, min.Z), new Vector2(uvStart.X, uvStart.Y), new Vector3(-1, 0, 0));
            PushVertex(new Vector3(min.X, max.Y, max.Z), new Vector2(uvEnd.X, uvEnd.Y), new Vector3(-1, 0, 0));
            PushVertex(new Vector3(min.X, max.Y, min.Z), new Vector2(uvStart.X, uvEnd.Y), new Vector3(-1, 0, 0));

            // Right
            if (HasIndices)
                PushIndex(vertexCount, vertexCount + 1, vertexCount + 2, vertexCount + 3, vertexCount + 4, vertexCount + 5);
            PushVertex(new Vector3(max.X, min.Y, min.Z), new Vector2(uvStart.X, uvStart.Y), new Vector3(1, 0, 0));
            PushVertex(new Vector3(max.X, max.Y, max.Z), new Vector2(uvEnd.X, uvEnd.Y), new Vector3(1, 0, 0));
            PushVertex(new Vector3(max.X, min.Y, max.Z), new Vector2(uvEnd.X, uvStart.Y), new Vector3(1, 0, 0));

            PushVertex(new Vector3(max.X, min.Y, min.Z), new Vector2(uvStart.X, uvStart.Y), new Vector3(1, 0, 0));
            PushVertex(new Vector3(max.X, max.Y, min.Z), new Vector2(uvStart.X, uvEnd.Y), new Vector3(1, 0, 0));
            PushVertex(new Vector3(max.X, max.Y, max.Z), new Vector2(uvEnd.X, uvEnd.Y), new Vector3(1, 0, 0));
        }

        /// <summary>
        /// Fügt Normals zu den Vertexdaten hinzu und überscheibt eventuell schon vorhandene Normals.
        /// </summary>
        public void CalculateNormals()
        {
            EnsureBufferVertex(false, true, false);

            if (HasIndices)
            {
                for (int i = 0; i < indexCount; i += 3)
                {
                    int a = indices[i];
                    int b = indices[i + 1];
                    int c = indices[i + 2];

                    Vector3 firstvec = positions[b] - positions[a];
                    Vector3 secondvec = positions[a] - positions[c];
                    Vector3 normal = Vector3.Normalize(Vector3.Cross(firstvec, secondvec));


                    normals[a] += normal;
                    normals[b] += normal;
                    normals[c] += normal;
                }
            }
            else
            {
                for (int j = 0; j < vertexCount; j += 3)
                {
                    Vector3 firstvec = positions[j + 1] - positions[j];
                    Vector3 secondvec = positions[j] - positions[j + 2];
                    Vector3 normal = Vector3.Normalize(Vector3.Cross(firstvec, secondvec));
                    normals[j] += normal;
                    normals[j + 1] += normal;
                    normals[j + 2] += normal;
                }
            }

            for (int j = 0; j < vertexCount; j++)
                normals[j] = Vector3.Normalize(normals[j]);
        }

        /// <summary>
        /// Transformiert alle Positione mit der angebenen Matrix
        /// </summary>
        /// <param name="Matrix"></param>
        public void TransformVertices(Matrix4x4 Matrix)
        {
            for (int i = 0; i < vertexCount; i++)
                positions[i] = Vector3.Transform(positions[i], Matrix);
        }

        /// <summary>
        /// Erzeugt das Mesh und setzt den MeshBuilder zurück.
        /// </summary>
        /// <param name="name">Der Name für das zu erzeugende Mesh.</param>
        /// <returns></returns>
        public StaticMesh BuildMesh(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Can not be null or whitespace.", "name");

            float[] data = new float[(3 + (HasTexCoords ? 2 : 0) + (HasNormals ? 3 : 0) + (HasColors ? 4 : 0)) * vertexCount];

            // VertexDaten in float-Array kopieren
            int index = 0;
            for (int i = 0; i < vertexCount; i++)
            {
                data[index++] = positions[i].X;
                data[index++] = positions[i].Y;
                data[index++] = positions[i].Z;

                if (HasTexCoords)
                {
                    data[index++] = texCoords[i].X;
                    data[index++] = texCoords[i].Y;
                }

                if (HasNormals)
                {
                    data[index++] = normals[i].X;
                    data[index++] = normals[i].Y;
                    data[index++] = normals[i].Z;
                }

                if (HasColors)
                {
                    data[index++] = colors[i].X;
                    data[index++] = colors[i].Y;
                    data[index++] = colors[i].Z;
                    data[index++] = colors[i].W;
                }
            }
            StaticMesh mesh;
            if (!HasIndices)
                mesh = Engine.AssetManager.LoadMesh(name, data, CreateDescription());
            else
                mesh = Engine.AssetManager.LoadMesh(name, data, CreateDescription(), indices, IndexFormat.Int32);
            Reset();
            return mesh;
        }
    }
}