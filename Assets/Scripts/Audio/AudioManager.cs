using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {
	public static AudioManager instance;
	
	public AudioClip[] songList;
	
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
	private float volumeIncreaseEndTime;
	
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
		if (Time.time > volumeIncreaseEndTime)
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
	
	public void IncreaseVolume()
	{
		audio.volume = loudVolume;
		volumeIncreaseEndTime = Time.time + increaseVolumeDuration;
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
