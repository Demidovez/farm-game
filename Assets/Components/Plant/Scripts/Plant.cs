using System.Collections;
using InventoryScriptableObjectSpace;
using UnityEngine;

namespace PlantSpace
{
    public class Plant : MonoBehaviour
    {
        [SerializeField] private VegetableSO VegetableSo;
        [SerializeField] private int _maturationDuration = 1;
        
        private int _maturationStep = 4;
        public bool IsReady => _maturationStep == 4;

        public InventoryItem CreateInventoryItem()
        {
            _maturationStep = 0;

            StartCoroutine(PlantGrowth());
            
            return VegetableSo.Create();
        }

        private IEnumerator PlantGrowth()
        {
            while (_maturationStep < 4)
            {
                yield return new WaitForSeconds(_maturationDuration);
                _maturationStep++;
            }
        }
    }
}
