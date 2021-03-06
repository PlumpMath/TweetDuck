﻿using System.Windows.Forms;
using TweetDck.Configuration;

namespace TweetDck.Core.Other.Settings{
    class BaseTabSettings : UserControl{
        protected static UserConfig Config{
            get{
                return Program.UserConfig;
            }
        }

        public BaseTabSettings(){
            Padding = new Padding(6);
        }

        public virtual void OnReady(){}
        public virtual void OnClosing(){}

        protected static void PromptRestart(){
            if (MessageBox.Show("The application must restart for the setting to take place. Do you want to restart now?", Program.BrandName+" Settings", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes){
                Program.Restart();
            }
        }
    }
}
