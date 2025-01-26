using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField, Tooltip("The base speed of the character. This value serves as the basis for other speed calculations.")]
    private float baseSpeed = 20f; // ��{���x
    [SerializeField, Tooltip("The current speed of the character, dynamically adjusted during gameplay.")]
    private int maxLives = 3; // �ő僉�C�t��
    // Movement style
    public bool LANECHANGER = true;

    [Header("Runtime Values")]
   [SerializeField,Tooltip("The current speed of the character, dynamically adjusted during gameplay.")]
    public static float currentSpeed; // ���݂̑��x
    [SerializeField, Tooltip("The current number of lives the character has.This decreases when the character hits obstacles")]

    public static int currentParts; // ���݂̃��C�t��
    public Transform[] lanes; // ���[���̈ʒu
    private int currentLane = 1; // ���݂̃��[�� (0 = ��, 1 = ����, 2 = �E)
    [Header("Goal Settings")]
    public Transform goalTransform; // �S�[���n�_
    public float goalRadius = 1.0f; // �S�[���n�_�͈̔�
    public float timeLimit = 30.0f; // �������� (�b)

    [SerializeField, Header("elapsedTimeText")] TextMeshProUGUI elapsedTimeText;
    private float elapsedTime = 0.0f; // �o�ߎ���
    private bool isGoalReached = false; // �S�[������

    private bool isJumping = false;
    //[Header("UI Elements")]
    //public Text lifeText; // UI�\���p
    //public Text speedText; // UI�\���p
    public List<ParticleSystem> bubbleBodies = new List<ParticleSystem>(); // �o�u���̃��X�g
    public List<int> bubbleBodyMaxEmissionRates = new List<int>();
    private bool canUpdate = false;

    void Start()
    {
        currentParts = maxLives; // �ő僉�C�t�ŃX�^�[�g
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
        if(!canUpdate)
        {
            return;
        }
        // �o�ߎ��Ԃ�\��
        float remainingTime = timeLimit - elapsedTime;
        elapsedTimeText.text = "Time Left: " + Mathf.Max(remainingTime, 0).ToString("F2") + "s";  // �����_�ȉ�2���ŕ\��

        if (!isGoalReached) // �S�[���ɓ��B���Ă��Ȃ��ꍇ�̂ݐi�s
        {
            // �L�����N�^�[��O���Ɉړ�
            // Always go forward!
            transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);

            // Two movement options 
            if (LANECHANGER)
            {
                // ���[���̐؂�ւ�
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

            if (transform.position.y < -1f ^ transform.position.x > 7.2f ^ transform.position.x < -7.2f )
            {
                GameOver(); // Dont go through the floor
            }

            if (currentParts <= 0)
            {
                currentParts = 1; // �Œ�ł�1�ɂ���
            }

            Jump();

            // �S�[������
            elapsedTime += Time.deltaTime;

            // �v���C���[���S�[���n�_�ɓ��B���Ă��邩����
            float distanceToGoal = Vector3.Distance(transform.position, goalTransform.position);
            if (distanceToGoal <= goalRadius && elapsedTime <= timeLimit)
            {
                GoalReached(); // �S�[�����菈��
            }

            // ���Ԑ؂�̃`�F�b�N
            if (elapsedTime > timeLimit)
            {
                TimeOut(); // �^�C���A�E�g����
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
            currentSpeed -= baseSpeed / maxLives; // �X�s�[�h���Čv�Z 
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
        isGoalReached = true; // �S�[�����B�t���O
        //Debug.Log("Goal Reached!");
        SceneTransitionManager.instance.NextSceneButton(3);

        // �S�[���B�����̏����������ɒǉ�
    }

    private void TimeOut()
    {
        Debug.Log("Time Out! Game Over.");
        GameOver();
        // ���Ԑ؂ꎞ�̏����������ɒǉ�

    }

    //private void QuickTimeEvent()
    //{
    //    // �N�C�b�N�^�C���C�x���g�̃��W�b�N (�����ł͊ȗ���)
    //    Debug.Log("QuickTime Event Triggered!");
    //}

    //private void UpdateUI()
    //{
    //    lifeText.text = "Life: " + currentParts;
    //    speedText.text = "Speed: " + currentSpeed.ToString("F1");
    //}
    public void RecoverLife(int amount)
    {
        currentParts = Mathf.Clamp(currentParts + amount, 0, maxLives); // ���C�t�񕜁A�ő�l�𒴂��Ȃ�
        currentSpeed += baseSpeed / currentParts; // �X�s�[�h���Čv�Z
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
        Debug.Log("Game Over");
        MultiAudio.ins.PlaySEByName("GameOverSound");
        currentSpeed = 0;
        SceneTransitionManager.instance.NextSceneButton(0);
        // �Q�[���I������
    }

    public void SetNewBubbleDensity()
    {
        foreach (ParticleSystem bubble in bubbleBodies)
        {
            var emission = bubble.emission;
            float newEmissionRate = bubbleBodyMaxEmissionRates[bubbleBodies.IndexOf(bubble)] * (currentParts / maxLives );
            Debug.Log(newEmissionRate);
            emission.rateOverTime = newEmissionRate;
        }
    }
}
