using EnterTheMines.EnterTheMines.Interactables.Items;
using Godot;
using System.Collections.Generic;

namespace EnterTheMines.EnterTheMines.Interactables.ShredderCore;

public partial class Shredder : Node3D
{
    private AudioShredItem audioShredItem;
    private Node3D grindStart;
    private Node3D grindEnd;

    [Export] private float shakeForce = 0.2f;
    [Export] private float grindSpeed = 0.25f;

    private List<BaseItem> grindingItems = [];
    private List<float> grindingTimes = [];

    public override void _Ready()
    {
        audioShredItem = GetNode<AudioShredItem>("Audio_Shred");
        grindStart = GetNode<Node3D>("grind_start");
        grindEnd = GetNode<Node3D>("grind_end");
    }

    public override void _Process(double delta)
    {
        if (grindingItems.Count > 0)
        {
            _GrindItems(delta);
        }
    }

    public void OnBodyEntered(Node3D body)
    {
        if (grindingItems.Count != 0) return;

        if (body != null && body.GetParent() is BaseItem)
        {
            BaseItem base_item = (BaseItem)body.GetParent();
            base_item.Grind();
            audioShredItem.GrindPlay();
            grindingItems.Add(base_item);
            grindingTimes.Add(0);
        }
    }

    private void _GrindItems(double delta)
    {
        bool any_removed = false;
        for (int i = 0; i < grindingItems.Count; i++)
        {
            any_removed = any_removed || _GrindItem(i, delta, any_removed);
        }
    }

    private bool _GrindItem(int index, double delta, bool any_removed)
    {
        grindingTimes[index] += (float)delta * grindSpeed;
        BaseItem grinding_item = grindingItems[index];
        float t = grindingTimes[index];

        grinding_item.Rb.GlobalPosition = grindStart.GlobalPosition.Lerp(grindEnd.GlobalPosition, t);

        var xrot = _Shake(t);
        var zrot = _Shake(t + 0.25f);
        var targetRot = new Vector3(xrot, 0, zrot);
        grinding_item.Rb.Rotation = targetRot;

        // allow 1 removal by frame
        if (!any_removed && t >= 1)
        {
            grindingItems.RemoveAt(index);
            grindingTimes.RemoveAt(index);
            grinding_item.Destroy();
            return true;
        }

        return false;
    }

    private float _Shake(float t)
    {
        return Mathf.Sin(100 * t) * Mathf.Cos(150 * t) * shakeForce;
    }
}
