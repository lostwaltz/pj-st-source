using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageHUD : MonoBehaviour
{
    [Header("메인 세팅")]
    public float lifetime = 2f;


    [Header("캔버스 세팅")]
    public bool consistentScreenSize = true;
    public float baseDistance = 15f;
    public float closeDistance = 5f;
    public float farDistance = 50f;
    public float closeScale = 2f;
    public float farScale = 0.5f;

    [Header("FadeInOut 세팅")]
    public float fadeInduration = 0.2f;
    public float fadeOutDuration = 0.2f;
    public Vector2 fadeInScale = new Vector2(2f, 2f);
    public Vector2 fadeOutScale = new Vector2(1f, 1f);

    [Header("Movement 세팅")]
    public float lerpSpeed = 5f;
    public Vector2 offsetXRange = new Vector2(-0.4f, 0.4f);
    public Vector2 offsetYRange = new Vector2(0.6f, 1f);

    private TextMeshPro textMeshPro;
    private Transform targetCamera;
    private Vector3 targetPosition;
    private Vector3 startPosition;
    private float currentFade;
    private float startTime;
    private Vector3 originalScale;

    [SerializeField] private Vector3 maxScale;
    [SerializeField] private Vector3 minScale;

    private float currentDamage = 0f;
    private float targetDamage = 0f;

    private void Awake()
    {
        textMeshPro = GetComponent<TextMeshPro>();
        targetCamera = Camera.main.transform;
        originalScale = transform.localScale;
    }

    private Vector3 GetRandomOffset()
    {
        return new Vector3(
            Random.Range(offsetXRange.x, offsetXRange.y),
            Random.Range(offsetYRange.x, offsetYRange.y),
            0f
        );
    }

    public void Initialize(Vector3 position, int damage)
    {
        startTime = Time.time;
        currentFade = 0f;
        currentDamage = damage;
        targetDamage = damage;

        startPosition = position;
        targetPosition = position + GetRandomOffset();
        transform.position = startPosition;

        textMeshPro.text = damage.ToString();
        transform.localScale = ClampScale(Vector3.zero);
        transform.rotation = targetCamera.rotation;

        gameObject.SetActive(true);
    }

    public void SetDamage(float damage)
    {
        currentDamage = damage;
        textMeshPro.text = Mathf.RoundToInt(damage).ToString();
    }

    private void Update()
    {
        float timeSinceStart = Time.time - startTime;
        if (timeSinceStart <= fadeInduration)
        {
            float t = timeSinceStart / fadeInduration;
            currentFade = t;
            Vector3 scale = Vector3.Lerp(Vector3.zero, originalScale, t);
            transform.localScale = ClampScale(scale);
        }
        else if (timeSinceStart >= lifetime - fadeOutDuration)
        {
            float t = (lifetime - timeSinceStart) / fadeOutDuration;
            currentFade = t;
            Vector3 scale = Vector3.Lerp(Vector3.zero, originalScale, t);
            transform.localScale = ClampScale(scale);

            if (timeSinceStart >= lifetime)
            {
                Core.ObjectPoolManager.ReleaseObject("DamageHUD", this);
                return;
            }
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * lerpSpeed);

        if (consistentScreenSize)
        {
            float distance = Vector3.Distance(transform.position, targetCamera.position);
            float scaleFactor = distance / baseDistance;

            if (distance < closeDistance)
            {
                scaleFactor *= closeScale;
            }
            else if (distance > farDistance)
            {
                scaleFactor *= farScale;
            }
            else
            {
                float t = (distance - closeDistance) / (farDistance - closeDistance);
                scaleFactor *= Mathf.Lerp(closeScale, farScale, t);
            }

            transform.localScale = ClampScale(originalScale * scaleFactor * currentFade);
        }

        transform.rotation = targetCamera.rotation;
    }

    Vector3 ClampScale(Vector3 expected)
    {
        expected.x = Mathf.Clamp(expected.x, minScale.x, maxScale.x);
        expected.y = Mathf.Clamp(expected.y, minScale.y, maxScale.y);
        expected.z = Mathf.Clamp(expected.z, minScale.z, maxScale.z);

        return expected;
    }

}


