using UnityEngine;
using System.Collections;

public class ClubState : MonoBehaviour {
	static public ClubState instance;
	
	public int startingClubSpace = 50;
	public int startingClubPatrons = 30;
	
	public int currentClubSpace = 50;
	public int currentClubPatrons = 30;
	public int currentAlienCount;
	public int aliensTurnedAway;
	public int humansTurnedAway;
	
	public GUIStyle questionButtonStyle;
	
	private enum GameState
	{
		Playing,
		Scoring
	}
	
	private GameState state;
	
	private int questionsAsked;

	private bool[] patronUsed;
	private ClubPatron currentPatron;
	
	private GUIContent descriptionOrResponse = new GUIContent();
	private CharacterDialog[] currentQuestion = new CharacterDialog[3];

	void Awake()
	{
		if (instance != null)
		{
			Destroy(instance.gameObject);
		}
		instance = this;
	}
	
	// Use this for initialization
	void Start ()
	{
	}
	
	public void StartGame()
	{
		currentClubSpace = startingClubSpace;
		currentClubPatrons = startingClubPatrons;
		currentAlienCount = 0;
		aliensTurnedAway = 0;
		humansTurnedAway = 0;
		
		state = GameState.Playing;
		
		patronUsed = new bool[PatronManager.instance.patronList.Length];
		for (int i = 0; i < patronUsed.Length; ++i)
		{
			patronUsed[i] = false;
		}
		

		currentPatron = GetNextPatron();
		
		SetupCurrentPatron();
		
		questionsAsked = 0;
	}
	
	void SetupCurrentPatron()
	{
		descriptionOrResponse.text = currentPatron.characterTextDescription;
		currentQuestion[0] = currentPatron.initialQuestion1;
		currentQuestion[1] = currentPatron.initialQuestion2;
		currentQuestion[2] = currentPatron.initialQuestion3;
	}
	
	ClubPatron GetNextPatron()
	{
		int patronIndex = Random.Range(0, patronUsed.Length);
		for (int i = 0; i < patronUsed.Length; ++i)
		{
			if (!patronUsed[patronIndex])
			{
				patronUsed[patronIndex] = true;
				return PatronManager.instance.patronList[patronIndex];
			}
			++patronIndex;
			if (patronIndex >= patronUsed.Length)
			{
				patronIndex = 0;
			}
		}
		return null;
	}
	
	void AskQuestion(int questionIndex)
	{
		++questionsAsked;
		
		descriptionOrResponse.text = currentQuestion[questionIndex].answer;
		
		currentQuestion[questionIndex] = currentQuestion[questionIndex].nextDialog;
	}
	
	void DecideOnPatron(bool allowIn)
	{
		if (allowIn)
		{
			++currentClubPatrons;
			if ( currentPatron.isAlien ) ++currentAlienCount;
			if ( currentClubPatrons == currentClubSpace )
			{
				// Game complete, let's score it
				state = GameState.Scoring;
				return;
			}
		}
		else
		{
			if ( currentPatron.isAlien )
			{
				++aliensTurnedAway;
			}
			else
			{
				++humansTurnedAway;
			}
			
			currentPatron = GetNextPatron();
			
			if (null != currentPatron)
			{
				SetupCurrentPatron();
			}
			else
			{
				// We ran out of possible patrons, score it
				state = GameState.Scoring;
			}
		}
	}

	void OnGUI()
	{
		switch (state)
		{
		case GameState.Playing:
			RenderPlayingGUI();
			break;
		case GameState.Scoring:
			RenderScoringGUI();
			break;
		}
	}
	
	void RenderPlayingGUI()
	{
	}
	
	void RenderScoringGUI()
	{
	}
}
