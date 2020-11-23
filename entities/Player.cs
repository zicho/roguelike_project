using System;
using System.Linq;
using Extensions;
using Godot;
using GoRogue;
using GoRogue.GameFramework;
using Helpers;

namespace Actors {
    public class Player : Actor {
        public override void _Ready() {
            base._Ready();
            GD.Print($"Player pos: {MapPosition}");
        }

        public override void _Process(float delta) {
            if (Input.IsActionJustPressed("N")) MoveIn(Direction.UP);
            if (Input.IsActionJustPressed("NE")) MoveIn(Direction.UP_RIGHT);
            if (Input.IsActionJustPressed("E")) MoveIn(Direction.RIGHT);
            if (Input.IsActionJustPressed("SE")) MoveIn(Direction.DOWN_RIGHT);
            if (Input.IsActionJustPressed("S")) MoveIn(Direction.DOWN);
            if (Input.IsActionJustPressed("SW")) MoveIn(Direction.DOWN_LEFT);
            if (Input.IsActionJustPressed("W")) MoveIn(Direction.LEFT);
            if (Input.IsActionJustPressed("NW")) MoveIn(Direction.UP_LEFT);

            if (Input.IsActionJustPressed("ui_accept")) {
                foreach (var node in GetTree().GetNodesInGroup("Enemies")) {
                    if (node is Enemy enemy) {
                        enemy.MoveRandom();
                    }
                }
            }
        }

        private void OnPlayerActed(object sender, ItemMovedEventArgs<IGameObject> e) {
            GD.Print("player acted");
            foreach (var node in GetTree().GetNodesInGroup("Enemies")) {
                if (node is Enemy enemy) {
                    // enemy.MoveRandom();
                }
            }

            CalculateFOV();
            UpdateEnemyVisibility();
        }

        private void CalculateFOV() {
            foreach (var cell in CurrentMap.FOV.CurrentFOV) {
                MapHelper.FogMap.SetCell(cell.X, cell.Y, 1);
            }

            CurrentMap.CalculateFOV(MapPosition.ToCoord(), 30);
            MapHelper.PlayerFOV = CurrentMap.FOV.CurrentFOV.ToList();

            foreach (var cell in CurrentMap.FOV.CurrentFOV) {
                MapHelper.FogMap.SetCell(cell.X, cell.Y, -1);
            }
        }

        private void UpdateEnemyVisibility() {
            try {
                foreach (Enemy enemy in GetTree().GetNodesInGroup("Enemies")) {
                    enemy.Visible = MapHelper.PlayerFOV.Contains(enemy.MapPosition.ToCoord());
                }
            } catch (InvalidCastException ice) {
                GD.Print($"Failed to cast node to enemy. Enemy group may contain a class which is not an enemy. Message: {ice}");
            }

        }
    }
}