using UnityEngine;

public class ProceduralLegPlacement : MonoBehaviour
{
    public Vector3 initialRestingPosition;

    public Transform mainBody;

    public Vector3 restingPosition
    {
        get
        {
            return mainBody.TransformPoint(initialRestingPosition);
        }
    }

    public Vector3 worldVelocity;

    public Vector3 worldTarget = Vector3.zero;
    private Vector3 lastHitPoint;
    public Transform ikTarget;
    public Transform ikPoleTarget;

    public LayerMask solidLayer;
    public AnimationCurve stepHeightCurve;
    public float stepHeightMultiplier = 1f;
    public float stepDuration;
    public float lastStep;


    private void Start()
    {
        Step();
    }

    public void Update()
    {
        UpdateIkTarget();
    }

    public void UpdateIkTarget()
    {
        float percent = Mathf.Clamp01((Time.time - lastStep) / stepDuration);
        ikTarget.position = Vector3.Lerp(ikTarget.position, worldTarget, percent) 
            + mainBody.transform.up * stepHeightCurve.Evaluate(percent) * stepHeightMultiplier;
    }

    public void Step()
    {
        RaycastHit hit;
        if (Physics.Raycast(restingPosition + mainBody.transform.up, -mainBody.transform.up, out hit, solidLayer))
        {
            worldTarget = hit.point + worldVelocity;
            lastHitPoint = hit.point;
        }
        else
        {
            worldTarget = lastHitPoint;
        }
        lastStep = Time.time;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(restingPosition, worldTarget);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(restingPosition, 0.1f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(worldTarget, 0.1f);
    }
}