using Extensions;
using GoRogue;
using GoRogue.MapViews;
using GoRogue.Pathing;

namespace Helpers
{
    public static class PathHelper
    {
        public static GoalMap GoalMap { get; set; }
        public static FleeMap FleeMap { get; set; }
        public static IMapView<GoalState> BaseMap { get; set; }

        public static void CreateGoalMap()
        {
            BaseMap = new LambdaMapView<GoalState>(MapHelper.CurrentMap.Width, MapHelper.CurrentMap.Height, PlayerGoalMapFunc);
            GoalMap = new GoalMap(BaseMap, Distance.CHEBYSHEV);
            FleeMap = new FleeMap(GoalMap);
            GoalMap.Update();
        }

        public static void UpdateGoalMap()
        {
            BaseMap = new LambdaMapView<GoalState>(MapHelper.CurrentMap.Width, MapHelper.CurrentMap.Height, PlayerGoalMapFunc);
            GoalMap.UpdatePathsOnly();
        }

        private static GoalState PlayerGoalMapFunc(Coord position)
        {
            if (position == EntityHelper.PlayerPosition.ToCoord()) return GoalState.Goal;
            return MapHelper.CurrentMap.WalkabilityView[position] ? GoalState.Clear : GoalState.Obstacle;
        }
    }
}