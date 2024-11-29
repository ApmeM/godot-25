using Godot;

[SceneReference("LevelButton.tscn")]
[Tool]
public partial class LevelButton
{
    [Export]
    public PackedScene GameToStart;

    [Export]
    public NodePath NextLevelButton;

    private string title;

    [Export(PropertyHint.MultilineText)]
    public string Title
    {
        get => title;
        set
        {
            if (IsInsideTree())
            {
                this.titleLabel.Text = value;
            }
            title = value;
        }
    }

    public override void _Ready()
    {
        base._Ready();
        this.AddToGroup(Groups.LevelButton);
        this.Title = this.title;
    }

    public LevelButton GetNextLevel()
    {
        if (this.NextLevelButton.IsEmpty())
        {
            return null;
        }

        return this.GetNode<LevelButton>(this.NextLevelButton);
    }
}
