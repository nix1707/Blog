using Blog.Helpers;
using Blog.Models;
using Blog.Models.Comments;
using Blog.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data.Repository;

public class Repository(AppDbContext context) : IRepository
{
    private readonly AppDbContext _context = context;

    public void AddPost(Post post)
        => _context.Add(post);


    public List<Post> GetAllPosts()
        => [.. _context.Posts];

    public IndexViewModel GetAllPosts(int pageNumber, string category, string search)
    {
        var InCategory = (Post post) => post.Category.ToLower().Equals(category.ToLower());

        int skipAmount = Pagination.PAGE_SIZE * (pageNumber - 1);

        var query = _context.Posts.AsNoTracking().AsQueryable();

        if (string.IsNullOrEmpty(category) == false)
            query = query.Where(post => InCategory(post));

        if (string.IsNullOrEmpty(search) == false)
            query = query.Where(x => 
                EF.Functions.Like(x.Body, $"%{search}%") 
                || EF.Functions.Like(x.Description, $"%{search}%") 
                || EF.Functions.Like(x.Category, $"%{search}%"));

        int postsCount = query.Count();
        int pageCount = (int)Math.Ceiling((double)postsCount / Pagination.PAGE_SIZE);

        return new IndexViewModel
        {
            PageNumber = pageNumber,
            PageCount = pageCount,
            Pages = Pagination.GetPageNumbers(pageNumber, pageCount),
            Posts = [.. query.Skip(skipAmount).Take(Pagination.PAGE_SIZE)],
            Search = search,
            NextPage = postsCount > skipAmount + Pagination.PAGE_SIZE,
            Category = category
        };
    }

    public Post GetPost(int id)
        => _context.Posts
            .Include(p => p.MainComments)
            .ThenInclude(mc => mc.SubComments)
            .FirstOrDefault(blog => blog.Id == id)!;

    public void RemovePost(int id)
        => _context.Posts.Remove(GetPost(id));

    public void UpdatePost(Post post)
        => _context.Posts.Update(post);

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    public void AddSubComment(SubComment comment)
    {
        _context.SubComments.Add(comment);
    }
}
