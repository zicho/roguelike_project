using System;
using System.Collections.Generic;
using Extensions;
using Godot;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapViews;
using Helpers;

namespace Actors
{
    public class Actor : Node2D, IGameObject
    {
        public IGameObject _backingField;
        public Actor Target { get; set; }
        public bool IsStatic => _backingField.IsStatic;

        public bool IsTransparent { get => _backingField.IsTransparent; set => _backingField.IsTransparent = value; }
        public bool IsWalkable { get => _backingField.IsWalkable; set => _backingField.IsWalkable = value; }

        public uint ID => _backingField.ID;

        public int Layer => 1;

        public Vector2 MapPosition
        {
            get => MapHelper.GetMapPosition(Position);
            set
            {
                _backingField.Position = value.ToCoord();
                Position = MapHelper.SetMapPosition(value);
            }
        }

        Coord IGameObject.Position
        {
            get => _backingField.Position;
            set
            {
                _backingField.Position = value;
                MapPosition = value.ToVector2();
            }
        }

        public Map CurrentMap => _backingField.CurrentMap;

        public event EventHandler<ItemMovedEventArgs<IGameObject>> Moved
        {
            add => _backingField.Moved += value;
            remove => _backingField.Moved -= value;
        }

        public event EventHandler Acted;

        public Actor()
        {
            var pos = MapHelper.RandomEmpty.ToCoord();

            _backingField = new GameObject(
                position: pos,
                layer: 1,
                parentObject: this,
                isStatic: false,
                isWalkable: false,
                isTransparent: false);

            // MapHelper.TileMap.AddChild(this);
        }

        public override void _Ready()
        {
            AddToGroup("Actors");
            MapPosition = _backingField.Position.ToVector2(); // used to set the position on the graphical map, should always "mirror" the backing field position
        }

        public void OnMapChanged(Map newMap) => _backingField.OnMapChanged(newMap);

        public bool MoveIn(Direction direction)
        {
            Coord newPos = _backingField.Position + direction;
            var s = _backingField.MoveIn(direction);
            if (s) MapPosition = newPos.ToVector2();
            Acted?.Invoke(this, null);

            foreach (var pos in CurrentMap.Positions())
            {
                if (CurrentMap.WalkabilityView[pos])
                {
                    MapHelper.WalkMap.SetCell(pos.X, pos.Y, 0);
                }
                else
                {
                    MapHelper.WalkMap.SetCell(pos.X, pos.Y, 1);
                }
            }

            if (MapPosition.ToCoord() != _backingField.Position) GD.Print("There is a pos mismatch");

            return s;
        }

        public void AddComponent(object component)
        {
            throw new NotImplementedException();
        }

        public T GetComponent<T>()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetComponents<T>()
        {
            throw new NotImplementedException();
        }

        public bool HasComponent(Type componentType)
        {
            throw new NotImplementedException();
        }

        public bool HasComponent<T>()
        {
            throw new NotImplementedException();
        }

        public bool HasComponents(params Type[] componentTypes)
        {
            throw new NotImplementedException();
        }

        public void RemoveComponent(object component)
        {
            throw new NotImplementedException();
        }

        public void RemoveComponents(params object[] components)
        {
            throw new NotImplementedException();
        }
    }
}