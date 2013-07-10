using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project2.Entity;

namespace Project2.Component
{
    public class Drawable3D :Component
    {
        public Model model { get; protected set; }
        protected float alpha = 1.0f;
        public List<BoundingBox> boundingBoxes { get; protected set; }
        public static ModelManager modelManager { get; set; }
        public ModelExtractor modelExtractor { get; protected set; }
        public BoundingBox modelBoundingBox { get; protected set; }

        private bool modelBlinking = false;
        private float timeToBlink = 0.0f;

        public static void init(Game game)
        {
            modelManager = new ModelManager(game);
        }

        public Drawable3D(Game game, Entity.Entity parent,Model model)
            : base(game, parent)
        {
            this.model = model;
            modelManager.addModelToDrawList(Draw);

            boundingBoxes = new List<BoundingBox>();
            Vector3[] data;
            VertexDeclaration vd = model.Meshes[0].MeshParts[0].VertexBuffer.VertexDeclaration;
           
            foreach (ModelMesh mesh in model.Meshes)
            {                
                if (mesh.Name.Length > 3 && mesh.Name.Substring(0, 3) == "UCX")
                {
                    ModelMeshPart mmp = mesh.MeshParts[0];
                    data = new Vector3[mmp.NumVertices];
                    mesh.MeshParts[0].VertexBuffer.GetData<Vector3>(mmp.VertexOffset * vd.VertexStride, data, 0, mmp.NumVertices, vd.VertexStride);
                    Matrix transform = GetAbsoluteTransform(mesh.ParentBone);
                    boundingBoxes.Add(createBoundingBox(data, mesh.ParentBone.Transform));
                }
                else
                {
                    ModelMeshPart mmp = mesh.MeshParts[0];
                    data = new Vector3[mmp.NumVertices];
                    mesh.MeshParts[0].VertexBuffer.GetData<Vector3>(mmp.VertexOffset * vd.VertexStride, data, 0, mmp.NumVertices, vd.VertexStride);
                    Matrix transform = GetAbsoluteTransform(mesh.ParentBone);
                    createModelBoundingBox(data, mesh.ParentBone.Transform);
                }                        
            }
        }

        private void createModelBoundingBox(Vector3[] data, Matrix transform)
        {
            BoundingBox meshBoundingBox = createBoundingBox(data, transform);
            Vector3 min = Vector3.Zero;
            Vector3 max = Vector3.Zero;
            if (modelBoundingBox == null)
            {
                modelBoundingBox = meshBoundingBox;
            }
            else
            {
                min.X = getSmaller(meshBoundingBox.Min.X, modelBoundingBox.Min.X);
                min.Y = getSmaller(meshBoundingBox.Min.Y, modelBoundingBox.Min.Y);
                min.Z = getSmaller(meshBoundingBox.Min.Z, modelBoundingBox.Min.Z);

                max.X = getLarger(meshBoundingBox.Max.X, modelBoundingBox.Max.X);
                max.Y = getLarger(meshBoundingBox.Max.Y, modelBoundingBox.Max.Y);
                max.Z = getLarger(meshBoundingBox.Max.Z, modelBoundingBox.Max.Z);
                modelBoundingBox = new BoundingBox(min, max);
            }
        }

        public void adjustModelBoundingBoxWithCollisionBoxes()
        {
            Vector3 min = Vector3.Zero;
            Vector3 max = Vector3.Zero;
            BoundingBox meshBoundingBox = new BoundingBox();

            for (int i = 0; i < boundingBoxes.Count; i++)
            {
                meshBoundingBox = boundingBoxes[i];

                min.X = getSmaller(meshBoundingBox.Min.X, modelBoundingBox.Min.X);
                min.Y = getSmaller(meshBoundingBox.Min.Y, modelBoundingBox.Min.Y);
                min.Z = getSmaller(meshBoundingBox.Min.Z, modelBoundingBox.Min.Z);

                max.X = getLarger(meshBoundingBox.Max.X, modelBoundingBox.Max.X);
                max.Y = getLarger(meshBoundingBox.Max.Y, modelBoundingBox.Max.Y);
                max.Z = getLarger(meshBoundingBox.Max.Z, modelBoundingBox.Max.Z);
                modelBoundingBox = new BoundingBox(min, max);
            }
        }

        private float getSmaller(float alpha, float beta)
        {
            if (alpha<beta)
            {
                return alpha;
            }
            else
            {
                return beta;
            }
        }

        private float getLarger(float alpha, float beta)
        {
            if (alpha > beta)
            {
                return alpha;
            }
            else
            {
                return beta;
            }
        }

        private Matrix GetAbsoluteTransform(ModelBone bone)
        {
            if (bone == null)
                return Matrix.Identity;
            return bone.Transform * GetAbsoluteTransform(bone.Parent);
        }

