using System.IO;
using Spells.EditorTool;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


public class SpellDatabaseEditorWindow : EditorWindow
{
    private const string DefaultRootFolder = "Assets/SpellSystem";
    private const string DefaultElementFolder = "Assets/SpellSystem/Elements";
    private const string DefaultSpellTypeFolder = "Assets/SpellSystem/SpellTypes";
    private const string DefaultComboFolder = "Assets/SpellSystem/Combos";
    private const float LeftPanelWidth = 320f;
    private const float HeaderHeight = 28f;
    private const float CellWidth = 150f;
    private const float CellHeight = 56f;

    private SpellDatabase database;

    private ObjectField databaseField;
    private ScrollView elementsListView;
    private ScrollView spellTypesListView;
    private ScrollView matrixScrollView;
    private Label statusLabel;

    [MenuItem("Tools/Spells/Spell Database Editor")]
    public static void OpenWindow()
    {
        SpellDatabaseEditorWindow window = GetWindow<SpellDatabaseEditorWindow>();
        window.titleContent = new GUIContent("Spell Database");
        window.minSize = new Vector2(1000, 600);
    }

    public void CreateGUI()
    {
        rootVisualElement.style.flexDirection = FlexDirection.Column;
        rootVisualElement.style.paddingLeft = 6;
        rootVisualElement.style.paddingRight = 6;
        rootVisualElement.style.paddingTop = 6;
        rootVisualElement.style.paddingBottom = 6;

        BuildToolbar();
        BuildBody();
        RefreshAll();
    }

    private void BuildToolbar()
    {
        Toolbar toolbar = new Toolbar();

        databaseField = new ObjectField("Database")
        {
            objectType = typeof(SpellDatabase),
            allowSceneObjects = false
        };
        databaseField.style.minWidth = 340;
        databaseField.RegisterValueChangedCallback(evt =>
        {
            database = evt.newValue as SpellDatabase;
            RefreshAll();
        });

        Button createDatabaseButton = new Button(CreateNewDatabase)
        {
            text = "Create Database"
        };

        Button refreshButton = new Button(RefreshAll)
        {
            text = "Refresh"
        };

        Button cleanButton = new Button(CleanDatabase)
        {
            text = "Clean Nulls"
        };

        Button saveButton = new Button(SaveDatabase)
        {
            text = "Save"
        };

        statusLabel = new Label("No database selected.");
        statusLabel.style.unityTextAlign = TextAnchor.MiddleLeft;
        statusLabel.style.flexGrow = 1;
        statusLabel.style.marginLeft = 10;

        toolbar.Add(databaseField);
        toolbar.Add(createDatabaseButton);
        toolbar.Add(refreshButton);
        toolbar.Add(cleanButton);
        toolbar.Add(saveButton);
        toolbar.Add(statusLabel);

        rootVisualElement.Add(toolbar);
    }

    private void BuildBody()
    {
        VisualElement body = new VisualElement();
        body.style.flexGrow = 1;
        body.style.flexDirection = FlexDirection.Row;
        body.style.marginTop = 6;

        VisualElement leftPanel = new VisualElement();
        leftPanel.style.width = LeftPanelWidth;
        leftPanel.style.minWidth = LeftPanelWidth;
        leftPanel.style.flexShrink = 0;
        leftPanel.style.marginRight = 8;
        leftPanel.style.flexDirection = FlexDirection.Column;

        leftPanel.Add(BuildElementsPanel());
        leftPanel.Add(BuildSpellTypesPanel());

        matrixScrollView = new ScrollView(ScrollViewMode.VerticalAndHorizontal);
        matrixScrollView.style.flexGrow = 1;
        matrixScrollView.style.borderTopWidth = 1;
        matrixScrollView.style.borderBottomWidth = 1;
        matrixScrollView.style.borderLeftWidth = 1;
        matrixScrollView.style.borderRightWidth = 1;
        matrixScrollView.style.borderTopColor = new Color(0.25f, 0.25f, 0.25f);
        matrixScrollView.style.borderBottomColor = new Color(0.25f, 0.25f, 0.25f);
        matrixScrollView.style.borderLeftColor = new Color(0.25f, 0.25f, 0.25f);
        matrixScrollView.style.borderRightColor = new Color(0.25f, 0.25f, 0.25f);

        body.Add(leftPanel);
        body.Add(matrixScrollView);

        rootVisualElement.Add(body);
    }

