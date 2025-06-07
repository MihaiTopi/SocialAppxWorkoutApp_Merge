using ServerLibraryProject.Models;
using Workout.Core.Models;

namespace Workout.Web.ViewModels.PostViewModels
{
    public class PostViewModel
    {
        public Post Post { get; set; }
        public UserModel Author { get; set; }
        public List<Reaction> Reactions { get; set; } 
        public List<CommentViewModel> Comments { get; set; } 
    }
}
