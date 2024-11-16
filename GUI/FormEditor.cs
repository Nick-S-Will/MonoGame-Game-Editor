using Editor.Editor;
using Editor.Engine;
using Editor.Extensions;
using Editor.GUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace GUI.Editor;

public partial class FormEditor : Form
{
    private const string PrefabExtension = ".prefab";

    private static readonly HashSet<AssetMonitor.AssetType> dragDropAssetTypes = new() { AssetMonitor.AssetType.Model, AssetMonitor.AssetType.Texture, AssetMonitor.AssetType.Effect, AssetMonitor.AssetType.Sound };

    public GameEditor GameEditor
    {
        get => gameEditor;
        set
        {
            gameEditor = value;
            AddGameEditorEventListeners();
            gameEditor.OnAssetsUpdated += UpdateAssets;
        }
    }

    private GameEditor gameEditor;
    private Process mgcbProcess;

    public FormEditor()
    {
        InitializeComponent();
        KeyPreview = true;

        KeyDown += InputController.HandlePressEvent;
        KeyUp += InputController.HandleReleaseEvent;

        toolStripStatusLabel1.Text = Directory.GetCurrentDirectory();
    }

    private void AddGameEditorEventListeners()
    {
        Form gameForm = FromHandle(gameEditor.Window.Handle) as Form;
        gameForm.MouseMove += InputController.HandleMouseEvent;
        gameForm.MouseDown += InputController.HandlePressEvent;
        gameForm.MouseUp += InputController.HandleReleaseEvent;
        gameForm.MouseWheel += InputController.HandleMouseEvent;
    }

    private void FormEditor_SizeChanged(object sender, EventArgs e)
    {
        GameEditor?.AdjustAspectRatio();
    }

    private void splitContainer_Panel1_SizeChanged(object sender, EventArgs e)
    {
        GameEditor?.AdjustAspectRatio();
    }

    private void FormEditor_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (mgcbProcess == null) return;

