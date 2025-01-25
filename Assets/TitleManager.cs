using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
   [SerializeField]  private Button startButton;
    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(() =>
            SceneTransitionManager.instance.NextSceneButton(1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
