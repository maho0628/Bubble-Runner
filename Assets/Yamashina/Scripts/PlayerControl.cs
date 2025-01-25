using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField, Tooltip("The base speed of the character. This value serves as the basis for other speed calculations.")]
    private float baseSpeed = 10f; // 基本速度
    [SerializeField, Tooltip("The current speed of the character, dynamically adjusted during gameplay.")]
    private int maxParts = 6; // 最大ライフ数

    [Header("Runtime Values")]
   [SerializeField,Tooltip("The current speed of the character, dynamically adjusted during gameplay.")]
    private float currentSpeed; // 現在の速度
    [SerializeField, Tooltip("The current number of lives the character has.This decreases when the character hits obstacles")]
  
    private int currentParts; // 現在のライフ数
    public Transform[] lanes; // レーンの位置
    private int currentLane = 1; // 現在のレーン (0 = 左, 1 = 中央, 2 = 右)

    [Header("UI Elements")]
    public Text lifeText; // UI表示用
    public Text speedText; // UI表示用

    void Start()
    {
        currentParts = maxParts; // 最大ライフでスタート
        currentSpeed = baseSpeed * currentParts; // 初期速度設定
        UpdateUI();
    }
    void Update()
    {
        // キャラクターを前方に移動
        transform.Translate(Vector3.forward *currentSpeed * Time.deltaTime);

        // レーンの切り替え
        if (Input.GetKeyDown(KeyCode.A))
        {
            ChangeLane(-1);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            ChangeLane(1);
        }
    }
    private void ChangeLane(int direction)
    {
        int newLane = Mathf.Clamp(currentLane + direction, 0, lanes.Length - 1);
        if (newLane != currentLane)
        {
            currentLane = newLane;
            Vector3 newPosition = new Vector3(lanes[currentLane].position.x, transform.position.y, transform.position.z);
            transform.position = newPosition;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            HitObstacle();
        }
    }

    private void HitObstacle()
    {
        if (currentParts > 1)
        {
            currentParts--;
            currentSpeed = baseSpeed * currentParts; // スピードを再計算
            QuickTimeEvent(); // クイックタイムイベントを呼び出す
        }
        else
        {
            GameOver();
        }
        UpdateUI();
    }

    private void QuickTimeEvent()
    {
        // クイックタイムイベントのロジック (ここでは簡略化)
        Debug.Log("QuickTime Event Triggered!");
    }

    private void UpdateUI()
    {
        lifeText.text = "Life: " + currentParts;
        speedText.text = "Speed: " + currentSpeed.ToString("F1");
    }

    private void GameOver()
    {
        Debug.Log("Game Over");
        currentSpeed = 0;
        SceneTransitionManager.instance.NextSceneButton(0);
        // ゲーム終了処理
    }
}
