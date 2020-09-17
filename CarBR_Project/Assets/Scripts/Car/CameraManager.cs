using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject focus;
    public float distance = 7f;
    public float height = 4f;
    public float dampening = 1f;

    // Update is called once per frame
    /// <summary>
    /// la camera bouge lorque qu'elle est avec le joueur
    /// </summary>

    private void Start()
    {
    }
    void FixedUpdate()
    {
        if (Vector3.Dot(focus.transform.up, Vector3.down) > 0)
        {
            transform.position = Vector3.Lerp(transform.position, focus.transform.position + focus.transform.TransformDirection(new Vector3(0f, -height - 1f, distance)), 0.7f * dampening * Time.deltaTime);
            transform.LookAt(focus.transform);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, focus.transform.position + focus.transform.TransformDirection(new Vector3(0f, height, distance)), dampening * Time.deltaTime);
            transform.LookAt(focus.transform);
        }

    }
}
