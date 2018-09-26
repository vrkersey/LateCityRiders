using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMovement : MonoBehaviour {

    public float scrollSpeed = .5f;
    public float xPercent = 1f;
    public float yPercent = 0f;
    float offset;
    float rotate;
 
    void Update()
    {
        offset += (Time.deltaTime * scrollSpeed) / 10.0f;

        transform.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(xPercent * offset, yPercent * offset));

    }
}
