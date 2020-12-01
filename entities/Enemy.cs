using System;
using System.Collections.Generic;
using System.Linq;
using Actors;
using Extensions;
using Godot;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapViews;
using GoRogue.Pathing;
using Helpers;

namespace Actors
{
    public class Enemy : Actor
    {
        private readonly Random r = new Random();
        private readonly List<Direction> directions = new List<Direction>() {
            Direction.UP,
            Direction.UP_RIGHT,
            Direction.RIGHT,
            Direction.DOWN_RIGHT,
            Direction.DOWN,
            Direction.DOWN_LEFT,
            Direction.LEFT,
            Direction.UP_LEFT,
        };

        internal Direction RandomDirection => directions[r.Next(directions.Count)];

        public override void _Ready()
        {
            base._Ready();
            AddToGroup("Enemies");
            Moved += OnEnemyActed;
        }

        private void OnEnemyActed(object sender, ItemMovedEventArgs<IGameObject> e) { }

        internal void Move()
        {
            CalculateFOV();
            GD.Print("Player is on tile: " + PathHelper.GoalMap.BaseMap[EntityHelper.PlayerPosition.ToCoord()]);
            
            if (CurrentMap.FOV.CurrentFOV.Contains(EntityHelper.PlayerPosition.ToCoord()))
            {
                FleeFromTarget();
            }
            else
            {
                MoveRandom();
            }
        }

        private void FleeFromTarget()
        {
            try
            {
                var dir = PathHelper.FleeMap.GetDirectionOfMinValue(_backingField.Position);
                GD.Print(dir);
                MoveIn(dir);
            }
            catch (Exception ex)
            {
                GD.Print(ex.Message);
            }
        }

        internal void MoveRandom()
        {
            MoveIn(RandomDirection);
        }

        public void MoveToTarget()
        {
            var dir = PathHelper.GoalMap.GetDirectionOfMinValue(_backingField.Position);
            MoveIn(dir);
        }
    }
}