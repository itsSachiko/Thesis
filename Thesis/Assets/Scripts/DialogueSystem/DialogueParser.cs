using Subtegral.DialogueSystem.DataContainers;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        [Space]
        [SerializeField] bool startWithDialogue;

        private void Start()
        {
            if (startWithDialogue)
                StartDialogue(dialogue);
        }
        public void StartDialogue(DialogueContainer _dialogue)
        {
            var narrativeData = _dialogue.NodeLinks.First(); //Entrypoint node
            ProceedToNarrative(narrativeData.TargetNodeGUID);
            //Time.timeScale = 0;
            GameManager.Instance.playerInput.enabled = false;
            GameManager.Instance.playerInteracter.enabled = false;
            //GameManager.Instance.playerController.stopMovementView = true;
        }
        public void EndDialogue()
        {
            //Time.timeScale = 1;
            gameObject.SetActive(false);
            GameManager.Instance.playerInput.enabled = true;
            GameManager.Instance.playerInteracter.enabled = true;
            //GameManager.Instance.playerController.stopMovementView = false;
        }

        private void ProceedToNarrative(string narrativeDataGUID)
        {
            var text = dialogue.DialogueNodeData.Find(x => x.NodeGUID == narrativeDataGUID).DialogueText;
            var textActor = dialogue.DialogueNodeData.Find(x => x.NodeGUID == narrativeDataGUID).ActorText;
            var choices = dialogue.NodeLinks.Where(x => x.BaseNodeGUID == narrativeDataGUID);
            dialogueText.text = ProcessProperties(text);
            actorName.text = ProcessProperties(textActor);
            var buttons = buttonContainer.GetComponentsInChildren<Button>();
            for (int i = 0; i < buttons.Length; i++)
            {
                Destroy(buttons[i].gameObject);
            }

            if (choices.Count<NodeLinkData>() == 1)
            {
                var button = Instantiate(goPrefab, goContainer);
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => 
                {
                    ProceedToNarrative(choices.First<NodeLinkData>().TargetNodeGUID);
                    Destroy(button.gameObject);
                });
            }
            else if (choices.Count<NodeLinkData>() <= 0)
            {
                var button = Instantiate(goPrefab, goContainer);
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() =>
                {
                    UIManager.Instance.EndDialogue();
                    Destroy(button.gameObject);
                });
            }
            else
            {
                foreach (var choice in choices)
                {
                    var button = Instantiate(choicePrefab, buttonContainer);
                    button.GetComponentInChildren<TMP_Text>().text = ProcessProperties(choice.PortName);
                    button.onClick.RemoveAllListeners();
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
    }
}