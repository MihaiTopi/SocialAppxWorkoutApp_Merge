namespace DesktopProject.Components
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.UI;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Media;
    using NeoIsisJob;
    using Workout.Core.Enums;
    using Workout.Core.IServices;
    using Workout.Core.Models;

    public sealed partial class PostComponent : UserControl
    {
        private string title;
        private PostVisibility visibility;
        private int userId;
        private string content;
        private DateTime createdDate;
        private long postId;
        private PostTag tag;

        private IReactionService reactionService;
        private ICommentService commentService;

        public DateTime PostCreationTime { get; set; }

        public string TimeSincePost
        {
            get
            {
                var timeSpan = DateTime.Now - this.PostCreationTime;
                if (timeSpan.TotalDays >= 1)
                {
                    return $"{(int)timeSpan.TotalDays} days ago";
                }
                else if (timeSpan.TotalHours >= 1)
                {
                    return $"{(int)timeSpan.TotalHours} hours ago";
                }
                else
                {
                    return $"{(int)timeSpan.TotalMinutes} minutes ago";
                }
            }
        }

        public PostVisibility PostVisibility
        {
            get => this.visibility;
            set
            {
                this.visibility = value;
                this.VisibilityText.Text = this.visibility.ToString();
            }
        }

        public PostComponent()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        public PostComponent(string title, PostVisibility visibility, int userId, string content, DateTime createdDate, PostTag tag, long postId = 0)
        {
            this.title = title;
            this.DataContext = this;
            this.InitializeComponent();
            this.PostVisibility = visibility;
            this.userId = userId;
            this.content = content;
            this.createdDate = createdDate;
            this.postId = postId;
            this.PostCreationTime = createdDate;
            this.tag = tag;

            this.reactionService = App.Services.GetService<IReactionService>();
            this.commentService = App.Services.GetService<ICommentService>();

            this.Title.Text = title;
            this.TimeSince.Text = this.TimeSincePost; // Use the property for time display

            // change background color based on tag
            switch (tag)
            {
                case PostTag.Food:
                    this.PostBorder.Background = new SolidColorBrush(Colors.Orange);
                    break;
                case PostTag.Workout:
                    this.PostBorder.Background = new SolidColorBrush(Colors.LightGreen);
                    break;
                default:
                    break;
            }

            this.SetContent(); // Set text or image
            this.Loaded += this.LoadReactionCounts_async;
        }

        public async void LoadReactionCounts_async(object sender, RoutedEventArgs e)
        {
            await this.LoadReactionCounts();
        }

        private void SetContent()
        {
            const string imagePrefix = "image://";
            if (this.content.StartsWith(imagePrefix))
            {
                string base64Image = this.content.Substring(imagePrefix.Length);
                this.PostImage.Visibility = Visibility.Visible;
                this.Content.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.Content.Text = this.content;
                this.PostImage.Visibility = Visibility.Collapsed;
                this.Content.Visibility = Visibility.Visible;
            }
        }

        private async Task LoadReactionCounts()
        {
            var reactions = await this.reactionService.GetReactionsByPostId(this.postId);
            this.LikeCount.Text = reactions.Count(r => r.Type == ReactionType.Like).ToString();
            this.LoveCount.Text = reactions.Count(r => r.Type == ReactionType.Love).ToString();
            this.LaughCount.Text = reactions.Count(r => r.Type == ReactionType.Laugh).ToString();
            this.AngryCount.Text = reactions.Count(r => r.Type == ReactionType.Anger).ToString();
        }

        private async Task LoadComments()
        {
            var comments = await this.commentService.GetCommentsByPostId(this.postId);
            this.CommentsListView.ItemsSource = comments;
        }

        private async void OnLikeButtonClick(object sender, RoutedEventArgs e)
        {
            if (AppController.CurrentUser != null)
            {
                await this.reactionService.AddReaction(new Reaction { UserId = AppController.CurrentUser.ID, PostId = this.postId, Type = ReactionType.Like });
                await this.LoadReactionCounts();
            }
        }

        private async void OnLoveButtonClick(object sender, RoutedEventArgs e)
        {
            if (AppController.CurrentUser != null)
            {
                await this.reactionService.AddReaction(new Reaction { UserId = AppController.CurrentUser.ID, PostId = this.postId, Type = ReactionType.Love });
                await this.LoadReactionCounts();
            }
        }

        private async void OnLaughButtonClick(object sender, RoutedEventArgs e)
        {
            if (AppController.CurrentUser != null)
            {
                await this.reactionService.AddReaction(new Reaction { UserId = AppController.CurrentUser.ID, PostId = postId, Type = ReactionType.Laugh });
                await this.LoadReactionCounts();
            }
        }

        private async void OnAngryButtonClick(object sender, RoutedEventArgs e)
        {
            if (AppController.CurrentUser != null)
            {
                await this.reactionService.AddReaction(new Reaction { UserId = AppController.CurrentUser.ID, PostId = postId, Type = ReactionType.Anger });
                await this.LoadReactionCounts();
            }
        }

        private async void OnCommentButtonClick(object sender, RoutedEventArgs e)
        {
            await this.LoadComments();
            this.CommentSection.Visibility = Visibility.Visible;
        }

        private void OnSubmitCommentButtonClick(object sender, RoutedEventArgs e)
        {
            string commentText = this.CommentTextBox.Text;
            if (!string.IsNullOrEmpty(commentText))
            {
                this.commentService.AddComment(commentText, this.userId, this.postId);
                this.CommentTextBox.Text = string.Empty;
                this.CommentSection.Visibility = Visibility.Collapsed;
            }
        }
    }
}