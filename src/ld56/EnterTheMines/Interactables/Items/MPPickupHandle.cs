using EnterTheMines.EnterTheMines.Interactables.Items;
using Godot;

[GlobalClass]
[Icon("res://EnterTheMines/Interactable/Items/MPPickupHandle.png")]
public partial class MPPickupHandle : Area3D
{
	private BaseItem parent;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (Engine.IsEditorHint())
		{
			SetProcess(false);
			return;
		}

        parent = GetParent<BaseItem>();
    }

	public BaseItem GetItem()
    {
        return parent;
    }

    public ItemProperties GetItemProperties()
    {
        return parent.ItemProperties;
    }
}
