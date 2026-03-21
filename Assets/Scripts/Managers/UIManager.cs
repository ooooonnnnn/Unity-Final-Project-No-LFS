
using TMPro;
using UnityEngine;

    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text logObject;
        [SerializeField] private GameObject levelCompleteUI;
        [SerializeField] private GameObject levelLostUI;
        [SerializeField] private GameObject MainMenu;
        public static UIManager Instance { get; private set; }

        void Awake()
        {
            if (Instance && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
         
        }

        public void LevelWon()
        {
            //TO TO : show level complete UI with button to level selector 
            levelCompleteUI.SetActive(true);
            

            
        }

    }
