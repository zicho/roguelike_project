using System;
using System.Collections.Generic;
using Actors;
using Extensions;
using Godot;
using GoRogue;
using GoRogue.GameFramework;

namespace Helpers {
    public static class MapHelper {
        public static Map CurrentMap { get; set; }
        public static TileMap TileMap { get; set; }
        public static TileMap FogMap { get; set; }
        public static List<Vector2> EmptyTiles { get; set; } = new List<Vector2>();
        public static List<Vector2> KnownTiles { get; set; } = new List<Vector2>();
        public static List<Vector2> FogTiles { get; set; } = new List<Vector2>();

        public static List<Vector2> EntityPositions { get; set; } = new List<Vector2>();

        public static Vector2 RandomEmpty => EmptyTiles[new Random().Next(EmptyTiles.Count)];

        public static void AddEntity(PackedScene scene) {
            try {
                var entity = scene.Instance() as Actor;
                // entity.RenderSpawnData();
                // TileMap.AddChild(entity);
                
            } catch {

            }

        }

        public static Vector2 SetMapPosition(Vector2 pos) => TileMap.MapToWorld(pos);
        public static Vector2 GetMapPosition(Vector2 pos) => TileMap.WorldToMap(pos);
    }
}