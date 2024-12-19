using Blog.Models.Comments;

namespace Blog.Models;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Tags { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public DateTime Created { get; set; } = DateTime.Now;
    public List<MainComment> MainComments { get; set; } = [];
}
