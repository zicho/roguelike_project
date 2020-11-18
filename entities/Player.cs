using System;
using Extensions;
using Godot;
using GoRogue;
using GoRogue.GameFramework;
using Helpers;

namespace Actors
{
    public class Player : Actor
    {
        [Signal]
        public delegate void PlayerActed();

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
        }

        public Player() : base()
        {
            Moved += OnPlayerActed;
            AddToGroup("Enemies");
        }

        private void OnPlayerActed(object sender, ItemMovedEventArgs<IGameObject> e)
        {
            foreach(var node in GetTree().GetNodesInGroup("Enemies")) {
                if(node is Enemy enemy) {
                    enemy.MoveRandom();
                }
            }

            // en.MoveRandom();
        }
    }
}