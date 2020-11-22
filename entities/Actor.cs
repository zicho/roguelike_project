using System;
using System.Collections.Generic;
using Extensions;
using Godot;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapViews;
using Helpers;

namespace Actors {
    public class Actor : Node2D, IGameObject {
        public readonly IGameObject _backingField;
        private readonly Coord _origPos;

        public Map CurrentMap { get; set; }

        private static readonly IDGenerator generator = new IDGenerator();

        public bool IsStatic => false;

        public bool IsTransparent { get; set; }
        public bool IsWalkable { get; set; }

        public uint ID => generator.UseID();

        public int Layer => 1;

        public Vector2 MapPosition {
            get => MapHelper.GetMapPosition(Position);
            set {
                Position = MapHelper.SetMapPosition(value);
                _backingField.Position = value.ToCoord();
                Moved?.Invoke(this, new ItemMovedEventArgs<IGameObject>(this, _backingField.Position, value.ToCoord()));
            }
        }

        Coord IGameObject.Position {
            get => _backingField.Position;
            set {
                _backingField.Position = value;
                MapPosition = value.ToVector2();
            }
        }

        public event EventHandler<ItemMovedEventArgs<IGameObject>> Moved;

        public Actor() {
            var pos = MapHelper.RandomEmpty.ToCoord();

            _backingField = new GameObject(
                position: pos,
                layer: 1,
                parentObject: this,
                isStatic: false,
                isWalkable: false,
                isTransparent: IsTransparent);

            _origPos = pos;

            MapHelper.CurrentMap.AddEntity(this);
            MapHelper.TileMap.AddChild(this);
        }

        public bool MoveIn(Direction direction) {

            // GD.Print("original pos walkable?: " + _origPos + " " + CurrentMap.WalkabilityView[_origPos]);

            // GD.Print($"bPos: {_backingField.Position}, mapPos: {MapPosition}");
            Coord newPos = _backingField.Position + direction;

            // if (_backingField.Position == newPos || (CurrentMap?.Contains(newPos) == false))
            //     return false;

            if(IsTransparent) GD.Print($"New Pos is: {newPos}, walkable: {CurrentMap.WalkabilityView[newPos]}");

            if (!MapHelper.EmptyTiles.Contains(newPos.ToVector2()) || !CurrentMap.WalkabilityView[newPos]) {
                return false;
            }

            MapPosition = newPos.ToVector2();
            return true;//_backingField.MoveIn(direction);
        }

        public void AddComponent(object component) {
            throw new NotImplementedException();
        }

        public T GetComponent<T>() {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetComponents<T>() {
            throw new NotImplementedException();
        }

        public bool HasComponent(Type componentType) {
            throw new NotImplementedException();
        }

        public bool HasComponent<T>() {
            throw new NotImplementedException();
        }

        public bool HasComponents(params Type[] componentTypes) {
            throw new NotImplementedException();
        }

        public void OnMapChanged(Map newMap) {
            CurrentMap = newMap;
        }

        public void RemoveComponent(object component) {
            throw new NotImplementedException();
        }

        public void RemoveComponents(params object[] components) {
            throw new NotImplementedException();
        }

        public override void _Ready()
        {
            AddToGroup("Actors");
            MapPosition = _backingField.Position.ToVector2();
            // GD.Print($"Entity {Name} was placed at {MapPosition}");
        }
    }
}