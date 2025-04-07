using StellarFactor;
using UnityEngine;

public class CloseWindowButton : MonoBehaviour
{
    public void OnClick()
    {
        QuestionManager.MGR.CancelQuestion();
    }
}
