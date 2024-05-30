namespace BlogMCC;

public class PostsWithLastCommentModel
{
    public string PostTitle { get; set; }
    public CommentModel? LastComment { get; set; }
}