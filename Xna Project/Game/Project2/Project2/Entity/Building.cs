using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project2.Component;
using Project2.Events;
using Project2.Entity_Managers;

namespace Project2.Entity
{
    enum BuildingType{
        Bridge,
        Building_With_Tower,
        Building,
        Circular_Tower,
        City_Building,
        Iffel_Tower,
        Pointed_Tower,
        Skyscraper,
        Tower,
        Triangle_Building,
        Warehouse

    }
    class Building:Entity
    {
        public static List<Building_Manager> managers;
        public Building(Game game, BuildingType type,Building_Manager parent, Vector3 position,float scale = 1.0f, bool rotate90 = false)
            :base(game)
        {
            addComponent(new Spatial(game, this,Vector3.Zero));
            if(rotate90)
                getComponent<Spatial>().transform = Matrix.CreateScale(scale) * Matrix.CreateRotationY((float)Math.PI/2.0f) * Matrix.CreateTranslation(position);
            else
                getComponent<Spatial>().transform = Matrix.CreateScale(scale) * Matrix.CreateTranslation(position);

            addComponent(new Collidable(game, this, CollisionType.environment, onHit, 0, 1000,parent.getComponent<Collidable>(),1400));
            Model m;
            switch (type)
            {
                case BuildingType.Bridge:
                    m = Game.Content.Load<Model>(@"Models\Buildings\Bridge");
                    break;
                case BuildingType.Building_With_Tower:
                    m = Game.Content.Load<Model>(@"Models\Buildings\Building_With_Tower");
                    break;
                case BuildingType.Building:
                    m = Game.Content.Load<Model>(@"Models\Buildings\Building_02");
                    break;
                case BuildingType.Circular_Tower:
                    m = Game.Content.Load<Model>(@"Models\Buildings\Circular_Tower");
                    break;
                case BuildingType.City_Building:
                    m = Game.Content.Load<Model>(@"Models\Buildings\City_Building");
                    break;
                case BuildingType.Iffel_Tower:
                    m = Game.Content.Load<Model>(@"Models\Buildings\Iffel_Tower");
                    break;
                case BuildingType.Pointed_Tower:
                    m = Game.Content.Load<Model>(@"Models\Buildings\Pointed_Tower");
                    break;
                case BuildingType.Skyscraper:
                    m = Game.Content.Load<Model>(@"Models\Buildings\Skyscraper");
                    break;
                case BuildingType.Tower:
                    m = Game.Content.Load<Model>(@"Models\Buildings\Tower");
                    break;
                case BuildingType.Triangle_Building:
                    m = Game.Content.Load<Model>(@"Models\Buildings\Triangle_Building");
                    break;
                case BuildingType.Warehouse:
                    m = Game.Content.Load<Model>(@"Models\Buildings\Warehouse");
                    break;
                default:
                    m = Game.Content.Load<Model>(@"Models\Buildings\Building");
                    break;
            }
            addComponent(new Drawable3D(game,this,m));
            parent.addBuilding(this);
        }

        public static void init(Game game)
        {
            managers = new List<Building_Manager>();
            managers.Add(new Building_Manager(game));
            managers.Add(new Building_Manager(game));
            managers.Add(new Building_Manager(game));
            managers.Add(new Building_Manager(game));
            managers.Add(new Building_Manager(game));
            managers.Add(new Building_Manager(game));
            managers.Add(new Building_Manager(game));
            managers[0].buildBlock(Vector3.Zero);
            managers[1].buildBlock(new Vector3(Building_Manager.getBlockSize().X + (5 * Building_Manager.xShift), 0, 0));
            managers[2].buildBlock(new Vector3(0, 0, Building_Manager.getBlockSize().Y + (5 * Building_Manager.zShift)));

            managers[3].buildBlock(new Vector3(-Building_Manager.getBlockSize().X - (5 * Building_Manager.xShift), 0, 0));
            managers[4].buildSpecial(new Vector3(0, 0, -Building_Manager.getBlockSize().Y - (5 * Building_Manager.zShift)));

            managers[3].buildSpecial(new Vector3(-Building_Manager.getBlockSize().X - (7 * Building_Manager.xShift), 0, -Building_Manager.getBlockSize().Y - (7 * Building_Manager.zShift)));

            managers[5].buildBlock(new Vector3(Building_Manager.getBlockSize().X + (5 * Building_Manager.xShift), 0, Building_Manager.getBlockSize().Y + (5 * Building_Manager.zShift)));
            managers[6].buildBlock(new Vector3(Building_Manager.getBlockSize().X + (5 * Building_Manager.xShift), 0, Building_Manager.getBlockSize().Y + (5 * Building_Manager.zShift)));
        }

        public void onHit(eCollision e)
        {

        }
    }
}
