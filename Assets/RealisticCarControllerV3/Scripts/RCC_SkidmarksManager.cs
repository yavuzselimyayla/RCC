using UnityEngine;

public class RCC_SkidmarksManager : MonoBehaviour {

    public RCC_Skidmarks[] skidmarks;
    public int groundIndex = 0;
    public int lastSkidmark = -1;

    public TireTracks wheelTracks;

    void Start() {
        skidmarks = new RCC_Skidmarks[RCC_GroundMaterials.Instance.frictions.Length];

        for (int i = 0; i < skidmarks.Length; i++) {
            skidmarks[i] = Instantiate(RCC_GroundMaterials.Instance.frictions[i].skidmark, Vector3.zero, Quaternion.identity);
            skidmarks[i].transform.name = skidmarks[i].transform.name + "_" + RCC_GroundMaterials.Instance.frictions[i].groundMaterial.name;
            skidmarks[i].transform.SetParent(transform, true);
        }
    }

    // Function called by the wheels that is skidding. Gathers all the information needed to
    // create the mesh later. Sets the intensity of the skidmark section b setting the alpha
    // of the vertex color.
    public int AddSkidMark(Vector3 pos, Vector3 normal, float intensity, int lastIndex, int groundIndex) {
        lastSkidmark = skidmarks[groundIndex].AddSkidMark(pos, normal, intensity, lastIndex);
        return lastSkidmark;
    }

    public void AddTrack(Vector3 pos) {
        wheelTracks.AddWheelTrack(pos);
    }
}
