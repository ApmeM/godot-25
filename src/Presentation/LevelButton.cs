using Godot;

[SceneReference("LevelButton.tscn")]
public partial class LevelButton
{
    [Export]
    public PackedScene GameToStart;

    public override void _Ready()
    {
        base._Ready();
        this.AddToGroup(Groups.LevelButton);
    }
}

public partial class Groups
{
    public static string LevelButton = nameof(LevelButton);
}
