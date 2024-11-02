using Editor.Editor;
using Editor.Engine;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace GUI.Editor;

public partial class FormEditor : Form
{
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

    #region File/Project
    private void createToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SaveFileDialog saveFileDialog = new();
        if (saveFileDialog.ShowDialog() != DialogResult.OK) return;

        GameEditor.Project = new(GameEditor.Content, GameEditor.GraphicsDevice);
        GameEditor.Project.SetPath(saveFileDialog.FileName);
        Text = "Our Cool Editor - " + GameEditor.Project.Name;
        GameEditor.AdjustAspectRatio();
        saveToolStripMenuItem_Click(sender, e);
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
            var assets = GameEditor.Project.AssetMonitor.Assets;
            if (!assets.ContainsKey(AssetMonitor.AssetTypes.Model)) return;

            foreach (string asset in assets[AssetMonitor.AssetTypes.Model])
            {
                assetListBox.Items.Add(asset);
            }
        });
    } 
    #endregion
}