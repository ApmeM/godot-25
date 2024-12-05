using System;
using System.Collections.Generic;
using Godot;
using Newtonsoft.Json;

[Injectable(false)]
public class Repository
{
    public void SaveGame(string gameName, TimeSpan time)
    {
        var f = new File();
        f.Open($"user://Game{gameName}.save", File.ModeFlags.Write);
        f.Store64((ulong)time.Ticks);
        f.Close();
    }

    public TimeSpan LoadGame(string gameName)
    {
        var f = new File();
        if (!f.FileExists($"user://Game{gameName}.save"))
        {
            return TimeSpan.MaxValue;
        }

        f.Open($"user://Game{gameName}.save", File.ModeFlags.Read);
        var ticks = (long)f.Get64();
        f.Close();
        return new TimeSpan(ticks);
    }

    public void SaveProgress(string gameName)
    {
        var current = LoadProgress();
        current.Add(gameName);
        var f = new File();
        f.Open($"user://GameProgress.save", File.ModeFlags.Write);
        f.StorePascalString(JsonConvert.SerializeObject(current));
        f.Close();
    }

    public HashSet<string> LoadProgress()
    {
        var f = new File();
        if (!f.FileExists($"user://GameProgress.save"))
        {
            return new HashSet<string>();
        }

        f.Open($"user://GameProgress.save", File.ModeFlags.Read);
        var data = JsonConvert.DeserializeObject<HashSet<string>>(f.GetPascalString());
        f.Close();
        return data;
    }
}