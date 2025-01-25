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
    private float currentSpeed; // ���݂̑��x
    [SerializeField, Tooltip("The current number of lives the character has.This decreases when the character hits obstacles")]
  
    private int currentParts; // ���݂̃��C�t��
    public Transform[] lanes; // ���[���̈ʒu
    private int currentLane = 1; // ���݂̃��[�� (0 = ��, 1 = ����, 2 = �E)

    [Header("UI Elements")]
    public Text lifeText; // UI�\���p
    public Text speedText; // UI�\���p

    void Start()
    {
        currentParts = maxParts; // �ő僉�C�t�ŃX�^�[�g
        currentSpeed = baseSpeed * currentParts; // �������x�ݒ�
        UpdateUI();
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
            currentSpeed = baseSpeed * currentParts; // �X�s�[�h���Čv�Z
            QuickTimeEvent(); // �N�C�b�N�^�C���C�x���g���Ăяo��
        }
        else
        {
            GameOver();
        }
        UpdateUI();
    }

    private void QuickTimeEvent()
    {
        // �N�C�b�N�^�C���C�x���g�̃��W�b�N (�����ł͊ȗ���)
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
        // �Q�[���I������
    }
}
