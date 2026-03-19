using ScriptsMilana;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private int levelIndex;
    [SerializeField] private Button button;
    [SerializeField] private Image lockIcon;

    private void Start()
    {
        int unlocked = SaveSystem.CurrentSave.highestUnlockedLevel;

        bool isUnlocked = levelIndex <= unlocked;

        button.interactable = isUnlocked;

        if (lockIcon)
        {
            lockIcon.gameObject.SetActive(!isUnlocked);
           
        }
        button.image.color = isUnlocked ? Color.white : Color.gray;
        
          
    }

    public void LoadLevel()
    {
        LevelManager.Instance.LoadLevel(levelIndex);
    }
}