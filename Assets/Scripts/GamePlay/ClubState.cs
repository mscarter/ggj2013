using UnityEngine;
using System.Collections;

public class ClubState : MonoBehaviour {
	static public ClubState instance;
	
	public int clubSpace = 50;
	public int currentClubPatrons = 30;
	
	void Awake()
	{
		if (instance != null)
		{
			Destroy(instance.gameObject);
		}
		instance = this;
	}
	
	// Use this for initialization
	void Start () {
	
	}

	void OnGUI()
	{
	}
}
