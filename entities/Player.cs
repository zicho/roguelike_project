using System;
using System.Linq;
using Extensions;
using Godot;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapViews;
using Helpers;

namespace Actors
{
    public class Player : Actor
    {
        public double FovRadius { get; } = 20;

        public event EventHandler PlayerActed;

        public override void _Ready()
        {
            base._Ready();
            EntityHelper.Player = this;
            // GD.Print($"Player pos: {MapPosition}");
            CalculateFOV();
            Acted += OnPlayerActed;
            // PlayerActed += OnPlayerActed;
        }

        public override void _Process(float delta)
        {
            if (Input.IsActionJustPressed("N")) MoveIn(Direction.UP);
            if (Input.IsActionJustPressed("NE")) MoveIn(Direction.UP_RIGHT);
            if (Input.IsActionJustPressed("E")) MoveIn(Direction.RIGHT);
            if (Input.IsActionJustPressed("SE")) MoveIn(Direction.DOWN_RIGHT);
            if (Input.IsActionJustPressed("S")) MoveIn(Direction.DOWN);
            if (Input.IsActionJustPressed("SW")) MoveIn(Direction.DOWN_LEFT);
            if (Input.IsActionJustPressed("W")) MoveIn(Direction.LEFT);
            if (Input.IsActionJustPressed("NW")) MoveIn(Direction.UP_LEFT);
            if (Input.IsActionJustPressed("WAIT")) MoveIn(Direction.NONE);

            if (Input.IsActionJustPressed("ui_accept"))
            {   
                // EntityHelper.EnemiesActive = !EntityHelper.EnemiesActive;

                foreach (var node in GetTree().GetNodesInGroup("Enemies"))
                {
                    if (node is Enemy enemy)
                    {
                        // if(enemy.Target == null) enemy.Target = this; else enemy.Target = null;
                        // enemy.MoveRandom();
                        // PlayerActed.Invoke(this, null);
                    }
                }
            }

            // if (Input.IsActionJustPressed("ui_accept"))
            // {
            //     MapPosition = MapHelper.RandomEmpty;
            //     // PlayerActed.Invoke(this, null);
            // }
        }

        private void OnPlayerActed(object sender, EventArgs e)
        {
            GD.Print("player acted");
            if (EntityHelper.EnemiesActive)
            {
                foreach (var node in GetTree().GetNodesInGroup("Enemies"))
                {
                    if (node is Enemy enemy)
                    {
                        enemy.MoveRandom();
                    }
                }
            }

            CalculateFOV();
            UpdateEnemyVisibility();
        }

        private void CalculateFOV()
        {
            if (CurrentMap == null || MapHelper.FogMap == null) return; // failsafe, should not occur

            foreach (var cell in CurrentMap.FOV.CurrentFOV)
            {
                MapHelper.FogMap.SetCell(cell.X, cell.Y, 1);
            }

            CurrentMap.CalculateFOV(MapPosition.ToCoord(), FovRadius);
            MapHelper.PlayerFOV = CurrentMap.FOV.CurrentFOV.ToList();

            foreach (var cell in CurrentMap.FOV.CurrentFOV)
            {
                MapHelper.FogMap.SetCell(cell.X, cell.Y, -1);
            }

            PlayerActed?.Invoke(this, null);
        }

        private void UpdateEnemyVisibility()
        {
            if(!MapHelper.FogMap.Visible) return;

            try
            {
                foreach (Enemy enemy in GetTree().GetNodesInGroup("Enemies"))
                {
                    enemy.Visible = MapHelper.PlayerFOV.Contains(enemy.MapPosition.ToCoord());
                }
            }
            catch (InvalidCastException ice)
            {
                GD.Print($"Failed to cast node to enemy. Enemy group may contain a class which is not an enemy. Message: {ice}");
            }

        }
    }
}