using UnityEngine;
using UnityEngine.UI;

public class ResultSceneController : MonoBehaviour
{

    [SerializeField] private Text currentPartsText;// UI�\���p
    [SerializeField] private Text currentSpeedText;// UI�\���p
    [SerializeField] private Button resultSceneButton;

    private void Start()
    {
        currentPartsText.text = PlayerControl.currentParts.ToString();
        currentSpeedText.text = PlayerControl.currentSpeed.ToString();
        resultSceneButton.onClick.AddListener(() =>
        SceneTransitionManager.instance.NextSceneButton(0));
    }


}
