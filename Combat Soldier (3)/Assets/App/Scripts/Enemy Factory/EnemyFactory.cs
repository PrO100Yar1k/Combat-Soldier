using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    public void InitializeManager() => CreateEnemies();

    private void CreateEnemies()
    {
        IEnemyFactory enemyFactory = new EasyEnemyFactory(); // controll user input

        enemyFactory.CreateEnemies();
    }
}

public interface IEnemyFactory
{
    public void CreateEnemies();
}