using System;
using System.Collections.Generic;
using Actors;
using Extensions;
using Godot;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapViews;
using GoRogue.Pathing;
using Helpers;

namespace Actors {
    public class Enemy : Actor {

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

        public override void _Ready() {
            base._Ready();
            AddToGroup("Enemies");
            Moved += OnEnemyActed;

            UpdateGoalMap();

            GD.Print(GoalMap.GetDirectionOfMinValue(_backingField.Position));
        }

        private void UpdateGoalMap()
        {
            var baseMap = new ArrayMap<GoalState>(CurrentMap.Width, CurrentMap.Height);

            foreach (var pos in CurrentMap.Positions())
            {
                if (CurrentMap.WalkabilityView[pos])
                {
                    baseMap[pos] = GoalState.Clear;
                }
            }

            baseMap[EntityHelper.PlayerPosition.ToCoord()] = GoalState.Goal;

            GoalMap = new GoalMap(baseMap, Distance.CHEBYSHEV);
        }

        public override void _Process(float delta)
        {
            base._Process(delta);
        }

        private void OnEnemyActed(object sender, ItemMovedEventArgs<IGameObject> e)
        {
            
        }

        internal void MoveRandom()
        {
            MoveIn(RandomDirection);
        }

        public void MoveToTarget()
        {
            GD.Print(GoalMap.BaseMap[EntityHelper.PlayerPosition.ToCoord()]);

            var dir = GoalMap.GetDirectionOfMinValue(_backingField.Position);

            var newPos = _backingField.Position + dir;

            if(CurrentMap.WalkabilityView[newPos]) MoveIn(dir);
            UpdateGoalMap();

            GD.Print("Player is on tile: " + GoalMap.BaseMap[EntityHelper.PlayerPosition.ToCoord()]);
        }
    }
}