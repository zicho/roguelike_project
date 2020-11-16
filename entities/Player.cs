using System;
using System.Collections.Generic;
using Extensions;
using Godot;
using GoRogue;
using GoRogue.GameFramework;
using Helpers;

public class Player : Node2D, IGameObject {
    private static readonly IDGenerator generator = new IDGenerator();
    public bool IsStatic => false;

    public bool IsTransparent { get; set; }
    public bool IsWalkable { get; set; }

    public uint ID => generator.UseID();

    public int Layer => 1;

    public Map CurrentMap { get; set; }

    // private Coord _position;
    Coord IGameObject.Position {
        get => _position;
        set {
            _position = value;
            // _backingField.Position = value;
        }
    }

    public Coord MapPosition {
        get => _position;
        set {
            Position = MapHelper.TileMap.MapToWorld(value.ToVector2());
            IGameObject.Position = value;
        }
    }

    private readonly IGameObject _backingField;

    public Player() : base() {
        _backingField = new GameObject(
            position: new Coord(1, 0),
            layer : 1, parentObject : this,
            isStatic : true,
            isWalkable : false,
            isTransparent : false);
    }

    public event EventHandler<ItemMovedEventArgs<IGameObject>> Moved;

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

    public bool MoveIn(Direction direction) {
        throw new NotImplementedException();
    }

    public void OnMapChanged(Map newMap) {
        CurrentMap = newMap;

        // GD.Print("Success? " + newMap.AddEntity(this));
    }

    public void RemoveComponent(object component) {
        throw new NotImplementedException();
    }

    public void RemoveComponents(params object[] components) {
        throw new NotImplementedException();
    }

    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {

    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}