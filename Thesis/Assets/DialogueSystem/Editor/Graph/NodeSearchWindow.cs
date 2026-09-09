using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Subtegral.DialogueSystem.Editor
{
    /// <summary>
    /// this class provides a search window for creating nodes in the StoryGraphView.
    /// interactions button on the graph
    /// </summary>
    public class NodeSearchWindow : ScriptableObject,ISearchWindowProvider
    {
        private EditorWindow window;
        private StoryGraphView graphView;

        private Texture2D indentationIcon;
        
        public void Configure(EditorWindow window,StoryGraphView graphView)
        {
            this.window = window;
            this.graphView = graphView;
            
            
            indentationIcon = new Texture2D(1,1);
            indentationIcon.SetPixel(0,0,new Color(0,0,0,0));
            indentationIcon.Apply();
        }
        
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var tree = new List<SearchTreeEntry>
            {
                new SearchTreeGroupEntry(new GUIContent("Create Node"), 0),
                new SearchTreeGroupEntry(new GUIContent("Dialogue"), 1),
                new SearchTreeEntry(new GUIContent("Dialogue Node", indentationIcon))
                {
                    level = 2, userData = new DialogueNode()
                },
                new SearchTreeEntry(new GUIContent("Comment Block",indentationIcon))
                {
                    level = 1,
                    userData = new Group()
                }
            };

            return tree;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            var mousePosition = window.rootVisualElement.ChangeCoordinatesTo(window.rootVisualElement.parent,
                context.screenMousePosition - window.position.position);
            var graphMousePosition = graphView.contentViewContainer.WorldToLocal(mousePosition);
            switch (SearchTreeEntry.userData)
            {
                case DialogueNode dialogueNode:
                    graphView.CreateNewDialogueNode("Dialogue Node", "Actor Name",graphMousePosition);
                    return true;
                case Group group:
                    var rect = new Rect(graphMousePosition, graphView.DefaultCommentBlockSize);
                     graphView.CreateCommentBlock(rect);
                    return true;
            }
            return false;
        }
    }
}