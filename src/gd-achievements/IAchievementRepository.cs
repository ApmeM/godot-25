using System.Collections.Generic;
using System.Threading.Tasks;

namespace Godot25.Achievements
{
    public interface IAchievementRepository
    {
        bool ProgressAchievement(string key, int progress);
        bool UnlockAchievement(string key);
        Task<Achievement> GetAchievement(string key);
        Task<IEnumerable<Achievement>> GetForList();
    }
}