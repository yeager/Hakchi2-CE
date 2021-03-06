﻿using com.clusterrr.hakchi_gui.Properties;
using System;
using System.IO;
using System.Windows.Forms;

namespace com.clusterrr.hakchi_gui
{
    public partial class SelectSystemDialog : Form
    {
        private void fillSystems()
        {
            int i = 0;
            foreach (var system in CoreCollection.Systems)
            {
                checkedListBox.Items.Add(system);
                if(Directory.Exists(Shared.PathCombine(Program.BaseDirectoryExternal, "art", system)))
                {
                    checkedListBox.SetItemChecked(i, true);
                }
                ++i;
            }
        }

        public SelectSystemDialog()
        {
            InitializeComponent();
            Icon = Resources.icon;
            fillSystems();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonCheckAll_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < checkedListBox.Items.Count; ++i)
            {
                checkedListBox.SetItemChecked(i, true);
            }
        }

        private void buttonUncheckAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox.Items.Count; ++i)
            {
                checkedListBox.SetItemChecked(i, false);
            }
        }

        private void buttonPrepare_Click(object sender, EventArgs e)
        {
            if (Tasks.MessageForm.Show(this, Resources.ApplyChanges, Resources.ApplyChangesQ, Resources.sign_question, new Tasks.MessageForm.Button[] { Tasks.MessageForm.Button.Yes, Tasks.MessageForm.Button.No }, Tasks.MessageForm.DefaultButton.Button1) == Tasks.MessageForm.Button.Yes)
            {
                for (int i = 0; i < checkedListBox.Items.Count; ++i)
                {
                    var path = Shared.PathCombine(Program.BaseDirectoryExternal, "art", checkedListBox.Items[i] as string);
                    if (checkedListBox.GetItemChecked(i))
                    {
                        Directory.CreateDirectory(path);
                    }
                    else
                    {
                        if (Directory.Exists(path))
                        {
                            try
                            {
                                // this will only delete empty directories, which is the desired outcome
                                Directory.Delete(path);
                            }
                            catch { }
                        }
                    }
                }
                if (!ConfigIni.Instance.DisablePopups)
                    Tasks.MessageForm.Show(Resources.Wow, Resources.Done, Resources.sign_check);
            }
            Close();
        }
    }
}
