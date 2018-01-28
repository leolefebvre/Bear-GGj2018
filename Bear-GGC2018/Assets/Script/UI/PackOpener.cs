using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackOpener : Singleton<PackOpener>
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

    public void OpenPack()
    {
        Debug.Log("Oppening pack");
        animator.SetTrigger("OpenPack");
    }
}
