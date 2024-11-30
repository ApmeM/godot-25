using Godot;
using Godot25.Presentation.Utils;
using System;
using System.Collections.Generic;

[SceneReference("GameBase.tscn")]
public partial class GameBase
{
    private int nextToClick;
    private DateTime? startedDate;

    [Signal]
    public delegate void ExitClick();

    [Signal]
    public delegate void LevelPassed();

    public readonly List<int> Data = new List<int>();

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        InitButtons();
        foreach (GameButton b in this.gameField.GetChildren())
        {
            b.Connect(nameof(GameButton.Clicked), this, nameof(GameButtonPressed));
        }

        this.restartGame.Visible = false;
        this.exitGame.Connect(CommonSignals.Pressed, this, nameof(ExitGameClicked));
        this.startGame.Connect(CommonSignals.Pressed, this, nameof(GameStartButtonPressed));
        this.restartGame.Connect(CommonSignals.Pressed, this, nameof(GameRestartButtonPressed));
    }

    private void ExitGameClicked()
    {
        this.EmitSignal(nameof(ExitClick));
    }

    private void InitButtons()
    {
        this.Data.Clear();
        InitData();
        Shuffle(Data);

        var i = 0;
        foreach (GameButton b in this.gameField.GetChildren())
        {
            b.Value = Data[i % Data.Count];
            b.Disabled = false;
            i++;
        }

        this.Data.Sort();
    }

    protected virtual void InitData()
    {
    }

    private void GameStartButtonPressed()
    {
        this.restartGame.Visible = true;
        this.startGame.Text = "Restart";
        GameRestartButtonPressed();
    }

    private void GameRestartButtonPressed()
    {
        this.hoverContainer.Visible = false;

        InitButtons();
        nextToClick = 0;
        startedDate = DateTime.Now;
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        if (startedDate.HasValue)
        {
            this.time.Text = (DateTime.Now - startedDate.Value).ToString(@"hh\:mm\:ss");
        }
    }

    private void GameButtonPressed(GameButton button)
    {
        if (!startedDate.HasValue)
        {
            return;
        }

        if (this.Data[this.nextToClick] != button.Value)
        {
            button.Shake();
            return;
        }

        button.Disabled = true;
        this.nextToClick++;

        if (nextToClick == 25)
        {
            var finalScore = DateTime.Now - startedDate.Value;
            startedDate = null;
            this.finalTime.Text = "Your time:\n" + finalScore.ToString(@"hh\:mm\:ss"); ;

            var bestScore = di.repository.LoadGame(this.GetType().Name);
            if (bestScore > finalScore)
            {
                di.repository.SaveGame(this.GetType().Name, finalScore);
                this.finalTime.Text += "\nNEW BEST!!!";
            }
            else
            {
                this.finalTime.Text += "\nBest time:\n" + bestScore.ToString(@"hh\:mm\:ss"); ;
            }

            this.time.Text = "";
            this.hoverContainer.Visible = true;
            this.EmitSignal(nameof(LevelPassed));
        }
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
