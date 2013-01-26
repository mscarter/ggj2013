using UnityEngine;
using System.Collections;

public class PatronManager : MonoBehaviour {
	static public PatronManager instance;
	
	public ClubPatron[] patronList;
	
	void Awake()
	{
		instance = this;
	}
}
