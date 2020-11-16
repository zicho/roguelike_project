using Godot;
using GoRogue;

namespace Extensions {
    public static class Converters {
        internal static Coord ToCoord(this Vector2 value) => new Coord((int) value.x, (int) value.y);
        internal static Vector2 ToVector2(this Coord value) => new Vector2(value.X, value.Y);

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