using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : SingletonMonoBehaviour<GameManager> {
    [SerializeField] private Text time;

    public enum Judge {
        NULL,
        Wrong,
        Safe,
        Fine,
        finE,
        Cool,
    }

    [SerializeField] private float baseFlame = 30f; //基本フレーム
    [SerializeField] private float bufferFlame = 15f; //前後フレーム
    [SerializeField] private float spawnSpan = 3f; //生成間隔
    [SerializeField] private int max = 5; //生成回数
    private int count = 0;
    [SerializeField] private float currentFlame = 0; //現在フレーム
    [SerializeField] private float fps = 0;

    // Use this for initialization
    void Start() {
        StartCoroutine(Spawn());

    }

    // Update is called once per frame
    void Update() {
        fps = 1f / Time.deltaTime;
        Debug.Log(Inputting());
    }

    private bool isSpawn = false;
    private IEnumerator Spawn() {
        if (isSpawn) { yield break; }
        count = max;
        isSpawn = true;
        while (0 < count) {
            count--;
            timer = StartCoroutine(Timer());
            yield return new WaitForSeconds(spawnSpan);
        }
        isSpawn = false;
    }

    [SerializeField] private GameObject note;
    Coroutine timer = null;
    private IEnumerator Timer() {
        currentFlame = baseFlame + bufferFlame * 2;
        note.SetActive(true);
        while (0 < currentFlame) {
            currentFlame--;
            yield return null;
        }
        note.SetActive(false);
        currentFlame = 0;
    }

    private Judge Inputting() {
        if (currentFlame <= 0 || baseFlame + bufferFlame * 2 <= currentFlame) { return Judge.NULL; }
        if (Input.GetKeyDown(KeyCode.Space)) {
            float tmp = (baseFlame + bufferFlame * 2 - currentFlame) / fps;
            time.text = tmp.ToString();
            Debug.Log(currentFlame);
            StopCoroutine(timer);
            note.SetActive(false);
            timer = null;
            if (0 < currentFlame && currentFlame < bufferFlame) {
                return Judge.Fine;
            } else if (bufferFlame <= currentFlame && currentFlame <= baseFlame + bufferFlame) {
                return Judge.Cool;
            } else if (baseFlame + bufferFlame < currentFlame && currentFlame < baseFlame + bufferFlame * 2) {
                return Judge.finE;
            } else {
                Debug.Log(currentFlame);
                return Judge.Safe;
            }
        }
        return Judge.Wrong;
    }
}
