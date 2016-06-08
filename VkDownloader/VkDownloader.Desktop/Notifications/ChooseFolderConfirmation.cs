namespace VkDownloader.Desktop.Notifications
{
    using Prism.Interactivity.InteractionRequest;

    class ChooseFolderConfirmation : Confirmation
    {
        public string FolderPath { get; set; }
    }
}
