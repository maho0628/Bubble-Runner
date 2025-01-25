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
    public static float currentSpeed; // 現在の速度
    [SerializeField, Tooltip("The current number of lives the character has.This decreases when the character hits obstacles")]

    public static int currentParts; // 現在のライフ数
    public Transform[] lanes; // レーンの位置
    private int currentLane = 1; // 現在のレーン (0 = 左, 1 = 中央, 2 = 右)
    [Header("Goal Settings")]
    public Transform goalTransform; // ゴール地点
    public float goalRadius = 1.0f; // ゴール地点の範囲
    public float timeLimit = 30.0f; // 制限時間 (秒)

    private float elapsedTime = 0.0f; // 経過時間
    private bool isGoalReached = false; // ゴール判定

    private bool isJumping = false;
    //[Header("UI Elements")]
    //public Text lifeText; // UI表示用
    //public Text speedText; // UI表示用

    void Start()
    {
        currentParts = maxParts; // 最大ライフでスタート
        currentSpeed = baseSpeed;
        //UpdateUI();
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
        if (currentParts <= 0)
        {
            currentParts = 1; // 最低でも1にする
        }

        Jump();

        if (!isGoalReached)
        {
            elapsedTime += Time.deltaTime;

            // プレイヤーがゴール地点に到達しているか判定
            float distanceToGoal = Vector3.Distance(transform.position, goalTransform.position);
            if (distanceToGoal <= goalRadius && elapsedTime <= timeLimit)
            {
                GoalReached(); // ゴール判定処理
            }

            // 時間切れのチェック
            if (elapsedTime > timeLimit)
            {
                TimeOut(); // タイムアウト処理
            }


        }
    }
    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (!isJumping)
            {
                GetComponentInChildren<Rigidbody>().AddForce(new Vector2(0, 5), ForceMode.Impulse);
                isJumping = true;
            }
            StartCoroutine(ResetJump());
        }
    }
    IEnumerator ResetJump()
    {
        yield return new WaitForSeconds(1);
        isJumping = false;
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
        if (other.CompareTag("Obstacle")) //player Obstacle hit
        {
            HitObstacle();
        }

        //if (other.CompareTag("NPC"))//npc player hit
        //{
        //    QuickTimeEvent();
        //}
    }

    private void HitObstacle()
    {
        if (currentParts > 1)
        {
            currentParts--;
            Debug.Log(currentParts);
            currentSpeed -= baseSpeed / currentParts; // スピードを再計算
        }
        else
        {
            GameOver();
        }
        //UpdateUI();
    }
    private void GoalReached()
    {
        isGoalReached = true; // ゴール到達フラグ
        Debug.Log("Goal Reached!");
        SceneTransitionManager.instance.NextSceneButton(2);

        // ゴール達成時の処理をここに追加
    }

    private void TimeOut()
    {
        Debug.Log("Time Out! Game Over.");
        GameOver();
        // 時間切れ時の処理をここに追加

    }

    //private void QuickTimeEvent()
    //{
    //    // クイックタイムイベントのロジック (ここでは簡略化)
    //    Debug.Log("QuickTime Event Triggered!");
    //}

    //private void UpdateUI()
    //{
    //    lifeText.text = "Life: " + currentParts;
    //    speedText.text = "Speed: " + currentSpeed.ToString("F1");
    //}
    public void RecoverLife(int amount)
    {
        currentParts = Mathf.Clamp(currentParts + amount, 0, maxParts); // ライフ回復、最大値を超えない
        currentSpeed += baseSpeed / currentParts; // スピードを再計算
        Debug.Log($"Life recovered! Current life: {currentParts}");
        //UpdateUI();
    }
    private void GameOver()
    {
        Debug.Log("Game Over");
        currentSpeed = 0;
        SceneTransitionManager.instance.NextSceneButton(0);
        // ゲーム終了処理
    }
}
