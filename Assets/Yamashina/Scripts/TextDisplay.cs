using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextDisplay : MonoBehaviour
{
    [SerializeField]
    private TextAsset[] textAsset;   //メモ帳のファイル(.txt)　配列

    [SerializeField]
    private Text currentTextAsset;  //画面上の文字
    public List<Text> allTextBoxes = new();

    [SerializeField]
    private float TypingSpeed = 1.0f;  //文字の表示速度


    private int LoadText = 0;   //何枚目のテキストを読み込んでいるのか


    [SerializeField]
    private string customNewline = "[BR]"; // 改行として扱う文字列を指定

    [Header("次の文字が表示されるまでの時間")]
    [SerializeField]
    float TextSpeed = 0.1f;

    [SerializeField, Header("クリアのイメージが出てからシーン遷移するまでの時間")]

    private float clearToTransitionTime = 0.1f;

    private bool isTextFullyDisplayed = false; // 現在のテキストが完全に表示されたか

    private Coroutine TypingCroutine;  //コルーチンの管理

    float timer = 0;
    public bool IsTextFullyDisplayed()
    {
        return isTextFullyDisplayed; // メソッドを通じて状態を取得
    }
    //ゲームクリアパネル

    // Start is called before the first frame update
    void Start()
    {
        currentTextAsset.text = "";// 初期化
        foreach (Text atb in allTextBoxes)
        {
            atb.text = "";
        }

        UpdateText();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log($"isTextFullyDisplayedは{isTextFullyDisplayed}");

        if (Input.anyKeyDown)
        {
            if (!isTextFullyDisplayed)
            {
                DisplayFullText(); //テキスト全表示
            }
            else
            {
                if (LoadText < textAsset.Length - 1)
                {
                    LoadNextText(); // 次のテキストを表示
                    UpdateText();
                    return;
                }
                //Debug.Log(textAsset.Length);
                //GameMgr.ChangeState(GameState.Main);    //GameStateがMainに変わる
                SceneTransitionManager.instance.NextSceneButton(2); ; // 全てのテキストを読み終えたら閉じる


            }
        }
    }
    public void UpdateText()
    {
        if (TypingCroutine != null)
        {

            Debug.Log("Stopping previous TypingCoroutine");

            StopCoroutine(TypingCroutine);
        }

        Debug.Log($"TypingCroutineは{TypingCroutine}");

        //Debug.Log($"UpdateText: LoadText = {LoadText}");
        if (textAsset.Length > LoadText)
        {
            currentTextAsset.text = "";
            isTextFullyDisplayed = false;
            //Debug.Log($"Displaying text: {textAsset[LoadText].text}");
            //Debug.Log("Starting new TypingCoroutine");

            TypingCroutine = StartCoroutine(TextCoroutine());
        }
        else
        {
            //Debug.Log("全テキストが表示されました");
        }
    }
    IEnumerator TextCoroutine()
    {
        Debug.Log("TextCoroutine started");


        string currentText = textAsset[LoadText].text;

        if (!string.IsNullOrEmpty(customNewline))
        {
            currentText = currentText.Replace(customNewline, "\n");
        }

        for (int i = 0; i < currentText.Length; i++)   //テキストの中の文字を取得して、文字数を増やしていく
        {
            string currentChra = currentText.Substring(0, i); //現在の文字を所得する
            //Debug.Log($"Setting Text.text: {currentChra}");

            if (string.IsNullOrWhiteSpace(currentChra))
            {
                currentTextAsset.text = currentChra; //空白部分をそのまま設定する
                Debug.Log($"Text.text is now: {currentTextAsset.text}");

                yield return new WaitForSeconds(TextSpeed);
                continue;  //次のループへ

            }
            //テキストが進むたびにコルーチンが呼び出される
            //textAsset[LoadText].text.Lengthによって中のテキストデータの文字数の所得
            yield return new WaitForSeconds(TextSpeed); //指定された時間待機する

            currentTextAsset.text = currentChra;  //iが増えるたびに文字を一文字ずつ表示していく

        }

        isTextFullyDisplayed = true; //全ての文字が表示されたかを示すフラグ
        Debug.Log("TextCoroutine completed");

    }
    private void DisplayFullText()
    {
        if (TypingCroutine != null)
        {
            StopCoroutine(TypingCroutine); // コルーチンを停止
        }

        string fullText = textAsset[LoadText].text;

        if (!string.IsNullOrEmpty(customNewline))
        {

            fullText = fullText.Replace(customNewline, "\n");
        }
        Debug.Log($"Setting full text: {fullText}");

        // 現在のテキストをすべて表示
        currentTextAsset.text = fullText;
        Debug.Log($"Text.text after full display: {currentTextAsset.text}");

        isTextFullyDisplayed = true; // 完全表示状態にする
    }
    // 次のテキストを読み込む
    private void LoadNextText()
    {
        if (LoadText < textAsset.Length - 1)
        {
            LoadText++;
            currentTextAsset = allTextBoxes[LoadText];
            //UpdateText(); // 新しいテキストを表示ß
        }
        else
        {
            Debug.Log("最後のテキストです");
        }
    }

    // テキストエリアを閉じる

    //private void OnGUI()
    //{
    //    GUI.skin.label.fontSize = 30;  // 例えば30に設定

    //    GUI.Label(new Rect(1000.0f, 500.0f, Screen.width, Screen.height), isTextFullyDisplayed.ToString());
    //    //GUI.Label(new Rect(1000.0f, 1000.0f, Screen.width, Screen.height), TypingCroutine.ToString());
    //    if (TypingCroutine == null)
    //    {

    //    }

    //}
}