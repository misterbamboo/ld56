using Godot;

public partial class MonsterSpawnDirector : Node
{
	[Export] public int Difficulty = 1;
	[Export] public Godot.Collections.Array<PackedScene> MonsterScenes;
	[Export] public PackedScene SkitterScene;


	public double timer = 30;

    public override void _Ready()
	{
		if(!Multiplayer.IsServer())
		{
			SetProcess(false);
		}
	}

	public override void _Process(double delta)
	{
		timer -= delta;
		if(timer <= 0)
        {
            timer = 30;
            SpawnMonster();
        }
    }

	public void SpawnMonster()
	{
		var obj = SkitterScene.Instantiate();
		AddChild(obj);
    }
}
