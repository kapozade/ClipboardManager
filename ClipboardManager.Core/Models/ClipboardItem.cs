namespace ClipboardManager.Core.Models;

public class ClipboardItem
{
    /// <summary>
    /// Id of the item in clipboard
    /// </summary>
    public Guid Id { get; init; }
    
    /// <summary>
    /// When it's copied
    /// </summary>
    public DateTime CopiedAt { get; init; }

    /// <summary>
    /// Complete snapshot for restore
    /// </summary>
    public Dictionary<string, byte[]> RawData { get; init; } = new();

    /// <summary>
    /// For display in the picker UI only
    /// </summary>
    public ClipboardContentType PrimaryType { get; init; }
    
    /// <summary>
    /// Shown in list for text/files
    /// </summary>
    public string? TextPreview { get; init; }
    
    /// <summary>
    /// Shown in list for images
    /// </summary>
    public byte[]? ThumbnailPreview { get; init; }
}

public enum ClipboardContentType
{
    Text = 1, 
    RichText = 2, 
    Image = 3, 
    File = 4,
    Unknown = 5
}
