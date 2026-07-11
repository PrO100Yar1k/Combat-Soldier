using Assets.App.Scripts;

public class EnemyFactoryManager : System.IDisposable
{
    public void Dispose()
    {

    }

    public void CreateEnemies()
    {
        IEnemyFactory enemyFactory = new EasyEnemyFactory(); // control user input

        enemyFactory.CreateEnemies();
    }
}