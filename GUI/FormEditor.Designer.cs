using System.Windows.Forms;

namespace GUI.Editor;

partial class FormEditor
{
	/// <summary>
	/// Required designer variable.
	/// </summary>
	private System.ComponentModel.IContainer components = null;

	/// <summary>
	/// Clean up any resources being used.
	/// </summary>
	/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
	protected override void Dispose(bool disposing)
	{
		if (disposing && (components != null))
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        menuStrip1 = new MenuStrip();
        fileToolStripMenuItem = new ToolStripMenuItem();
        pRjeToolStripMenuItem = new ToolStripMenuItem();
        createToolStripMenuItem = new ToolStripMenuItem();
        saveToolStripMenuItem = new ToolStripMenuItem();
        loadToolStripMenuItem = new ToolStripMenuItem();
        exitToolStripMenuItem = new ToolStripMenuItem();
        assetsToolStripMenuItem = new ToolStripMenuItem();
        importToolStripMenuItem = new ToolStripMenuItem();
        prefabsToolStripMenuItem = new ToolStripMenuItem();
        createToolStripMenuItem1 = new ToolStripMenuItem();
        statusStrip1 = new StatusStrip();
        toolStripStatusLabel1 = new ToolStripStatusLabel();
        splitContainer = new SplitContainer();
        splitContainer2 = new SplitContainer();
        splitContainer3 = new SplitContainer();
        levelListBox = new ListBox();
        prefabListBox = new ListBox();
        splitContainer1 = new SplitContainer();
        propertyGrid = new PropertyGrid();
        assetListBox = new ListBox();
        menuStrip1.SuspendLayout();
        statusStrip1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
        splitContainer.Panel1.SuspendLayout();
        splitContainer.Panel2.SuspendLayout();
        splitContainer.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
        splitContainer2.Panel1.SuspendLayout();
        splitContainer2.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitContainer3).BeginInit();
        splitContainer3.Panel1.SuspendLayout();
        splitContainer3.Panel2.SuspendLayout();
        splitContainer3.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
        splitContainer1.Panel1.SuspendLayout();
        splitContainer1.Panel2.SuspendLayout();
        splitContainer1.SuspendLayout();
        SuspendLayout();
        // 
        // menuStrip1
        // 
        menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
        menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, assetsToolStripMenuItem, prefabsToolStripMenuItem });
        menuStrip1.Location = new System.Drawing.Point(0, 0);
        menuStrip1.Name = "menuStrip1";
        menuStrip1.Size = new System.Drawing.Size(1286, 28);
        menuStrip1.TabIndex = 0;
        menuStrip1.Text = "menuStrip1";
        // 
        // fileToolStripMenuItem
        // 
        fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { pRjeToolStripMenuItem, exitToolStripMenuItem });
        fileToolStripMenuItem.Name = "fileToolStripMenuItem";
        fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
        fileToolStripMenuItem.Text = "File";
        // 
        // pRjeToolStripMenuItem
        // 
        pRjeToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { createToolStripMenuItem, saveToolStripMenuItem, loadToolStripMenuItem });
        pRjeToolStripMenuItem.Name = "pRjeToolStripMenuItem";
        pRjeToolStripMenuItem.Size = new System.Drawing.Size(138, 26);
        pRjeToolStripMenuItem.Text = "Project";
        // 
        // createToolStripMenuItem
        // 
        createToolStripMenuItem.Name = "createToolStripMenuItem";
        createToolStripMenuItem.Size = new System.Drawing.Size(135, 26);
        createToolStripMenuItem.Text = "Create";
        createToolStripMenuItem.Click += createToolStripMenuItem_Click;
        // 
        // saveToolStripMenuItem
        // 
        saveToolStripMenuItem.Name = "saveToolStripMenuItem";
        saveToolStripMenuItem.Size = new System.Drawing.Size(135, 26);
        saveToolStripMenuItem.Text = "Save";
        saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
        // 
        // loadToolStripMenuItem
        // 
        loadToolStripMenuItem.Name = "loadToolStripMenuItem";
        loadToolStripMenuItem.Size = new System.Drawing.Size(135, 26);
        loadToolStripMenuItem.Text = "Load";
        loadToolStripMenuItem.Click += loadToolStripMenuItem_Click;
        // 
        // exitToolStripMenuItem
        // 
        exitToolStripMenuItem.Name = "exitToolStripMenuItem";
        exitToolStripMenuItem.Size = new System.Drawing.Size(138, 26);
        exitToolStripMenuItem.Text = "Exit";
        exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
        // 
        // assetsToolStripMenuItem
        // 
        assetsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { importToolStripMenuItem });
        assetsToolStripMenuItem.Name = "assetsToolStripMenuItem";
        assetsToolStripMenuItem.Size = new System.Drawing.Size(64, 24);
        assetsToolStripMenuItem.Text = "Assets";
        // 
        // importToolStripMenuItem
        // 
        importToolStripMenuItem.Name = "importToolStripMenuItem";
        importToolStripMenuItem.Size = new System.Drawing.Size(137, 26);
        importToolStripMenuItem.Text = "Import";
        importToolStripMenuItem.Click += importToolStripMenuItem_Click;
        // 
        // prefabsToolStripMenuItem
        // 
        prefabsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { createToolStripMenuItem1 });
        prefabsToolStripMenuItem.Name = "prefabsToolStripMenuItem";
        prefabsToolStripMenuItem.Size = new System.Drawing.Size(72, 24);
        prefabsToolStripMenuItem.Text = "Prefabs";
        // 
        // createToolStripMenuItem1
        // 
        createToolStripMenuItem1.Name = "createToolStripMenuItem1";
        createToolStripMenuItem1.Size = new System.Drawing.Size(224, 26);
        createToolStripMenuItem1.Text = "Create";
        createToolStripMenuItem1.Click += createToolStripMenuItem1_Click;
        // 
        // statusStrip1
        // 
        statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
        statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
        statusStrip1.Location = new System.Drawing.Point(0, 527);
        statusStrip1.Name = "statusStrip1";
        statusStrip1.Size = new System.Drawing.Size(1286, 26);
        statusStrip1.TabIndex = 1;
        statusStrip1.Text = "statusStrip1";
        // 
        // toolStripStatusLabel1
        // 
        toolStripStatusLabel1.Name = "toolStripStatusLabel1";
        toolStripStatusLabel1.Size = new System.Drawing.Size(67, 20);
        toolStripStatusLabel1.Text = "Toolstrip";
        // 
        // splitContainer
        // 
        splitContainer.Dock = DockStyle.Fill;
        splitContainer.Location = new System.Drawing.Point(0, 28);
        splitContainer.Name = "splitContainer";
        // 
        // splitContainer.Panel1
        // 
        splitContainer.Panel1.AllowDrop = true;
        splitContainer.Panel1.Controls.Add(splitContainer2);
        splitContainer.Panel1.SizeChanged += splitContainer_Panel1_SizeChanged;
        // 
        // splitContainer.Panel2
        // 
        splitContainer.Panel2.Controls.Add(splitContainer1);
        splitContainer.Size = new System.Drawing.Size(1286, 499);
        splitContainer.SplitterDistance = 970;
        splitContainer.TabIndex = 2;
        // 
        // splitContainer2
        // 
        splitContainer2.Dock = DockStyle.Fill;
        splitContainer2.Location = new System.Drawing.Point(0, 0);
        splitContainer2.Name = "splitContainer2";
        // 
        // splitContainer2.Panel1
        // 
        splitContainer2.Panel1.Controls.Add(splitContainer3);
        // 
        // splitContainer2.Panel2
        // 
        splitContainer2.Panel2.AllowDrop = true;
        splitContainer2.Panel2.DragDrop += GamePanel_DragDrop;
        splitContainer2.Panel2.DragOver += GamePanel_DragOver;
        splitContainer2.Size = new System.Drawing.Size(970, 499);
        splitContainer2.SplitterDistance = 246;
        splitContainer2.TabIndex = 0;
        // 
        // splitContainer3
        // 
        splitContainer3.Dock = DockStyle.Fill;
        splitContainer3.Location = new System.Drawing.Point(0, 0);
        splitContainer3.Name = "splitContainer3";
        splitContainer3.Orientation = Orientation.Horizontal;
        // 
        // splitContainer3.Panel1
        // 
        splitContainer3.Panel1.Controls.Add(levelListBox);
        // 
        // splitContainer3.Panel2
        // 
        splitContainer3.Panel2.Controls.Add(prefabListBox);
        splitContainer3.Size = new System.Drawing.Size(246, 499);
        splitContainer3.SplitterDistance = 235;
        splitContainer3.TabIndex = 0;
        // 
        // levelListBox
        // 
        levelListBox.Dock = DockStyle.Fill;
        levelListBox.FormattingEnabled = true;
        levelListBox.ItemHeight = 20;
        levelListBox.Location = new System.Drawing.Point(0, 0);
        levelListBox.Name = "levelListBox";
        levelListBox.Size = new System.Drawing.Size(246, 235);
        levelListBox.TabIndex = 0;
        levelListBox.SelectedIndexChanged += levelListBox_SelectedIndexChanged;
        // 
        // prefabListBox
        // 
        prefabListBox.Dock = DockStyle.Fill;
        prefabListBox.FormattingEnabled = true;
        prefabListBox.ItemHeight = 20;
        prefabListBox.Location = new System.Drawing.Point(0, 0);
        prefabListBox.Name = "prefabListBox";
        prefabListBox.Size = new System.Drawing.Size(246, 260);
        prefabListBox.TabIndex = 0;
        prefabListBox.MouseDown += prefabListBox_MouseDown;
        // 
        // splitContainer1
        // 
        splitContainer1.Dock = DockStyle.Fill;
        splitContainer1.Location = new System.Drawing.Point(0, 0);
        splitContainer1.Name = "splitContainer1";
        splitContainer1.Orientation = Orientation.Horizontal;
        // 
        // splitContainer1.Panel1
        // 
        splitContainer1.Panel1.Controls.Add(propertyGrid);
        // 
        // splitContainer1.Panel2
        // 
        splitContainer1.Panel2.Controls.Add(assetListBox);
        splitContainer1.Size = new System.Drawing.Size(312, 499);
        splitContainer1.SplitterDistance = 259;
        splitContainer1.TabIndex = 1;
        // 
        // propertyGrid
        // 
        propertyGrid.Dock = DockStyle.Fill;
        propertyGrid.Location = new System.Drawing.Point(0, 0);
        propertyGrid.Name = "propertyGrid";
        propertyGrid.Size = new System.Drawing.Size(312, 259);
        propertyGrid.TabIndex = 1;
        // 
        // assetListBox
        // 
        assetListBox.Dock = DockStyle.Fill;
        assetListBox.FormattingEnabled = true;
        assetListBox.ItemHeight = 20;
        assetListBox.Location = new System.Drawing.Point(0, 0);
        assetListBox.Name = "assetListBox";
        assetListBox.Size = new System.Drawing.Size(312, 236);
        assetListBox.TabIndex = 0;
        assetListBox.MouseDown += assetListBox_MouseDown;
        // 
        // FormEditor
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(1286, 553);
        Controls.Add(splitContainer);
        Controls.Add(statusStrip1);
        Controls.Add(menuStrip1);
        MainMenuStrip = menuStrip1;
        Name = "FormEditor";
        Text = "Our Cool Editor";
        FormClosing += FormEditor_FormClosing;
        SizeChanged += FormEditor_SizeChanged;
        menuStrip1.ResumeLayout(false);
        menuStrip1.PerformLayout();
        statusStrip1.ResumeLayout(false);
        statusStrip1.PerformLayout();
        splitContainer.Panel1.ResumeLayout(false);
        splitContainer.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
        splitContainer.ResumeLayout(false);
        splitContainer2.Panel1.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
        splitContainer2.ResumeLayout(false);
        splitContainer3.Panel1.ResumeLayout(false);
        splitContainer3.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitContainer3).EndInit();
        splitContainer3.ResumeLayout(false);
        splitContainer1.Panel1.ResumeLayout(false);
        splitContainer1.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
        splitContainer1.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private System.Windows.Forms.MenuStrip menuStrip1;
	private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
	private System.Windows.Forms.StatusStrip statusStrip1;
	private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
	private System.Windows.Forms.SplitContainer splitContainer;
	private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
	private System.Windows.Forms.ToolStripMenuItem pRjeToolStripMenuItem;
	private System.Windows.Forms.ToolStripMenuItem createToolStripMenuItem;
	private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
	private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
    private System.Windows.Forms.SplitContainer splitContainer1;
    private System.Windows.Forms.PropertyGrid propertyGrid;
    private System.Windows.Forms.ListBox assetListBox;
    private System.Windows.Forms.ToolStripMenuItem assetsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
    private System.Windows.Forms.SplitContainer splitContainer2;
    private System.Windows.Forms.SplitContainer splitContainer3;
    private System.Windows.Forms.ListBox levelListBox;
    private System.Windows.Forms.ListBox prefabListBox;
    private ToolStripMenuItem prefabsToolStripMenuItem;
    private ToolStripMenuItem createToolStripMenuItem1;

    public SplitterPanel GamePanel => splitContainer2.Panel2;
    public PropertyGrid Inspector => propertyGrid;
    public ListBox Hierarchy => levelListBox;
}