﻿using Blog.Models;

namespace Blog.ViewModels;

public class IndexViewModel
{
    public int PageNumber { get; set; }
    public int PageCount { get; set; }
    public bool NextPage { get; set; }
    public string Category { get; set; }
    public string Search { get; set; }
    public IEnumerable<int> Pages { get; set; }
    public IEnumerable<Post> Posts { get; set; }
}
