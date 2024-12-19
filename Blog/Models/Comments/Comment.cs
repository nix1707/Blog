namespace Blog.Models.Comments;

public class Comment
{
    public int Id { get; set; }
    public string Message { get;set; } = string.Empty;
    public DateTime Created { get; set; }
}