        private BoundingBox createBoundingBox(Vector3[] data, Matrix transform)
        {
            Vector3 max = data[0];
            Vector3 min = max;
            float length=0.0f;
            float testLength = 0.0f;
            foreach (Vector3 vec in data)
            {
                testLength = (vec-min).LengthSquared();
                if (testLength > length)
                {
                    length = testLength;
                    max = vec;
                }
            }
            if (min.LengthSquared() > max.LengthSquared())
            {
                Vector3 temp = min;
                min = max;
                max = temp;
            }
            min = Vector3.Transform(min, transform);
            max = Vector3.Transform(max, transform);
            return new BoundingBox(min, max);
        }

        public override void cleanUp()
        {
            modelManager.removeFromDrawList(Draw);
        }

        public void turnOnStealth(float alpha = 0.15f)
        {
            this.alpha = alpha;
        }

        public void turnOffStealth()
        {
            alpha = 1.0f;
        }

        public virtual void Draw(Camera camera)
        {
            ((Game1)Game).GraphicsDevice.BlendState = BlendState.AlphaBlend;
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in model.Meshes)
            {
                // alternating these will display models and collisions meshes
               if (mesh.Name.Length < 3 || mesh.Name.Substring(0, 3) != "UCX") 
               //if (mesh.Name.Length > 3 && mesh.Name.Substring(0, 3) == "UCX") //debugShapes, collisions
               {                   
                   foreach (BasicEffect be in mesh.Effects)
                   {
                      // be.EnableDefaultLighting();
                       be.Projection = camera.projection;
                       be.View = camera.view;
                       be.World = getComponent<Spatial>().transform * mesh.ParentBone.Transform;
                       be.Alpha = alpha;
                       //be.FogEnabled = true;
                       //be.FogColor = new Vector3(1,1,1);
                       //be.FogStart = 9.75f;
                       //be.FogEnd = 10.25f;
                       be.LightingEnabled = true;
                       be.DirectionalLight0.Direction = new Vector3(0,-35,-30);
                       be.DirectionalLight0.DiffuseColor = /*new Vector3(1,0,0);*/new Vector3(0.85f, 0.85f, 0.85f);
                       be.DirectionalLight0.SpecularColor = new Vector3(.9f,.9f,.9f);
                       be.AmbientLightColor = new Vector3(.15f,.15f,.15f);
                   }
                   if (parent.GetType() == typeof(Player))
                   {
                       if (mesh.Name.Substring(0, 4) == "Wing")
                       {
                           if (mesh.Name.Substring(4, 1) == "1")
                           {
                               foreach (BasicEffect be in mesh.Effects)
                               {
                                   Vector3 trans = new Vector3(-2.5f, 0, 0.4f);
                                   be.World = Matrix.CreateTranslation(trans) * Matrix.CreateFromAxisAngle(Vector3.Up, -((Entity.Player)parent).angleOfWings / 180.0f * (float)Math.PI) * Matrix.CreateTranslation(-trans) * getComponent<Spatial>().transform * mesh.ParentBone.Transform;
                               }
                           }
                           else
                           {
                               foreach (BasicEffect be in mesh.Effects)
                               {
                                   Vector3 trans = new Vector3(2.5f, 0, 0.4f);
                                   be.World = Matrix.CreateTranslation(trans) * Matrix.CreateFromAxisAngle(Vector3.Up, ((Entity.Player)parent).angleOfWings / 180.0f * (float)Math.PI) * Matrix.CreateTranslation(-trans) * getComponent<Spatial>().transform * mesh.ParentBone.Transform;
                               }
                           }
                       }
                   }                   
                   mesh.Draw();
               }
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (modelBlinking)
            {
                BlinkPlayer();
                timeToBlink -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (timeToBlink <= 0.0f)
                {
                    timeToBlink = 0.0f;
                    modelBlinking = false;
                    turnOffStealth();
                }
            }

            base.Update(gameTime);
        }

        private void BlinkPlayer()
        {
            if (alpha == 0.15f)
                turnOffStealth();
            else
                turnOnStealth();
        }

        public void TurnOnBlinking(float duration)
        {
            timeToBlink = duration;
            modelBlinking = true;
        }

        public ModelExtractor perfromModelExtraction()
        {
            foreach (ModelMesh meshModel in model.Meshes)
            {
                foreach (ModelMeshPart modelMeshPartModel in meshModel.MeshParts)
                {
                    modelExtractor = new ModelExtractor(modelMeshPartModel, new Vector3[modelMeshPartModel.NumVertices * 2], new VertexPositionTexture[modelMeshPartModel.NumVertices]);
                    modelExtractor.ExtractVertices();
                }
            }

            return modelExtractor;
        }


    }
}
