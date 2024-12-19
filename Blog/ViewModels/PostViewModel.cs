namespace Blog.ViewModels;

public class PostViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Tags { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string CurrentImage { get; set; } = string.Empty;

    public IFormFile Image { get; set; } = null;
}
