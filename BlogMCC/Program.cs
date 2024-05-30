using System.Text.Json;
using BlogMCC.Services;
using Microsoft.Extensions.Logging;

namespace BlogMCC;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello World!");

        var loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });

        var context = new MyDbContext(loggerFactory);
        context.Database.EnsureCreated();
        
        DataInitialization.InitializeData(context);
        
        var blogService = new BlogService();

        Console.WriteLine("All posts:");
        var data = context.BlogPosts.Select(x => x.Title).ToList();
        Console.WriteLine(JsonSerializer.Serialize(data));
        
        Console.WriteLine("How many comments each user left:");
        foreach (var user in blogService.GetNumberOfCommentsPerUser(context))
        {
            Console.WriteLine($"{user.UserName}: {user.NumberOfComments}");
        }
        
        // Expected result (format could be different, e.g. object serialized to JSON is ok):
        // Ivan: 4
        // Petr: 2
        // Elena: 3

        Console.WriteLine("Posts ordered by date of last comment. Result should include text of last comment:");
        foreach (var post in blogService.GetPostsOrderedByLastCommentDate(context))
        {
            Console.WriteLine($"{post.PostTitle}: '{post.LastComment!.CommentCreatedDate}', '{post.LastComment!.CommentText}'");
        }
        
        // Expected result (format could be different, e.g. object serialized to JSON is ok):
        // Post2: '2020-03-06', '4'
        // Post1: '2020-03-05', '8'
        // Post3: '2020-02-14', '9'

        Console.WriteLine("How many last comments each user left:");
        // 'last comment' is the latest Comment in each Post
        foreach (var user in blogService.GetNumberOfLastCommentsLeftByUser(context))
        {
            Console.WriteLine($"{user.UserName}: {user.NumberOfComments}");
        }
        // Expected result (format could be different, e.g. object serialized to JSON is ok):
        // Ivan: 2
        // Petr: 1
    }
}