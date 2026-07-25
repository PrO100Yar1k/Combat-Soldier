using System;
using UnityEngine;
using Assets.App.Scripts;
using System.Collections.Generic;
using Assets.App.Scripts.Infrastructure.Interfaces;

public class TrenchLineController : MonoBehaviour, ITrenchFactory
{
    [SerializeField] private Transform _trenchParent = default;
    [SerializeField] private TrenchUnit _trenchUnitPrefab = default;

    [SerializeField, Space(3)] private Direction _targetDirection = default;

    private readonly HashSet<Vector3Int> _occupiedPositions = new HashSet<Vector3Int>();
    private readonly Dictionary<Direction, Vector3> _directionVectors = new() {
        { Direction.Up,    new Vector3(0, 0, 1)  },
        { Direction.Right, new Vector3(1, 0, 0)  },
        { Direction.Down,  new Vector3(0, 0, -1) },
        { Direction.Left,  new Vector3(-1, 0, 0) }
    };

    private const float _unitSpacing = 1f;

    private const int _branchingChance = 20;
    private const int _maxBranchingAttempts = 25;

    private void Awake() // to do
    {
        CreateTrench();
    }

    public void CreateTrench() // to do
    {
        const int unitCount = 20;
        Vector3 startPosition = transform.position;

        GenerateTrench(startPosition, _targetDirection, unitCount);
    }

    private void GenerateTrench(Vector3 startPosition, Direction baseDirection, int unitCount)
    {
        Vector3 currentPosition = startPosition;
        Direction currentDirection = baseDirection;

        _occupiedPositions.Add(Vector3Int.RoundToInt(currentPosition));

        for (int i = 0; i < unitCount; i++)
        {
            int randomBranchingNumber = UnityEngine.Random.Range(0, 100);
            bool shouldBranchChance = randomBranchingNumber < _branchingChance;

            if (shouldBranchChance) {
                Direction nextDir = GetClosestDirection(currentDirection, 1);
                Direction prevDir = GetClosestDirection(currentDirection, -1);

                currentDirection = UnityEngine.Random.Range(0, 2) == 0 ? nextDir : prevDir;
            }
            else {
                currentDirection = baseDirection;
            }

            int currentBranchingAttempt = 0;

            Vector3 nextPosition = currentPosition + _directionVectors[currentDirection] * _unitSpacing;
            Vector3Int gridPos = Vector3Int.RoundToInt(nextPosition);

            while (_occupiedPositions.Contains(gridPos) && currentBranchingAttempt < _maxBranchingAttempts)
            {
                currentDirection = GetRandomDirection();
                nextPosition = currentPosition + _directionVectors[currentDirection] * _unitSpacing;
                gridPos = Vector3Int.RoundToInt(nextPosition);

                currentBranchingAttempt++;
            }

            if (currentBranchingAttempt >= _maxBranchingAttempts)
            {
                Debug.LogWarning($"Generating finished on unit {i}: There are no empty units closely");
                break;
            }

            currentPosition = nextPosition;
            _occupiedPositions.Add(gridPos);

            TrenchUnit trenchUnit = Instantiate(_trenchUnitPrefab, currentPosition, Quaternion.identity);
            trenchUnit.transform.SetParent(_trenchParent);
            trenchUnit.gameObject.name = $"Unit - Trench {i}";
        }
    }

    private Direction GetClosestDirection(Direction direction, int movementPosition)
    {
        int total = GetDirectionLength();

        int currentDirection = (int) direction;
        int nextIndex = (total + currentDirection + movementPosition) % total;

        return (Direction) nextIndex;
    }

    private Direction GetRandomDirection()
    {
        return (Direction) UnityEngine.Random.Range(0, GetDirectionLength());
    }

    private int GetDirectionLength()
    {
        return Enum.GetNames(typeof(Direction)).Length;
    }
}