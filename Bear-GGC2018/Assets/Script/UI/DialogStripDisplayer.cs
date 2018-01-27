﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogStripDisplayer : Singleton<DialogStripDisplayer>
{
    private Animator _animator = null;
    public Animator animator
    {
        get
        {
            if(_animator == null)
            {
                _animator = GetComponent<Animator>();
            }
            return _animator;
        }
    }

    public void OpenDialogStrip()
    {
        animator.SetTrigger("Open");
    }

    public void CloseDialogStrip()
    {
        animator.SetTrigger("Close");
    }
}
