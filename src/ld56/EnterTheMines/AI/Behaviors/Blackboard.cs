using Godot;

namespace EnterTheMines.EnterTheMines.AI.Behaviors;

public partial class Blackboard : Resource
{
    [Export] public Godot.Collections.Dictionary<string, Variant> Contents { get; set; } = new Godot.Collections.Dictionary<string, Variant>();

    public void SetValue(string key, Variant value)
    {
        if (Contents.ContainsKey(key))
        {
            Contents[key] = value;
        }
        else
        {
            Contents.Add(key, value);
        }
    }

    public Variant GetValue(string key)
    {
        if (Contents.ContainsKey(key))
        {
            return Contents[key];
        }
        else
        {
            return new Variant();
        }
    }

    public void RemoveValue(string key)
    {
        if (Contents.ContainsKey(key))
        {
            Contents.Remove(key);
        }
    }

    public void Clear()
    {
        Contents.Clear();
    }
}
