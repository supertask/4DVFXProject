using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using UnityEditor;

using System.Collections;


[ExecuteInEditMode]
public class VolumetricVideoSwitcher : MonoBehaviour
{
	[SerializeField] private TextAsset[] metaFiles;
	[SerializeField] private VideoClip[] videos;
	[SerializeField] private TimelineAsset[] timelines;
	[SerializeField] private GameObject[] authorUILabels;
	[SerializeField] public Dancer dancer;

	private Depthkit.Clip clip;
	private VideoPlayer player;
	private PlayableDirector director;
	private Dancer exDancer;

	public enum Dancer 
	{
		Mao1 = 0,
		Mao2 = 1,
		Mao3 = 2,
		Roy1 = 3,
		Roy2 = 4,
		Roy3 = 5
	}

	/*
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
	*/
	
	
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
	
	void Update()
	{
#if UNITY_EDITOR
		if (! EditorApplication.isPlaying)
		{ 
			/*
			if (Time.frameCount % 60 == 0)
			{
				Debug.Log("Dancer:" + dancer);
				this.AssignVolumetricVideoInfo((int)dancer);
			}
			*/
			
			// Dancerが切り替わったら, データも切り替える
			if (this.exDancer != this.dancer)
			{
				Debug.Log("Dancer:" + dancer);
				this.AssignVolumetricVideoInfo((int)dancer);
				this.exDancer = this.dancer;
			}
		}
#endif
	}
	
	void OnDestroy()
	{
#if UNITY_EDITOR
		//DepthKitのDepthKitClipは終了後，metadataが初期化されるので，上書きする
		Debug.Log("Dancer:" + dancer);
		this.AssignVolumetricVideoInfo((int)dancer);
#endif
	}

	/*
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
	*/
}