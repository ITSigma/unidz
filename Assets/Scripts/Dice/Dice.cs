using System;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    private new Rigidbody rigidbody;
    private IReadOnlyDictionary<int, Func<Vector3>> faceNormalsByScore;

    private const float DiceFaceEpsilon = 0.6f;
    private const float StopCheckingEpsilon = 0.001f;

    public bool IsRolling => rigidbody.linearVelocity.sqrMagnitude > StopCheckingEpsilon
                             || rigidbody.angularVelocity.sqrMagnitude > StopCheckingEpsilon;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        faceNormalsByScore = new Dictionary<int, Func<Vector3>>
        {
            { 1, () => -rigidbody.transform.forward },
            { 2, () => rigidbody.transform.right },
            { 3, () => rigidbody.transform.up },
            { 4, () => -rigidbody.transform.up },
            { 5, () => -rigidbody.transform.right },
            { 6, () => rigidbody.transform.forward }
        };
    }

    public void Roll(Vector3 forceVector, Vector3 torqueVector)
    {
        rigidbody.AddForce(forceVector, ForceMode.Impulse);
        rigidbody.AddTorque(torqueVector, ForceMode.Impulse);
    }

    public int GetValue()
    {
        foreach (var (score, normalVectorFunc) in faceNormalsByScore)
        {
            var scalar = Vector3.Dot(normalVectorFunc(), Vector3.up);
            if (scalar > DiceFaceEpsilon)
            {
                return score;
            }
        }

        return 0;
    }
}