    private VisualElement BuildElementsPanel()
    {
        VisualElement panel = new VisualElement();
        panel.style.flexGrow = 1;
        panel.style.marginBottom = 8;
        panel.style.borderTopWidth = 1;
        panel.style.borderBottomWidth = 1;
        panel.style.borderLeftWidth = 1;
        panel.style.borderRightWidth = 1;
        panel.style.borderTopColor = new Color(0.25f, 0.25f, 0.25f);
        panel.style.borderBottomColor = new Color(0.25f, 0.25f, 0.25f);
        panel.style.borderLeftColor = new Color(0.25f, 0.25f, 0.25f);
        panel.style.borderRightColor = new Color(0.25f, 0.25f, 0.25f);

        Label title = new Label("Elements");
        title.style.unityFontStyleAndWeight = FontStyle.Bold;
        title.style.paddingLeft = 6;
        title.style.paddingTop = 6;
        title.style.paddingBottom = 6;

        Button addButton = new Button(CreateNewElement)
        {
            text = "Add Element"
        };
        addButton.style.marginLeft = 6;
        addButton.style.marginRight = 6;
        addButton.style.marginBottom = 6;

        elementsListView = new ScrollView();
        elementsListView.style.flexGrow = 1;
        elementsListView.style.paddingLeft = 6;
        elementsListView.style.paddingRight = 6;
        elementsListView.style.paddingBottom = 6;

        panel.Add(title);
        panel.Add(addButton);
        panel.Add(elementsListView);

        return panel;
    }

    private VisualElement BuildSpellTypesPanel()
    {
        VisualElement panel = new VisualElement();
        panel.style.flexGrow = 1;
        panel.style.borderTopWidth = 1;
        panel.style.borderBottomWidth = 1;
        panel.style.borderLeftWidth = 1;
        panel.style.borderRightWidth = 1;
        panel.style.borderTopColor = new Color(0.25f, 0.25f, 0.25f);
        panel.style.borderBottomColor = new Color(0.25f, 0.25f, 0.25f);
        panel.style.borderLeftColor = new Color(0.25f, 0.25f, 0.25f);
        panel.style.borderRightColor = new Color(0.25f, 0.25f, 0.25f);

        Label title = new Label("Spell Types");
        title.style.unityFontStyleAndWeight = FontStyle.Bold;
        title.style.paddingLeft = 6;
        title.style.paddingTop = 6;
        title.style.paddingBottom = 6;

        Button addButton = new Button(CreateNewSpellType)
        {
            text = "Add Spell Type"
        };
        addButton.style.marginLeft = 6;
        addButton.style.marginRight = 6;
        addButton.style.marginBottom = 6;

        spellTypesListView = new ScrollView();
        spellTypesListView.style.flexGrow = 1;
        spellTypesListView.style.paddingLeft = 6;
        spellTypesListView.style.paddingRight = 6;
        spellTypesListView.style.paddingBottom = 6;

        panel.Add(title);
        panel.Add(addButton);
        panel.Add(spellTypesListView);

        return panel;
    }

    private void RefreshAll()
    {
        RefreshStatus();
        RefreshElementsPanel();
        RefreshSpellTypesPanel();
        RefreshMatrix();
    }

    private void RefreshStatus()
    {
        if (database == null)
        {
            statusLabel.text = "No database selected.";
            return;
        }

        statusLabel.text =
            $"Elements: {database.elements.Count} | Types: {database.spellTypes.Count} | Combos: {database.combos.Count}";
    }

    private void RefreshElementsPanel()
    {
        elementsListView?.Clear();

        if (database == null)
            return;

        for (int i = 0; i < database.elements.Count; i++)
        {
            SpellElementDefinition element = database.elements[i];
            elementsListView.Add(CreateAssetRow(
                element,
                typeof(SpellElementDefinition),
                newValue =>
                {
                    database.elements[i] = newValue as SpellElementDefinition;
                    MarkDatabaseDirty();
                    RefreshAll();
                },
                () =>
                {
                    if (element != null)
                    {
                        Selection.activeObject = element;
                        EditorGUIUtility.PingObject(element);
                    }
                },
                () =>
                {
                    database.elements.RemoveAt(i);
                    MarkDatabaseDirty();
                    RefreshAll();
                }));
        }
    }

    private void RefreshSpellTypesPanel()
    {
        spellTypesListView?.Clear();

        if (database == null)
            return;

        for (int i = 0; i < database.spellTypes.Count; i++)
        {
            SpellTypeDefinition spellType = database.spellTypes[i];
            spellTypesListView.Add(CreateAssetRow(
                spellType,
                typeof(SpellTypeDefinition),
                newValue =>
                {
                    database.spellTypes[i] = newValue as SpellTypeDefinition;
                    MarkDatabaseDirty();
                    RefreshAll();
                },
                () =>
                {
                    if (spellType != null)
                    {
                        Selection.activeObject = spellType;
                        EditorGUIUtility.PingObject(spellType);
                    }
                },
                () =>
                {
                    database.spellTypes.RemoveAt(i);
                    MarkDatabaseDirty();
                    RefreshAll();
                }));
        }
    }

