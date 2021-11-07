using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lasp;

public class SpeakerAudioBinder : MonoBehaviour
{
    public AudioLevelTracker highPassAudioLevelTracker;
    public AudioLevelTracker midPassAudioLevelTracker;
    public AudioLevelTracker lowPassAudioLevelTracker;
    public AudioLevelTracker byPassAudioLevelTracker;

    public RenderTexture audioLevelRenderTex;
    
    private Texture2D audioLevelTex;
    
    /*
    static class ShaderIDs
    {
        public static int HighPassAudioLevel = Shader.PropertyToID("HighPassAudioLevel");
        public static int MidPassAudioLevel = Shader.PropertyToID("MidPassAudioLevel");
        public static int LowPassAudioLevel = Shader.PropertyToID("LowPassAudioLevel");
        public static int ByPassAudioLevel = Shader.PropertyToID("ByPassAudioLevel");
    }
    */
    
    void Start()
    {
        this.audioLevelTex = new Texture2D(audioLevelRenderTex.width, audioLevelRenderTex.height);
    }

    void Update()
    {
        // Applying on entire materials is not good for performance. So I use Render Texture.
        audioLevelTex.SetPixel(0, 0, new Color(
            this.highPassAudioLevelTracker.normalizedLevel,
            this.midPassAudioLevelTracker.normalizedLevel,
            this.lowPassAudioLevelTracker.normalizedLevel,
            this.byPassAudioLevelTracker.normalizedLevel)
        );
        audioLevelTex.Apply();
        Graphics.Blit(audioLevelTex, audioLevelRenderTex);
    }
}
