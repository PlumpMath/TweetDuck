﻿using System;
using System.Windows.Forms;
using TweetDck.Updates;
using TweetDck.Updates.Events;

namespace TweetDck.Core.Other.Settings{
    partial class TabSettingsGeneral : BaseTabSettings{
        private readonly UpdateHandler updates;
        private int updateCheckEventId = -1;

        public TabSettingsGeneral(UpdateHandler updates){
            InitializeComponent();

            this.updates = updates;
            this.updates.CheckFinished += updates_CheckFinished;
            Disposed += (sender, args) => this.updates.CheckFinished -= updates_CheckFinished;
            
            comboBoxTrayType.Items.Add("Disabled");
            comboBoxTrayType.Items.Add("Display Icon Only");
            comboBoxTrayType.Items.Add("Minimize to Tray");
            comboBoxTrayType.Items.Add("Close to Tray");
            comboBoxTrayType.Items.Add("Combined");
            comboBoxTrayType.SelectedIndex = Math.Min(Math.Max((int)Config.TrayBehavior, 0), comboBoxTrayType.Items.Count-1);

            checkExpandLinks.Checked = Config.ExpandLinksOnHover;
            checkSpellCheck.Checked = Config.EnableSpellCheck;
            checkTrayHighlight.Checked = Config.EnableTrayHighlight;

            checkUpdateNotifications.Checked = Config.EnableUpdateCheck;
        }

        public override void OnReady(){
            checkExpandLinks.CheckedChanged += checkExpandLinks_CheckedChanged;
            checkSpellCheck.CheckedChanged += checkSpellCheck_CheckedChanged;

            comboBoxTrayType.SelectedIndexChanged += comboBoxTrayType_SelectedIndexChanged;
            checkTrayHighlight.CheckedChanged += checkTrayHighlight_CheckedChanged;

            checkUpdateNotifications.CheckedChanged += checkUpdateNotifications_CheckedChanged;
            btnCheckUpdates.Click += btnCheckUpdates_Click;
        }

        private void checkExpandLinks_CheckedChanged(object sender, EventArgs e){
            Config.ExpandLinksOnHover = checkExpandLinks.Checked;
        }

        private void checkSpellCheck_CheckedChanged(object sender, EventArgs e){
            Config.EnableSpellCheck = checkSpellCheck.Checked;
            PromptRestart();
        }

        private void comboBoxTrayType_SelectedIndexChanged(object sender, EventArgs e){
            Config.TrayBehavior = (TrayIcon.Behavior)comboBoxTrayType.SelectedIndex;
        }

        private void checkTrayHighlight_CheckedChanged(object sender, EventArgs e){
            Config.EnableTrayHighlight = checkTrayHighlight.Checked;
        }

        private void checkUpdateNotifications_CheckedChanged(object sender, EventArgs e){
            Config.EnableUpdateCheck = checkUpdateNotifications.Checked;
        }

        private void btnCheckUpdates_Click(object sender, EventArgs e){
            updateCheckEventId = updates.Check(true);

            if (updateCheckEventId == -1){
                MessageBox.Show("Sorry, your system is no longer supported.", "Unsupported System", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else{
                btnCheckUpdates.Enabled = false;
                updates.DismissUpdate(string.Empty);
            }
        }

        private void updates_CheckFinished(object sender, UpdateCheckEventArgs e){
            if (e.EventId == updateCheckEventId){
                btnCheckUpdates.Enabled = true;

                if (!e.UpdateAvailable){
                    MessageBox.Show("Your version of "+Program.BrandName+" is up to date.", "No Updates Available", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
