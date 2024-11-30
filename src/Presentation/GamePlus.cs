using System;
using System.Collections.Generic;
using Godot;

[SceneReference("GamePlus.tscn")]
public partial class GamePlus
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

            var arg1 = r.Next(newValue - 1) + 1;
            var arg2 = newValue - arg1;

            this.Data.Add(new DataContent
            {
                ButtonValue = newValue,
                HelpText = $"{arg1}+{arg2}=?",
                Order = j
            });
        }
    }
}
