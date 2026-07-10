using Assets.App.Scripts;
using UnityEngine;

public class EnemyFactoryManager : MonoBehaviour
{
    public void CreateEnemies()
    {
        IEnemyFactory enemyFactory = new EasyEnemyFactory(); // control user input

        enemyFactory.CreateEnemies();
    }
}