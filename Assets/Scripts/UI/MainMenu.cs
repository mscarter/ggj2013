using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	public static MainMenu instance;
	
	public GUIStyle menuStyle;
	
	public GUIContent playButton;
	public Vector2 playButtonSize;
	public Vector2 playButtonPosition;
	
	void Awake()
	{
		instance = this;
	}
	
	void Start()
	{
	}

	void OnGUI()
	{
		if (GUI.Button(new Rect(playButtonPosition.x, playButtonPosition.y, playButtonSize.x, playButtonSize.y), playButton, menuStyle))
		{
			// TODO: start a new game
		}
	}
}
