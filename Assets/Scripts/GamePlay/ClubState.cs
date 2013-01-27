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
	
	#region GUI parameters
	// Splash screen parameters
	public Texture gameLogoTexture;
	public Rect gameLogoRect;
	public Texture backgroundTexture;
	public Rect backgroundRect;
	public GUIStyle viewIntroButtonStyle;
	public Rect viewIntroButtonRect;
	
	// Intro screen parameters
	public Rect introLogoRect;
	public GUIStyle playGameButtonStyle;
	public Rect playGameButtonRect;
	public GUIStyle gameBackgroundStyle;
	public string gameBackgroundText;
	public Rect gameBackgroundTextRect;
	
	// Playing game parameters
	public Rect questionBackgroundRect;
	public Texture questionBackgroundTexture;
	public Rect identityCardRect;
	public GUIStyle responseTextStyle;
	public Rect responseRect;
	public Rect rightQuoteRect;
	public Texture rightQuoteTexture;
	public Rect leftQuoteRect;
	public Texture leftQuoteTexture;
	public GUIStyle questionButtonStyle;
	public Rect question1Rect;
	public Rect question2Rect;
	public Rect question3Rect; 
	public Rect choiceRect;
	public GUIStyle choiceBackgroundStyle;
	public Rect allowInButtonRect;
	public GUIStyle allowInButtonStyle;
	public Rect kickOutButtonRect;
	public GUIStyle kickOutButtonStyle;
	public Rect clubMapRect;
	public Texture clubMapTexture;
	public Rect clubPatronCountRect;
	public GUIStyle clubPatronCountStyle;
	public Rect clubBackgroundRect;
	public Texture clubBackgroundTexture;
	public Texture clubBackgroundOpenDoorTexture;
	
	// Credits screen parameters
	public Rect[] creditRects;
	public Texture[] creditTextures;
	#endregion GUI parameters
	
	#region Audio references
	public AudioSource buttonClickSource;
	public AudioSource introScreenSource;
	public AudioSource denyAudioSource;
	#endregion
	
	private enum GameState
	{
		SplashScreen,
		IntroScreen,
		Playing,
		LettingIn,
		Rejected,
		Scoring,
		Credits
	}
	
	private GameState state;
	
	private int questionsAsked;

	private bool[] patronUsed;
	private ClubPatron currentPatron;
	
	private GUIContent descriptionOrResponse = new GUIContent();
	private CharacterDialog[] currentQuestion = new CharacterDialog[3];
	
	private string clubPatronCount;
	
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
		state = GameState.SplashScreen;
	}
	
	public void StartGame()
	{
		currentClubSpace = startingClubSpace;
		currentClubPatrons = startingClubPatrons;
		currentAlienCount = 0;
		aliensTurnedAway = 0;
		humansTurnedAway = 0;
		
		clubPatronCount = string.Format("{0} of {1}", currentClubPatrons, currentClubSpace);
		
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
			AudioManager.instance.IncreaseVolume();
			++currentClubPatrons;
			clubPatronCount = string.Format("{0} of {1}", currentClubPatrons, currentClubSpace);
			if ( currentPatron.isAlien ) ++currentAlienCount;
			descriptionOrResponse.text = "Go on in.";
			state = GameState.LettingIn;
		}
		else
		{
			denyAudioSource.Play();
			if ( currentPatron.isAlien )
			{
				++aliensTurnedAway;
			}
			else
			{
				++humansTurnedAway;
			}
			descriptionOrResponse.text = "None shall pass!";
			state = GameState.Rejected;
		}
		
		StartCoroutine(NextPatron());
	}
	
	IEnumerator NextPatron()
	{
		yield return new WaitForSeconds(5);
		
		if (currentClubPatrons == currentClubSpace)
		{
			state = GameState.Scoring;
		}
		else
		{
			currentPatron = GetNextPatron();
			
			if (null != currentPatron)
			{
				SetupCurrentPatron();
				state = GameState.Playing;
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
		case GameState.SplashScreen:
			RenderSplashScreenGUI();
			break;
		case GameState.IntroScreen:
			RenderIntroScreenGUI();
			break;
		case GameState.Playing:
			RenderPlayingGUI();
			break;
		case GameState.LettingIn:
			RenderLettingInGUI();
			break;
		case GameState.Rejected:
			RenderRejectedGUI();
			break;
		case GameState.Scoring:
			RenderScoringGUI();
			break;
		case GameState.Credits:
			RenderCreditsGUI();
			break;
		}
	}
	
	void RenderSplashScreenGUI()
	{
		GUI.DrawTexture(backgroundRect, backgroundTexture);
		
		GUI.DrawTexture(gameLogoRect, gameLogoTexture);

		if (GUI.Button(viewIntroButtonRect, "", viewIntroButtonStyle))
		{
			state = GameState.IntroScreen;
			introScreenSource.Play();
		}
	}
	
	void RenderIntroScreenGUI()
	{
		GUI.DrawTexture(backgroundRect, backgroundTexture);
		
		GUI.DrawTexture(introLogoRect, gameLogoTexture);

		GUI.Label(gameBackgroundTextRect, gameBackgroundText, gameBackgroundStyle);
		
		if (GUI.Button(playGameButtonRect, "", playGameButtonStyle))
		{
			buttonClickSource.Play();
			StartGame();
		}
	}
	
	void RenderPlayingGUI()
	{
		GUI.DrawTexture(backgroundRect, backgroundTexture);
		
		GUI.DrawTexture(clubBackgroundRect, clubBackgroundTexture);
		
		GUI.DrawTexture(questionBackgroundRect, questionBackgroundTexture);
		
		GUI.DrawTexture(identityCardRect, currentPatron.idCardTexture);
		
		GUI.Label(responseRect, descriptionOrResponse, responseTextStyle);

		GUI.DrawTexture(leftQuoteRect, leftQuoteTexture);
		GUI.DrawTexture(rightQuoteRect, rightQuoteTexture);
		
		if (null != currentQuestion[0] && GUI.Button(question1Rect, currentQuestion[0].question, questionButtonStyle))
		{
			buttonClickSource.Play();
			AskQuestion(0);
		}
		
		if (null != currentQuestion[1] && GUI.Button(question2Rect, currentQuestion[1].question, questionButtonStyle))
		{
			buttonClickSource.Play();
			AskQuestion(1);
		}
		
		if (null != currentQuestion[2] && GUI.Button(question3Rect, currentQuestion[2].question, questionButtonStyle))
		{
			buttonClickSource.Play();
			AskQuestion(2);
		}
		
		GUI.Label(choiceRect, "Select a question on the right or\nchoose a fate below at any time.", choiceBackgroundStyle);
		
		if (GUI.Button(allowInButtonRect, "", allowInButtonStyle))
		{
			DecideOnPatron(true);
		}
		
		if (GUI.Button(kickOutButtonRect, "", kickOutButtonStyle))
		{
			DecideOnPatron(false);
		}
		
		GUI.DrawTexture(clubMapRect, clubMapTexture);
		
		GUI.Label(clubPatronCountRect, clubPatronCount, clubPatronCountStyle);
	}
	
	void RenderLettingInGUI()
	{
		GUI.DrawTexture(backgroundRect, backgroundTexture);
		
		GUI.DrawTexture(clubBackgroundRect, clubBackgroundOpenDoorTexture);
		
		GUI.DrawTexture(questionBackgroundRect, questionBackgroundTexture);
		
		GUI.DrawTexture(identityCardRect, currentPatron.idCardTexture);
		
		GUI.Label(responseRect, descriptionOrResponse, responseTextStyle);

		GUI.DrawTexture(leftQuoteRect, leftQuoteTexture);
		GUI.DrawTexture(rightQuoteRect, rightQuoteTexture);
		
		GUI.Label(choiceRect, "Select a question on the right or\nchoose a fate below at any time.", choiceBackgroundStyle);
		
		GUI.Label(allowInButtonRect, "", allowInButtonStyle);
		
		GUI.Label(kickOutButtonRect, "", kickOutButtonStyle);
		
		GUI.DrawTexture(clubMapRect, clubMapTexture);
		
		GUI.Label(clubPatronCountRect, clubPatronCount, clubPatronCountStyle);
	}
	
	void RenderRejectedGUI()
	{
		GUI.DrawTexture(backgroundRect, backgroundTexture);
		
		GUI.DrawTexture(clubBackgroundRect, clubBackgroundTexture);
		
		GUI.DrawTexture(questionBackgroundRect, questionBackgroundTexture);
		
		GUI.DrawTexture(identityCardRect, currentPatron.idCardTexture);
		
		GUI.Label(responseRect, descriptionOrResponse, responseTextStyle);

		GUI.DrawTexture(leftQuoteRect, leftQuoteTexture);
		GUI.DrawTexture(rightQuoteRect, rightQuoteTexture);
		
		GUI.Label(choiceRect, "Select a question on the right or\nchoose a fate below at any time.", choiceBackgroundStyle);
		
		GUI.Label(allowInButtonRect, "", allowInButtonStyle);
		
		GUI.Label(kickOutButtonRect, "", kickOutButtonStyle);
		
		GUI.DrawTexture(clubMapRect, clubMapTexture);
		
		GUI.Label(clubPatronCountRect, clubPatronCount, clubPatronCountStyle);
	}
	
	void RenderScoringGUI()
	{
		GUI.DrawTexture(backgroundRect, backgroundTexture);
	}
	
	public void GotoIntro()
	{
		introScreenSource.Play();
		state = GameState.IntroScreen;	
	}
	
	public void ShowCredits()
	{
		state = GameState.Credits;
	}
	
	void RenderCreditsGUI()
	{
		int creditsCount = Mathf.Min(creditRects.Length, creditTextures.Length);
		
		for (int i = 0; i < creditsCount; ++i)
		{
			GUI.DrawTexture(creditRects[i], creditTextures[i]);
		}
	}
}
