using System.Collections.Generic;
using UnityEngine;

public class TrenchController : MonoBehaviour
{
    [SerializeField] private Transform _trenchParent = default;
    [SerializeField] private TrenchUnit _trenchUnitPrefab = default;

    private readonly HashSet<Vector3Int> _occupiedPositions = new HashSet<Vector3Int>();

    private const int unitCount = 300;
    private const float unitSpacing = 1f;

    private const int branchingChance = 20;
    private const int maxBranchingAttempts = 50;

    private readonly Vector3[] DirectionVectors =
    {
        new Vector3(0, 0, 1), // up
        new Vector3(1, 0, 0), // right
        new Vector3(0, 0, -1), // down
        new Vector3(-1, 0, 0) // left
    };

    private void Start() //
    {
        Vector3 startPosition = Vector3.zero;
        int randomDirection = GetRandomDirection();

        GenerateTrench(startPosition, randomDirection);
    }

    private void GenerateTrench(Vector3 startPosition, int directionIndex) // to do
    {
        Vector3 targetPosition = startPosition;

        int targetDirection = directionIndex; // setup random direction only in the start

        for (int i = 0; i < unitCount; i++)
        {
            int currentBranchingAttempt = 0;
            int randomBranchingNumber = Random.Range(0, 100);

            bool isBranchingCondition = randomBranchingNumber < branchingChance;

            if (isBranchingCondition == true)
            {
                int nextDirection = GetClosestDirection(targetDirection, 1);
                int previousDirection = GetClosestDirection(targetDirection, -1);

                float randomDirectionChance = Random.Range(0, 2);

                targetDirection = randomDirectionChance == 0 ? nextDirection : previousDirection;
            }

            if (i % 110 == 0) directionIndex = GetClosestDirection(directionIndex, 1); //

            targetDirection = !isBranchingCondition ? directionIndex : targetDirection; // always save base direction of trench

            targetPosition += DirectionVectors[targetDirection] * unitSpacing;
            Vector3Int gridPos = Vector3Int.RoundToInt(targetPosition);

            while (_occupiedPositions.Contains(gridPos) && currentBranchingAttempt < maxBranchingAttempts)
            {
                //Debug.Log($"Number: {i - 1} | Cannot branch to {targetDirection} | Attempt: {currentBranchingAttempt}");

                targetDirection = GetRandomDirection();
                targetPosition += DirectionVectors[targetDirection] * unitSpacing;

                gridPos = Vector3Int.RoundToInt(targetPosition);
                currentBranchingAttempt++;
            }

            if (currentBranchingAttempt >= maxBranchingAttempts)
                break;

            //if (currentBranchingAttempt > 0)
                //Debug.Log($"Number: {i - 1} | Succefully branched to {targetDirection}");

            _occupiedPositions.Add(gridPos);

            TrenchUnit trenchUnit = Instantiate(_trenchUnitPrefab, targetPosition, Quaternion.identity);

            trenchUnit.transform.SetParent(_trenchParent);
            trenchUnit.gameObject.name = $"Unit - Trench {i}";
        }
    }

    private int GetClosestDirection(int directionIndex, int movementPosition)
    {
        int total = DirectionVectors.Length;
        return (total + directionIndex + movementPosition) % total;
    }

    private int GetRandomDirection()
    {
        return Random.Range(0, DirectionVectors.Length);
    }
}
