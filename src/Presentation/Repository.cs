using System;
using System.Collections.Generic;
using Godot;

[Injectable(false)]
public class Repository
{
    public void SaveGame(string gameName, TimeSpan time)
    {
        var f = new File();
        f.Open($"Game{gameName}.save", File.ModeFlags.Write);
        f.Store64((ulong)time.Ticks);
        f.Close();
    }

    public TimeSpan LoadGame(string gameName)
    {
        var f = new File();
        if (!f.FileExists($"Game{gameName}.save"))
        {
            return TimeSpan.MaxValue;
        }

        f.Open($"Game{gameName}.save", File.ModeFlags.Read);
        var ticks = (long)f.Get64();
        return new TimeSpan(ticks);
    }
}