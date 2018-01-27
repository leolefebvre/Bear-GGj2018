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
}
