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
        menuStrip1 = new System.Windows.Forms.MenuStrip();
        fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        pRjeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        createToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        assetsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        statusStrip1 = new System.Windows.Forms.StatusStrip();
        toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
        splitContainer = new System.Windows.Forms.SplitContainer();
        splitContainer1 = new System.Windows.Forms.SplitContainer();
        propertyGrid = new System.Windows.Forms.PropertyGrid();
        assetListBox = new System.Windows.Forms.ListBox();
        menuStrip1.SuspendLayout();
        statusStrip1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
        splitContainer.Panel2.SuspendLayout();
        splitContainer.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
        splitContainer1.Panel1.SuspendLayout();
        splitContainer1.Panel2.SuspendLayout();
        splitContainer1.SuspendLayout();
        SuspendLayout();
        // 
        // menuStrip1
        // 
        menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
        menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { fileToolStripMenuItem, assetsToolStripMenuItem });
        menuStrip1.Location = new System.Drawing.Point(0, 0);
        menuStrip1.Name = "menuStrip1";
        menuStrip1.Size = new System.Drawing.Size(1038, 28);
        menuStrip1.TabIndex = 0;
        menuStrip1.Text = "menuStrip1";
        // 
        // fileToolStripMenuItem
        // 
        fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { pRjeToolStripMenuItem, exitToolStripMenuItem });
        fileToolStripMenuItem.Name = "fileToolStripMenuItem";
        fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
        fileToolStripMenuItem.Text = "File";
        // 
        // pRjeToolStripMenuItem
        // 
        pRjeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { createToolStripMenuItem, saveToolStripMenuItem, loadToolStripMenuItem });
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
        assetsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { importToolStripMenuItem });
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
        // statusStrip1
        // 
        statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
        statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripStatusLabel1 });
        statusStrip1.Location = new System.Drawing.Point(0, 527);
        statusStrip1.Name = "statusStrip1";
        statusStrip1.Size = new System.Drawing.Size(1038, 26);
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
        splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
        splitContainer.Location = new System.Drawing.Point(0, 28);
        splitContainer.Name = "splitContainer";
        // 
        // splitContainer.Panel1
        // 
        splitContainer.Panel1.SizeChanged += splitContainer_Panel1_SizeChanged;
        // 
        // splitContainer.Panel2
        // 
        splitContainer.Panel2.Controls.Add(splitContainer1);
        splitContainer.Size = new System.Drawing.Size(1038, 499);
        splitContainer.SplitterDistance = 732;
        splitContainer.TabIndex = 2;
        // 
        // splitContainer1
        // 
        splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
        splitContainer1.Location = new System.Drawing.Point(0, 0);
        splitContainer1.Name = "splitContainer1";
        splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
        // 
        // splitContainer1.Panel1
        // 
        splitContainer1.Panel1.Controls.Add(propertyGrid);
        // 
        // splitContainer1.Panel2
        // 
        splitContainer1.Panel2.Controls.Add(assetListBox);
        splitContainer1.Size = new System.Drawing.Size(302, 499);
        splitContainer1.SplitterDistance = 259;
        splitContainer1.TabIndex = 1;
        // 
        // propertyGrid
        // 
        propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
        propertyGrid.Location = new System.Drawing.Point(0, 0);
        propertyGrid.Name = "propertyGrid";
        propertyGrid.Size = new System.Drawing.Size(302, 259);
        propertyGrid.TabIndex = 1;
        // 
        // assetListBox
        // 
        assetListBox.Dock = System.Windows.Forms.DockStyle.Fill;
        assetListBox.FormattingEnabled = true;
        assetListBox.ItemHeight = 20;
        assetListBox.Location = new System.Drawing.Point(0, 0);
        assetListBox.Name = "assetListBox";
        assetListBox.Size = new System.Drawing.Size(302, 236);
        assetListBox.TabIndex = 0;
        // 
        // FormEditor
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(1038, 553);
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
        splitContainer.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
        splitContainer.ResumeLayout(false);
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
	public System.Windows.Forms.SplitContainer splitContainer;
	private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
	private System.Windows.Forms.ToolStripMenuItem pRjeToolStripMenuItem;
	private System.Windows.Forms.ToolStripMenuItem createToolStripMenuItem;
	private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
	private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
    private System.Windows.Forms.SplitContainer splitContainer1;
    public System.Windows.Forms.PropertyGrid propertyGrid;
    private System.Windows.Forms.ListBox assetListBox;
    private System.Windows.Forms.ToolStripMenuItem assetsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
}