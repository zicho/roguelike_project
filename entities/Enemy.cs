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
        public FleeMap FleeMap { get; private set; }
        public IMapView<GoalState> BaseMap { get; private set; }

        public override void _Ready()
        {
            base._Ready();
            AddToGroup("Enemies");
            Moved += OnEnemyActed;
            CreateGoalMap();
        }

        private void CreateGoalMap()
        {
            BaseMap = new LambdaMapView<GoalState>(CurrentMap.Width, CurrentMap.Height, PlayerGoalMapFunc);
            GoalMap = new GoalMap(BaseMap, Distance.CHEBYSHEV);
            GoalMap.Update();
            FleeMap = new FleeMap(GoalMap);
            GoalMap.Update();
        }

        private void UpdateGoalMap()
        {
            BaseMap = new LambdaMapView<GoalState>(CurrentMap.Width, CurrentMap.Height, PlayerGoalMapFunc);
            GoalMap.Update();
        }

        private void OnEnemyActed(object sender, ItemMovedEventArgs<IGameObject> e) { }

        private GoalState PlayerGoalMapFunc(Coord position)
        {
            if (position == EntityHelper.PlayerPosition.ToCoord()) return GoalState.Goal;
            else if (position == _backingField.Position) return GoalState.Clear;
            return CurrentMap.WalkabilityView[position] ? GoalState.Clear : GoalState.Obstacle;
        }

        internal void Move()
        {
            CalculateFOV();

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
                UpdateGoalMap();
                var f = FleeMap;
                var dir = FleeMap.GetDirectionOfMinValue(_backingField.Position);
                GD.Print(dir);
                MoveIn(dir);
            }
            catch (Exception ex)
            {
                GD.Print(ex.Message);
            }

        }

        public override void CalculateFOV()
        {
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