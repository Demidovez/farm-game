using System;
using System.Collections;
using System.Collections.Generic;
using InventoryScriptableObjectSpace;
using UnityEngine;

namespace PlantSpace
{
    public class Plant : MonoBehaviour
    {
        [SerializeField] private VegetableSO VegetableSo;
        [SerializeField] private int _maturationDuration = 1;
        [SerializeField] private List<GameObject> _maturationSteps;
        
        private int _maturationStep;
        public bool IsReady => _maturationStep == _maturationSteps.Count - 1;

        private void Start()
        {
            _maturationStep = _maturationSteps.Count - 1;
        }

        public InventoryItem CreateInventoryItem()
        {
            StartCoroutine(PlantGrowth());
            
            return VegetableSo.Create();
        }

        private IEnumerator PlantGrowth()
        {
            _maturationSteps[_maturationStep].SetActive(false);
            _maturationStep = 0;
            _maturationSteps[_maturationStep].SetActive(true);
            yield return new WaitForSeconds(_maturationDuration);
            
            while (_maturationStep < _maturationSteps.Count - 1)
            {
                _maturationSteps[_maturationStep].SetActive(false);
                _maturationStep++;
                _maturationSteps[_maturationStep].SetActive(true);
                yield return new WaitForSeconds(_maturationDuration);
            }
        }
    }
}
