using System;
using Project.Infrastructure;
using Project.Utilities;
using Reflex.Scripts.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    public sealed class SelectedUI : MonoBehaviour
    {
        [SerializeField] private Text _selectedName;
        [SerializeField] private ProcessorConfiguration _processorConfiguration;
        [SerializeField] private BugConfiguration _bugConfiguration;
        
        [MonoInject]
        private void Construct(SharedData data)
        {
            data.SelectedUI = this;
        }

        public void UpdateSelected(SelectableType selectable)
        {
            switch (selectable)
            {
                case SelectableType.Processor:
                    UpdateSelectedFromConfig(_processorConfiguration);
                    break;
                case SelectableType.Bug:
                    UpdateSelectedFromConfig(_bugConfiguration);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(selectable), selectable, null);
            }
        }

        private void UpdateSelectedFromConfig(ProcessorConfiguration config)
        {
            _selectedName.text = config.Name;
        }

        private void UpdateSelectedFromConfig(BugConfiguration config)
        {
            _selectedName.text = config.Name;
        }
    }
}