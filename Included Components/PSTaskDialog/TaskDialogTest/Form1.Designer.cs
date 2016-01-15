namespace TaskDialog
{
  partial class Form1
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
      this.button1 = new System.Windows.Forms.Button();
      this.lbResult = new System.Windows.Forms.Label();
      this.button2 = new System.Windows.Forms.Button();
      this.button3 = new System.Windows.Forms.Button();
      this.button4 = new System.Windows.Forms.Button();
      this.button5 = new System.Windows.Forms.Button();
      this.checkBox1 = new System.Windows.Forms.CheckBox();
      this.label1 = new System.Windows.Forms.Label();
      this.edWidth = new System.Windows.Forms.TextBox();
      this.btAsterisk = new System.Windows.Forms.Button();
      this.btQuestion = new System.Windows.Forms.Button();
      this.btHand = new System.Windows.Forms.Button();
      this.btExclamation = new System.Windows.Forms.Button();
      this.btBeep = new System.Windows.Forms.Button();
      this.label2 = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(12, 12);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(158, 45);
      this.button1.TabIndex = 0;
      this.button1.Text = "Full Example";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // lbResult
      // 
      this.lbResult.Location = new System.Drawing.Point(224, 12);
      this.lbResult.Name = "lbResult";
      this.lbResult.Size = new System.Drawing.Size(158, 61);
      this.lbResult.TabIndex = 1;
      this.lbResult.Text = "lbResult";
      // 
      // button2
      // 
      this.button2.Location = new System.Drawing.Point(12, 63);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(158, 45);
      this.button2.TabIndex = 1;
      this.button2.Text = "MessageBox Example";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new System.EventHandler(this.button2_Click);
      // 
      // button3
      // 
      this.button3.Location = new System.Drawing.Point(12, 114);
      this.button3.Name = "button3";
      this.button3.Size = new System.Drawing.Size(158, 45);
      this.button3.TabIndex = 2;
      this.button3.Text = "Simple MessageBox Example";
      this.button3.UseVisualStyleBackColor = true;
      this.button3.Click += new System.EventHandler(this.button3_Click);
      // 
      // button4
      // 
      this.button4.Location = new System.Drawing.Point(12, 165);
      this.button4.Name = "button4";
      this.button4.Size = new System.Drawing.Size(158, 45);
      this.button4.TabIndex = 3;
      this.button4.Text = "RadioBox Example";
      this.button4.UseVisualStyleBackColor = true;
      this.button4.Click += new System.EventHandler(this.button4_Click);
      // 
      // button5
      // 
      this.button5.Location = new System.Drawing.Point(12, 216);
      this.button5.Name = "button5";
      this.button5.Size = new System.Drawing.Size(158, 45);
      this.button5.TabIndex = 4;
      this.button5.Text = "CommandBox Example";
      this.button5.UseVisualStyleBackColor = true;
      this.button5.Click += new System.EventHandler(this.button5_Click);
      // 
      // checkBox1
      // 
      this.checkBox1.AutoSize = true;
      this.checkBox1.Location = new System.Drawing.Point(12, 287);
      this.checkBox1.Name = "checkBox1";
      this.checkBox1.Size = new System.Drawing.Size(131, 17);
      this.checkBox1.TabIndex = 5;
      this.checkBox1.Text = "Force Emulation Mode";
      this.checkBox1.UseVisualStyleBackColor = true;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(162, 288);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(109, 13);
      this.label1.TabIndex = 7;
      this.label1.Text = "Emulated Form Width";
      // 
      // edWidth
      // 
      this.edWidth.Location = new System.Drawing.Point(277, 285);
      this.edWidth.Name = "edWidth";
      this.edWidth.Size = new System.Drawing.Size(74, 21);
      this.edWidth.TabIndex = 11;
      this.edWidth.Text = "450";
      // 
      // btAsterisk
      // 
      this.btAsterisk.Location = new System.Drawing.Point(276, 125);
      this.btAsterisk.Name = "btAsterisk";
      this.btAsterisk.Size = new System.Drawing.Size(75, 23);
      this.btAsterisk.TabIndex = 6;
      this.btAsterisk.Text = "Asterisk";
      this.btAsterisk.UseVisualStyleBackColor = true;
      this.btAsterisk.Click += new System.EventHandler(this.btAsterisk_Click);
      // 
      // btQuestion
      // 
      this.btQuestion.Location = new System.Drawing.Point(276, 238);
      this.btQuestion.Name = "btQuestion";
      this.btQuestion.Size = new System.Drawing.Size(75, 23);
      this.btQuestion.TabIndex = 10;
      this.btQuestion.Text = "Question";
      this.btQuestion.UseVisualStyleBackColor = true;
      this.btQuestion.Click += new System.EventHandler(this.btQuestion_Click);
      // 
      // btHand
      // 
      this.btHand.Location = new System.Drawing.Point(276, 212);
      this.btHand.Name = "btHand";
      this.btHand.Size = new System.Drawing.Size(75, 23);
      this.btHand.TabIndex = 9;
      this.btHand.Text = "Hand";
      this.btHand.UseVisualStyleBackColor = true;
      this.btHand.Click += new System.EventHandler(this.btHand_Click);
      // 
      // btExclamation
      // 
      this.btExclamation.Location = new System.Drawing.Point(276, 183);
      this.btExclamation.Name = "btExclamation";
      this.btExclamation.Size = new System.Drawing.Size(75, 23);
      this.btExclamation.TabIndex = 8;
      this.btExclamation.Text = "Exclamation";
      this.btExclamation.UseVisualStyleBackColor = true;
      this.btExclamation.Click += new System.EventHandler(this.btExclamation_Click);
      // 
      // btBeep
      // 
      this.btBeep.Location = new System.Drawing.Point(276, 154);
      this.btBeep.Name = "btBeep";
      this.btBeep.Size = new System.Drawing.Size(75, 23);
      this.btBeep.TabIndex = 7;
      this.btBeep.Text = "Beep";
      this.btBeep.UseVisualStyleBackColor = true;
      this.btBeep.Click += new System.EventHandler(this.btBeep_Click);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(274, 109);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(80, 13);
      this.label2.TabIndex = 14;
      this.label2.Text = "System Sounds";
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(394, 316);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.btBeep);
      this.Controls.Add(this.btExclamation);
      this.Controls.Add(this.btHand);
      this.Controls.Add(this.btQuestion);
      this.Controls.Add(this.btAsterisk);
      this.Controls.Add(this.edWidth);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.checkBox1);
      this.Controls.Add(this.button5);
      this.Controls.Add(this.button4);
      this.Controls.Add(this.button3);
      this.Controls.Add(this.button2);
      this.Controls.Add(this.lbResult);
      this.Controls.Add(this.button1);
      this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Name = "Form1";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Form1";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Label lbResult;
    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.Button button3;
    private System.Windows.Forms.Button button4;
    private System.Windows.Forms.Button button5;
    private System.Windows.Forms.CheckBox checkBox1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox edWidth;
    private System.Windows.Forms.Button btAsterisk;
    private System.Windows.Forms.Button btQuestion;
    private System.Windows.Forms.Button btHand;
    private System.Windows.Forms.Button btExclamation;
    private System.Windows.Forms.Button btBeep;
    private System.Windows.Forms.Label label2;

  }
}

