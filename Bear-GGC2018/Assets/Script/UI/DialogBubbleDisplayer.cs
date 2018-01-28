using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogBubbleDisplayer : Singleton<DialogBubbleDisplayer>
{
    public Image bubbleImage;
    public Image itemImage;

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
        UpdateBubbleColor(QuestManager.Instance.CurrentQuestColor);
        animator.SetTrigger("GoodAnswer");
    }

    public void LaunchBadDialog()
    {
        UpdateBubbleColor(QuestManager.Instance.DefaultColor);
        animator.SetTrigger("BadAnswer");
    }

    public void LaunchOpenPackAnim()
    {
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

    public void UpdateBubbleColor(Color newColor)
    {

        //bubbleImage.color = new Color(newColor.r, newColor.g, newColor.b, bubbleImage.color.a);
    }

    public void UpdateQuestItemSpire()
    {
        itemImage.sprite = QuestManager.Instance.currentQuest.questObject;
    }

    public void AllowDialog()
    {
        MainCharacterControler.Instance.canDialog = true;
    }
}
