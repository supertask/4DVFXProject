using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lasp;

public class AudioLevelBaker : MonoBehaviour
{
    public AudioLevelTracker highPassAudioLevelTracker;
    public AudioLevelTracker lowPassAudioLevelTracker;
    public RenderTexture audioLevelRenderTex;
    public List<Material> materials;
    public float referenceWaveVelocity = 1.0f;
    
    private Texture2D audioLevelTex;
    private float waveMigrationLength = 0.0f;

    //public Material material;
    
    static class ShaderIDs
    {
        public static int HighPassAudioLevel = Shader.PropertyToID("HighPassAudioLevel");
        public static int LowPassAudioLevel = Shader.PropertyToID("LowPassAudioLevel");

    }
    
    void Start()
    {
        this.audioLevelTex = new Texture2D(audioLevelRenderTex.width, audioLevelRenderTex.height);
    }

    void Update()
    {
        // Applying on entire materials is not good for performance. So I use Render Texture.
        audioLevelTex.SetPixel(0, 0, new Color(
            //1, 1, 1)
            this.highPassAudioLevelTracker.normalizedLevel,
            this.lowPassAudioLevelTracker.normalizedLevel, 0)
        );
        audioLevelTex.Apply();
        Graphics.Blit(audioLevelTex, audioLevelRenderTex);
        
        float waveVelocity = referenceWaveVelocity * this.highPassAudioLevelTracker.normalizedLevel;
        this.waveMigrationLength += waveVelocity * Time.deltaTime;
        foreach(Material material in this.materials)
        {
            material.SetFloat("WaveMigrationLength", waveMigrationLength);
        }
        //this.material.SetFloat("HighPassAudioLevel",  this.highPassAudioLevelTracker.normalizedLevel);
        //this.material.SetFloat("LowPassAudioLevel",  this.lowPassAudioLevelTracker.normalizedLevel);
    }
}
