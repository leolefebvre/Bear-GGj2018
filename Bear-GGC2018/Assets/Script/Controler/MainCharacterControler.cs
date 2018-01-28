﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterControler : Singleton<MainCharacterControler>
{
    [Header("Movements paramters")]
    public float speed = 10.0f;

    [Header("TutoDisplayParameters")]
    public TutoDisplayer moveTutoDisplayer;
    public TutoDisplayer clueTutoDisplayer;
    public float timeBeforeFirstTuto = 2.0f;
    public float TimeBetweenTutos = 5.0f;

    [Header("Sounds data")]
    public AudioClip positiveFeedbackSound;
    public AudioClip negativeFeedbackSound;


    [SerializeField] private bool hasAlreadyMove = false;
    [SerializeField] private bool hasAlreadyDisplayClue = false;

    [Header("private variable, don't touch it")]
    [SerializeField] private float _translation = 0f;
    [SerializeField] private float _strafe = 0f;
    [SerializeField] private bool _isLookingAtClue = false;
    [SerializeField] private bool _isTalking = false;

    [SerializeField] private NonPlayerCharacter _currentNpcNear = null;
    
    private RigidbodyConstraints _defaultRigidbodyConstraint;

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

    #endregion

    #region components & links

    private ClueDisplayer _clueDisplayer = null;
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

    private QuestManager _questManager = null;
    public QuestManager questManager
    {
        get
        {
            if (_questManager == null)
            {
                _questManager = QuestManager.Instance;
            }
            return _questManager;
        }
    }

    private Rigidbody _characterRigidody;
    public Rigidbody characterRigidbody
    {
        get
        {
            if (_characterRigidody == null)
            {
                _characterRigidody = GetComponent<Rigidbody>();
            }
            return _characterRigidody;
        }
    }

    private AudioSource _charcaterAudioSource;
    public AudioSource characterAudioSource
    {
        get
        {
            if(_charcaterAudioSource == null)
            {
                _charcaterAudioSource = GetComponent<AudioSource>();
            }
            return _charcaterAudioSource;
        }
    }

    #endregion

    // Use this for initialization
    void Start ()
    {
        // Lock the mouse on the character
        Cursor.lockState = CursorLockMode.Locked;

        StartCoroutine(DisplayTutoMove());
    }
	
	// Update is called once per frame
	void Update ()
    {
        ManageInput();

       // FixRotationBug();
    }

    private void LateUpdate()
    {
        //FixRotationBug();
        /*
        if (GetComponent<Rigidbody>().angularVelocity == Vector3.zero)
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            */
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

        if(Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            ManageMovement();
        }
    }

    void ManageMovement ()
    {
        if(isCharacterLocked)
        {
            return;
        }

        if(!hasAlreadyMove)
        {
            HideMoveTuto();
        }

        _translation = Input.GetAxis("Vertical") * speed;
        _strafe = Input.GetAxis("Horizontal") * speed;

        _translation *= Time.deltaTime;
        _strafe *= Time.deltaTime;

        transform.Translate(_strafe, 0f, _translation);
    }

    void ManagelookingAtClue()
    {
        if(!hasAlreadyMove)
        {
            return;
        }

        if(isLookingAtClue)
        {
            currentClueDisplayer.HideClue();
            UnlockCharacter();
        }
        else
        {
            if(!hasAlreadyDisplayClue)
            {
                clueTutoDisplayer.HideTuto();
                hasAlreadyDisplayClue = true;
            }
            else
            {
                questManager.nextObjectiveDisplayer.HideTuto();
            }

            currentClueDisplayer.DisplayClue();
            LockCharacter();
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
            PlayFeedbackSound(positiveFeedbackSound);
            DialogBubbleDisplayer.Instance.LaunchGoodDialog();
            // launch finish quest Dialog
            questManager.FinishCurrentQuest();
        }
        else
        {
            PlayFeedbackSound(negativeFeedbackSound);
            DialogBubbleDisplayer.Instance.LaunchBadDialog();
            //Launch nope dialog
        }
    }

    IEnumerator DisplayTutoMove()
    {
        yield return new WaitForSeconds(timeBeforeFirstTuto);
        if(!hasAlreadyMove)
        {
            moveTutoDisplayer.DisplayTuto();
        }
    }

    void HideMoveTuto()
    {
        moveTutoDisplayer.HideTuto();
        hasAlreadyMove = true;
        StartCoroutine(DisplayTutoClue());
    }

    IEnumerator DisplayTutoClue()
    {
        yield return new WaitForSeconds(TimeBetweenTutos);
        if (!hasAlreadyDisplayClue)
        {
            clueTutoDisplayer.DisplayTuto();
        }
    }

    void LockCharacter()
    {
        if(isCharacterLocked) // if character is already locked
        {
            return;
        }

        _defaultRigidbodyConstraint = characterRigidbody.constraints;

        characterRigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }

    void UnlockCharacter()
    {
        if (!isCharacterLocked) // if character is already unlocked
        {
            return;
        }

        characterRigidbody.constraints = _defaultRigidbodyConstraint;
    }

    void PlayFeedbackSound(AudioClip soundToPlay)
    {
        characterAudioSource.PlayOneShot(soundToPlay);
    }

    #region trigger methods

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("I collide with " + other.name);
        if(other.tag == "NPC")
        {
            _currentNpcNear = other.GetComponent<NonPlayerCharacter>();
            DialogStripDisplayer.Instance.OpenDialogStrip();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "NPC")
        {
            if(_currentNpcNear == other.GetComponent<NonPlayerCharacter>())
            {
                _currentNpcNear = null;
                DialogStripDisplayer.Instance.CloseDialogStrip();
            }
        }
    }

    #endregion
}
