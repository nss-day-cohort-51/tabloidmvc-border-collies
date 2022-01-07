using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System;
using System.Security.Claims;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;
using TabloidMVC.Models;
using System;
using System.Collections.Generic;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly ITagRepository _tagRepository;

        public PostController(IPostRepository postRepository, ICategoryRepository categoryRepository, ISubscriptionRepository subscriptionRepository, ITagRepository tagRepository)
        {
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
            _subscriptionRepository = subscriptionRepository;
            _tagRepository = tagRepository;
        }

        public IActionResult Index()
        {
            var posts = _postRepository.GetAllPublishedPosts();
            return View(posts);
        }

        public IActionResult Details(int id)
        {
            var post = _postRepository.GetPublishedPostById(id);

            var tags = _tagRepository.GetAllTags();

            List<string> allTags = _postRepository.GetAllPostTagsForPost(id);

            if (post == null)
            {
                int userId = GetCurrentUserProfileId();
                post = _postRepository.GetUserPostById(id, userId);
                if (post == null)
                {
                    return NotFound();
                }
            }
            PostViewModel vm = new PostViewModel()
            {

                Post = post,
                Subscription = new Subscription(),
                PostId = id,
                ProviderUserProfileId = id,
                Tags = tags,
                AllTags = allTags

            };

 
            return View(vm);
        }



        [HttpPost]
        public IActionResult AddPostTag(PostViewModel vm)
        {
         
            try
            {
                _postRepository.AddPostTag(vm.PostTag);

                return RedirectToAction("Details", new { id = vm.Post.Id });
            }
            catch
            {
                return View(vm);
            }
        }

        public IActionResult ManageTags(int id)
        {
            var selectedTags = _postRepository.GetPostTagsByPostId(id);
            
            var allTags = _tagRepository.GetAllTags();

            Post post = _postRepository.GetPublishedPostById(id);

            var vm = new TagViewModel
            {
                Post = post,
                Tags = allTags,
                SelectedTags = selectedTags
            };


            return View(vm);
            
        }

        [HttpPost]
        public IActionResult ManageTags (int id, TagViewModel vm)
        {
            foreach (int tagId in vm.SelectedTags)
            {
                PostTag postTag = new PostTag
                {
                     TagId = tagId,
                     PostId = id
                };

                _tagRepository.AddTagToPost(postTag);
            }

            return RedirectToAction("Index");

        }

        public IActionResult Create()
        {
            var vm = new PostCreateViewModel();
            vm.CategoryOptions = _categoryRepository.GetAll();
            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(PostCreateViewModel vm)
        {
            try
            {
                vm.Post.CreateDateTime = DateAndTime.Now;
                vm.Post.IsApproved = true;
                vm.Post.UserProfileId = GetCurrentUserProfileId();

                _postRepository.Add(vm.Post);

                return RedirectToAction("Details", new { id = vm.Post.Id });
            } 
            catch
            {
                vm.CategoryOptions = _categoryRepository.GetAll();
                return View(vm);
            }
        }
        public IActionResult Subscribe(int id)
        {
            try
            { 
                var post = _postRepository.GetPublishedPostById(id);

                Subscription subscription = new Subscription()
                {

                    BeginDateTime = DateAndTime.Now,
                    EndDateTime = null,
                    SubscriberUserProfileId = GetCurrentUserProfileId(),
                    ProviderUserProfileId = post.UserProfileId,

                };


                _subscriptionRepository.Subscribe(subscription);


                return RedirectToAction("Details", new { id });
            }
            
            catch (Exception ex)
            {
                return RedirectToAction("Details", new { id });
            }
        }

        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }



        // GET: OwnersController1/Edit/5
        public ActionResult Edit(int id)
        {
            Post post = _postRepository.GetPublishedPostById(id);


            


            List<Category> categories = _categoryRepository.GetAll();

            PostCreateViewModel vm = new PostCreateViewModel()
            {
                Post = post,
                CategoryOptions = categories
            };


            if (vm == null)
            {
                return NotFound();
            }

            return View(vm);
        }

        // POST: OwnersController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, PostCreateViewModel vm)
        {
            try
            {
                _postRepository.UpdatePost(vm.Post);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(vm);
            }
        }




        // GET: PostsController1/Delete/1
        public ActionResult Delete(int id)
        {
            Post post = _postRepository.GetPublishedPostById(id);

            return View(post);
        }


        // POST: PostsController1/Delete/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Post post)
        {
            try
            {
                _postRepository.DeletePost(id);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(post);
            }
        }


    }
}
