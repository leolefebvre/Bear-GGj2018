using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    public List<Quest> questList = new List<Quest>();

    public TutoDisplayer nextObjectiveDisplayer;
    public float timeBeforeDisplayNextObjectiveFeedback = 2.0f;

    private Quest _currentQuest = null;
    public Quest currentQuest
    {
        get { return _currentQuest; }
        private set { _currentQuest = value; }
    }

    private int _currentQuestIndex = 0;

    private ClueDisplayer _clueDisplayer = null;
    public ClueDisplayer clueDisplayer
    {
        get
        {
            if(_clueDisplayer == null)
            {
                _clueDisplayer = ClueDisplayer.Instance;
            }
            return _clueDisplayer;
        }
    }

	// Use this for initialization
	void Start () {
        LaunchQuest(0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void LaunchQuest (int questIndex)
    {
        if(questIndex > questList.Count)
        {
            Debug.Log("You try to reach a quest index that does not exists, it shouldn't happens");
            return;
        }
        
        currentQuest = questList[questIndex];
        _currentQuestIndex = questIndex;

        clueDisplayer.ChangeClueImage(currentQuest.questClue);
    }

    public void FinishCurrentQuest()
    {
        StartCoroutine(DisplayNextObjectiveFeedback());
        LaunchQuest(_currentQuestIndex + 1);
    }

    public bool IsItTheDestinator(NonPlayerCharacter NPC)
    {
        return NPC.destinator == currentQuest.questDestinator;
    }

    IEnumerator DisplayNextObjectiveFeedback()
    {
        yield return new WaitForSeconds(timeBeforeDisplayNextObjectiveFeedback);
        
        nextObjectiveDisplayer.DisplayTuto();
    }
}
