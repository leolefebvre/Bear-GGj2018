using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClueDisplayer : Singleton<ClueDisplayer>
{
    public Image clueImage;
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

	public void DisplayClue()
    {
        animator.SetTrigger("DisplayClue");
    }

    public void HideClue()
    {
        animator.SetTrigger("HideClue");
    }

    public void ChangeClueImage(Sprite newClueSprite)
    {
        clueImage.sprite = newClueSprite;
    }
}
