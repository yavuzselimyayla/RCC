using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TireTracks : MonoBehaviour {
    public CustomRenderTexture _splatmap;
    public Vector2Int textureSize;

    public Material _drawMaterial;
    public Material _snowMaterial;

    [Range(0, 1000)] public float _brushSize;
    [Range(0, 50)] public float _brushStrength;

    private void Awake() {
        _splatmap = new CustomRenderTexture(textureSize.x, textureSize.y, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);

        _splatmap.updateMode = CustomRenderTextureUpdateMode.OnDemand;
        _splatmap.material = _drawMaterial;
        _splatmap.initializationSource = CustomRenderTextureInitializationSource.Material;
        _splatmap.initializationMaterial = _drawMaterial;
        _splatmap.doubleBuffered = true;

        _drawMaterial.SetVector("_DrawColor", Color.red);
        _drawMaterial.SetTexture("_SplatMap", _splatmap);
        _snowMaterial.SetTexture("_SplatMap", _splatmap);
    }

    public void AddWheelTrack(Vector3 pos) {
        StartCoroutine(DrawTrack(pos));
    }

    IEnumerator DrawTrack(Vector3 pos) {
        _drawMaterial.SetFloat("_BrushStrength", _brushStrength);
        _drawMaterial.SetFloat("_BrushSize", _brushSize);
        _splatmap.Initialize();

        string wheelCoord = "_WheelCoord1";
        _drawMaterial.SetVector(wheelCoord, new Vector4(pos.x, pos.z, 0, 0));

        _splatmap.Update();
        yield return null;
    }
}
