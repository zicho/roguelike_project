using Godot;
using GoRogue;

namespace Extensions {
    public static class Converters {
        internal static Coord ToCoord(this Vector2 value) => new Coord((int) value.x, (int) value.y);
        internal static Vector2 ToVector2(this Coord value) => new Vector2(value.X, value.Y);
        internal static Direction Reverse(this Direction direction) {
            if(direction == Direction.UP) return Direction.DOWN;
            if(direction == Direction.UP_RIGHT) return Direction.DOWN_LEFT;
            if(direction == Direction.RIGHT) return Direction.LEFT;
            if(direction == Direction.DOWN_RIGHT) return Direction.UP_LEFT;
            if(direction == Direction.DOWN) return Direction.UP;
            if(direction == Direction.DOWN_LEFT) return Direction.UP_RIGHT;
            if(direction == Direction.LEFT) return Direction.RIGHT;
            if(direction == Direction.UP_LEFT) return Direction.DOWN_RIGHT;

            GD.Print("Could not reverse direction");

            return direction;
        }

        // internal static Direction ToDirection(this Vector2 value) {
        //     if(value == Vector2.Up) return Direction.UP;
        //     else if(value == Vector2.Up) return Direction.UP;
        //     else if(value.x == Vector2.Up.x && value.y == Vector2.Right) return Direction.UP;
        //     else if(value == Vector2.Up) return Direction.UP;
        //     else if(value == Vector2.Up) return Direction.UP;
        //     else if(value == Vector2.Up) return Direction.UP;
        //     else if(value == Vector2.Up) return Direction.UP;
        //     else if(value == Vector2.Up) return Direction.UP;
        //     else if(value == Vector2.Up) return Direction.UP;
        //     return Direction.NONE;
        // }
    }
}