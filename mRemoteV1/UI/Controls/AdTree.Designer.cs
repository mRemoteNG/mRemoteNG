namespace mRemoteNG.UI.Controls
{
    partial class AdTree
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdTree));
            this.TvAd = new System.Windows.Forms.TreeView();
            this.ImglTree = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // TvAd
            // 
            this.TvAd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TvAd.Location = new System.Drawing.Point(0, 0);
            this.TvAd.Name = "TvAd";
            this.TvAd.Size = new System.Drawing.Size(800, 450);
            this.TvAd.TabIndex = 0;
            this.TvAd.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.TvAD_AfterExpand);
            this.TvAd.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TvAD_AfterSelect);
            // 
            // ImglTree
            // 
            this.ImglTree.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImglTree.ImageStream")));
            this.ImglTree.TransparentColor = System.Drawing.Color.Transparent;
            this.ImglTree.Images.SetKeyName(0, "Root.png");
            this.ImglTree.Images.SetKeyName(1, "OU.png");
            this.ImglTree.Images.SetKeyName(2, "Folder.png");
            this.ImglTree.Images.SetKeyName(3, "Question.png");
            // 
            // AdTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.TvAd);
            this.Name = "AdTree";
            this.Text = "AdTree2";
            this.Load += new System.EventHandler(this.ADtree_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView TvAd;
        private System.Windows.Forms.ImageList ImglTree;
    }
}