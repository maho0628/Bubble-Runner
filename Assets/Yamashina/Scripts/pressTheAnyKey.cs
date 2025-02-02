using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class pressTheAnyKey : MonoBehaviour
{
    public MultiAudio multiAudio;

    // Start is called before the first frame update
    void Start()
    {
        if (multiAudio == null)
        {
            Debug.LogError("Audio is not assigned.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!SceneTransitionManager.instance.isReloading)
        {
            if (Input.anyKeyDown && !IsMouseButtonDown())
            {
		if (Input.GetKeyDown(KeyCode.Escape))
			{
				Application.Quit();
			}
		else
			{
                		SceneTransitionManager.instance.SceneChange(SceneInformation.SCENE.Scenario);
                		multiAudio.PlaySEByName("GameStartSound");
			}
            }
        }
    }

    private bool IsMouseButtonDown()
    {
        return Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2);
    }

}
