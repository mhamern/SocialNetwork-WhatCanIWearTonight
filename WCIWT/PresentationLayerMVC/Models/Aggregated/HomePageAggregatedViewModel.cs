using PresentationLayerMVC.Models.FriendRequests;
using PresentationLayerMVC.Models.Friends;
using PresentationLayerMVC.Models.Posts;

namespace PresentationLayerMVC.Models.Aggregated
{
    public class HomePageAggregatedViewModel
    {
        public PostListViewModel Posts;
        public FriendRequestListViewModel FriendRequests;
        public FriendListViewModel Friends;
    }
}