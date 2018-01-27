using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterControler : Singleton<MainCharacterControler>
{
    [Header("Movements paramters")]
    public float speed = 10.0f;

    [Header("private variable, don't touch it")]
    [SerializeField] private float _translation = 0f;
    [SerializeField] private float _strafe = 0f;
    [SerializeField] private bool _isLookingAtClue = false;
    [SerializeField] private bool _isTalking = false;
    [SerializeField] private ClueDisplayer _clueDisplayer = null;
    [SerializeField] private NonPlayerCharacter _currentNpcNear = null;

    private QuestManager _questManager = null;

    #region properties

    public bool isCharacterLocked { get { return (isLookingAtClue) || (isTalking); } }

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

    public bool canInteractWithNPC
    {
        get { return _currentNpcNear != null; }
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
    public QuestManager questManager
    {
        get
        {
            if(_questManager == null)
            {
                _questManager = QuestManager.Instance;
            }
            return _questManager;
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

        if(Input.GetMouseButtonDown(0))
        {
            ManageInteraction();
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

    void ManageInteraction()
    {
        if(isCharacterLocked)
        {
            return;
        }

        if(canInteractWithNPC)
        {
            LaunchDialog();
        }
    }

    void LaunchDialog()
    {
        if(questManager.IsItTheDestinator(_currentNpcNear))
        {
            Debug.Log("Is the destinator");
            // lauchn finish quest Dialog
            questManager.FinishCurrentQuest();
        }
        else
        {
            Debug.Log("Not the destinator");
            //Launch nope dialog
        }
    }


    void OnTriggerEnter(Collider other)
    {
        Debug.Log("I collide with " + other.name);
        if(other.tag == "NPC")
        {
            _currentNpcNear = other.GetComponent<NonPlayerCharacter>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "NPC")
        {
            if(_currentNpcNear == other.GetComponent<NonPlayerCharacter>())
            {
                _currentNpcNear = null;
            }
        }
    }
}
