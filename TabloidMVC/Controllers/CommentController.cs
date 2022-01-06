using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;
        private int _id = -1;
        public CommentController(ICommentRepository commentRepository, IPostRepository postRepository)
        {
            _commentRepository = commentRepository;
            _postRepository = postRepository;
        }
        // GET: CommentController
        public ActionResult Index(int postId)
        {         
            if (_id == -1)
            {
                _id = postId;
            }
            CommentViewModel vm = new CommentViewModel()
            {
                PostId = _id,
                Comments = _commentRepository.GetAllPostComments(postId, _postRepository)
            };
            return View(vm);
        }

        // GET: CommentController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CommentController/Create
        public IActionResult Create(int postId)
        {
            CommentCreateViewModel vm = new CommentCreateViewModel()
            {
                Comment = new Comment(),
                PostId = postId
            };
            return View(vm);
        }

        // POST: CommentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CommentCreateViewModel vm)
        {
            try
            {
                vm.Comment.PostId = vm.PostId;
                vm.Comment.UserProfileId = _postRepository.GetUserProfileId(vm.PostId);
                _commentRepository.AddComment(vm.Comment);
                return RedirectToAction("Index", new { postId = vm.PostId });
            }
            catch
            {
                return View(vm.Comment);
            }
        }

        // GET: CommentController/Edit/5
        public ActionResult Edit(int id, int postId)
        {
            Comment commment = _commentRepository.GetCommentById(postId, id);      
            if (commment == null)
            {
                return NotFound();
            }
            CommentEditViewModel vm = new CommentEditViewModel()
            {
                PostId = postId,
                Comment = commment
            };
            return View(vm);
        }

        // POST: CommentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, CommentEditViewModel vm)
        {
            try
            {
                _commentRepository.UpdateComment(vm.Comment);
                return RedirectToAction("Index", new { postId = vm.PostId });
            }
            catch
            {
                return View(vm);
            }
        }

        // GET: CommentController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CommentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
