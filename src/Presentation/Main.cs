using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot25.Presentation.Utils;

[SceneReference("Main.tscn")]
public partial class Main
{
    public TextureButton[] buttons = new TextureButton[25];

    private int nextToClick = 0;
    private DateTime? startedDate;

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        buttons[0] = this.textureButton1;
        buttons[1] = this.textureButton2;
        buttons[2] = this.textureButton3;
        buttons[3] = this.textureButton4;
        buttons[4] = this.textureButton5;
        buttons[5] = this.textureButton6;
        buttons[6] = this.textureButton7;
        buttons[7] = this.textureButton8;
        buttons[8] = this.textureButton9;
        buttons[9] = this.textureButton10;
        buttons[10] = this.textureButton11;
        buttons[11] = this.textureButton12;
        buttons[12] = this.textureButton13;
        buttons[13] = this.textureButton14;
        buttons[14] = this.textureButton15;
        buttons[15] = this.textureButton16;
        buttons[16] = this.textureButton17;
        buttons[17] = this.textureButton18;
        buttons[18] = this.textureButton19;
        buttons[19] = this.textureButton20;
        buttons[20] = this.textureButton21;
        buttons[21] = this.textureButton22;
        buttons[22] = this.textureButton23;
        buttons[23] = this.textureButton24;
        buttons[24] = this.textureButton25;

        // For debug purposes all achievements can be reset
        // this.di.localAchievementRepository.ResetAchievements();

        this.achievementsButton.Connect(CommonSignals.Pressed, this, nameof(AchievementsButtonPressed));
        this.startGame.Connect(CommonSignals.Pressed, this, nameof(LevelSelected));

        for (var i = 0; i < buttons.Length; i++)
        {
            var b = buttons[i];
            b.Connect(CommonSignals.Pressed, this, nameof(ButtonPressed), new Godot.Collections.Array { i });
        }
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        if (startedDate.HasValue)
        {
            this.time.Text = (DateTime.Now - startedDate.Value).ToString();
        }
    }

    private void ButtonPressed(int idx)
    {
        if (!startedDate.HasValue)
        {
            return;
        }

        if (this.nextToClick == idx)
        {
            buttons[idx].Disabled = true;
            this.nextToClick++;
        }

        if (idx == 24)
        {
            startedDate = null;
        }
    }

    private void LevelSelected()
    {
        var childs = this.gameField.GetChildren().Cast<Node>().ToList();
        Shuffle(childs);
        foreach (var c in childs)
        {
            this.gameField.RemoveChild(c);
        }
        foreach (var c in childs)
        {
            this.gameField.AddChild(c);
        }

        foreach (var b in buttons)
        {
            b.Disabled = false;
        }
        nextToClick = 0;
        startedDate = DateTime.Now;
    }

    private void AchievementsButtonPressed()
    {
        // See achievements definitions in gd-achievements/achievements.json
        this.achievementList.ReloadList();
        this.customPopup.Show();
    }

    private readonly Random r = new Random();
    private void Shuffle<T>(IList<T> list)
    {
        var n = list.Count;
        while (n > 1)
        {
            n--;
            var k = r.Next(n + 1);
            (list[n], list[k]) = (list[k], list[n]);
        }
    }
}
