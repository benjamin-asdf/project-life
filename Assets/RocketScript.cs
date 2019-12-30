using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketScript : MonoBehaviour {
    public float buildProgress;
    public float size;
    public string startSequence;
    public int startProgressIndex;

}

public static class RocketExt {
    public static void WithDefaults(this RocketScript rocket) {
        rocket.buildProgress = 0f;
        rocket.size = 1f;
        rocket.startSequence = "";
        rocket.startProgressIndex = 0;
        for (int i = 0; i < RocketConst.startSequenceSize; i++) {
            rocket.startSequence += Mathf.Floor(Random.value * 10f);
        }
    }

}