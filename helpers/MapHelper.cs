using System;
using System.Collections.Generic;
using Extensions;
using Godot;
using GoRogue.GameFramework;

namespace Helpers {
    public static class MapHelper {
        public static Map CurrentMap { get; set; }
        public static TileMap TileMap { get; set; }
        public static TileMap FogMap { get; set; }
        public static List<Vector2> EmptyTiles { get; set; } = new List<Vector2>();
        public static List<Vector2> KnownTiles { get; set; } = new List<Vector2>();
        public static List<Vector2> FogTiles { get; set; } = new List<Vector2>();

        public static Vector2 RandomEmpty => EmptyTiles[new Random().Next(EmptyTiles.Count)];

        public static void AddEntity(Player entity) {
            entity.MapPosition = RandomEmpty.ToCoord();
            TileMap.AddChild(entity);
            CurrentMap.AddEntity(entity);
        }
    }
}