using UnityEngine;

public class CheckpointSingle : MonoBehaviour {

    private TrackCheckpoints trackCheckpoints;
    private MeshRenderer meshRenderer;

    private void Awake() {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start() {
        //Hide();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            trackCheckpoints.CarThroughCheckpoint(this, other.GetComponentInParent<RCC_CarControllerV3>());
        }
    }

    public void SetTrackCheckpoints(TrackCheckpoints trackCheckpoints) 
        => this.trackCheckpoints = trackCheckpoints;

    public void Show() => meshRenderer.enabled = true;
    public void Hide() => meshRenderer.enabled = false;
}
