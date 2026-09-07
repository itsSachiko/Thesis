using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Subtegral.DialogueSystem.DataContainers;

namespace Subtegral.DialogueSystem.Runtime
{
    public class DialogueParser : MonoBehaviour
    {
        [SerializeField] private DialogueContainer dialogue;
        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private TextMeshProUGUI actorName;
        [SerializeField] private Button choicePrefab;
        [SerializeField] private Button goPrefab;
        [SerializeField] private Transform buttonContainer;
        [SerializeField] private Transform goContainer;

        private void Start()
        {
            var narrativeData = dialogue.NodeLinks.First(); //Entrypoint node
            ProceedToNarrative(narrativeData.TargetNodeGUID);
        }

        private void ProceedToNarrative(string narrativeDataGUID)
        {
            var text = dialogue.DialogueNodeData.Find(x => x.NodeGUID == narrativeDataGUID).DialogueText;
            var choices = dialogue.NodeLinks.Where(x => x.BaseNodeGUID == narrativeDataGUID);
            dialogueText.text = ProcessProperties(text);
            actorName.text = ProcessProperties(text);
            var buttons = buttonContainer.GetComponentsInChildren<Button>();
            for (int i = 0; i < buttons.Length; i++)
            {
                Destroy(buttons[i].gameObject);
            }
            if (choices.Count<NodeLinkData>() == 1)
            {
                var button = Instantiate(goPrefab, goContainer);
                button.onClick.AddListener(() => ProceedToNarrative(choices.First<NodeLinkData>().TargetNodeGUID));
            }
            else
            {
                foreach (var choice in choices)
                {
                    var button = Instantiate(choicePrefab, buttonContainer);
                    button.GetComponentInChildren<TMP_Text>().text = ProcessProperties(choice.PortName);
                    button.onClick.AddListener(() => ProceedToNarrative(choice.TargetNodeGUID));
                }
            }
        
        }

        private string ProcessProperties(string text)
        {
            foreach (var exposedProperty in dialogue.ExposedProperties)
            {
                text = text.Replace($"[{exposedProperty.PropertyName}]", exposedProperty.PropertyValue);
            }
            return text;
        }

        private string ProcessNameProperties(string text)
        {
            foreach (var exposedProperty in dialogue.ExposedProperties)
            {
                text = text.Replace($"[{exposedProperty.PropertyName}]", exposedProperty.PropertyValue);
            }
            return text;
        }
    }
}