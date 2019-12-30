using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public Transform container;
    Vector3[] containerCorners = new Vector3[4];
    public PlanetView planetView;

    // building rockets
    float buildRockedProgress;
    bool buildingRocket;
    const float rockedBuildSpeed = 0.05f * 0.5f;
    public GameObject rocketPrefab;



    // starting rockets
    bool launchingRocket;

    RocketScript currentStartRocket;

    public Text blueText;
    public Text greenText;

    void Update() {

        // behaves nicely for continuoes strokes
        if (Input.GetKey(KeyCode.J)) {

            if (buildingRocket) {
                buildRockedProgress += rockedBuildSpeed;

                if (buildRockedProgress >= 1f) {

                    // build a rocket
                    buildRockedProgress = 0f;
                    buildingRocket = false;
                    var rocket = GameObject.Instantiate(rocketPrefab,container);
                    var rt = rocket.transform as RectTransform;
                    rt.pivot = new Vector2(0.5f,-3.28f);
                    rocket.transform.localPosition = new Vector3(0f,0f,-100);
                    rocket.transform.rotation = Quaternion.Euler(0,0,Random.Range(0f,360f));


                    // start launching
                    launchingRocket = true;
                    var script = rocket.GetComponent<RocketScript>();
                    Debug.Assert(script);
                    script.WithDefaults();
                    currentStartRocket = script;
                    UpdateRocketText();

                }
            }
        }

        if (Input.GetKeyDown(KeyCode.F) && !buildingRocket) {
            buildRockedProgress = 0f;
            buildingRocket = true;
        }


        // render planet
        planetView.slider.value = buildRockedProgress;
        planetView.slider.gameObject.SetActive(buildingRocket);


        // launchRocket

        if (launchingRocket) {
            for (int i = 0; i < 10; i++) {
                var zeroKey = KeyCode.Alpha0;
                if (Input.GetKeyDown((KeyCode)(zeroKey + i))) {
                    if (currentStartRocket.startSequence[currentStartRocket.startProgressIndex].Equals(i.ToString()[0])) {
                        currentStartRocket.startProgressIndex++;
                        if(currentStartRocket.startProgressIndex == currentStartRocket.startSequence.Length) {
                            launchingRocket = false;
                            StartCoroutine(LaunchRocketCoroutine(currentStartRocket.transform,Vector3.up));
                        }

                        UpdateRocketText();

                    } 

                }
            }

        }

    }

    void Start() {
        UpdateRocketText();
        var rt = container as RectTransform;
        rt.GetWorldCorners(containerCorners);


        var direction = containerCorners[1] - containerCorners[0];
        direction.Normalize();
        // Debug.Log($"direction: {direction}");



        // var ray = new Ray(containerCorners[0], direction);
        
        Debug.DrawLine(containerCorners[1], containerCorners[2]);

        Debug.DrawLine(Vector3.zero, containerCorners[0]);




    }


    IEnumerator LaunchRocketCoroutine(Transform rocketTransfrom, Vector3 direction) {
        const float speed = 0.1f;

        float moved = 0f;

        while (true) {
            yield return null;
            moved += speed;

            var currPos = rocketTransfrom.transform.position;
            var newPos = currPos + direction * speed;
            Debug.Log($"currPos: {currPos}, newPos: {newPos}");
            rocketTransfrom.transform.position = newPos;


            if (newPos.y > containerCorners[1].y) {
                Debug.Log($"y is higher than world corner of canvas");
                GameObject.Destroy(rocketTransfrom.gameObject);
                break;

            }

            else if (moved > 100f) {
                Debug.Log("moved is higher than 100");
                GameObject.Destroy(rocketTransfrom.gameObject);
                break;
            }

        }


    }



    void UpdateRocketText() {
        if (launchingRocket) {
            blueText.text = currentStartRocket.startSequence;
            greenText.text = currentStartRocket.startSequence.Substring(0,currentStartRocket.startProgressIndex);
        }
        blueText.gameObject.SetActive(launchingRocket);
        greenText.gameObject.SetActive(launchingRocket);
    }




































    // aborted mission, for now
    void FindRocketLand() {
        var rt = container.transform as RectTransform;
        var worldCorners = new Vector3[4];
        rt.GetWorldCorners(worldCorners);

        foreach (var v in worldCorners) {
            Debug.Log(v);
        }

        // var randPoint = Random.insideUnitCircle * rt.

        var x = Random.Range(rt.rect.xMin,rt.rect.xMax);
        var y = Random.Range(rt.rect.yMin,rt.rect.yMax);

        Debug.Log($"rt min is {rt.rect.xMin}");

        var rocket = GameObject.Instantiate(rocketPrefab);

        rocketPrefab.transform.position = new Vector3(x,y,0f);
        rocketPrefab.transform.localPosition = new Vector3(
            rocketPrefab.transform.localPosition.x, 
            rocketPrefab.transform.localPosition.y, 
            -100f
            );



    }






}





