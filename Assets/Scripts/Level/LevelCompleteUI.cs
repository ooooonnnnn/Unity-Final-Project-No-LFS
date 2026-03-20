using UnityEngine;

public class LevelCompleteUI : MonoBehaviour
{
    public void ReturnToLevelSelect()
    {
        LevelManager.Instance.ReturnToLevelSelect();
    }
}