using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {
	
	public AudioClip[] songList;
	
	public Rect audioIconRect;
	public Texture audioIcon;
	
	public Rect nowPlayingTextRect;
	public GUIStyle nowPlayingTextStyle;
	
	private int currentSong;
	private string nowPlayingText;
	private float songEndTime;

	// Use this for initialization
	void Start()
	{
		currentSong = 0;
		audio.clip = songList[currentSong];
		audio.Play();
		songEndTime = Time.time + audio.clip.length;
		nowPlayingText = string.Format("Now Playing:\n{0}", songList[currentSong].name);
	}
	
	void Update()
	{
		if (Time.time > songEndTime)
		{
			PlayNextSong();
		}
	}
	
	public void PlayNextSong()
	{
		++currentSong;
		if (currentSong >= songList.Length) currentSong = 0;
		audio.Stop();
		audio.clip = songList[currentSong];
		audio.Play();
		songEndTime = Time.time + audio.clip.length;
		nowPlayingText = string.Format("Now Playing:\n{0}", songList[currentSong].name);
	}
	
	// Update is called once per frame
	void OnGUI()
	{
		GUI.DrawTexture(audioIconRect, audioIcon);
		
		if (GUI.Button(nowPlayingTextRect, nowPlayingText, nowPlayingTextStyle))
		{
			PlayNextSong();
		}
	}
}
