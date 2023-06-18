using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueGraph : EditorWindow
{
    private DialogueGraphView graphView;
    private string filedName = "New Narrative";

    [MenuItem("Graph/Dialogue Graph")]
    public static void OpenDialogueGraphWindow()
    {
        var window = GetWindow<DialogueGraph>();
        window.titleContent = new GUIContent(text:"Dialogue Graph");
    }

    private void OnEnable()
    {
        ConstructGraphView();
        GenerateToolbar();
        GenerateMiniMap();
    }

    /// <summary>
    /// 构造 GraphView
    /// </summary>
    private void ConstructGraphView()
    {
        graphView = new DialogueGraphView()
        {
            name = "Dialogue Graph"
        };

        graphView.StretchToParentSize();
        rootVisualElement.Add(graphView);
    }

    private void GenerateToolbar()
    {
        var toolbar = new Toolbar();

        var filedNameTextField = new TextField("File Name:");
        filedNameTextField.SetValueWithoutNotify(filedName);
        filedNameTextField.MarkDirtyRepaint();
        filedNameTextField.RegisterValueChangedCallback(evt => filedName = evt.newValue);
        toolbar.Add(filedNameTextField);

        toolbar.Add(new Button(() => RequestDataOperation(true)) { text = "Save Data" });
        toolbar.Add(new Button(() => RequestDataOperation(false)) { text = "Load Data" });

        var nodeCreateButton = new Button(() => { graphView.CreateNode("Dialogue Node");});
        nodeCreateButton.text = "Create Node";
        toolbar.Add(nodeCreateButton);

        rootVisualElement.Add(toolbar);
    }

    private void GenerateMiniMap()
    {
        var miniMap = new MiniMap { anchored = true };
        miniMap.SetPosition(new Rect(10, 30, 200, 140));
        graphView.Add(miniMap);
    }

    private void RequestDataOperation(bool save)
    {
        if (string.IsNullOrEmpty(filedName))
        {
            EditorUtility.DisplayDialog("Invalid file name!", "Please enter a valid file name.", "OK");
            return;
        }

        var saveUtility = GraphSaveUtility.GetInstance(graphView);

        if (save)
            saveUtility.SaveGraph(filedName);
        else
            saveUtility.LoadGraph(filedName);
    }

    private void OnDisable()
    {
        rootVisualElement.Remove(graphView);
    }
}
