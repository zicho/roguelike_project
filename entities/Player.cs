using Extensions;
using Godot;
using GoRogue;

namespace Actors
{
    public class Player : Actor
    {
        public override void _Ready()
        {
            MapPosition = _backingField.Position.ToVector2();
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
        }
    }
}