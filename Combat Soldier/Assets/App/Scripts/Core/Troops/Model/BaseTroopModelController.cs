using System.Collections;
using System.Collections.Generic;
using App.Scripts.Core.Troops.TroopScripts;
using App.Scripts.Infrastructure.Interfaces;
using UnityEngine;

namespace App.Scripts.Core.Troops.Model
{
    public abstract class BaseTroopModelController : MonoBehaviour, IChangeMaterial
    {
        [SerializeField] protected List<Material> _damagedUnitMaterial;
        [SerializeField] protected SkinnedMeshRenderer _meshRenderer;

        protected TroopController _troopController;

        protected Material[] _defaultMaterialsArray;
        protected Material[] _damagedMaterialsArray;

        protected LayerMask _defaultLayer;

        protected Coroutine _damagedMaterialCoroutine;

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
