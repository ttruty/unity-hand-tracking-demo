using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGrabbingBehaviour : OVRGrabber
{
    private OVRHand m_hand;
    private float pinchThreshold = 0.7f;

    protected override void Start()
    {
        base.Start();
        m_hand = GetComponent<OVRHand>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        CheckIndexPinch();
    }

    void CheckIndexPinch()
    {
        float pinchStrength = m_hand.GetFingerPinchStrength(OVRHand.HandFinger.Index);
        bool isPinching = pinchStrength > pinchThreshold;
        Debug.Log(isPinching);
        if (!m_grabbedObj && isPinching && m_grabCandidates.Count > 0)
        {
            GrabBegin();
            Debug.Log("GRABBING");
        }
        else if (m_grabbedObj && !(pinchStrength > pinchThreshold))
        {
            GrabEnd();
        }
    }

    protected override void GrabEnd()
    {
        if (m_grabbedObj)
        {
            int multiplier = m_grabbedObj.GetComponent<CustomGrabbable>().velocityMultiplier;
            Vector3 linearVelocity = (multiplier * (transform.position - m_lastPos)) / Time.fixedDeltaTime;
            Vector3 angularVelocity = (multiplier * (transform.eulerAngles - m_lastRot.eulerAngles)) / Time.fixedDeltaTime;

            GrabbableRelease(linearVelocity, angularVelocity);
            
        }

        GrabVolumeEnable(true);
    }
}