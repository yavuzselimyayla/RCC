using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMove : MonoBehaviour
{
    public float speed = 1f;
    public float totalDistance = 0f;

    
    void Update()
    {
        float vert = Input.GetAxis("Vertical");
        transform.Translate(Vector3.forward * vert * speed * Time.deltaTime);
        totalDistance += vert * speed * Time.deltaTime;
        SetMudAmount(totalDistance);
    }

    public void SetMudAmount(float amount) {
        transform.GetChild(0).GetComponent<MeshRenderer>().sharedMaterial.SetFloat("DirtHeight", 
            Mathf.Clamp(amount,0,2));
    }
}
