using UnityEngine;

public class ButtonAudioTrigger : MonoBehaviour
{
    public void PlayGlobalAudio()
    {
        GlobalAudioPlayer.Instance?.PlayAudio();
    }
}