using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Editor.Editor;

public partial class FormEditor : Form
{
	public GameEditor GameEditor { get; set; }

	public FormEditor()
	{
		InitializeComponent();
	}

	private void exitToolStripMenuItem_Click(object sender, EventArgs e)
	{
		GameEditor.Exit();
	}

	private void FormEditor_SizeChanged(object sender, EventArgs e)
	{
		GameEditor?.AdjustAspectRatio();
	}

	private void splitContainer_Panel1_SizeChanged(object sender, EventArgs e)
	{
		GameEditor?.AdjustAspectRatio();
	}

	private void createToolStripMenuItem_Click(object sender, EventArgs e)
	{
		SaveFileDialog saveFileDialog = new();
		if (saveFileDialog.ShowDialog() != DialogResult.OK) return;

		GameEditor.Project = new(GameEditor.Content, saveFileDialog.FileName);
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
}
