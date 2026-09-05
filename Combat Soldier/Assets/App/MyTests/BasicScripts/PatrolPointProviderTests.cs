using NUnit.Framework;
using System.Collections.Generic;
using App.Scripts.Core.Services;
using UnityEngine;

[TestFixture]
public class PatrolPointProviderTests
{
    private GameObject[] _pointObjects;
    private List<Transform> _pointsList;
    private PatrolPointProvider _provider;

    [SetUp]
    public void SetUp()
    {
        _pointObjects = new GameObject[3];
        _pointsList = new List<Transform>();

        for (int i = 0; i < 3; i++)
        {
            _pointObjects[i] = new GameObject($"Point_{i}");
            _pointsList.Add(_pointObjects[i].transform);
        }

        _provider = new PatrolPointProvider(_pointsList);
    }

    [TearDown]
    public void TearDown()
    {
        foreach (var obj in _pointObjects)
        {
            if (obj != null) Object.DestroyImmediate(obj);
        }
    }

    [Test]
    public void GetRandomPatrolPoints_ShouldReturnAllPoints()
    {
        var result = _provider.GetRandomPatrolPoints();

        Assert.IsNotNull(result);
        Assert.AreEqual(3, result.Length);
    }
}