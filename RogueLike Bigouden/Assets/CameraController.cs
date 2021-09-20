using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Controller controls;
    public GameObject Player;
    
    public Transform player;
    private Vector3 target, mousePos, refVel, shakeOffset, shakeVector;
    private float smoothTime = 0.2f, cameraDist = 0.2f, zStart;
    private float shakeMag, shakeTimeEnd;
    private bool shaking;
    
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        player = Player.transform;
        target = player.position;
        zStart = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = CaptureMousePos();
        shakeOffset = UpdateShake();
        target = UpdateTargetPos();
        UpdateCameraPosition();
    }
    
    Vector3 CaptureMousePos()
    {
        Vector2 ret = player.GetChild(1).transform.position;
        ret *= 2f;
        ret -= Vector2.one;
        float max = 1.3f;
        if (Mathf.Abs(ret.x) > max || Mathf.Abs(ret.y) > max)
            ret = ret.normalized;
        return ret;
    }

    Vector3 UpdateTargetPos()
    {
        Vector3 mouseOffset = mousePos * cameraDist;
        Vector3 ret = player.position + mouseOffset;
        ret += shakeOffset; 
        ret.z = zStart;
        return ret;
    }

    private void UpdateCameraPosition()
    {
        Vector3 tempPos;
        tempPos = Vector3.SmoothDamp(transform.position, target, 
            ref refVel, smoothTime);
        transform.position = tempPos;
    }
    
    private Vector3 UpdateShake()
    {
        if (!shaking || Time.time > shakeTimeEnd)
        {
            shaking = false; 
            return Vector3.zero; 
        }
        Vector3 tempOffset = shakeVector;
        tempOffset *= shakeMag;
        return tempOffset;
    }
}
