using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;

public class ResultSceneController : MonoBehaviour
{

    [SerializeField] private Text currentPartsText;// UI表示用
    [SerializeField] private Text currentSpeedText;// UI表示用


    private void Start()
    {
        currentPartsText.text = PlayerControl.currentParts.ToString();
        currentSpeedText.text = PlayerControl.currentSpeed.ToString();

        Invoke("SceneChange", 0.5f);
    }

    void SceneChange()
    {
        SceneTransitionManager.instance.NextSceneButton(0);
    }
}
