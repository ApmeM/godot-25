using System;
using System.Collections.Generic;
using Godot;

[SceneReference("Game99.tscn")]
public partial class Game99
{
    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();
    }

    private Random r = new Random();

    protected override void InitData()
    {
        base.InitData();
        var set = new HashSet<int>();
        for (var j = 0; j < 25; j++)
        {
            var newValue = r.Next(99) + 1;
            while (set.Contains(newValue))
            {
                newValue = r.Next(99) + 1;
            }
            set.Add(newValue);
            this.Data.Add(newValue);
        }
    }
}
