using ServerLibraryProject.Models;
using Workout.Core.Models;

namespace Workout.Web.ViewModels
{
    public class CommentViewModel
    {
        public Comment Comment { get; set; }
        public UserModel Author { get; set; }
    }

}
