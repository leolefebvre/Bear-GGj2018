using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogBubbleDisplayer : Singleton<DialogBubbleDisplayer>
{
    private Animator _animator = null;
    public Animator animator
    {
        get
        {
            if (_animator == null)
            {
                _animator = GetComponent<Animator>();
            }
            return _animator;
        }
    }

    public void LaunchGoodDialog()
    {
        animator.SetTrigger("GoodAnswer");
    }

    public void LaunchBadDialog()
    {
        animator.SetTrigger("BadAnswer");
    }

    public void LaunchOpenPackAnim()
    {
        Debug.Log("Prout");

        PackOpener.Instance.OpenPack();
    }

    public void PlaySoundPositiveFeedback()
    {
        MainCharacterControler.Instance.PlayFeedbackSound(MainCharacterControler.Instance.positiveFeedbackSound);
    }

    public void PlaySoundNegativeFeedback()
    {
        MainCharacterControler.Instance.PlayFeedbackSound(MainCharacterControler.Instance.negativeFeedbackSound);
    }

    public void PlaySoundTearPaper()
    {
        MainCharacterControler.Instance.PlayFeedbackSound(MainCharacterControler.Instance.tearPaperSound);
    }
}
