namespace VkDownloader.Desktop.Notifications
{
    using Prism.Interactivity.InteractionRequest;

    class ChooseFolderConfirmation : Confirmation
    {
        public ChooseFolderConfirmation()
        {
            Title = string.Empty;
            FolderPath = string.Empty;
        }

        public string FolderPath { get; set; }
    }
}
