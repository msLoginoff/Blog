namespace BlogMCC.Services;

public class BlogService
{

    public BlogService()
    {
    }

    public List<NumberOfCommentsPerUserModel> GetNumberOfCommentsPerUser(MyDbContext context)
    {
        return context.BlogComments
            .GroupBy(s => s.UserName)
            .Select(p => new NumberOfCommentsPerUserModel
            {
                UserName = p.Key,
                NumberOfComments = p.Count()
            }).ToList();
    }

    public List<PostsWithLastCommentModel> GetPostsOrderedByLastCommentDate(MyDbContext context)
    {
        return context.BlogPosts
            .Select(post => new PostsWithLastCommentModel
            {
                PostTitle = post.Title,
                LastComment = post.Comments
                    .OrderByDescending(c => c.CreatedDate)
                    .Select(x => new CommentModel
                    {
                        CommentCreatedDate = x.CreatedDate,
                        CommentText = x.Text
                    })
                    .FirstOrDefault() ?? new CommentModel
                {
                    CommentCreatedDate = null,
                    CommentText = null
                }
            })
            .ToList()
            .OrderByDescending(x =>
                x.LastComment!.CommentCreatedDate)
            .ToList();
    }

    public List<NumberOfCommentsPerUserModel> GetNumberOfLastCommentsLeftByUser(MyDbContext context)
    {
        return context.BlogPosts
            .Select(p => p.Comments.OrderByDescending(v => v.CreatedDate).FirstOrDefault())
            .Where(p => p != null)
            .GroupBy(c => c!.UserName)
            .Select(p => new NumberOfCommentsPerUserModel
            {
                UserName = p.Key,
                NumberOfComments = p.Count()
            }).ToList();
    }
}