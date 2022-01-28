using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTimer : MonoBehaviour
{
    private const float TickTimeMinDuration = 5;
    
    public Image avatar;
    public Image timer;

    [SerializeField]
    private Image medal;

    private Animator animator;
    
    static string[] medalSpriteNames = {"medal_1","medal_2","medal_3"};
    static int nextMedalSprite = 0;

    Coroutine timerCoroutine;

    private bool isTickPlaying;
    
    private void Awake()
    {
        animator = avatar.GetComponent<Animator>();
        avatar.rectTransform.localScale = Vector3.one * 0.7f;
        timer.fillAmount = 1.0f;
        isTickPlaying = false;
        medal.enabled = false;
    }

    public void startTimer(float duration) 
    {
        animator.enabled = true;
        avatar.rectTransform.localScale = Vector3.one;
        timerCoroutine = StartCoroutine(fillTimer(duration));
        SoundManager.Instance.playStartTimerSound();
    }

    public void endTimer() {
        if(timerCoroutine != null) {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }
        animator.SetTrigger("TurnEnd");
        animator.enabled = false;
        avatar.rectTransform.localScale = Vector3.one * 0.7f;
        timer.fillAmount = 1.0f;
        isTickPlaying = false;

        SoundManager.Instance.StopTickingSound();
        SoundManager.Instance.playEndTimerSound();
    }

    IEnumerator fillTimer(float duration) {
        float normalizedTime = 0;
        float timePassed = 0;
        
        while(normalizedTime <= 1f)
        {
            timePassed += Time.deltaTime;
            
            if (duration - timePassed < TickTimeMinDuration && !isTickPlaying)
            {
                SoundManager.Instance.playTickingSound();
                animator.SetTrigger("OutOfTime");
                isTickPlaying = true;
            }

            timer.fillAmount = 1.0f - normalizedTime;
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }
        endTimer();
    }

    public void OnEndedTrack() {
        if(!medal.enabled) {
            medal.enabled = true;
            medal.overrideSprite = Resources.Load<Sprite>(medalSpriteNames[nextMedalSprite++]);

            nextMedalSprite = nextMedalSprite % medalSpriteNames.Length;
        }
    }
}
