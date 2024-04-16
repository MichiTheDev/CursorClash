using UnityEngine;

namespace MichiTheDev
{
   public class GameManager : MonoBehaviour
   {
      public static GameManager Instance { private set; get; }

      [SerializeField] private AudioClipInfo _mainTheme;
      
      private AudioSourceObject _audioSourceObject;
      
      private void Awake()
      {
         if(Instance != null)
         {
            Destroy(gameObject);
            return;
         }
         Instance = this;
         DontDestroyOnLoad(gameObject);

         _audioSourceObject = new GameObject("Music [Audio]").AddComponent<AudioSourceObject>();
         _audioSourceObject.Play(_mainTheme);
      }
   }
}