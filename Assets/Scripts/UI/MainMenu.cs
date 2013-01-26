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
	
	void Awake()
	{
		instance = this;
	}
	
	void Start()
	{
	}

	void OnGUI()
	{
		GUI.DrawTexture(menuBackgroundRect, menuBackgroundTexture);
		
		if (GUI.Button(newGameButtonRect, "", newGameButtonStyle))
		{
			ClubState.instance.StartGame();
		}

		if (GUI.Button(creditsButtonRect, "", creditsButtonStyle))
		{
			ClubState.instance.ShowCredits();
		}

		if (GUI.Button(exitGameButtonRect, "", exitGameButtonStyle))
		{
			Application.Quit();
		}
	}
}
