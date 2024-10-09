using EnterTheMines.EnterTheMines.Events;
using EnterTheMines.EnterTheMines.Services;
using Godot;
using System;

namespace EnterTheMines.EnterTheMines.Interactables.Items;

public partial class ItemSpawner : Node3D
{
    [Export] private int Depth { get; set; } = 1; // 1 to 4

    [Export] private PackedScene GoldRing { get; set; }
    [Export] private PackedScene Emerald { get; set; }
    [Export] private PackedScene Amethyst { get; set; }
    [Export] private PackedScene Kryptonite { get; set; }

    public override void _Ready()
    {
        GameEvents.Register<StartDayGameEvent>(SpawnItem);
    }

    public void SpawnItem(StartDayGameEvent _)
    {
        // 1 in 9 chance
        if (new Random().Next(9) != 0) return;

        int rand = new Random().Next(100);
        switch (Depth)
        {
            case 1:
                if (rand < 75)
                    Spawn(Emerald);
                else
                    Spawn(GoldRing);
                break;
            case 2:
                if (rand < 50)
                    Spawn(Emerald);
                else if (rand < 75)
                    Spawn(GoldRing);
                else
                    Spawn(Amethyst);
                break;
            case 3:
                if (rand < 25)
                    Spawn(Emerald);
                else if (rand < 50)
                    Spawn(GoldRing);
                else if (rand < 75)
                    Spawn(Amethyst);
                else
                    Spawn(Kryptonite);
                break;
            case 4:
                if (rand < 10)
                    Spawn(Emerald);
                if (rand < 30)
                    Spawn(GoldRing);
                if (rand < 65)
                    Spawn(Amethyst);
                else
                    Spawn(Kryptonite);
                break;
        }
    }

    public void Spawn(PackedScene scene)
    {
        Node3D item = scene.Instantiate() as Node3D;
        item.GlobalTransform = GlobalTransform;
        GetParent().AddChild(item);
    }

    // Add icon to editor 3D view with plugin
    public static Godot.Collections.Dictionary<string, Variant> _DevIcon()
    {
        return new Godot.Collections.Dictionary<string, Variant>
        {
            { "path", "res://EnterTheMines/interactables/items/item_spawner_icon.png" },
            { "size", 0.5f }
        };
    }
}
