using Blog.Data.FileManager;
using Blog.Data.Repository;
using Blog.Models;
using Blog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;

[Authorize(Roles = "Admin")]
public class PanelController(IRepository repository, IFileManager fileManager) : Controller
{
    private readonly IRepository _repository = repository;
    private readonly IFileManager _fileManager= fileManager;


    public IActionResult Index()
    {
        var posts = _repository.GetAllPosts();

        return View(posts);
    }

    [HttpGet]
    public IActionResult Edit(int? id)
    {
        if (id == null)
            return View(new PostViewModel());

        var post = _repository.GetPost(id.Value);
        return View(new PostViewModel
        {
            Id = post.Id,
            Title = post.Title,
            Body = post.Body,
            CurrentImage = post.Image,
            Description = post.Description,
            Category = post.Category,
            Tags = post.Tags
        });
    }

    [HttpPost]
    public async Task<IActionResult> Edit(PostViewModel vm)
    {
        var post = new Post
        {
            Id = vm.Id,
            Title = vm.Title,
            Body = vm.Body,
            Description = vm.Description,
            Category = vm.Category,
            Tags = vm.Tags
        };

        if(vm.Image == null)
        {
            post.Image = vm.CurrentImage;
        }
        else
        {
            if (string.IsNullOrEmpty(vm.CurrentImage) == false)
                _fileManager.RemoveImage(vm.CurrentImage);

            post.Image = await _fileManager.SaveImageAsync(vm.Image);
        }
            

        if (post.Id > 0)
            _repository.UpdatePost(post);
        else
            _repository.AddPost(post);

        return await _repository.SaveChangesAsync() == true
            ? RedirectToAction("Index")
            : View(post);
    }

    [HttpGet]
    public async Task<IActionResult> Remove(int id)
    {
        _repository.RemovePost(id);
        await _repository.SaveChangesAsync();
        return RedirectToAction("Index");
    }
}