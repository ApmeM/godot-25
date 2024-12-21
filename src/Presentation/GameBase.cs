using Godot;
using Godot25.Presentation.Utils;
using System;
using System.Collections.Generic;

[SceneReference("GameBase.tscn")]
public partial class GameBase
{
    private int nextToClick = 0;
    private int NextToClick
    {
        get => this.nextToClick;
        set
        {
            this.nextToClick = value;
            if (IsInsideTree() && this.Data.Count > this.NextToClick && !string.IsNullOrWhiteSpace(this.Data[this.NextToClick].HelpText))
            {
                this.labelNext.Text = $"Find: {this.Data[this.NextToClick].HelpText}";
            }
        }
    }
    private DateTime? startedDate;

    [Signal]
    public delegate void ExitClick(bool openNextLevel);

    [Signal]
    public delegate void LevelPassed(float bestScore);

    [Export]
    public bool NextLevelVisible { get; set; }

    protected struct DataContent
    {
        public string HelpText;
        public int ButtonValue;
        public int Order;
    }

    protected readonly List<DataContent> Data = new List<DataContent>();

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        InitButtons();
        this.NextToClick = 0;
        foreach (GameButton b in this.gameField.GetChildren())
        {
            b.Connect(nameof(GameButton.Clicked), this, nameof(GameButtonPressed));
        }

        this.restartGame.Visible = false;
        this.exitGame.Connect(CommonSignals.Pressed, this, nameof(ExitGameClicked));
        this.startGame.Connect(CommonSignals.Pressed, this, nameof(GameStartButtonPressed));
        this.restartGame.Connect(CommonSignals.Pressed, this, nameof(GameRestartButtonPressed));
        this.nextLevel.Connect(CommonSignals.Pressed, this, nameof(NextlevelButtonPressed));
    }

    private void NextlevelButtonPressed()
    {
        this.EmitSignal(nameof(ExitClick), true);
    }

    private void ExitGameClicked()
    {
        this.EmitSignal(nameof(ExitClick), false);
    }

    private void InitButtons()
    {
        this.Data.Clear();
        InitData();
        Shuffle(Data);

        var i = 0;
        foreach (GameButton b in this.gameField.GetChildren())
        {
            b.Value = Data[i % Data.Count].ButtonValue;
            b.Disabled = false;
            i++;
        }

        this.Data.Sort((a, b) => a.Order - b.Order);
    }

    protected virtual void InitData()
    {
    }

    private void GameStartButtonPressed()
    {
        this.restartGame.Visible = true;
        this.startGame.Text = "Restart";
        this.nextLevel.Visible = this.NextLevelVisible;
        GameRestartButtonPressed();
    }

    private void GameRestartButtonPressed()
    {
        this.hoverContainer.Visible = false;

        InitButtons();
        NextToClick = 0;
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

        if (this.Data[this.NextToClick].ButtonValue != button.Value)
        {
            button.Shake();
            return;
        }

        button.Disabled = true;
        this.NextToClick++;

        if (NextToClick >= 25)
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
                this.finalTime.Text += "\nBest time:\n" + bestScore.ToString(@"hh\:mm\:ss");
            }

            this.time.Text = "";
            this.hoverContainer.Visible = true;
            this.EmitSignal(nameof(LevelPassed), (float)(long)bestScore.TotalMilliseconds);
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