        mgcbProcess.Kill();
    }

    private void exitToolStripMenuItem_Click(object sender, EventArgs e)
    {
        GameEditor.Exit();
    }

    #region File
    private void createToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SaveFileDialog saveFileDialog = new();
        if (saveFileDialog.ShowDialog() != DialogResult.OK) return;

        GameEditor.Project = new(GameEditor.Content, GameEditor.GraphicsDevice);
        GameEditor.Project.SetPath(saveFileDialog.FileName);

        saveToolStripMenuItem_Click(sender, e);

        UpdateAssets();

        Text = "Our Cool Editor - " + GameEditor.Project.Name;
        GameEditor.AdjustAspectRatio();
    }

    private void saveToolStripMenuItem_Click(object sender, EventArgs e)
    {
        string fileName = Path.Combine(GameEditor.Project.Folder, GameEditor.Project.Name);
        using FileStream fileStream = File.Open(fileName, FileMode.Create);
        using BinaryWriter binaryWriter = new(fileStream, Encoding.UTF8, false);
        GameEditor.Project.Serialize(binaryWriter);
    }

    private void loadToolStripMenuItem_Click(object sender, EventArgs e)
    {
        OpenFileDialog openFileDialog = new() { Filter = "OCE Files | *.oce" };
        if (openFileDialog.ShowDialog() != DialogResult.OK) return;

        using FileStream fileStream = File.Open(openFileDialog.FileName, FileMode.Open);
        using BinaryReader binaryReader = new(fileStream, Encoding.UTF8, false);
        GameEditor.Project = new();
        GameEditor.Project.Deserialize(binaryReader, GameEditor.Content);

        UpdateAssets();

        Text = "Our Cool Editor - " + GameEditor.Project.Name;
        GameEditor.AdjustAspectRatio();
    }
    #endregion

    #region Assets
    private void importToolStripMenuItem_Click(object sender, EventArgs e)
    {
        string mgcbPath = ConfigurationManager.AppSettings["MGCB_EditorPath"];
        ProcessStartInfo startInfo = new()
        {
            FileName = "\"" + Path.Combine(mgcbPath, "mgcb-editor-windows.exe") + "\"",
            Arguments = "\"" + GameEditor.Project.ContentPath + "\""
        };

        mgcbProcess = Process.Start(startInfo);
    }

    private void UpdateAssets()
    {
        Invoke(delegate
        {
            assetListBox.Items.Clear();
            foreach (var assetType in Enum.GetValues<AssetMonitor.AssetType>())
            {
                assetListBox.Items.Add(assetType.ToString().ToUpper() + "S:");

                foreach (string assetName in GameEditor.Project.AssetMonitor[assetType])
                {
                    assetListBox.Items.Add(new AssetListItem(assetName, assetType));
                }

                assetListBox.Items.Add("");
            }

            prefabListBox.Items.Clear();
            string[] filesPaths = Directory.GetFiles(GameEditor.Project.Folder, $"*{PrefabExtension}");
            foreach (string filePath in filesPaths)
            {
                prefabListBox.Items.Add(new PrefabListItem(Path.GetFileName(filePath)));
            }

            levelListBox.Items.Clear();
            foreach (var modelRenderer in GameEditor.Project.CurrentLevel.ModelRenderers)
            {
                levelListBox.Items.Add(new LevelListItem(modelRenderer));
            }
        });
    }

    #region Instantiate
    private void assetListBox_MouseDown(object sender, MouseEventArgs e)
    {
        if (assetListBox.Items.Count == 0 || assetListBox.SelectedIndex == -1) return;
        if (assetListBox.SelectedItem is not AssetListItem assetListItem || !dragDropAssetTypes.Contains(assetListItem.Type)) return;

        _ = DoDragDrop(assetListItem, DragDropEffects.Copy);
    }

    private void DropModel(string modelName)
    {
        ModelRenderer newModel = new(GameEditor.Content, modelName, "DefaultTexture", "MyEffect", Vector3.Zero, Vector3.Zero, 1f);
        GameEditor.Project.CurrentLevel.AddModel(newModel);
        levelListBox.Items.Add(new LevelListItem(newModel));
    }

    private void DropTexture(System.Drawing.Point screenPosition, string textureName)
    {
        foreach (var material in GameEditor.Project.CurrentLevel.GetObjects(screenPosition.ToVector()).OfType<IMaterial>())
        {
            material.Texture = GameEditor.Content.Load<Texture2D>(textureName);
            material.Texture.Tag = textureName;
        }
    }

    private void DropEffect(System.Drawing.Point screenPosition, string effectName)
    {
        foreach (var material in GameEditor.Project.CurrentLevel.GetObjects(screenPosition.ToVector()).OfType<IMaterial>())
        {
            material.Effect = GameEditor.Content.Load<Effect>(effectName);
            material.Effect.Tag = effectName;
        }
    }

    private void DropSound(System.Drawing.Point screenPosition, AssetListItem assetListItem)
    {
        if (assetListItem.Type != AssetMonitor.AssetType.Sound) return;

        var soundEmitters = GameEditor.Project.CurrentLevel.GetObjects(screenPosition.ToVector()).OfType<ISoundEmitter>();
        if (!soundEmitters.Any()) return;

        void SelectSound(object sender, EventArgs e)
        {
            var menuItem = sender as ToolStripMenuItem;
            var assetListItem = menuItem.Tag as AssetListItem;

            var sound = Enum.Parse<ISoundEmitter.Sound>(menuItem.Text);
            foreach (var soundEmitter in soundEmitters)
            {
                soundEmitter.SoundEffects[sound] = ISoundEmitter.CreateSoundEffect(GameEditor.Content, assetListItem.Name);
            }
        }

        ContextMenuStrip contextMenuStrip = new();
        foreach (var sound in Enum.GetNames<ISoundEmitter.Sound>())
        {
            ToolStripMenuItem menuItem = new(sound);
            menuItem.Click += SelectSound;
            menuItem.Tag = assetListItem;
            contextMenuStrip.Items.Add(menuItem);
        }
        contextMenuStrip.Show(screenPosition);
    }
    #endregion
    #endregion

    #region Prefabs
    private void createToolStripMenuItem1_Click(object sender, EventArgs e)
    {
        var selectedObjects = GameEditor.Project.CurrentLevel.SelectedObjects.OfType<ModelRenderer>();
        if (!selectedObjects.Any())
        {
            MessageBox.Show("An object must be selected to create a prefab.", "No Object Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        ModelRenderer modelRenderer = selectedObjects.First();
        string rawModelName = new Regex(@" \(\d+\)$").Replace(modelRenderer.Name, "");
        string baseFilePath = Path.Combine(GameEditor.Project.Folder, rawModelName);
        int duplicateCount = 0;
        string GetFullFilePath(string prefix) => prefix + (duplicateCount > 0 ? $" ({duplicateCount})" : "") + PrefabExtension;

        while (File.Exists(GetFullFilePath(baseFilePath))) duplicateCount++;

        using FileStream fileStream = File.Open(GetFullFilePath(baseFilePath), FileMode.Create);
        using BinaryWriter binaryWriter = new(fileStream, Encoding.UTF8, false);
        modelRenderer.Serialize(binaryWriter);

        prefabListBox.Items.Add(new PrefabListItem(GetFullFilePath(rawModelName)));
    }

    private void prefabListBox_MouseDown(object sender, MouseEventArgs e)
    {
        if (prefabListBox.Items.Count == 0 || prefabListBox.SelectedIndex == -1) return;
        if (prefabListBox.SelectedItem is not PrefabListItem prefabListItem) return;

        _ = DoDragDrop(prefabListItem, DragDropEffects.Copy);
    }
    #endregion

    #region Hierarchy Selection
    private void levelListBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (levelListBox.Items.Count == 0) return;

        GameEditor.Project.CurrentLevel.ClearSelectedObjects();
        int index = levelListBox.SelectedIndex;
        if (index == -1) return;

        GameEditor.Project.CurrentLevel.SelectObject((levelListBox.Items[index] as LevelListItem).Model);
    }
    #endregion

    #region Drag Into Game
    private void GamePanel_DragOver(object sender, DragEventArgs e)
    {
        if (e.Data.GetData(typeof(AssetListItem)) is AssetListItem draggedAsset)
        {
            e.Effect = dragDropAssetTypes.Contains(draggedAsset.Type) ? DragDropEffects.Copy : DragDropEffects.None;
        }
        else if (e.Data.GetData(typeof(PrefabListItem)) is PrefabListItem)
        {
            e.Effect = DragDropEffects.Copy;
        }
    }

    private void GamePanel_DragDrop(object sender, DragEventArgs e)
    {
        if (e.Data.GetData(typeof(AssetListItem)) is AssetListItem droppedAsset)
        {
            var mousePosition = (FromHandle(GameEditor.Window.Handle) as Form).PointToClient(new(e.X, e.Y));
            switch (droppedAsset.Type)
            {
                case AssetMonitor.AssetType.Model: DropModel(droppedAsset.Name); break;
                case AssetMonitor.AssetType.Texture: DropTexture(mousePosition, droppedAsset.Name); break;
                case AssetMonitor.AssetType.Effect: DropEffect(mousePosition, droppedAsset.Name); break;
                case AssetMonitor.AssetType.Sound: DropSound(mousePosition, droppedAsset); break;
            }
        }
        else if (e.Data.GetData(typeof(PrefabListItem)) is PrefabListItem droppedPrefab)
        {
            string filePath = Path.Combine(GameEditor.Project.Folder, droppedPrefab.Name);
            using FileStream fileStream = File.Open(filePath, FileMode.Open);
            using BinaryReader binaryReader = new(fileStream, Encoding.UTF8, false);

            ModelRenderer modelRenderer = new();
            modelRenderer.Deserialize(binaryReader, GameEditor.Content);
            GameEditor.Project.CurrentLevel?.AddModel(modelRenderer);

            levelListBox.Items.Add(new LevelListItem(modelRenderer));
        }
    }
    #endregion
}