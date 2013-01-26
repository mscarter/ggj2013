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
			// TODO: start a new game
		}

		if (GUI.Button(creditsButtonRect, "", creditsButtonStyle))
		{
			//TODO: display credits GUI (is this an alternate club state?
		}

		if (GUI.Button(exitGameButtonRect, "", exitGameButtonStyle))
		{
			Application.Quit();
		}
	}
}
