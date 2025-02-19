using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField, Tooltip("The base speed of the character. This value serves as the basis for other speed calculations.")]
    private float baseSpeed = 20f; // 基本速度
    [SerializeField, Tooltip("The current speed of the character, dynamically adjusted during gameplay.")]
    private int maxLives = 3; // 最大ライフ数
    // Movement style
    public bool LANECHANGER = true;

    [Header("Runtime Values")]
    [SerializeField, Tooltip("The current speed of the character, dynamically adjusted during gameplay.")]
    public static float currentSpeed; // 現在の速度
    [SerializeField, Tooltip("The current number of lives the character has.This decreases when the character hits obstacles")]

    public static int currentParts; // 現在のライフ数
    public Transform[] lanes; // レーンの位置
    private int currentLane = 1; // 現在のレーン (0 = 左, 1 = 中央, 2 = 右)
    [Header("Goal Settings")]
    public Transform goalTransform; // ゴール地点
    public float goalRadius = 1.0f; // ゴール地点の範囲
    public float timeLimit = 30.0f; // 制限時間 (秒)

    [SerializeField, Header("elapsedTimeText")] TextMeshProUGUI elapsedTimeText;
    private float elapsedTime = 0.0f; // 経過時間
    private bool isGoalReached = false; // ゴール判定
    private bool isGameOver = false;
    private bool isJumping = false;
    //[Header("UI Elements")]
    //public Text lifeText; // UI表示用
    //public Text speedText; // UI表示用
    public List<ParticleSystem> bubbleBodies = new List<ParticleSystem>(); // バブルのリスト
    public List<int> bubbleBodyMaxEmissionRates = new List<int>();
    private bool canUpdate = false;

    void Start()
    {
        currentParts = maxLives; // 最大ライフでスタート
        currentSpeed = baseSpeed;
        //UpdateUI
        bubbleBodies.AddRange(GetComponentsInChildren<ParticleSystem>());
        canUpdate = false;
        foreach (ParticleSystem bubbles in bubbleBodies)
        {
            bubbleBodyMaxEmissionRates.Add((int)bubbles.emission.rateOverTime.constant);
        }

        // Wait n seconds before starting to run
        StartCoroutine(DelayStart(3f));
    }
    IEnumerator DelayStart(float delay)
    {
        yield return new WaitForSeconds(delay);
        canUpdate = true;
    }

    void Update()
    {
        //Debug.Log($"isGoalReached flag :{isGoalReached}");
        // escape if before n seconds
        if (!canUpdate)
        {
            return;
        }
        // 経過時間を表示
        float remainingTime = timeLimit - elapsedTime;
        elapsedTimeText.text = "Time Left: " + Mathf.Max(remainingTime, 0).ToString("F2") + "s";  // 小数点以下2桁で表示

        if (!isGoalReached) // ゴールに到達していない場合のみ進行
        {
            // キャラクターを前方に移動
            // Always go forward!
            transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);

            // Two movement options 
            if (LANECHANGER)
            {
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
            else
            {
                float move = Input.GetAxis("Horizontal") * 20.0f * Time.deltaTime;
                transform.Translate(move, 0, 0);
            }

            if (transform.position.y < -1f ^ transform.position.x > 7.2f ^ transform.position.x < -7.2f)
            {
                GameOver(); // Dont go through the floor
            }

            if (currentParts <= 0)
            {
                currentParts = 1; // 最低でも1にする
            }

            Jump();

            // ゴール判定
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
            Debug.Log("Jump!");
            if (!isJumping)
            {
                MultiAudio.ins.PlaySEByName("JumpSound");
                GetComponentInChildren<Rigidbody>().AddForce(new Vector2(0, 5), ForceMode.Impulse);
                isJumping = true;
            }
            StartCoroutine(ResetJump());
        }
    }
    IEnumerator ResetJump()
    {
        yield return new WaitForSeconds(2);
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
        if (other.CompareTag("goal"))
        {
            GoalReached();
        }

        if (other.CompareTag("Obstacle")) //player Obstacle hit
        {
            HitObstacle();
            other.enabled = false;
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
            MultiAudio.ins.PlaySEByName("ObstacleCollide");
            currentSpeed -= baseSpeed / maxLives; // スピードを再計算 
            SetNewBubbleDensity();
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
        //Debug.Log("Goal Reached!");
        SceneTransitionManager.instance.NextSceneButton(3);

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
        currentParts = Mathf.Clamp(currentParts + amount, 0, maxLives); // ライフ回復、最大値を超えない
        currentSpeed += baseSpeed / currentParts; // スピードを再計算
        Debug.Log($"Life recovered! Current life: {currentParts}");
        MultiAudio.ins.PlaySEByName("PickUpSpeechBubblePlus");

        //bubbleDensity += (300 / 5);

        //if (bubbleDensity > 300)
        //{
        //    bubbleDensity = 300;
        //}

        SetNewBubbleDensity();

        //UpdateUI();
    }
    private void GameOver()
    {
        if(isGameOver) return;
        isGameOver = true;
        Debug.Log("Game Over");
        MultiAudio.ins.PlaySEByName("GameOverSound");
        currentSpeed = 0;
        SceneTransitionManager.instance.NextSceneButton(0);
        // ゲーム終了処理
    }

    public void SetNewBubbleDensity()
    {
        foreach (ParticleSystem bubble in bubbleBodies)
        {
            var emission = bubble.emission;
            float newEmissionRate = bubbleBodyMaxEmissionRates[bubbleBodies.IndexOf(bubble)] * (currentParts / maxLives);
            Debug.Log(newEmissionRate);
            emission.rateOverTime = newEmissionRate;
        }
    }
}
