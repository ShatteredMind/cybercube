using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowConfetti : MonoBehaviour
{
    [SerializeField] private Transform[] pfConfettis;

    private List<Confetti> confettis;
    private float spawnTimer;
    private const float timerMax = 0.05f;
    private void Awake()
    {
        confettis = new List<Confetti>();
        SpawnConfetti();
    }

    private void Update()
    {
        foreach (Confetti confetti in confettis)
        {
            confetti.Update();
        }
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            spawnTimer += timerMax;
            int spawnAmount = Random.Range(1, 7);
            for (int i = 0; i < spawnAmount; i++)
            {
                SpawnConfetti();
            }
        } 
    }

    private void SpawnConfetti()
    {
        float width = transform.GetComponent<RectTransform>().rect.width;
        float height = transform.GetComponent<RectTransform>().rect.height;
        Vector2 anchoredPosition = new Vector2(Random.Range(-width / 2f, width / 2f), height / 2);
        Transform pfConfetti = pfConfettis[Random.Range(0, pfConfettis.Length - 1)];
        Confetti confetti = new Confetti(pfConfetti, transform, anchoredPosition);
        confettis.Add(confetti);
    }

    private class Confetti
    {
        public Transform transform;
        private RectTransform rectTransform;
        private Vector2 anchoredPosition;
        private Vector2 moveAmount;
        private Vector3 euler;
        private float eulerSpeed;
        public Confetti(Transform prefab, Transform container, Vector2 anchoredPosition)
        {
            this.anchoredPosition = anchoredPosition;
            transform = Instantiate(prefab, container);
            rectTransform = transform.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = anchoredPosition;

            rectTransform.sizeDelta *= Random.Range(.8f, 1.2f);
            euler = new Vector3(0, 0, Random.Range(0, 360));
            eulerSpeed = Random.Range(300f, 400f);
            eulerSpeed *= Random.Range(0, 2) == 0 ? 1f : -1f;
            transform.localEulerAngles = euler;

            moveAmount = new Vector2(0, Random.Range(-150f, -350f));
        }

        public void Update()
        {
            anchoredPosition += moveAmount * Time.deltaTime;
            rectTransform.anchoredPosition = anchoredPosition;

            euler.z += eulerSpeed * Time.deltaTime;
            transform.localEulerAngles = euler;
        }
    }
}
