using UnityEngine;

public class TutorialStep : MonoBehaviour
{
    public string Sentence { get; private set; }
    public TutorialAction ActionRequired { get; private set; }
    public string AudioClipPath { get; private set; }

    public TutorialStep(string sentence, string audioClipPath, TutorialAction actionRequired = TutorialAction.None)
    {
        Sentence = sentence;
        ActionRequired = actionRequired;
        AudioClipPath = audioClipPath;
    }
}
