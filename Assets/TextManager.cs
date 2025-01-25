using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TextManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Button startButton;
    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(() =>
            SceneTransitionManager.instance.NextSceneButton(2));
    }
}
