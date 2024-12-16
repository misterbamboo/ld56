using Godot;

namespace EnterTheMines.EnterTheMines.Interactables.Items;

[GlobalClass]
public partial class ItemProperties : Resource
{
	[Export] public string ItemName = "Item";
	[Export] public int ItemPrice = 45;
}
