using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoDisplayer : MonoBehaviour
{
    private bool isDisplayed = false;

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

    public void DisplayTuto()
    {
        if(isDisplayed)
        {
            return;
        }
        animator.SetTrigger("DisplayTuto");
        isDisplayed = true;
    }

    public void HideTuto()
    {
        if(!isDisplayed)
        {
            return;
        }
        animator.SetTrigger("HideTuto");
        isDisplayed = false;
    }
}
