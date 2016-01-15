using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TaskDialog
{
  public partial class Form1 : Form
  {
    //--------------------------------------------------------------------------------
    public Form1()
    {
      InitializeComponent();
    }

    //--------------------------------------------------------------------------------
    void UpdateResult(DialogResult res)
    {
      lbResult.Text = "Result : " + Enum.GetName(typeof(DialogResult), res) + Environment.NewLine +
                      "RadioButtonIndex : " + PSTaskDialog.cTaskDialog.RadioButtonResult.ToString() + Environment.NewLine +
                      "CommandButtonIndex : " + PSTaskDialog.cTaskDialog.CommandButtonResult.ToString() + Environment.NewLine +
                      "Verify CheckBox : " + (PSTaskDialog.cTaskDialog.VerificationChecked ? "true" : "false");
    }

    //--------------------------------------------------------------------------------
    private void button1_Click(object sender, EventArgs e)
    {
      PSTaskDialog.cTaskDialog.ForceEmulationMode = checkBox1.Checked;
      try { PSTaskDialog.cTaskDialog.EmulatedFormWidth = Convert.ToInt32(edWidth.Text); }
      catch (Exception) { PSTaskDialog.cTaskDialog.EmulatedFormWidth = 450; }

      DialogResult res =
        PSTaskDialog.cTaskDialog.ShowTaskDialogBox(this,
                                                   "Task Dialog Title",
                                                   "The main instruction text for the TaskDialog goes here.",
                                                   "The content text for the task dialog is shown here and the text will automatically wrap as needed.",
                                                   "Any expanded content text for the task dialog is shown here and the text will automatically wrap as needed.",
                                                   "Optional footer text with an icon can be included",
                                                   "Don't show me this message again",
                                                   "Radio Option 1|Radio Option 2|Radio Option 3",
                                                   "Command &Button 1|Command Button 2\nLine 2\nLine 3|Command Button 3",
                                                   PSTaskDialog.eTaskDialogButtons.OKCancel,
                                                   PSTaskDialog.eSysIcons.Information,
                                                   PSTaskDialog.eSysIcons.Warning);
      UpdateResult(res);  
    }

    //--------------------------------------------------------------------------------
    private void button2_Click(object sender, EventArgs e)
    {
      PSTaskDialog.cTaskDialog.ForceEmulationMode = checkBox1.Checked;
      try { PSTaskDialog.cTaskDialog.EmulatedFormWidth = Convert.ToInt32(edWidth.Text); }
      catch (Exception) { PSTaskDialog.cTaskDialog.EmulatedFormWidth = 450; }

      DialogResult res =
        PSTaskDialog.cTaskDialog.MessageBox(this,
                                            "MessageBox Title",
                                            "The main instruction text for the message box is shown here.",
                                            "The content text for the message box is shown here and the text will automatically wrap as needed.",
                                            "Any expanded content text for the message box is shown here and the text will automatically wrap as needed.",
                                            "Optional footer text with an icon can be included",
                                            "ARRGHH! Don't show me this again!!!!",
                                            PSTaskDialog.eTaskDialogButtons.YesNoCancel,
                                            PSTaskDialog.eSysIcons.Information,
                                            PSTaskDialog.eSysIcons.Error);
      UpdateResult(res);
    }

    //--------------------------------------------------------------------------------
    private void button3_Click(object sender, EventArgs e)
    {
      PSTaskDialog.cTaskDialog.ForceEmulationMode = checkBox1.Checked;
      try { PSTaskDialog.cTaskDialog.EmulatedFormWidth = Convert.ToInt32(edWidth.Text); }
      catch (Exception) { PSTaskDialog.cTaskDialog.EmulatedFormWidth = 450; }

      DialogResult res =
        PSTaskDialog.cTaskDialog.MessageBox(this,
                                            "MessageBox Title",
                                            "The main instruction text for the message box is shown here.",
                                            "The content text for the message box is shown here and the text will automatically wrap as needed.",
                                            PSTaskDialog.eTaskDialogButtons.OK,
                                            PSTaskDialog.eSysIcons.Warning);
      UpdateResult(res);
    }

    //--------------------------------------------------------------------------------
    private void button4_Click(object sender, EventArgs e)
    {
      PSTaskDialog.cTaskDialog.ForceEmulationMode = checkBox1.Checked;
      try { PSTaskDialog.cTaskDialog.EmulatedFormWidth = Convert.ToInt32(edWidth.Text); }
      catch (Exception) { PSTaskDialog.cTaskDialog.EmulatedFormWidth = 450; }

      int idx = 
      PSTaskDialog.cTaskDialog.ShowRadioBox(this,
                                            "RadioBox Title",
                                            "The main instruction text for the radiobox is shown here.",
                                            "The content text for the radiobox is shown here and the text will automatically wrap as needed.",
                                            "Any expanded content text for the radiobox is shown here and the text will automatically wrap as needed.",
                                            "Optional footer text with an icon can be included",
                                            "Don't show this message again",
                                            "Radio Option 1|Radio Option 2|Radio Option 3|Radio Option 4|Radio Option 5",
                                            PSTaskDialog.eSysIcons.Information,
                                            PSTaskDialog.eSysIcons.Warning);

      lbResult.Text = "ShowRadioBox return value : " + idx.ToString();
    }

    //--------------------------------------------------------------------------------
    private void button5_Click(object sender, EventArgs e)
    {
      PSTaskDialog.cTaskDialog.ForceEmulationMode = checkBox1.Checked;
      try { PSTaskDialog.cTaskDialog.EmulatedFormWidth = Convert.ToInt32(edWidth.Text); }
      catch (Exception) { PSTaskDialog.cTaskDialog.EmulatedFormWidth = 450; }

      int idx =
      PSTaskDialog.cTaskDialog.ShowCommandBox(this,
                                              "CommandBox Title",
                                              "The main instruction text for the commandbox is shown here.",
                                              "The content text for the commandbox is shown here and the text will automatically wrap as needed.",
                                              "Any expanded content text for the commandbox is shown here and the text will automatically wrap as needed.",
                                              "Optional footer text with an icon can be included",
                                              "Don't show this message again",
                                              "Command Button 1|Command Button 2|Command Button 3|Command Button 4",
                                              true,
                                              PSTaskDialog.eSysIcons.Information,
                                              PSTaskDialog.eSysIcons.Warning);

      lbResult.Text = "ShowCommandBox return value : " + idx.ToString();
    }

    private void btAsterisk_Click(object sender, EventArgs e)
    {
      System.Media.SystemSounds.Asterisk.Play(); 
    }

    private void btQuestion_Click(object sender, EventArgs e)
    {
      System.Media.SystemSounds.Question.Play(); 
    }

    private void btHand_Click(object sender, EventArgs e)
    {
      System.Media.SystemSounds.Hand.Play();
    }

    private void btExclamation_Click(object sender, EventArgs e)
    {
      System.Media.SystemSounds.Exclamation.Play();
    }

    private void btBeep_Click(object sender, EventArgs e)
    {
      System.Media.SystemSounds.Beep.Play();
    }

    //--------------------------------------------------------------------------------
  }
}