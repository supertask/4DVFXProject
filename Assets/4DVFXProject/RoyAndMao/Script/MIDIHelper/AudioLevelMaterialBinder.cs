using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lasp;

public class AudioLevelMaterialBinder : MonoBehaviour
{

    [System.Serializable]
    public struct MaterialOverride
    {
        [SerializeField] Renderer _renderer;
        [SerializeField] string _propertyName;

        public Renderer Renderer {
            get => _renderer;
            set => _renderer = value;
        }

        public string PropertyName {
            get => _propertyName;
            set => _propertyName = value;
        }

        public int PropertyID => Shader.PropertyToID(_propertyName);
    }
    
    // Material override list
    [SerializeField] MaterialOverride[] _overrideList = null;
    public MaterialOverride[] overrideList {
        get => _overrideList;
        set => _overrideList = value;
    }
    
    [SerializeField] public AudioLevelTracker highPassAudioLevelTracker;
    [SerializeField] public AudioLevelTracker midPassAudioLevelTracker;
    [SerializeField] public AudioLevelTracker lowPassAudioLevelTracker;
    
    private MaterialPropertyBlock _block;

    void Start()
    {
    }

    void Update()
    {
        // Lazy initialization of the material property block.
        if (_block == null) _block = new MaterialPropertyBlock();

        // Apply the material overrides.
        foreach (var o in _overrideList)
        {
            o.Renderer.GetPropertyBlock(_block);
            _block.SetVector(o.PropertyID, new Vector3(
                highPassAudioLevelTracker.normalizedLevel,
                midPassAudioLevelTracker.normalizedLevel,
                lowPassAudioLevelTracker.normalizedLevel
            ));
            o.Renderer.SetPropertyBlock(_block);
        }
    }
}
