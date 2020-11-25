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

        public GoalMap GoalMap { get; private set; }

        public override void _Ready()
        {
            base._Ready();
            AddToGroup("Enemies");
            Moved += OnEnemyActed;

            // UpdateGoalMap();

            // GD.Print(GoalMap.GetDirectionOfMinValue(_backingField.Position));
        }

        private void UpdateGoalMap()
        {
            var baseMap = new LambdaMapView<GoalState>(CurrentMap.Width, CurrentMap.Height, PlayerGoalMapFunc);
            GoalMap = new GoalMap(baseMap, Distance.CHEBYSHEV);
        }

        private void OnEnemyActed(object sender, ItemMovedEventArgs<IGameObject> e) { }

        private GoalState PlayerGoalMapFunc(Coord position)
        {
            if (position == EntityHelper.PlayerPosition.ToCoord()) return GoalState.Goal;
            return CurrentMap.WalkabilityView[position] ? GoalState.Clear : GoalState.Obstacle;
        }

        internal void Move()
        {
            CalculateFOV();

            if(CurrentMap.FOV.CurrentFOV.Contains(EntityHelper.PlayerPosition.ToCoord())) {
                GD.Print("Player sighted!");
                MoveToTarget();
            } else {
                MoveRandom();
            }
        }

        public override void CalculateFOV() {
            base.CalculateFOV();

            foreach (var cell in CurrentMap.FOV.CurrentFOV)
            {
                MapHelper.SightMap.SetCell(cell.X, cell.Y, 0);
            }
        }

        internal void MoveRandom()
        {
            MoveIn(RandomDirection);
        }

        public void MoveToTarget()
        {
            UpdateGoalMap();
            var dir = GoalMap.GetDirectionOfMinValue(_backingField.Position);
            MoveIn(dir);

            GD.Print("Player is on tile: " + GoalMap.BaseMap[EntityHelper.PlayerPosition.ToCoord()]);
        }
    }
}