using System;
using System.Collections.Generic;
using Actors;
using Extensions;
using Godot;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapViews;
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
        public override void _Ready() {
            base._Ready();
            AddToGroup("Enemies");
            Moved += OnEnemyActed;
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
    }
}