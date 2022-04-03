using UnityEngine;

public class ForcedHover : MonoBehaviour
{
    public float forcedHeight = 1;
    public LayerMask solidLayer;
    public CreatureController creatureController;
    
    private RaycastHit lastHit;

    private Vector3 desiredPosition;

    // Update is called once per frame
    private void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, -transform.up, out hit, solidLayer))
        {
            if (transform.up != hit.normal)
            {
                transform.up = hit.normal;
            }

            lastHit = hit;
            desiredPosition = hit.point + transform.up * forcedHeight;
            transform.position = desiredPosition;
        }
        else
        {
            transform.position = lastHit.point + transform.up * forcedHeight;
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, lastHit.point);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, desiredPosition);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.1f);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(lastHit.point, 0.1f);
    }
}