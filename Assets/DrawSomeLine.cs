using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawSomeLine : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() {
        var rt = transform as RectTransform;
        var worldCorners = new Vector3[4];
        rt.GetWorldCorners(worldCorners);
        Debug.DrawLine(worldCorners[0], worldCorners[2], Color.yellow, Mathf.Infinity);
        
    }

    // Update is called once per frame
}
