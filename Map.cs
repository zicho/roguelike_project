using System.Linq;
using Actors;
using Godot;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapGeneration;
using GoRogue.MapViews;
using Helpers;

namespace MainGame
{
    public class Map : TileMap
    {
        public override void _Ready()
        {
            var terrain = new ArrayMap<bool>(200, 200);

            MapHelper.TileMap = this;
            MapHelper.FogMap = GetParent().GetNode<TileMap>("FogMap");
            MapHelper.WalkMap = GetParent().GetNode<TileMap>("WalkMap");
            

            QuickGenerators.GenerateCellularAutomataMap(terrain);
            //QuickGenerators.GenerateRectangleMap(terrain);

            var map = new GoRogue.GameFramework.Map(
                width: terrain.Width,
                height: terrain.Height,
                numberOfEntityLayers: 1,
                distanceMeasurement: Distance.CHEBYSHEV
            );

            foreach (var pos in terrain.Positions())
            {
                MapHelper.FogMap.SetCell(pos.X, pos.Y, 0);
                if (terrain[pos])
                {
                    SetCell(pos.X, pos.Y, 0);
                    MapHelper.WalkMap.SetCell(pos.X, pos.Y, 0);
                    map.SetTerrain(TerrainFactory.Floor(pos));
                    MapHelper.EmptyTiles.Add(new Vector2(pos.X, pos.Y));
                }
                else // Wall
                {
                    MapHelper.WalkMap.SetCell(pos.X, pos.Y, 1);
                    map.SetTerrain(TerrainFactory.Wall(pos));
                    SetCell(pos.X, pos.Y, 1);
                }
            }

            var playerScene = GD.Load<PackedScene>("res://entities/Player.tscn");
            var player = playerScene.Instance() as Player;

            map.AddEntity(player);
            MapHelper.TileMap.AddChild(player);

            const int enemyCount = 6;

            foreach (var enemy in from e in Enumerable.Range(0, enemyCount)
                                  let enemyScene = GD.Load<PackedScene>("res://entities/Enemy.tscn")
                                  let enemy = enemyScene.Instance() as Enemy
                                  select enemy)
            {
                map.AddEntity(enemy);
                MapHelper.TileMap.AddChild(enemy);
            }

            MapHelper.AStar = new GoRogue.Pathing.AStar(map.WalkabilityView, Distance.CHEBYSHEV);

            MapHelper.CurrentMap = map;

            GD.Print("Number of entities: " + MapHelper.CurrentMap.Entities.Count);
            GD.Print("Number of children: " + GetChildCount());
        }

        private static class TerrainFactory
        {
            public static GameObject Wall(Coord position) => new GameObject(position, layer: 0, parentObject: null, isStatic: true, isWalkable: false, isTransparent: false);
            public static GameObject Floor(Coord position) => new GameObject(position, layer: 0, parentObject: null, isStatic: true, isWalkable: true, isTransparent: true);
        }
    }
}