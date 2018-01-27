using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClueDisplayer : Singleton<ClueDisplayer>
{
    private Image _displayedImage = null;

    public Image displayedImage
    {
        get
        {
            if(_displayedImage == null)
            {
                _displayedImage = GetComponent<Image>();
            }
            return _displayedImage;
        }
    }

    void Start()
    {
        HideClue();
    }

	public void DisplayClue()
    {
        Debug.Log("Displaying clue");
        displayedImage.enabled = true;
    }

    public void HideClue()
    {
        Debug.Log("Hidding clue");
        displayedImage.enabled = false;
    }

    public void ChangeClueImage(Sprite newClueSprite)
    {
        displayedImage.sprite = newClueSprite;
    }
}
