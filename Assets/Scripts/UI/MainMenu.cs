using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	public static MainMenu instance;
	
	public Texture menuBackgroundTexture;
	public Rect menuBackgroundRect;
	
	public GUIStyle newGameButtonStyle;
	public Rect newGameButtonRect;
	
	public GUIStyle exitGameButtonStyle;
	public Rect exitGameButtonRect;
	
	public GUIStyle creditsButtonStyle;
	public Rect creditsButtonRect;
	
	private AudioSource buttonAudio;
	
	void Awake()
	{
		instance = this;
		buttonAudio = audio;
	}
	
	void Start()
	{
	}

	void OnGUI()
	{
		GUI.DrawTexture(menuBackgroundRect, menuBackgroundTexture);
		
		if (GUI.Button(newGameButtonRect, "", newGameButtonStyle))
		{
			buttonAudio.Play();
			ClubState.instance.GotoIntro();
		}

		if (GUI.Button(creditsButtonRect, "", creditsButtonStyle))
		{
			buttonAudio.Play();
			ClubState.instance.ShowCredits();
		}

		if (GUI.Button(exitGameButtonRect, "", exitGameButtonStyle))
		{
			Application.Quit();
		}
	}
}
