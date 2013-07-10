using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project2.Entity;
using Project2.Component;
using Project2.Events;
using Microsoft.Xna.Framework;

namespace Project2.Entity_Managers
{
    class Building_Manager :Entity.Entity
    {
        List<Building> Block;
        public static float xShift = 100;
        public static float zShift = 120;
        public Building_Manager(Game game):
            base(game)
        {
            Block = new List<Building>();
            addComponent(new Spatial(game, this, Vector3.Zero));
            addComponent(new Collidable(game, this, CollisionType.environment, onHit, 0, 0, 2000));
        }

        public void onHit(eCollision e)
        {

        }

        public void addBuilding(Building building)
        {
            Block.Add(building);
        }
        public static Vector2 getBlockSize()
        {
            return new Vector2((int) (12 * xShift),(int) (10 * zShift));
        }
        public void buildBlock(Vector3 start)
        {
            getComponent<Spatial>().position = start + new Vector3(xShift*6, 0, zShift*5);
            new Building(Game, BuildingType.Iffel_Tower,this, start,250.0f);
            new Building(Game, BuildingType.Building, this, start + new Vector3(3 * xShift, 0, 0 * zShift), 250.0f);
            new Building(Game, BuildingType.Warehouse, this, start + new Vector3(6 * xShift, 0, 0 * zShift), 280.0f);
            new Building(Game, BuildingType.Building, this, start + new Vector3(12 * xShift, 0, 0 * zShift), 180.0f);
            //
            new Building(Game, BuildingType.Building, this, start + new Vector3(0 * xShift, 0, 5 * zShift), 240.0f);
            new Building(Game, BuildingType.Building, this, start + new Vector3(3 * xShift, 0, 5 * zShift), 190.0f);
            new Building(Game, BuildingType.Bridge, this, start + new Vector3(12 * xShift, 0, 5 * zShift), 170.0f, true);
            //
            new Building(Game, BuildingType.Circular_Tower, this, start + new Vector3(0 * xShift, 0, 10 * zShift), 270.0f);
            new Building(Game, BuildingType.Skyscraper, this, start + new Vector3(5 * xShift, 0, 10 * zShift), 220.0f);
            new Building(Game, BuildingType.Building, this, start + new Vector3(12 * xShift, 0, 10 * zShift), 220.0f);
        }

        public void buildStreet(Vector3 start)
        {
            getComponent<Spatial>().position = start + new Vector3(xShift * 6, 0, zShift * 5);
            new Building(Game, BuildingType.Pointed_Tower, this, start, 200.0f);
            new Building(Game, BuildingType.Skyscraper, this, start + new Vector3(3 * xShift, 0, 0 * zShift), 200.0f);
            new Building(Game, BuildingType.Circular_Tower, this, start + new Vector3(6 * xShift, 0, 0 * zShift), 170.0f);
            new Building(Game, BuildingType.Building, this, start + new Vector3(12 * xShift, 0, 0 * zShift), 300.0f);
            //
            new Building(Game, BuildingType.Building, this, start + new Vector3(0 * xShift, 0, 5 * zShift), 300.0f);
            new Building(Game, BuildingType.Building, this, start + new Vector3(3 * xShift, 0, 5 * zShift), 250.0f);
            new Building(Game, BuildingType.Circular_Tower, this, start + new Vector3(6 * xShift, 0, 5 * zShift), 330.0f);
            new Building(Game, BuildingType.Pointed_Tower, this, start + new Vector3(12 * xShift, 0, 5 * zShift), 300.0f);
            //
            new Building(Game, BuildingType.Building, this, start + new Vector3(0 * xShift, 0, 10 * zShift), 330.0f);
            new Building(Game, BuildingType.Skyscraper, this, start + new Vector3(6 * xShift, 0, 10 * zShift), 330.0f);
            new Building(Game, BuildingType.Circular_Tower, this, start + new Vector3(12 * xShift, 0, 10 * zShift), 300.0f);
        }

        public void buildSpecial(Vector3 start)
        {
            getComponent<Spatial>().position = start + new Vector3(xShift * 6, 0, zShift * 5);
            new Building(Game, BuildingType.Iffel_Tower, this, start + new Vector3(0 * xShift, 0, 0 * zShift), 270.0f);

            new Building(Game, BuildingType.Building, this, start + new Vector3(-3 * xShift, 0, 0 * zShift), 220.0f);
            new Building(Game, BuildingType.Building, this, start + new Vector3(0 * xShift, 0, -3 * zShift), 230.0f);
            new Building(Game, BuildingType.Building, this, start + new Vector3(3 * xShift, 0, 0 * zShift), 190.0f);
            new Building(Game, BuildingType.Building, this, start + new Vector3(0 * xShift, 0, 3 * zShift), 220.0f);

            new Building(Game, BuildingType.Bridge, this, start + new Vector3(14 * xShift, 0, 8 * zShift), 140.0f, true);
            new Building(Game, BuildingType.Skyscraper, this, start + new Vector3(0 * xShift, 0, 6 * zShift), 205.0f);
            new Building(Game, BuildingType.Skyscraper, this, start + new Vector3(3 * xShift, 0, 6 * zShift), 200.0f);
            new Building(Game, BuildingType.Skyscraper, this, start + new Vector3(6 * xShift, 0, 6 * zShift), 95.0f);
            new Building(Game, BuildingType.Skyscraper, this, start + new Vector3(9 * xShift, 0, 6 * zShift), 230.0f);
            new Building(Game, BuildingType.Skyscraper, this, start + new Vector3(0 * xShift, 0, 12 * zShift), 200.0f);
            new Building(Game, BuildingType.Skyscraper, this, start + new Vector3(3 * xShift, 0, 12 * zShift), 240.0f);
            new Building(Game, BuildingType.Skyscraper, this, start + new Vector3(6 * xShift, 0, 12 * zShift), 240.0f);
            new Building(Game, BuildingType.Skyscraper, this, start + new Vector3(9 * xShift, 0, 12 * zShift), 205.0f);
        }
    }
}
