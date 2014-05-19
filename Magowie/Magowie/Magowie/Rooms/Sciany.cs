using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Design;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using Magowie.Camera;

namespace Magowie.Rooms
{
    class Sciany : parts
    {
        int nrSciany;
        public Sciany(int nrSciany)
        {
            this.nrSciany = nrSciany;
        }

        override public BoundingBox CalculateBoundingBox()
        {
            //Matrix[] modelTransforms = new Matrix[model.Bones.Count];
            //model.CopyAbsoluteBoneTransformsTo(modelTransforms);

            // Create variables to hold min and max xyz values for the model. Initialise them to extremes
            //Vector3 modelMax = new Vector3(1700, -10, 1430);
            //Vector3 modelMin = new Vector3(1100, 10, 1435);
            Vector3 modelMin = new Vector3(-1, -1, -1);
            Vector3 modelMax = new Vector3(-1, 1, 1);

            if (nrSciany == 1)
            {
                modelMin = new Vector3(-1330, -50, -1800);
                modelMax = new Vector3(-1327, 50, 1200);
            }
            if (nrSciany == 2)
            {
                modelMin = new Vector3(1400, -50, -1800);
                modelMax = new Vector3(1400, 50, 1200);
            }
            if (nrSciany == 3)
            {
                modelMin = new Vector3(-1800, -50, -1671);
                modelMax = new Vector3(1600, 50, -1670);
            }
            if (nrSciany == 4)
            {
                modelMin = new Vector3(-1800, -50, 1190);
                modelMax = new Vector3(1600, 50, 1191);
            }
            if (nrSciany == 5)
            {
                modelMin = new Vector3(-1330, -50, -1800);
                modelMax = new Vector3(-1327, 50, 1200);
            }
            /*
                        parts[9].Position(-650, 1360);
                        parts[10].Position(1200, 1360);
                        parts[11].Position(-650, -1450);
                        parts[12].Position(1200, -1450);
                        */
            //1700 1430 (bez rogu, trzeba dodac troche do 1 zmiennej )
            //-1100 1430 (bez rogu, trzeba odjac troche od 1 zmiennej )

            //foreach (ModelMesh mesh in model.Meshes)
            //{
            //    //Create variables to hold min and max xyz values for the mesh. Initialise them to extremes
            //    Vector3 meshMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);
            //    Vector3 meshMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

            //    // There may be multiple parts in a mesh (different materials etc.) so loop through each
            //    foreach (ModelMeshPart part in mesh.MeshParts)
            //    {
            //        // The stride is how big, in bytes, one vertex is in the vertex buffer
            //        // We have to use this as we do not know the make up of the vertex
            //        int stride = part.VertexBuffer.VertexDeclaration.VertexStride;

            //        byte[] vertexData = new byte[stride * part.NumVertices];
            //        part.VertexBuffer.GetData(part.VertexOffset * stride, vertexData, 0, part.NumVertices, 1); // fixed 13/4/11

            //        // Find minimum and maximum xyz values for this mesh part
            //        // We know the position will always be the first 3 float values of the vertex data
            //        Vector3 vertPosition = new Vector3();
            //        for (int ndx = 0; ndx < vertexData.Length; ndx += stride)
            //        {
            //            vertPosition.X = BitConverter.ToSingle(vertexData, ndx);
            //            vertPosition.Y = BitConverter.ToSingle(vertexData, ndx + sizeof(float));
            //            vertPosition.Z = BitConverter.ToSingle(vertexData, ndx + sizeof(float) * 2);

            //            // update our running values from this vertex
            //            meshMin = Vector3.Min(meshMin, vertPosition);
            //            meshMax = Vector3.Max(meshMax, vertPosition);
            //        }
            //    }

            //    // transform by mesh bone transforms
            //    meshMin = Vector3.Transform(meshMin, modelTransforms[mesh.ParentBone.Index] * World);
            //    meshMax = Vector3.Transform(meshMax, modelTransforms[mesh.ParentBone.Index] * World);

            //    // Expand model extents by the ones from this mesh
            //    modelMin = Vector3.Min(modelMin, meshMin);
            //    modelMax = Vector3.Max(modelMax, meshMax);
            //}


            // Create and return the model bounding box
            return new BoundingBox(modelMin, modelMax);

        }

        override public BoundingSphere getBoundingSphere()
        {
            BoundingSphere sphere = new BoundingSphere();

            //foreach (ModelMesh mesh in model.Meshes)
            //{
            //    if (sphere.Radius == 0)
            //        sphere = mesh.BoundingSphere;
            //    else
            //        sphere = BoundingSphere.CreateMerged(sphere, mesh.BoundingSphere);
            //}
            Vector3 position = new Vector3(1700, -10, 1430); ;


            sphere.Center = position;
            sphere.Radius = 10;
            //Console.WriteLine("zwracam sfere ryboczleka");

            return sphere;
        }

    }
}
