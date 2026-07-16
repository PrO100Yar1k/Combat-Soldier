using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.App.Scripts
{
    public abstract class BaseTroopModelController : MonoBehaviour, IChangeMaterial
    {
        [SerializeField] protected List<Material> _damagedUnitMaterial = default;
        [SerializeField] protected SkinnedMeshRenderer _meshRenderer = default;

        protected TroopController _troopController = default;
        protected Material[] _defaultMaterials = default;

        protected LayerMask _defaultLayer = default;

        protected Coroutine _damagedMaterialCoroutine = default;

        public virtual void Initialize(TroopController troopController)
        {
            _troopController = troopController;

            _defaultMaterials = _meshRenderer.sharedMaterials;
            _defaultLayer = _troopController.gameObject.layer;
        }

        public void ChangeMaterialToDamaged()
        {
            _damagedMaterialCoroutine = _troopController.StartCoroutine(ChangeMaterialToDamagedCoroutine());
        }

        private IEnumerator ChangeMaterialToDamagedCoroutine()
        {
            _meshRenderer.materials = _damagedUnitMaterial.ToArray();

            yield return new WaitForSeconds(0.25f);

            _meshRenderer.materials = _defaultMaterials;
        }
    }
}
