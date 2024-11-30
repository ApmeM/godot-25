using System;
using System.Collections.Generic;
using Godot;

[SceneReference("GameMult.tscn")]
public partial class GameMult
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
            var arg1 = r.Next(9) + 1;
            var arg2 = r.Next(9) + 1;
            var newValue = arg1 * arg2;
            while (set.Contains(newValue))
            {
                arg1 = r.Next(9) + 1;
                arg2 = r.Next(9) + 1;
                newValue = arg1 * arg2;
            }
            set.Add(newValue);

            this.Data.Add(new DataContent
            {
                ButtonValue = newValue,
                HelpText = $"{arg1}*{arg2}=?",
                Order = j
            });
        }
    }
}
