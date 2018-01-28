using System.Collections;
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
    public AudioClip openClueSound;
    public AudioClip closeClueSound;
    public AudioClip tearPaperSound;

    public AudioClip[] footstepsSounds;

    public AudioSource footstepAudioSource;

    [Header("footsteps cycle parameters")]
    public float footStepLenght = 1.0f;
    public float stepInterval = 2.0f;
    [SerializeField]private float _stepCycle = 0f;
    [SerializeField] private float _nextStepCycle = 0f;
    
    [Header("private variable, don't touch it")]
    [SerializeField] private float _translation = 0f;
    [SerializeField] private float _strafe = 0f;
    [SerializeField] private bool _isLookingAtClue = false;
    [SerializeField] private bool _isTalking = false;

    public bool canDialog = true;


    [SerializeField] private NonPlayerCharacter _currentNpcNear = null;
    
    private RigidbodyConstraints _defaultRigidbodyConstraint;

    private bool hasAlreadyMove = false;
    private bool hasAlreadyDisplayClue = false;

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

    public bool isCharacterMoving
    {
        get { return Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0; }
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
	
	void Update ()
    {
        ManageInput();
        
    }

    #region Input & controls managements

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

        if(isCharacterMoving)
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

        ProgressStepCycle();
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
            PlayFeedbackSound(closeClueSound);
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
            PlayFeedbackSound(openClueSound);
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
        if(!canDialog)
        {
            return;
        }

        if(questManager.IsItTheDestinator(_currentNpcNear))
        {
            //PlayFeedbackSound(positiveFeedbackSound);
            DialogBubbleDisplayer.Instance.LaunchGoodDialog();
            // launch finish quest Dialog
            questManager.FinishCurrentQuest();
        }
        else
        {
            //PlayFeedbackSound(negativeFeedbackSound);
            DialogBubbleDisplayer.Instance.LaunchBadDialog();
            //Launch nope dialog
        }

        canDialog = false;
    }

    #endregion

    #region Tuto related shit

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

    #endregion

    #region lock & unlock character

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

    #endregion

    #region sound

    public void PlayFeedbackSound(AudioClip soundToPlay)
    {
        characterAudioSource.PlayOneShot(soundToPlay);
    }

    private void ProgressStepCycle()
    {
        if (isCharacterMoving)
        {
            _stepCycle += footStepLenght * Time.deltaTime;
        }

        if (!(_stepCycle > _nextStepCycle))
        {
            return;
        }

        _nextStepCycle = _stepCycle + stepInterval;

        PlayFootStepAudio();
    }

    private void PlayFootStepAudio()
    {
        // pick & play a random footstep sound from the array,
        // excluding sound at index 0
        int n = Random.Range(1, footstepsSounds.Length);
        footstepAudioSource.clip = footstepsSounds[n];
        footstepAudioSource.PlayOneShot(footstepAudioSource.clip);
        // move picked sound to index 0 so it's not picked next time
        footstepsSounds[n] = footstepsSounds[0];
        footstepsSounds[0] = footstepAudioSource.clip;
    }

    #endregion

    #region trigger methods

    void OnTriggerEnter(Collider other)
    {
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
