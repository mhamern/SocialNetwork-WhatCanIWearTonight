﻿using BusinessLayer.Facades;
using PresentationLayerMVC.Models.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Common;
using BusinessLayer.DataTransferObjects.Filters;
using PresentationLayerMVC.Models.Aggregated;
using PresentationLayerMVC.Models.Posts;
using X.PagedList;

namespace PresentationLayerMVC.Controllers
{
    [RoutePrefix("users")]
    public class UserController : Controller
    {
        public const int PostsPageSize = 10;
        public const int ImagesPerPost = 5;
        private const string FilterSessionKey = "filter";

        public UserFacade UserFacade { get; set; }
        public PostFacade PostFacade { get; set; }

        // GET: user/{username}
        [HttpGet]
        [Route("{username}")]
        public async Task<ActionResult> Index(string username, int page = 1)
        {
            var user = await UserFacade.GetUserByUsernameAsync(username);
            if (user == null)
            {
                // TODO: Redirect 404
            }

            var posts = await GetPostModel(user.Id, page);
            var friendshipWithLoggedUser = await GetFriendshipWithLoggedUser(user.Id);
            var model = new UserProfileAggregatedViewModel()
            {
                User = user,
                PostListViewModel = posts,
                IsFriend =  ResolveIsFriendForModel(friendshipWithLoggedUser),
                HasPendingFriendRequest = ResolveHasPendingFriendRequestForModel(friendshipWithLoggedUser)
            };
            return View($"UserProfileView", model);
        }

        // GET: user/{username}/friends
        [HttpGet]
        [Route("{username}/friends")]
        public async Task<ActionResult> DisplayFriends(string username, int page = 1)
        {
           throw new NotImplementedException();
        }

        // POST: user/{username}/add-friend
        [HttpPost]
        [Route("{username}/add-friend")]
        public async Task<ActionResult> AddFriend(string username)
        {
            throw new NotImplementedException();
        }

        // POST: user/{username}/remove-friend
        [HttpPost]
        [Route("{username}/remove-friend")]
        public async Task<ActionResult> RemoveFriend(string username)
        {
            throw new NotImplementedException();;
        }

        private async Task<FriendshipDto> GetFriendshipWithLoggedUser(Guid userId)
        {
            var loggedUser = await GetLoggedUser();
            if (loggedUser == null)
            {
                return null;
            }
            return await UserFacade.GetFriendshipBetweenUsers(loggedUser.Id, userId);
        }

        private async Task<PostListViewModel> GetPostModel(Guid userId, int page)
        {
            var filter = Session[FilterSessionKey] as PostFilterDto ?? new PostFilterDto { PageSize = PostsPageSize };
            filter.RequestedPageNumber = page;

            var posts = await PostFacade.GetPostsByUserId(filter, userId);
            var imagesForPosts = new List<List<ImageDto>>();

            foreach (var post in posts.Items)
            {
                imagesForPosts.Add(await PostFacade.ListOfImagesForPost(post.Id));
            }

            return InitializePostListViewModel(posts, imagesForPosts);
        }

        private PostListViewModel InitializePostListViewModel(QueryResultDto<PostDto, PostFilterDto> postsResult,
            List<List<ImageDto>> imagesResult)
        {
            return new PostListViewModel
            {
                Posts = new StaticPagedList<PostDto>(postsResult.Items, postsResult.RequestedPageNumber ?? 1, PostsPageSize,
                    (int)postsResult.TotalItemsCount),
                ImagesForPosts = imagesResult,
                PostFilter = postsResult.Filter,
            };
        }

        private async Task<UserDto> GetLoggedUser()
        {
            return HttpContext.User?.Identity != null
                ? await UserFacade.GetUserByUsernameAsync(HttpContext.User.Identity.Name)
                : null;
        }

        private bool ResolveHasPendingFriendRequestForModel(FriendshipDto friendship) => friendship != null && !friendship.IsConfirmed;

        private bool ResolveIsFriendForModel(FriendshipDto friendship) => friendship != null && friendship.IsConfirmed;
    }
}