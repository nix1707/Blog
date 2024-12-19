using Blog.Data.FileManager;
using Blog.Data.Repository;
using Blog.Models.Comments;
using Blog.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;

public class HomeController(IRepository repository, IFileManager fileManager) : Controller
{
    private readonly IRepository _repository = repository;
    private readonly IFileManager _fileManager = fileManager;

    public IActionResult Index(int pageNumber, string category, string search)
    {
        if (pageNumber < 1)
            return RedirectToAction("Index", new { pageNumber = 1, category });

        var vm = _repository.GetAllPosts(pageNumber, category,search);

        return View(vm);
    }

    public IActionResult Post(int id)
    {
        var post = _repository.GetPost(id);
        return View(post);
    }

    [HttpGet("/Image/{image}"), ResponseCache(CacheProfileName = "Monthly")]
    public IActionResult Image(string image)
    {
        var mime = image[(image.LastIndexOf('.') + 1)..];
        return new FileStreamResult(_fileManager.ImageStream(image), $"image/{mime}");
    }

    [HttpPost]
    public async Task<IActionResult> Comment(CommentViewModel vm)
    {
        if (ModelState.IsValid == false)
            return RedirectToAction("Post", new { id = vm.PostId });

        var post = _repository.GetPost(vm.PostId);

        if (vm.MainCommentId == 0)
        {
            post.MainComments ??= [];
            post.MainComments.Add(new MainComment
            {
                Message = vm.Message,
                Created = DateTime.Now
            });

            _repository.UpdatePost(post);
        }
        else
        {
            var comment = new SubComment
            {
                MainCommentId = vm.MainCommentId,
                Message = vm.Message,
                Created = DateTime.Now
            };
            _repository.AddSubComment(comment);
        }
        await _repository.SaveChangesAsync();

        return RedirectToAction("Post", new { id = vm.PostId });
    }
}
