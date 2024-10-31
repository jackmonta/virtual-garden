using UnityEngine;

public class WaitForAction : MonoBehaviour
{
    private GameObject obj;
    [SerializeField] private TutorialAction action;

    void Start()
    {
        obj = this.gameObject;
        obj.SetActive(false);
        TutorialUI.onNextAction.AddListener(OnNextAction);    
    }

    private void OnNextAction(TutorialAction tutorialAction)
    {
        if (tutorialAction == action)
        {
            obj.SetActive(true);
            TutorialUI.onNextAction.RemoveListener(OnNextAction);
            Destroy(obj);
        }
    }
}
