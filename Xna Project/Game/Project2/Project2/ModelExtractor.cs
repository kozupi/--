using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;

namespace Project2
{
    public class ModelExtractor
    {
        private ModelMeshPart model;
        private Vector3[] vectorArray;
        public VertexPositionTexture[] vpcVerts { get; protected set; }

        public ModelExtractor(ModelMeshPart mmp, Vector3[] vecArray, VertexPositionTexture[] vpcVertexArray)
        {
            this.model = mmp;
            this.vectorArray = vecArray;
            this.vpcVerts = vpcVertexArray;
        }

        public void ExtractVertices()
        {
            this.model.VertexBuffer.GetData<Vector3>(this.vectorArray);
            for (int i = 0; i < vpcVerts.Length; i+=2)
            {
                this.vpcVerts[i].Position.X = vectorArray[i].X;
                this.vpcVerts[i].Position.Y = vectorArray[i].Y;
                this.vpcVerts[i].Position.Z = vectorArray[i].Z;
            }
        }
    }
}
