using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Timeline;
using UnityEngine.Playables;

using System.Collections;
//using UnityEditor;


//[ExecuteInEditMode]
public class VolumetricVideoSwitcher : MonoBehaviour
{
	private Depthkit.Clip clip;
	private VideoPlayer player;
	private PlayableDirector director;


	[SerializeField] private TextAsset[] metaFiles;
	[SerializeField] private VideoClip[] videos;
	[SerializeField] private TimelineAsset[] timelines;
	[SerializeField] private GameObject[] authorUILabels;

	
	[Button("Mao1")]
	public bool mao1;
	
	[Button("Mao2")]
	public bool mao2;

	[Button("Mao3")]
	public bool mao3;

	[Button("Roy1")]
	public bool roy1;
	
	[Button("Roy2")]
	public bool roy2;
	
	[Button("Roy3")]
	public bool roy3;
	
	
	private void Init()
	{
		this.clip = this.GetComponent<Depthkit.Clip>();
		this.player = this.GetComponent<VideoPlayer>();
		this.director = this.GetComponent<PlayableDirector>();
	}
	
	private void AssignVolumetricVideoInfo(int index)
	{
		this.Init();
		this.clip.metadataFile = metaFiles[index];
		this.player.clip = videos[index];
		this.director.playableAsset = timelines[index];
		
		for(int i = 0; i < authorUILabels.Length; i++)
		{
			this.authorUILabels[i].SetActive(false);
		}
		this.authorUILabels[index].SetActive(true);

	}

	
	//[ContextMenu("Switch Volumetric Video Mao 1")]
	public void Mao1 ()
	{
		Debug.Log("Mao1");
		this.AssignVolumetricVideoInfo(0);
	}

	//[ContextMenu("Switch Volumetric Video Mao 2")]
	public void Mao2 ()
	{
		Debug.Log("Mao2");
		this.AssignVolumetricVideoInfo(1);
	}

	//[ContextMenu("Switch Volumetric Video Mao 3")]
	public void Mao3 ()
	{
		Debug.Log("Mao3");
		this.AssignVolumetricVideoInfo(2);
	}
	
	//[ContextMenu("Switch Volumetric Video Roy 1")]
	public void Roy1 ()
	{
		Debug.Log("Roy1");
		this.AssignVolumetricVideoInfo(3);
	}
	
	//[ContextMenu("Switch Volumetric Video Roy 2")]
	public void Roy2 ()
	{
		Debug.Log("Roy2");
		this.AssignVolumetricVideoInfo(4);
	}
	
	//[ContextMenu("Switch Volumetric Video Roy 3")]
	public void Roy3 ()
	{
		Debug.Log("Roy3");
		this.AssignVolumetricVideoInfo(5);
	}
}