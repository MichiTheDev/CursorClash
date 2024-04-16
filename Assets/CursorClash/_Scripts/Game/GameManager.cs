using UnityEngine;

namespace MichiTheDev
{
   public class GameManager : MonoBehaviour
   {
      public static GameManager Instance { private set; get; }

      [SerializeField] private AudioClipInfo _mainThemeInfo;
      
      private void Awake()
      {
         if(Instance != null)
         {
            Destroy(gameObject);
            return;
         }
         Instance = this;
         DontDestroyOnLoad(gameObject);
      }

      private void Start()
      {
         AudioManager.Play(_mainThemeInfo);
      }
   }
}