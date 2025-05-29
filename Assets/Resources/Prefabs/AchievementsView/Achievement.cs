using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Achievement", menuName = "Achievement")]
public class Achievement : ScriptableObject
{
    public string title;
    public Sprite icon;
    public int coinsReward;
    
    private bool collected = false;
    public bool Collected
    {
        get { return collected; }
        set { collected = value; }
    }
    
    private bool done = false;
    public bool Done
    {
        get { return done; }
        set
        {
            done = value;
            if (done)
            {
                OnAchievementUnlocked?.Invoke(this);
            }
        }
    }

    public event Action<Achievement> OnAchievementUnlocked;
}
