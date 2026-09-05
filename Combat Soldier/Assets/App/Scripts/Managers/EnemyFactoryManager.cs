using App.Scripts.Core.EnemyFactories;
using App.Scripts.Infrastructure.Interfaces;

namespace App.Scripts.Managers
{
    public class EnemyFactoryManager : IEnemyFactory, System.IDisposable
    {
        #region Disposable

        public void Dispose()
        {

        }

        #endregion

        public void CreateEnemies()
        {
            IEnemyFactory enemyFactory = new EasyEnemyFactory(); // control user input to do
            enemyFactory.CreateEnemies();
        }
    }
}