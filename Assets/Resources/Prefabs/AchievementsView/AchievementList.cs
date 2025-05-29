using System;
using System.Collections.Generic;

[Serializable]
public class AchievementList
{
    public List<Achievement> achievements;
    
    public List<bool> collected;
    public List<bool> done;
}
