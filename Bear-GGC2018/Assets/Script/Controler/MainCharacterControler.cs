using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterControler : Singleton<MainCharacterControler>
{
    [Header ("Movements paramters")]
    public float speed = 10.0f;

    [Header ("private variable, don't touch it")]
    [SerializeField] private float _translation = 0f;
    [SerializeField] private float _strafe = 0f;
    [SerializeField] private bool _isLookingAtClue = false;
    [SerializeField] private bool _isTalking = false;
    [SerializeField] private ClueDisplayer _clueDisplayer = null;

    #region properties

    public bool isCharacterLocked { get { return (isLookingAtClue) || (isTalking); }}

    public bool isLookingAtClue
    {
        get { return _isLookingAtClue; }
        private set { _isLookingAtClue = value; }
    }
    
    public bool isTalking
    {
        get { return _isTalking; }
        private set { _isTalking = value; }
    }

    public ClueDisplayer currentClueDisplayer
    {
        get
        {
            if (_clueDisplayer == null)
            {
                _clueDisplayer = ClueDisplayer.Instance;
            }
            return _clueDisplayer;
        }
    }
    #endregion

    // Use this for initialization
    void Start ()
    {
        // Lock the mouse on the character
        Cursor.lockState = CursorLockMode.Locked;
	}
	
	// Update is called once per frame
	void Update ()
    {
        ManageInput();

    }

    void ManageInput ()
    {
        if(Input.GetMouseButtonDown(1))
        {
            ManagelookingAtClue();
        }

        ManageMovement();
    }

    void ManageMovement ()
    {
        if(isCharacterLocked)
        {
            return;
        }

        _translation = Input.GetAxis("Vertical") * speed;
        _strafe = Input.GetAxis("Horizontal") * speed;

        _translation *= Time.deltaTime;
        _strafe *= Time.deltaTime;

        transform.Translate(_strafe, 0f, _translation);
    }

    void ManagelookingAtClue()
    {
        if(isLookingAtClue)
        {
            currentClueDisplayer.HideClue();
        }
        else
        {
            currentClueDisplayer.DisplayClue();
        }

        isLookingAtClue = !isLookingAtClue;
    }
}
