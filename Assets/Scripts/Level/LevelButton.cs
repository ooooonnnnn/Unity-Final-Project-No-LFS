using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;


public class LevelButton : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private int levelIndex;
        [SerializeField] private Button button;
        [SerializeField] private Image lockIcon;
        
        [SerializeField] private float hoverScale = 1.1f;
        [SerializeField] private float tweenDuration = 0.2f;

        private Vector3 originalScale;
        private Tween scaleTween;
        
        private void Awake()
        {
            originalScale = transform.localScale;
        }

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
            SaveSystem.Load();
            GameManager.Instance.LoadLevel(levelIndex);
            
        }
        
        public void OnPlayClicked()
        {
            SaveSystem.Load();
            SceneManager.LoadScene("LevelSelect");
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!button.interactable) return;

            scaleTween?.Kill();

            scaleTween = transform.DOScale(originalScale * hoverScale, tweenDuration)
                .SetEase(Ease.OutBack);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!button.interactable) return;

            scaleTween?.Kill();

            scaleTween = transform.DOScale(originalScale, tweenDuration)
                .SetEase(Ease.OutQuad);
        }
    }
