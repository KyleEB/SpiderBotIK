using UnityEngine;

public class CreatureController : MonoBehaviour
{
    public float movementAcceleration = 5f;
    public Vector3 localVelocity;
    public Vector3 localInputVelocity;
    public Vector3 worldVelocity; 

    public float walkSpeed = 2f;
    public float sprintSpeed = 5f;
    public float rotationAcceleration = 10f;
    public float rotationSpeed = 10f;
    private float deltaYRotation = 0;

    public ProceduralLegPlacement[] legs;
    private int currentLegIndex;
    public float maxStepLength = 1f;
    public float lastStepTime = 0;

    void Update()
    {
        float moveSpeed = (Input.GetButton("Fire3") ? sprintSpeed : walkSpeed);
        localInputVelocity = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")).normalized;
        localVelocity = Vector3.MoveTowards(localVelocity, localInputVelocity, movementAcceleration);
        worldVelocity = Camera.main.transform.TransformDirection(localVelocity);

        deltaYRotation = Mathf.MoveTowards(deltaYRotation, Input.GetAxis("Turn") * rotationSpeed, rotationAcceleration * Time.deltaTime);

        transform.Rotate(new Vector3(0f, deltaYRotation, 0f));
        transform.position += worldVelocity * moveSpeed * Time.deltaTime;

      
        float stepDuration = maxStepLength / Mathf.Max(moveSpeed * localVelocity.magnitude, 
                                             Mathf.Abs(deltaYRotation * Mathf.Deg2Rad));

        if (Time.time > lastStepTime + (stepDuration / legs.Length) && legs != null)
        {
            if (legs[currentLegIndex] != null)
            {
                legs[currentLegIndex].stepDuration = stepDuration;
                legs[currentLegIndex].worldVelocity = worldVelocity;
                legs[currentLegIndex].Step();
                lastStepTime = Time.time;
                currentLegIndex = (currentLegIndex + 1) % legs.Length;
            }
        }
    }
}