    private VisualElement CreateAssetRow(
        Object currentObject,
        System.Type objectType,
        System.Action<Object> onObjectChanged,
        System.Action onSelectClicked,
        System.Action onRemoveClicked)
    {
        VisualElement row = new VisualElement();
        row.style.flexDirection = FlexDirection.Row;
        row.style.marginBottom = 4;
        row.style.alignItems = Align.Center;

        ObjectField field = new ObjectField
        {
            objectType = objectType,
            allowSceneObjects = false,
            value = currentObject
        };
        field.style.flexGrow = 1;
        field.RegisterValueChangedCallback(evt => onObjectChanged?.Invoke(evt.newValue));

        Button selectButton = new Button(onSelectClicked)
        {
            text = "Select"
        };
        selectButton.style.width = 60;
        selectButton.style.marginLeft = 4;

        Button removeButton = new Button(onRemoveClicked)
        {
            text = "X"
        };
        removeButton.style.width = 28;
        removeButton.style.marginLeft = 4;

        row.Add(field);
        row.Add(selectButton);
        row.Add(removeButton);

        return row;
    }

    private void RefreshMatrix()
    {
        matrixScrollView?.Clear();

        if (database == null)
            return;

        VisualElement matrixRoot = new VisualElement();
        matrixRoot.style.flexDirection = FlexDirection.Column;

        // Header row
        VisualElement headerRow = new VisualElement();
        headerRow.style.flexDirection = FlexDirection.Row;

        headerRow.Add(CreateHeaderCell("Spell Type \\ Element", 180));

        for (int e = 0; e < database.elements.Count; e++)
        {
            SpellElementDefinition element = database.elements[e];
            string text = element != null ? element.displayName : "Null";
            headerRow.Add(CreateHeaderCell(text, CellWidth));
        }

        matrixRoot.Add(headerRow);

        // Rows
        for (int t = 0; t < database.spellTypes.Count; t++)
        {
            SpellTypeDefinition spellType = database.spellTypes[t];

            VisualElement row = new VisualElement();
            row.style.flexDirection = FlexDirection.Row;

            string rowTitle = spellType != null ? spellType.displayName : "Null";
            row.Add(CreateHeaderCell(rowTitle, 180));

            for (int e = 0; e < database.elements.Count; e++)
            {
                SpellElementDefinition element = database.elements[e];
                row.Add(CreateMatrixCell(element, spellType));
            }

            matrixRoot.Add(row);
        }

        matrixScrollView.Add(matrixRoot);
    }

    private VisualElement CreateHeaderCell(string text, float width)
    {
        Label label = new Label(text);
        label.style.width = width;
        label.style.height = HeaderHeight;
        label.style.unityTextAlign = TextAnchor.MiddleCenter;
        label.style.unityFontStyleAndWeight = FontStyle.Bold;
        label.style.borderTopWidth = 1;
        label.style.borderBottomWidth = 1;
        label.style.borderLeftWidth = 1;
        label.style.borderRightWidth = 1;
        label.style.borderTopColor = new Color(0.25f, 0.25f, 0.25f);
        label.style.borderBottomColor = new Color(0.25f, 0.25f, 0.25f);
        label.style.borderLeftColor = new Color(0.25f, 0.25f, 0.25f);
        label.style.borderRightColor = new Color(0.25f, 0.25f, 0.25f);
        label.style.backgroundColor = new Color(0.18f, 0.18f, 0.18f);

        return label;
    }

    private VisualElement CreateMatrixCell(SpellElementDefinition element, SpellTypeDefinition spellType)
    {
        SpellComboDefinition combo = database.GetCombo(element, spellType);

        Button cellButton = new Button(() => OnMatrixCellClicked(element, spellType))
        {
            text = combo != null ? combo.displayName : "Create / Assign"
        };

        cellButton.style.width = CellWidth;
        cellButton.style.height = CellHeight;
        cellButton.style.whiteSpace = WhiteSpace.Normal;
        cellButton.style.unityTextAlign = TextAnchor.MiddleCenter;
        cellButton.style.borderTopWidth = 1;
        cellButton.style.borderBottomWidth = 1;
        cellButton.style.borderLeftWidth = 1;
        cellButton.style.borderRightWidth = 1;
        cellButton.style.borderTopColor = new Color(0.25f, 0.25f, 0.25f);
        cellButton.style.borderBottomColor = new Color(0.25f, 0.25f, 0.25f);
        cellButton.style.borderLeftColor = new Color(0.25f, 0.25f, 0.25f);
        cellButton.style.borderRightColor = new Color(0.25f, 0.25f, 0.25f);

        if (combo != null)
        {
            cellButton.style.backgroundColor = new Color(0.22f, 0.35f, 0.22f);
        }
        else
        {
            cellButton.style.backgroundColor = new Color(0.28f, 0.20f, 0.20f);
        }

        return cellButton;
    }

