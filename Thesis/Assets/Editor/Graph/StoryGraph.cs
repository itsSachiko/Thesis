using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Subtegral.DialogueSystem.DataContainers;

namespace Subtegral.DialogueSystem.Editor
{
    /// <summary>
    /// editor window => tool to create narrative graph 
    /// </summary>
    public class StoryGraph : EditorWindow
    {
        private string fileName = "New Narrative";

        private StoryGraphView graphView;
        private DialogueContainer dialogueContainer;

        [MenuItem("Graph/Narrative Graph")]
        public static void CreateGraphViewWindow()
        {
            var window = GetWindow<StoryGraph>();
            window.titleContent = new GUIContent("Narrative Graph");
        }

        private void ConstructGraphView() //costruttore che rende grande la blackboard a finestra 
        {
            graphView = new StoryGraphView(this)
            {
                name = "Narrative Graph",
            };
            graphView.StretchToParentSize();
            rootVisualElement.Add(graphView);
        }

        private void GenerateToolbar() //genera la barra in alto (save, load, name)
        {
            var toolbar = new Toolbar();

            var fileNameTextField = new TextField("File Name:");
            fileNameTextField.SetValueWithoutNotify(fileName);
            fileNameTextField.MarkDirtyRepaint(); //notificare una modifica, ma non si vede
            fileNameTextField.RegisterValueChangedCallback(evt => fileName = evt.newValue); //save
            toolbar.Add(fileNameTextField);

            toolbar.Add(new Button(() => RequestDataOperation(true)) {text = "Save Data"});

            toolbar.Add(new Button(() => RequestDataOperation(false)) {text = "Load Data"});
            // toolbar.Add(new Button(() => graphView.CreateNewDialogueNode("Dialogue Node")) {text = "New Node",});
            rootVisualElement.Add(toolbar);
        }

        private void RequestDataOperation(bool save)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                var saveUtility = GraphSaveUtility.GetInstance(graphView);
                if (save)
                    saveUtility.SaveGraph(fileName);
                else
                    saveUtility.LoadNarrative(fileName);
            }
            else
            {
                EditorUtility.DisplayDialog("Invalid File name", "Please Enter a valid filename", "OK");
            }
        }

        private void OnEnable()
        {
            ConstructGraphView();
            GenerateToolbar();
            GenerateMiniMap();
            GenerateBlackBoard();
        }

        private void GenerateMiniMap()
        {
            var miniMap = new MiniMap {anchored = true};
            var cords = graphView.contentViewContainer.WorldToLocal(new Vector2(this.maxSize.x - 10, 30));
            miniMap.SetPosition(new Rect(cords.x, cords.y, 200, 140));
            graphView.Add(miniMap);
        }

        private void GenerateBlackBoard()
        {
            var blackboard = new Blackboard(graphView);
            blackboard.Add(new BlackboardSection {title = "Exposed Variables"});
            blackboard.addItemRequested = _blackboard =>
            {
                graphView.AddPropertyToBlackBoard(ExposedProperty.CreateInstance(), false);
            };
            blackboard.editTextRequested = (_blackboard, element, newValue) =>
            {
                var oldPropertyName = ((BlackboardField) element).text;
                if (graphView.ExposedProperties.Any(x => x.PropertyName == newValue))
                {
                    EditorUtility.DisplayDialog("Error", "This property name already exists, please chose another one.",
                        "OK");
                    return;
                }

                var targetIndex = graphView.ExposedProperties.FindIndex(x => x.PropertyName == oldPropertyName);
                graphView.ExposedProperties[targetIndex].PropertyName = newValue;
                ((BlackboardField) element).text = newValue;
            };
            blackboard.SetPosition(new Rect(10,30,200,300));
            graphView.Add(blackboard);
            graphView.Blackboard = blackboard;
        }

        private void OnDisable()
        {
            rootVisualElement.Remove(graphView);
        }
    }
}