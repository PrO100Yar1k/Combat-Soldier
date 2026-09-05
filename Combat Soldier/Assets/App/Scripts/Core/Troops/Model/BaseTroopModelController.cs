using System.Collections;
using System.Collections.Generic;
using App.Scripts.Core.Troops.Troop_Scripts;
using App.Scripts.Infrastructure.Interfaces;
using UnityEngine;

namespace App.Scripts.Core.Troops.Model
{
    public abstract class BaseTroopModelController : MonoBehaviour, IChangeMaterial
    {
        [SerializeField] protected List<Material> _damagedUnitMaterial = default;
        [SerializeField] protected SkinnedMeshRenderer _meshRenderer = default;

        protected TroopController _troopController = default;

        protected Material[] _defaultMaterialsArray = default;
        protected Material[] _damagedMaterialsArray = default;

        protected LayerMask _defaultLayer = default;

        protected Coroutine _damagedMaterialCoroutine = default;

        private readonly WaitForSeconds _damageDelay
            = new WaitForSeconds(0.25f);

        public virtual void Initialize(TroopController troopController)
        {
            _troopController = troopController;

            _defaultMaterialsArray = _meshRenderer.sharedMaterials;
            _defaultLayer = _troopController.gameObject.layer;

            _damagedMaterialsArray = _damagedUnitMaterial.ToArray();
        }

        public void ChangeMaterialToDamaged()
        {
            if (_damagedMaterialCoroutine != null)
                _troopController.StopCoroutine(_damagedMaterialCoroutine);

            _damagedMaterialCoroutine = _troopController.StartCoroutine(ChangeMaterialToDamagedCoroutine());
        }

        private IEnumerator ChangeMaterialToDamagedCoroutine()
        {
            _meshRenderer.sharedMaterials = _damagedMaterialsArray;

            yield return _damageDelay;

            _meshRenderer.sharedMaterials = _defaultMaterialsArray;
        }
    }
}
