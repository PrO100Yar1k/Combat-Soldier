using UnityEngine;

public static class NavigationAI
{
    private static void FindPath(Vector3 startPos, Vector3 direction, float offsetScale)
    {
        Vector3 offsetDirection = new Vector3(direction.z, direction.y, direction.x) * offsetScale;

        for (int i = 0; i < Mathf.Infinity; i++)
        {
            Vector3 targetPos = startPos + offsetDirection;

            //if (Physics.Raycast())

        }

    }

    private static void GetWayFromWayPoint(Vector3 startPos)
    {
        for (int i = 0; i < 360; i++)
        {
            Vector3 direction = new Vector3(Mathf.Cos(i), startPos.y, Mathf.Sin(i));

            if (Physics.Raycast(startPos, direction, Mathf.Infinity))
            {

            }
        }
    }
}
