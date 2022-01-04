﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System;
using System.Security.Claims;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        public PostController(IPostRepository postRepository, ICategoryRepository categoryRepository, ISubscriptionRepository subscriptionRepository)
        {
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
            _subscriptionRepository = subscriptionRepository;
        }
        public IActionResult Index()
        {
            var posts = _postRepository.GetAllPublishedPosts();
            return View(posts);
        }
        public IActionResult Details(int id)
        {
            var post = _postRepository.GetPublishedPostById(id);

            
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
                Subscription = _subscriptionRepository.GetSubscriptionByUserProfileId(post.Id),
                PostId = id,
                ProviderUserProfileId = id

            };

 
            return View(vm);
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
        public IActionResult Unsubscribe(int id)
        {
            try
            { 
                var post = _postRepository.GetPublishedPostById(id);
                _subscriptionRepository.Unsubscribe(post.UserProfile.Id);
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
    }
}
