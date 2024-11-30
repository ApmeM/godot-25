using Godot;

[SceneReference("Game25.tscn")]
public partial class Game25
{
    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();
    }

    protected override void InitData()
    {
        base.InitData();
        for (var j = 0; j < 25; j++)
        {
            var newValue = j + 1;
            this.Data.Add(new DataContent
            {
                Text = newValue.ToString(),
                Value = newValue
            });
        }
    }
}
