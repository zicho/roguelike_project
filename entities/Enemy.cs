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

            // var view = new ArrayMap<GoalState>(CurrentMap.Height, CurrentMap.Width);

            // foreach (var pos in CurrentMap.Positions())
            // {
            //     if (CurrentMap.WalkabilityView[pos])
            //     {
            //         view[pos] = GoalState.Clear;
            //     }
            //     else
            //     {
            //         if (pos != _backingField.Position) view[pos] = GoalState.Obstacle;
            //     }
            // }

            // view[EntityHelper.Player.MapPosition.ToCoord()] = GoalState.Goal;

            // GoalMap = new GoalMap(view, Distance.CHEBYSHEV);

            // foreach(var pos in GoalMap.Positions()) GD.Print(GoalMap.BaseMap[pos]);

            // GD.Print(GoalMap.GetDirectionOfMinValue(EntityHelper.Player.MapPosition.ToCoord()));
        }

        private void OnEnemyActed(object sender, ItemMovedEventArgs<IGameObject> e)
        {

        }

        public void MoveTowardsTarget()
        {
            try
            {
                // var path = MapHelper.AStar.ShortestPath(_backingField.Position, EntityHelper.Player.MapPosition.ToCoord());
                var dir = GoalMap.GetDirectionOfMinValue(_backingField.Position);

                var newPos = _backingField.Position + dir;

                var origDistance = CurrentMap.DistanceMeasurement.Calculate(_backingField.Position, Target.MapPosition.ToCoord());
                var newDistance = CurrentMap.DistanceMeasurement.Calculate(newPos, Target.MapPosition.ToCoord());

                GD.Print($"Old: {origDistance}, new: {newDistance}");

                // if (origDistance > newDistance)
                MoveIn(dir);

                // var view = new ArrayMap<GoalState>(CurrentMap.Height, CurrentMap.Width);

                // foreach (var pos in CurrentMap.Positions())
                // {
                //     if (CurrentMap.WalkabilityView[pos])
                //     {
                //         view[pos] = GoalState.Clear;
                //     }
                //     else
                //     {
                //         // if (pos != _backingField.Position) view[pos] = GoalState.Obstacle;
                //     }
                // }

                // view[Target.MapPosition.ToCoord()] = GoalState.Goal;

                // GoalMap = new GoalMap(view, Distance.CHEBYSHEV);
                // GoalMap.Update();
            }
            catch
            {
                GD.Print("Move failed");
            }
        }


        public void MoveRandom() => MoveIn(RandomDirection);

        internal void Move()
        {
            if (Target != null)
            {
                MoveTowardsTarget();
            }
            else
            {
                MoveRandom();
            }
        }
    }
}