    private void OnMatrixCellClicked(SpellElementDefinition element, SpellTypeDefinition spellType)
    {
        if (database == null || element == null || spellType == null)
            return;

        SpellComboDefinition combo = database.GetCombo(element, spellType);

        if (combo == null)
        {
            bool create = EditorUtility.DisplayDialog(
                "Create Combo",
                $"No combo exists for:\n\nElement: {element.displayName}\nType: {spellType.displayName}\n\nCreate one now?",
                "Create",
                "Cancel");

            if (!create)
                return;

            combo = CreateNewCombo(element, spellType);
            database.SetCombo(element, spellType, combo);
            MarkDatabaseDirty();
        }

        Selection.activeObject = combo;
        EditorGUIUtility.PingObject(combo);
        RefreshAll();
    }

    private void CreateNewDatabase()
    {
        string folder = EnsureFolder(DefaultRootFolder);
        string path = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(folder, "SpellDatabase.asset"));

        SpellDatabase newDatabase = CreateInstance<SpellDatabase>();
        AssetDatabase.CreateAsset(newDatabase, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        database = newDatabase;
        databaseField.value = database;

        Selection.activeObject = database;
        EditorGUIUtility.PingObject(database);

        RefreshAll();
    }

    private void CreateNewElement()
    {
        if (database == null)
        {
            EditorUtility.DisplayDialog("Missing Database", "Create or assign a SpellDatabase first.", "OK");
            return;
        }

        string folder = EnsureFolder(DefaultElementFolder);
        string path = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(folder, "Element.asset"));

        SpellElementDefinition element = CreateInstance<SpellElementDefinition>();
        element.displayName = "New Element";
        element.elementId = "new_element";

        AssetDatabase.CreateAsset(element, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        database.elements.Add(element);
        MarkDatabaseDirty();

        Selection.activeObject = element;
        EditorGUIUtility.PingObject(element);

        RefreshAll();
    }

    private void CreateNewSpellType()
    {
        if (database == null)
        {
            EditorUtility.DisplayDialog("Missing Database", "Create or assign a SpellDatabase first.", "OK");
            return;
        }

        string folder = EnsureFolder(DefaultSpellTypeFolder);
        string path = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(folder, "SpellType.asset"));

        SpellTypeDefinition spellType = CreateInstance<SpellTypeDefinition>();
        spellType.displayName = "New Spell Type";
        spellType.typeId = "new_type";

        AssetDatabase.CreateAsset(spellType, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        database.spellTypes.Add(spellType);
        MarkDatabaseDirty();

        Selection.activeObject = spellType;
        EditorGUIUtility.PingObject(spellType);

        RefreshAll();
    }

    private SpellComboDefinition CreateNewCombo(SpellElementDefinition element, SpellTypeDefinition spellType)
    {
        string folder = EnsureFolder(DefaultComboFolder);
        string safeElement = SanitizeFileName(element.displayName);
        string safeType = SanitizeFileName(spellType.displayName);
        string fileName = $"{safeElement}_{safeType}_Combo.asset";
        string path = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(folder, fileName));

        SpellComboDefinition combo = CreateInstance<SpellComboDefinition>();
        combo.element = element;
        combo.spellType = spellType;
        combo.AutoNameFromReferences();

        AssetDatabase.CreateAsset(combo, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.SetDirty(combo);
        AssetDatabase.SaveAssets();

        return combo;
    }

    private void CleanDatabase()
    {
        if (database == null)
            return;

        database.RemoveNullReferences();
        database.combos.RemoveAll(entry => entry.combo == null);

        MarkDatabaseDirty();
        RefreshAll();
    }

    private void SaveDatabase()
    {
        if (database == null)
            return;

        EditorUtility.SetDirty(database);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        RefreshAll();
    }

    private void MarkDatabaseDirty()
    {
        if (database == null)
            return;

        EditorUtility.SetDirty(database);
        AssetDatabase.SaveAssets();
    }

    private string EnsureFolder(string fullFolderPath)
    {
        string[] parts = fullFolderPath.Split('/');

        string current = parts[0];
        for (int i = 1; i < parts.Length; i++)
        {
            string next = $"{current}/{parts[i]}";
            if (!AssetDatabase.IsValidFolder(next))
            {
                AssetDatabase.CreateFolder(current, parts[i]);
            }

            current = next;
        }

        return fullFolderPath;
    }

    private string SanitizeFileName(string value)
    {
        string sanitized = value.Trim();

        foreach (char c in Path.GetInvalidFileNameChars())
        {
            sanitized = sanitized.Replace(c.ToString(), "");
        }

        sanitized = sanitized.Replace(" ", "_");

        if (string.IsNullOrWhiteSpace(sanitized))
            sanitized = "Unnamed";

        return sanitized;
    }
}