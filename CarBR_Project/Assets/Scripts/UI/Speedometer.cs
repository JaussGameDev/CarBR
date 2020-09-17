using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour
{
    public CarEngine engine;
    public Rigidbody _rigidbody;

    private float currSpeed;
    public float maxSpeed = 200f;

    private Transform needleTransform;
    private Transform speedLabelTemplateTransform;
    public Transform uiTransform;

    private const float MAX_NEEDLE_ROTATION = -90f;
    private const float ZERO_NEEDLE_ROTATION = 160f;

    private void Awake()
    {
        engine = GetComponentInParent<CarEngine>();
        SetMaxSpeed();
        _rigidbody = engine.GetRigidbody();
        needleTransform = transform.Find("needle");
        speedLabelTemplateTransform = transform.Find("speedLabelTemplate");
        speedLabelTemplateTransform.gameObject.SetActive(false);

        CreateSpeedLabels();
    }
    private void FixedUpdate()
    {
        GetSpeed();
        needleTransform.eulerAngles = new Vector3(needleTransform.eulerAngles.x, needleTransform.eulerAngles.y, GetSpeedRotation());
    }

    private void CreateSpeedLabels()
    {
        int labelAmount = (Mathf.RoundToInt(maxSpeed) / 20);
        float totalAngleSize = ZERO_NEEDLE_ROTATION - MAX_NEEDLE_ROTATION;

        for (int i = 0; i <= labelAmount; i++)
        {
            Transform speedLabelTransform = Instantiate(speedLabelTemplateTransform, transform);
            float labelSpeedNormalized = (float)i / labelAmount;
            // un peu louche ce qu'il ce passe juste en dessous
            float speedLabelAngle = -(ZERO_NEEDLE_ROTATION - labelSpeedNormalized * totalAngleSize) + 8f;
            speedLabelTransform.eulerAngles = new Vector3(0, 0, -(speedLabelAngle - 95f));
            speedLabelTransform.Find("speedText").GetComponent<Text>().text = Mathf.RoundToInt(labelSpeedNormalized * maxSpeed).ToString();
            speedLabelTransform.Find("speedText").eulerAngles = Vector3.zero;
            speedLabelTransform.gameObject.SetActive(true);
        }
    }

    public float GetSpeed()
    {
        currSpeed = engine.GetSpeed();
        return currSpeed;
    }

    public float GetSpeedRotation()
    {
        float totalAngleSize =  ZERO_NEEDLE_ROTATION - MAX_NEEDLE_ROTATION;
        float normalizeSpeed = GetSpeed() / maxSpeed;
        normalizeSpeed = Mathf.Clamp(normalizeSpeed, 0f, 1.5f);
        return ZERO_NEEDLE_ROTATION - normalizeSpeed * totalAngleSize;
    }

    public void SetMaxSpeed()
    {
        maxSpeed = engine.GetMaxSpeed();
    }


}
