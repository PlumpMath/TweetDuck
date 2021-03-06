﻿using System.Windows.Forms;
using TweetDck.Core.Controls;
using TweetDck.Core.Notification;
using TweetDck.Core.Utils;

namespace TweetDck.Core.Bridge{
    sealed class TweetDeckBridge{
        public static string LastRightClickedLink = string.Empty;
        public static string LastHighlightedTweet = string.Empty;
        public static string LastHighlightedQuotedTweet = string.Empty;

        public static void ResetStaticProperties(){
            LastRightClickedLink = LastHighlightedTweet = LastHighlightedQuotedTweet = string.Empty;
        }

        private readonly FormBrowser form;
        private readonly FormNotificationMain notification;

        public TweetDeckBridge(FormBrowser form, FormNotificationMain notification){
            this.form = form;
            this.notification = notification;
        }

        public void LoadFontSizeClass(string fsClass){
            form.InvokeAsyncSafe(() => {
               TweetNotification.SetFontSizeClass(fsClass);
            });
        }

        public void LoadNotificationHeadContents(string headContents){
            form.InvokeAsyncSafe(() => {
               TweetNotification.SetHeadTag(headContents); 
            });
        }

        public void SetLastRightClickedLink(string link){
            form.InvokeAsyncSafe(() => LastRightClickedLink = link);
        }

        public void SetLastHighlightedTweet(string link, string quotedLink){
            form.InvokeAsyncSafe(() => {
                LastHighlightedTweet = link;
                LastHighlightedQuotedTweet = quotedLink;
            });
        }

        public void OpenContextMenu(){
            form.InvokeAsyncSafe(form.OpenContextMenu);
        }

        public void OnTweetPopup(string columnName, string tweetHtml, int tweetCharacters, string tweetUrl, string quoteUrl){
            notification.InvokeAsyncSafe(() => {
                form.OnTweetNotification();
                notification.ShowNotification(new TweetNotification(columnName, tweetHtml, tweetCharacters, tweetUrl, quoteUrl));
            });
        }

        public void OnTweetSound(){
            form.InvokeAsyncSafe(() => {
                form.OnTweetNotification();
                form.PlayNotificationSound();
            });
        }

        public void DisplayTooltip(string text, bool showInNotification){
            if (showInNotification){
                notification.InvokeAsyncSafe(() => notification.DisplayTooltip(text));
            }
            else{
                form.InvokeAsyncSafe(() => form.DisplayTooltip(text));
            }
        }

        public void LoadNextNotification(){
            notification.InvokeAsyncSafe(notification.FinishCurrentNotification);
        }

        public void ScreenshotTweet(string html, int width, int height){
            form.InvokeAsyncSafe(() => form.OnTweetScreenshotReady(html, width, height));
        }

        public void FixClipboard(){
            form.InvokeAsyncSafe(WindowsUtils.ClipboardStripHtmlStyles);
        }

        public void OpenBrowser(string url){
            BrowserUtils.OpenExternalBrowser(url);
        }

        public void Alert(string type, string contents){
            MessageBoxIcon icon;

            switch(type){
                case "error": icon = MessageBoxIcon.Error; break;
                case "warning": icon = MessageBoxIcon.Warning; break;
                case "info": icon = MessageBoxIcon.Information; break;
                default: icon = MessageBoxIcon.None; break;
            }

            MessageBox.Show(contents, Program.BrandName+" Browser Message", MessageBoxButtons.OK, icon);
        }

        public void Log(string data){
            System.Diagnostics.Debug.WriteLine(data);
        }
    }
}
