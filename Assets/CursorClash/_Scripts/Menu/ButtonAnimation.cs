using System.Collections;
using UnityEngine;
namespace MichiTheDev
{
    public class ButtonAnimation : MonoBehaviour
    {
        [SerializeField] private AudioClipInfo _hoveredClipInfo;
        [SerializeField] private AudioClipInfo _clickClipInfo;
        
        private Animator _anim;
        private Coroutine _resizeRoutine;
        private AudioSourceObject _audioSourceObject;

        private void Awake()
        {
            _anim = GetComponent<Animator>();

            _audioSourceObject = new GameObject("Button [Audio]").AddComponent<AudioSourceObject>();
        }

        public void PointerEnter()
        {
            _anim.SetBool("Mirror", Random.Range(0, 100) > 50);
            _anim.SetBool("Hovered", true);
            _anim.SetTrigger("Hover");
            _audioSourceObject.PlayOneShot(_hoveredClipInfo, true, new Vector2(0.75f, 1.25f));
            StartResize(new Vector2(1.1f, 1.1f), 0.25f);
        }

        public void PointerExit()
        {
            _anim.SetBool("Hovered", false);
            StartResize(new Vector2(1f, 1f), 0.25f);
        }

        public void PointerClick()
        {
            _audioSourceObject.PlayOneShot(_clickClipInfo);
        }

        public void StartResize(Vector2 targetSize, float transitionTime)
        {
            if(_resizeRoutine != null)
            {
                StopCoroutine(_resizeRoutine);
            }
            _resizeRoutine = StartCoroutine(Resize(targetSize, transitionTime));
        }
        
        private IEnumerator Resize(Vector2 targetSize, float transitionTime)
        {
            float fadeTimer = 0;
            Vector2 startSize = transform.localScale;
            while (fadeTimer < transitionTime)
            {
                transform.localScale = new Vector3(
                    Mathf.Lerp(startSize.x, targetSize.x, fadeTimer / transitionTime),
                    Mathf.Lerp(startSize.y, targetSize.y, fadeTimer / transitionTime)
                );
                fadeTimer += Time.deltaTime;
                yield return null;
            }
            _resizeRoutine = null;
        }
    }
}
