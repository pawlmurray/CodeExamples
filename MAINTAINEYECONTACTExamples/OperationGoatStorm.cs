using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OperationGoatStorm : Events {

    enum GoatState { FirstGoat, FullStampede, StampedeOver }
    GoatState currentState;

    public GameObject goat1;
    public GameObject goat2;
    public GameObject goat3;

    List<GameObject>  goats = new List<GameObject>();
    List<GoatScript> goatScripts = new List<GoatScript>();

    float goatsToMake;
    const float timeBetweenGoats = .35f;
    float timeSinceLastGoat = -2f;

    bool lastGoatClear = false;

	void Start () 
    {
        currentState = GoatState.FirstGoat;
        goatsToMake = Random.Range(20, 30);
        gameObjectsResponsibleFor = goats;
	}
	
	// Update is called once per frame
	void Update () {
        switch (currentState)
        {
            case(GoatState.FirstGoat):
                if (goats.Count < 1)
                {
                    MakeAGoat();
                }
                else if(goats.Count == 1 && goats[0].transform.position.x < -10)
                {
                    currentState = GoatState.FullStampede;
                    this.GetComponent<AudioSource>().Play();
                }
                break;
            case(GoatState.FullStampede):
                if (goatsToMake == 0)
                {
                    currentState = GoatState.StampedeOver;
                }
                else if (timeSinceLastGoat >= timeBetweenGoats)
                {
                    MakeAGoat();
                    timeSinceLastGoat -= timeBetweenGoats;
                }
                timeSinceLastGoat += Time.deltaTime;
                break;
            case(GoatState.StampedeOver):
                if (goats[goats.Count - 1].transform.position.x < -20)
                {
                    lastGoatClear = true;
                }
                break;
        }

        for (int g = 0; g < goats.Count; g++)
        {
            goats[g].transform.position += new Vector3(goatScripts[g].speed, 0, 0);
        }
	}

    // Makes a goat.
    void MakeAGoat()
    {
        Vector3 startingPosition = new Vector3(Random.Range(15.25f, 17f), Random.Range(-5.5f, -2f), Random.Range(7.5f, 14f));
        GameObject goat = goat1;
        switch (Random.Range(0, 3))
        {
            case(0):
                goat = GameObject.Instantiate(goat1, startingPosition, Quaternion.identity) as GameObject;
                break;
            case(1):
                goat = GameObject.Instantiate(goat2, startingPosition, Quaternion.identity) as GameObject;
                break;
            case(2):
                goat = GameObject.Instantiate(goat3, startingPosition, Quaternion.identity) as GameObject;
                break;
        }
        float goatScale = Random.Range(.3f, .8f);
        goat.transform.localScale = new Vector3(goatScale, goatScale, goatScale);
        goat.GetComponent<GoatScript>().speed = Random.Range(.05f, .13f) * -1;
        goats.Add(goat);
        goatScripts.Add(goat.GetComponent<GoatScript>());

        float pitchFinder1 = (goatScale - .3f) / .5f;
        float pitchFinder2 = 1 - pitchFinder1;
        float pitch = pitchFinder2 + .5f;
        goat.GetComponent<AudioSource>().pitch = pitch;
        goat.GetComponent<AudioSource>().volume = goatScale * 1.5f;
        goatsToMake--;
    }

    public override bool ShouldEnd()
    {
        return lastGoatClear ;
    }
}
