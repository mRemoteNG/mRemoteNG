using System;
using System.Windows.Forms;
using mRemoteNG.Themes;
using System.Linq;
using System.Collections.Generic;
using BrightIdeasSoftware;
using mRemoteNG.Properties;
using mRemoteNG.UI.TaskDialog;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    [SupportedOSPlatform("windows")]
    public sealed partial class ThemePage
    {
        #region Private Fields

        private readonly ThemeManager _themeManager;
        private readonly bool _oriActiveTheming;
        private ThemeInfo _oriActiveTheme;
        private readonly List<ThemeInfo> modifiedThemes = [];

        #endregion

        public ThemePage()
        {
            InitializeComponent();
            PageIcon = Resources.ImageConverter.GetImageAsIcon(Properties.Resources.AppearanceEditor_16x);
            _themeManager = ThemeManager.getInstance();
            if (!_themeManager.ThemingActive) return;
            _themeManager = ThemeManager.getInstance();
            _themeManager.ThemeChanged += ApplyTheme;
            _oriActiveTheming = _themeManager.ThemingActive;
        }

        public override string PageName
        {
            get => Language.Theme;
            set { }
        }

        public override void ApplyLanguage()
        {
            base.ApplyLanguage();

            btnThemeDelete.Text = Language._Delete;
            btnThemeNew.Text = Language._New;
            labelRestart.Text = Language.OptionsThemeChangeWarning;
            keyCol.Text = Language.Element;
            ColorCol.Text = Language.Color;
            ColorNameCol.Text = Language.ColorName;
        }

        private new void ApplyTheme()
        {
            if (!_themeManager.ThemingActive)
                return;
            base.ApplyTheme();
        }

        public override void LoadSettings()
        {
            //At first we cannot create or delete themes, depends later on the type of selected theme
            btnThemeNew.Enabled = false;
            btnThemeDelete.Enabled = false;
            //Load the list of themes
            cboTheme.Items.Clear();
            // ReSharper disable once CoVariantArrayConversion
            cboTheme.Items.AddRange(_themeManager.LoadThemes().OrderBy(x => x.Name).ToArray());
            cboTheme.SelectedItem = _themeManager.ActiveTheme;
            // Store the original active theme for reverting
            _oriActiveTheme = _themeManager.ActiveTheme;
            cboTheme_SelectionChangeCommitted(this, new EventArgs());
            cboTheme.DisplayMember = "Name";

            listPalette.FormatCell += ListPalette_FormatCell; //Color cell formatter
        }

        private void ListPalette_FormatCell(object sender, FormatCellEventArgs e)
        {
            if (e.ColumnIndex != ColorCol.Index) return;
            PseudoKeyColor colorElem = (PseudoKeyColor)e.Model;
            e.SubItem.BackColor = colorElem.Value;
        }


        public override void SaveSettings()
        {
            base.SaveSettings();

            Properties.OptionsThemePage.Default.ThemingActive = true;

            // Save the theme settings form close so we don't run into unexpected results while modifying...
            // Prompt the user that a restart is required to apply the new theme...
            if (cboTheme.SelectedItem != null
            ) // LoadSettings calls SaveSettings, so these might be null the first time around
            {
                if (!Properties.OptionsThemePage.Default.ThemeName.Equals(((ThemeInfo)cboTheme.SelectedItem).Name))
                {
                    Properties.OptionsThemePage.Default.ThemeName = ((ThemeInfo)cboTheme.SelectedItem).Name;
                    CTaskDialog.MessageBox("Theme Changed", "Restart Required.", "Please restart mRemoteNG to apply the selected theme.", ETaskDialogButtons.Ok, ESysIcons.Information);
                }
            }

            foreach (ThemeInfo updatedTheme in modifiedThemes)
            {
                _themeManager.updateTheme(updatedTheme);
            }
        }

        public override void RevertSettings()
        {
            base.RevertSettings();
            _themeManager.ThemingActive = _oriActiveTheming;
            
            // Clear the modified themes list without saving
            modifiedThemes.Clear();
            
            // Restore the original theme selection
            if (_oriActiveTheme != null)
            {
                _themeManager.ActiveTheme = _oriActiveTheme;
                // Reload the theme list to reflect the original state
                cboTheme.Items.Clear();
                cboTheme.Items.AddRange(_themeManager.LoadThemes().OrderBy(x => x.Name).ToArray());
                cboTheme.SelectedItem = _oriActiveTheme;
                cboTheme_SelectionChangeCommitted(this, new EventArgs());
            }
        }

        #region Private Methods

        #region Event Handlers

        private void cboTheme_SelectionChangeCommitted(object sender, EventArgs e)
        {
            btnThemeNew.Enabled = false;
            btnThemeDelete.Enabled = false;

            // don't display listPalette if it's not an Extendable theme...
            listPalette.CellClick -= ListPalette_CellClick;
            listPalette.Enabled = false;
            listPalette.Visible = false;

            if (!_themeManager.ThemingActive) return;

            btnThemeNew.Enabled = true;

            ThemeInfo selectedTheme = (ThemeInfo)cboTheme.SelectedItem;

            if (selectedTheme != null && selectedTheme.IsExtendable)
            {
                // it's Extendable, so now we can do this more expensive operations...
                listPalette.ClearObjects();
                ColorMeList(selectedTheme);
                listPalette.Enabled = true;
                listPalette.Visible = true;
                listPalette.CellClick += ListPalette_CellClick;
            }

            if (selectedTheme != null && selectedTheme.IsThemeBase) return;

            btnThemeDelete.Enabled = true;
        }

        /// <summary>
        /// Edit an object, since KeyValuePair value cannot be set without creating a new object, a parallel object model exist in the list
        /// besides the one in the active theme, so any modification must be done to the two models
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListPalette_CellClick(object sender, CellClickEventArgs e)
        {
            PseudoKeyColor colorElem = (PseudoKeyColor)e.Model;

            ColorDialog colorDlg = new()
            {
                AllowFullOpen = true,
                FullOpen = true,
                AnyColor = true,
                SolidColorOnly = false,
                Color = colorElem.Value
            };

            if (colorDlg.ShowDialog() != DialogResult.OK) return;
            modifiedThemes.Add(_themeManager.ActiveTheme);
            _themeManager.ActiveTheme.ExtendedPalette.replaceColor(colorElem.Key, colorDlg.Color);
            colorElem.Value = colorDlg.Color;
            listPalette.RefreshObject(e.Model);
            _themeManager.refreshUI();
        }

        private void ColorMeList(ThemeInfo ti)
        {
            foreach (KeyValuePair<string, System.Drawing.Color> colorElem in ti.ExtendedPalette.ExtColorPalette)
            {
                string display = ColorKeyDisplayName(colorElem.Key);
                listPalette.AddObject(new PseudoKeyColor(colorElem.Key, colorElem.Value, display));
            }
        }

        /// <summary>
        /// Map an internal color key (e.g. "Button_Hover_Background") to a
        /// localized, human-readable label shown in the theme color list.
        /// Falls back to the raw key when no mapping is defined.
        /// </summary>
        private static string ColorKeyDisplayName(string key)
        {
            return ColorKeyJaMap.TryGetValue(key, out var jp) ? jp : key;
        }

        // Japanese display labels for the palette keys defined in
        // ColorMapTheme.resx. Format: "<部品>: <用途>" or
        // "<部品> (<状態>): <用途>".
        private static readonly Dictionary<string, string> ColorKeyJaMap = new()
        {
            // ボタン
            { "Button_Background", "ボタン: 背景" },
            { "Button_Border", "ボタン: 枠線" },
            { "Button_Foreground", "ボタン: 文字" },
            { "Button_Hover_Background", "ボタン (ホバー): 背景" },
            { "Button_Hover_Border", "ボタン (ホバー): 枠線" },
            { "Button_Hover_Foreground", "ボタン (ホバー): 文字" },
            { "Button_Pressed_Background", "ボタン (押下): 背景" },
            { "Button_Pressed_Border", "ボタン (押下): 枠線" },
            { "Button_Pressed_Foreground", "ボタン (押下): 文字" },
            { "Button_Disabled_Background", "ボタン (無効): 背景" },
            { "Button_Disabled_Border", "ボタン (無効): 枠線" },
            { "Button_Disabled_Foreground", "ボタン (無効): 文字" },

            // チェックボックス
            { "CheckBox_Background", "チェックボックス: 背景" },
            { "CheckBox_Border", "チェックボックス: 枠線" },
            { "CheckBox_Border_Hover", "チェックボックス (ホバー): 枠線" },
            { "CheckBox_Border_Pressed", "チェックボックス (押下): 枠線" },
            { "CheckBox_Border_Disabled", "チェックボックス (無効): 枠線" },
            { "CheckBox_Glyph", "チェックボックス: チェック印" },
            { "CheckBox_Glyph_Disabled", "チェックボックス (無効): チェック印" },
            { "CheckBox_Text", "チェックボックス: 文字" },
            { "CheckBox_Text_Disabled", "チェックボックス (無効): 文字" },

            // ドロップダウン (ComboBox)
            { "ComboBox_Background", "ドロップダウン: 背景" },
            { "ComboBox_Border", "ドロップダウン: 枠線" },
            { "ComboBox_Foreground", "ドロップダウン: 文字" },
            { "ComboBox_MouseOver_Border", "ドロップダウン (ホバー): 枠線" },
            { "ComboBox_Disabled_Background", "ドロップダウン (無効): 背景" },
            { "ComboBox_Disabled_Foreground", "ドロップダウン (無効): 文字" },
            { "ComboBox_PopUp", "ドロップダウン: ポップアップ" },
            { "ComboBox_PopUp_Border", "ドロップダウン: ポップアップ枠線" },
            { "ComboBox_Button_Background", "ドロップダウン (ボタン): 背景" },
            { "ComboBox_Button_Border", "ドロップダウン (ボタン): 枠線" },
            { "ComboBox_Button_Foreground", "ドロップダウン (ボタン): 文字" },
            { "ComboBox_Button_MouseOver_Background", "ドロップダウン (ボタン, ホバー): 背景" },
            { "ComboBox_Button_MouseOver_Border", "ドロップダウン (ボタン, ホバー): 枠線" },
            { "ComboBox_Button_MouseOver_Foreground", "ドロップダウン (ボタン, ホバー): 文字" },
            { "ComboBox_Button_Pressed_Background", "ドロップダウン (ボタン, 押下): 背景" },
            { "ComboBox_Button_Pressed_Foreground", "ドロップダウン (ボタン, 押下): 文字" },

            // メニュー
            { "CommandBarMenuDefault_Background", "メニュー: 背景" },
            { "CommandBarMenuDefault_Foreground", "メニュー: 文字" },

            // ダイアログ
            { "Dialog_Background", "ダイアログ: 背景" },
            { "Dialog_Foreground", "ダイアログ: 文字" },

            // エラー / 警告
            { "ErrorText_Background", "エラー表示: 背景" },
            { "ErrorText_Foreground", "エラー表示: 文字" },
            { "WarningText_Background", "警告表示: 背景" },
            { "WarningText_Foreground", "警告表示: 文字" },

            // グループ枠 (GroupBox)。元のリソースキーが "Backgorund" とタイポ
            { "GroupBox_Backgorund", "グループ枠: 背景" },
            { "GroupBox_Foreground", "グループ枠: 文字" },
            { "GroupBox_Line", "グループ枠: 線" },
            { "GroupBox_Disabled_Background", "グループ枠 (無効): 背景" },
            { "GroupBox_Disabled_Foreground", "グループ枠 (無効): 文字" },
            { "GroupBox_Disabled_Line", "グループ枠 (無効): 線" },

            // リスト
            { "List_Background", "リスト: 背景" },
            { "List_Header_Background", "リスト ヘッダー: 背景" },
            { "List_Header_Foreground", "リスト ヘッダー: 文字" },
            { "List_Item_Background", "リスト項目: 背景" },
            { "List_Item_Border", "リスト項目: 枠線" },
            { "List_Item_Foreground", "リスト項目: 文字" },
            { "List_Item_Selected_Background", "リスト項目 (選択): 背景" },
            { "List_Item_Selected_Border", "リスト項目 (選択): 枠線" },
            { "List_Item_Selected_Foreground", "リスト項目 (選択): 文字" },
            { "List_Item_Disabled_Background", "リスト項目 (無効): 背景" },
            { "List_Item_Disabled_Border", "リスト項目 (無効): 枠線" },
            { "List_Item_Disabled_Foreground", "リスト項目 (無効): 文字" },

            // 進捗バー
            { "ProgressBar_Background", "進捗バー: 背景" },
            { "ProgressBar_Fill", "進捗バー: 塗り" },
            { "ProgressBar_Fill_Warning", "進捗バー: 塗り (警告)" },
            { "ProgressBar_Fill_Critical", "進捗バー: 塗り (危険)" },

            // タブ
            { "Tab_Background", "タブ: 背景" },
            { "Tab_Item_Background", "タブ項目: 背景" },
            { "Tab_Item_Foreground", "タブ項目: 文字" },
            { "Tab_Item_Disabled_Background", "タブ項目 (無効): 背景" },
            { "Tab_Item_Disabled_Foreground", "タブ項目 (無効): 文字" },

            // 入力欄 (TextBox)
            { "TextBox_Background", "入力欄: 背景" },
            { "TextBox_Border", "入力欄: 枠線" },
            { "TextBox_Foreground", "入力欄: 文字" },
            { "TextBox_Border_Focused", "入力欄 (フォーカス): 枠線" },
            { "TextBox_Focused_Background", "入力欄 (フォーカス): 背景" },
            { "TextBox_Focused_Foreground", "入力欄 (フォーカス): 文字" },
            { "TextBox_Border_Disabled", "入力欄 (無効): 枠線" },
            { "TextBox_Disabled_Background", "入力欄 (無効): 背景" },
            { "TextBox_Disabled_Foreground", "入力欄 (無効): 文字" },

            // ツリーパネル (接続ツリー)
            { "TreeView_Background", "ツリーパネル: 背景" },
            { "TreeView_Foreground", "ツリーパネル: 文字" },
            { "Treeview_SelectedItem_Active_Background", "ツリーパネル 選択項目 (アクティブ): 背景" },
            { "Treeview_SelectedItem_Active_Foreground", "ツリーパネル 選択項目 (アクティブ): 文字" },
            { "Treeview_SelectedItem_Inactive_Background", "ツリーパネル 選択項目 (非アクティブ): 背景" },
            { "Treeview_SelectedItem_Inactive_Foreground", "ツリーパネル 選択項目 (非アクティブ): 文字" },
        };

        private void btnThemeNew_Click(object sender, EventArgs e)
        {
            using (FrmInputBox frmInputBox = new(Language.OptionsThemeNewThemeCaption, Language.OptionsThemeNewThemeText, _themeManager.ActiveTheme.Name))
            {
                DialogResult dr = frmInputBox.ShowDialog();
                if (dr != DialogResult.OK) return;
                if (_themeManager.isThemeNameOk(frmInputBox.returnValue))
                {
                    ThemeInfo? addedTheme = _themeManager.addTheme(_themeManager.ActiveTheme, frmInputBox.returnValue);
                    if (addedTheme != null)
                        _themeManager.ActiveTheme = addedTheme;
                    LoadSettings();
                }
                else
                {
                    CTaskDialog.ShowTaskDialogBox(this, Language.Errors, Language.OptionsThemeNewThemeError, "", "", "", "", "", "", ETaskDialogButtons.Ok, ESysIcons.Error, ESysIcons.Information, 0);
                }
            }
        }

        private void btnThemeDelete_Click(object sender, EventArgs e)
        {
            DialogResult res = CTaskDialog.ShowTaskDialogBox(this, Language.Warnings,
                                                    Language.OptionsThemeDeleteConfirmation, "", "", "", "", "", "",
                                                    ETaskDialogButtons.YesNo,
                                                    ESysIcons.Question, ESysIcons.Information, 0);

            if (res != DialogResult.Yes) return;
            if (modifiedThemes.Contains(_themeManager.ActiveTheme))
                modifiedThemes.Remove(_themeManager.ActiveTheme);
            _themeManager.deleteTheme(_themeManager.ActiveTheme);
            LoadSettings();
        }

        #endregion

        #endregion
    }
}