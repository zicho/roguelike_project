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

            var terrain = new ArrayMap<bool>(20, 20);

            MapHelper.TileMap = this;
            MapHelper.FogMap = GetParent().GetNode<TileMap>("FogMap");
            MapHelper.SightMap = GetParent().GetNode<TileMap>("SightMap");

            QuickGenerators.GenerateCellularAutomataMap(terrain);

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
                    map.SetTerrain(TerrainFactory.Floor(pos));
                    MapHelper.EmptyTiles.Add(new Vector2(pos.X, pos.Y));
                }
                else // Wall
                {
                    map.SetTerrain(TerrainFactory.Wall(pos));
                    SetCell(pos.X, pos.Y, 1);
                }
            }

            var playerScene = GD.Load<PackedScene>("res://entities/Player.tscn");
            var player = playerScene.Instance() as Player;

            map.AddEntity(player);
            MapHelper.TileMap.AddChild(player);

            var enemyScene = GD.Load<PackedScene>("res://entities/Enemy.tscn");

            foreach (var e in Enumerable.Range(0, 2))
            {
                var enemy = enemyScene.Instance() as Enemy;
                map.AddEntity(enemy);
                MapHelper.TileMap.AddChild(enemy);
            }

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