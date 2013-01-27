using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {
	public static AudioManager instance;
	
	public AudioClip[] songList;
	
	public GUIStyle audioBackgroundStyle;
	public Rect audioBackgroundRect;
	public Rect audioIconRect;
	public Texture audioIcon;
	
	public Rect nowPlayingTextRect;
	public GUIStyle nowPlayingTextStyle;
	
	public float increaseVolumeDuration;
	public float quietVolume;
	public float loudVolume;
	
	private int currentSong;
	private string nowPlayingText;
	private float songEndTime;
	private float volumeChangeEndTime;
	
	void Awake()
	{
		instance = this;
	}
	
	// Use this for initialization
	void Start()
	{
		currentSong = 0;
		audio.clip = songList[currentSong];
		audio.Play();
		audio.volume = quietVolume;
		songEndTime = Time.time + audio.clip.length;
		nowPlayingText = string.Format("Now Playing:\n{0}", songList[currentSong].name);
	}
	
	void Update()
	{
		if (Time.time > volumeChangeEndTime)
		{
			audio.volume = quietVolume;
		}
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
	
	public void IncreaseVolume(float duration)
	{
		audio.volume = loudVolume;
		volumeChangeEndTime = Time.time + duration;
	}
	
	public void Mute(float duration)
	{
		audio.volume = 0;
		volumeChangeEndTime = Time.time + duration;
	}
	
	// Update is called once per frame
	void OnGUI()
	{
		if (ClubState.instance.state == ClubState.GameState.Credits) return;
		
		if (GUI.Button(audioBackgroundRect, "", audioBackgroundStyle))
		{
			PlayNextSong();
		}
		
		GUI.DrawTexture(audioIconRect, audioIcon);
		
		GUI.Label(nowPlayingTextRect, nowPlayingText, nowPlayingTextStyle);
	}
}
