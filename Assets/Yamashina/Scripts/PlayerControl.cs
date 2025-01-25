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
    private float baseSpeed = 10f; // ��{���x
    [SerializeField, Tooltip("The current speed of the character, dynamically adjusted during gameplay.")]
    private int maxParts = 6; // �ő僉�C�t��

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

    private float elapsedTime = 0.0f; // �o�ߎ���
    private bool isGoalReached = false; // �S�[������

    private bool isJumping = false;
    //[Header("UI Elements")]
    //public Text lifeText; // UI�\���p
    //public Text speedText; // UI�\���p

    void Start()
    {
        currentParts = maxParts; // �ő僉�C�t�ŃX�^�[�g
        currentSpeed = baseSpeed;
        //UpdateUI();
    }
    void Update()
    {
        // �L�����N�^�[��O���Ɉړ�
        transform.Translate(Vector3.forward *currentSpeed * Time.deltaTime);

        // ���[���̐؂�ւ�
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
            currentParts = 1; // �Œ�ł�1�ɂ���
        }

        Jump();

        if (!isGoalReached)
        {
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
            currentSpeed -= baseSpeed / currentParts; // �X�s�[�h���Čv�Z
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
        Debug.Log("Goal Reached!");
        SceneTransitionManager.instance.NextSceneButton(2);

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
        currentParts = Mathf.Clamp(currentParts + amount, 0, maxParts); // ���C�t�񕜁A�ő�l�𒴂��Ȃ�
        currentSpeed += baseSpeed / currentParts; // �X�s�[�h���Čv�Z
        Debug.Log($"Life recovered! Current life: {currentParts}");
        //UpdateUI();
    }
    private void GameOver()
    {
        Debug.Log("Game Over");
        currentSpeed = 0;
        SceneTransitionManager.instance.NextSceneButton(0);
        // �Q�[���I������
    }
}
