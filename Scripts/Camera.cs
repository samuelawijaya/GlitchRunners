using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] private Transform player1;
    [SerializeField] private Transform player2;
    [SerializeField] private float followSpeed = 5f;
    [SerializeField] private Vector2 cameraOffset;
    [SerializeField] private float yThreshold = 0f;  // Y-level that separates top and bottom halves

    private void LateUpdate()
    {
        Vector3 midpoint = (player1.position + player2.position) / 2f;

        // Determine which half of the loop we're on
        bool isTopHalf = midpoint.y > yThreshold;

        // Determine leader based on which direction is 'forward' in this half
        Transform leader;
        if (isTopHalf)
        {
            // On top half: +X means moving right
            leader = player1.position.x > player2.position.x ? player1 : player2;
        }
        else
        {
            // On bottom half: -X means moving left
            leader = player1.position.x < player2.position.x ? player1 : player2;
        }

        // Blend midpoint and leader position
        Vector3 targetPos = midpoint + new Vector3(cameraOffset.x, cameraOffset.y, -10f);
        targetPos.x = Mathf.Lerp(midpoint.x, leader.position.x, 0.5f);

        // Smooth follow
        transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
    }
}
