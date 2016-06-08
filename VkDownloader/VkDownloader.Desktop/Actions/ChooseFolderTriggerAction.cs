using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VkDownloader.Desktop.Actions
{
    using System.Windows;
    using System.Windows.Forms;
    using System.Windows.Interactivity;

    using Prism.Interactivity.InteractionRequest;

    using VkDownloader.Desktop.Notifications;

    internal class ChooseFolderTriggerAction : TriggerAction<DependencyObject>
    {
        protected override void Invoke(object parameter)
        {
            var args = parameter as InteractionRequestedEventArgs;
            if (args == null)
            {
                return;
            }

            var confirmation = args.Context as ChooseFolderConfirmation;
            if (confirmation == null)
            {
                return;
            }

            var browser = new System.Windows.Forms.FolderBrowserDialog();
            var dialogResult = browser.ShowDialog();
            switch (dialogResult)
            {
                case DialogResult.OK:
                case DialogResult.Yes:
                    confirmation.FolderPath = browser.SelectedPath;
                    confirmation.Confirmed = true;
                    break;
            }
            args.Callback();
        }
    }
}
