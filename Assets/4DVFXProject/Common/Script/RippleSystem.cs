using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

using Lasp;
using ComputeShaderUtil;

public struct GPUParticle
{
    public int isActive;       // 有効フラグ
    public int pid;       // particle id
    public Vector3 position;    // 座標
    public Vector3 velocity;    // 速
    public Vector3 force;    // 力
    public Color color;         // 色
    //public float duration;      // 年齢
    public float age;      // 年齢
    public float lifetime; //寿命
    public float scale;         // サイズ
    public override string ToString() {
        return $"(isActive = {isActive}, position = {position}, velocity = {velocity}, age = {age}, lifetime = {lifetime} scale = {scale}, color = {color})";
    }
}

public class RippleSystem : GPUParticleBase<GPUParticle>
{
    #region public
    public Vector2 velocityRange = new Vector2(-0.3f, 2);
    public Vector3 startPositions = new Vector3(0.05f, 0.07f, 0.08f);
    public Vector2 lifeTimeRange = new Vector3(0.3f, 0.9f);
    //public Vector2 positionRange = new Vector2(0.1f, 0.3f);
    //public float lifeTime = 1;
    public float scaleMin = 1;
    public float scaleMax = 2;
    public RenderTexture rippleTex;
    public ComputeShader rippleCS;
    public AudioLevelTracker highPassAudioLevelTracker;
    public AudioLevelTracker midPassAudioLevelTracker;
    public AudioLevelTracker lowPassAudioLevelTracker;

    [Range(0,1)] public float sai = 1;   // 彩度
    [Range(0,1)] public float val = 1;   // 明るさ
    
    public float normalizedWaveHalfAmplitude = 0.15f;

    
    private RenderTexture rippleTmpTex;
    private Kernel resetWaveTexKernel;
    private Kernel updateWaveTexKernel;
    //public Material material;
    #endregion

    protected override void Initialize()
    {
        base.Initialize();
        this.resetWaveTexKernel = new Kernel(this.rippleCS, "ResetWaveTex");
        this.updateWaveTexKernel = new Kernel(this.rippleCS, "UpdateWaveTex");
        this.rippleTmpTex = RenderTexUtil.CreateRenderTexture(
            rippleTex.width, rippleTex.height, 0,
            RenderTextureFormat.ARGBFloat, TextureWrapMode.Repeat,
            FilterMode.Bilinear);
    }

    /// <summary>
    /// パーティクルの更新
    /// </summary>
    protected override void UpdateParticle()
    {
        particleActiveBuffer.SetCounterValue(0);

        cs.SetFloat("_DT", Time.deltaTime);
        cs.SetVector("_LifeTimeRange", lifeTimeRange);
        cs.SetVector("_AudioAmplitude", new Vector3(
            highPassAudioLevelTracker.normalizedLevel,
            midPassAudioLevelTracker.normalizedLevel,
            lowPassAudioLevelTracker.normalizedLevel
        ));
        cs.SetVector("_VelocityRange", velocityRange);
        cs.SetBuffer(updateKernel, "_Particles", particleBuffer);
        cs.SetBuffer(updateKernel, "_DeadList", particlePoolBuffer);
        cs.SetBuffer(updateKernel, "_ActiveList", particleActiveBuffer);

        cs.Dispatch(updateKernel, particleNum / THREAD_NUM_X, 1, 1);

        particleActiveCountBuffer.SetData(particleCounts);
        ComputeBuffer.CopyCount(particleActiveBuffer, particleActiveCountBuffer, 0);
        //particleActiveCountBuffer.GetData(particleCounts);
        //particleActiveNum = particleCounts[0];
    }

    /// <summary>
    /// パーティクルの発生
    /// THREAD_NUM_X分発生
    /// </summary>
    /// <param name="position"></param>
    void EmitParticle()
    {
        particlePoolCountBuffer.SetData(particleCounts);
        ComputeBuffer.CopyCount(particlePoolBuffer, particlePoolCountBuffer, 0);
        particlePoolCountBuffer.GetData(particleCounts);
        //Debug.Log("EmitParticle Pool Num " + particleCounts[0] + " position " + position);
        particlePoolNum = particleCounts[0];

        if (particleCounts[0] < emitNum) return;   // emitNum未満なら発生させない

        cs.SetVector("_AudioAmplitude", new Vector3(
            highPassAudioLevelTracker.normalizedLevel,
            midPassAudioLevelTracker.normalizedLevel,
            lowPassAudioLevelTracker.normalizedLevel
        ));
        //cs.SetVector("_PositionRange", positionRange);
        cs.SetVector("_StartPositions", startPositions);
        cs.SetVector("_VelocityRange", velocityRange);
        cs.SetVector("_LifeTimeRange", lifeTimeRange);
        cs.SetFloat("_ScaleMin", scaleMin);
        cs.SetFloat("_ScaleMax", scaleMax);
        cs.SetFloat("_Sai", sai);
        cs.SetFloat("_Val", val);
        cs.SetFloat("_Time", Time.time);
        cs.SetBuffer(emitKernel, "_ParticlePool", particlePoolBuffer);
        cs.SetBuffer(emitKernel, "_Particles", particleBuffer);

        //cs.Dispatch(emitKernel, particleCounts[0] / THREAD_NUM_X, 1, 1);
        cs.Dispatch(emitKernel, emitNum / THREAD_NUM_X, 1, 1);   // emitNumの数だけ発生
    }
    

    private void ResetWaveTex ()
    {
        Debug.Log(this.rippleTex.width );
        Debug.Log( Mathf.CeilToInt ( (float) this.rippleTex.width / this.resetWaveTexKernel.ThreadX) );
        this.rippleCS.SetTexture(this.resetWaveTexKernel.Index, "_RippleTex", this.rippleTmpTex);
        this.rippleCS.Dispatch(this.resetWaveTexKernel.Index,
            Mathf.CeilToInt ( (float) this.rippleTex.width / this.resetWaveTexKernel.ThreadX), 1, 1);
    }

    private void UpdateWaveTex ()
    {
        this.rippleCS.SetInt("_RippleTextureWidth", this.rippleTmpTex.width);
        this.rippleCS.SetFloat("_NormalizedWaveHalfAmplitude", normalizedWaveHalfAmplitude);
        this.rippleCS.SetVector("_AudioAmplitude", new Vector3(
            highPassAudioLevelTracker.normalizedLevel,
            midPassAudioLevelTracker.normalizedLevel,
            lowPassAudioLevelTracker.normalizedLevel
        ));

        this.rippleCS.SetBuffer(this.updateWaveTexKernel.Index, "_Particles", particleBuffer);
        this.rippleCS.SetTexture(this.updateWaveTexKernel.Index, "_RippleTex", this.rippleTmpTex);
        this.rippleCS.Dispatch(this.updateWaveTexKernel.Index,
            Mathf.CeilToInt (emitNum / this.updateWaveTexKernel.ThreadX), 1, 1);
    }

    // Update is called once per frame
    protected override void Update ()
    {
        //if (Input.GetMouseButton(0))
        //{
        //}

        //if (Time.frameCount % 2 == 0)
        //{
        EmitParticle();
        //}
        UpdateParticle();
        
        ResetWaveTex();
        UpdateWaveTex();
        
        Graphics.Blit(this.rippleTmpTex, this.rippleTex);
        //ComputeBufferUtil.DebugBuffer<GPUParticle>(particleBuffer, 0, 1);
    }


}
