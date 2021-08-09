using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudMasker : MonoBehaviour
{
    public RCC_Skidmarks skidmarks;
    private void Awake() {
        skidmarks = GetComponent<RCC_Skidmarks>();
    }
    
    private void Update() {
        
    }
}
