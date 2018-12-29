using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Traveller.Models;

namespace Traveller.Controllers
{
    public class TravellerController : Controller
    {
     public   ServiceReference1.Service1Client client = new ServiceReference1.Service1Client();
        
        // GET: Traveller
        public ActionResult Index()
        {
            
            return View(client.GetTravelers());
        }

        public ActionResult Login()
        {

            return View();
        }
        public ActionResult PostCommentIndex()
        {
            
            return View(client.GetComments());
        }
        public ActionResult PostComment(int id)
        {
            if (Session["UserId"] != null)
            {
                ViewBag.UserId = Session["UserId"];
                ViewBag.postId = id;
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
           
        }
        [HttpPost]
        public ActionResult PostComment(PostComment postComment)
        {
            var PostedComment = new ServiceReference1.PostComment() {
                comment1 = postComment.comment1,
                Vote = postComment.Vote,
                postId = postComment.postId,
                UserId = postComment.UserId
            };
            client.PostComment(PostedComment);

            int pId = Convert.ToInt32(Session["PostId"].ToString());
            return RedirectToAction("PostDetail", new { id = pId });
        }
        public ActionResult PostDetail(int id)
        {
            Session["PostId"] = id;
            IList<ImagePost> ls = new List<ImagePost>();
          var list =   client.SearchImage(id);
            foreach (var item in list)
            {
                var image = new ImagePost()
                {
                    Id = item.Id,
                    Name = item.Name,
                   Path = item.Path
                };
                ls.Add(image);
            }
            ViewData["ImageList"] = ls;

            var posted = client.PostDetail(id);

            var postedUser = client.GetTrallerById(posted.UserId);

            var UserImage = client.GetImageByUserId(posted.UserId);

            var detailPosted = new PostDetail()
            {
                Content = posted.Content,
                CreatedAt = posted.CreatedAt,
                id = posted.id,
                Title = posted.Title,                
                firstName = postedUser.firstName,
                lastName = postedUser.LastName,
                ImagePath = UserImage.Path

            };

            

            IList<CommentDetail> listCommentClient = new List<CommentDetail>();
            var listComment = client.GetCommentsbyPostId(id);
            foreach (var item in listComment)
            {
                int ? UserId = item.UserId;
                var User = client.GetTrallerById(UserId);
                var ImageUser = client.GetImageByUserId(UserId);
                List<SubComment> subsList = new List<SubComment>();
                var subCommentlist = client.GetSubCommentsbyCommentId(item.Id);
                foreach (var subComment in subCommentlist)
                {
                    var sub_Comment = new SubComment()
                    {
                        Id = subComment.Id,
                        Subcomment = subComment.Subcomment,


                        CommentId = subComment.CommentId,
                        CreateAt = subComment.CreateAt

                    };
                    subsList.Add(sub_Comment);
                }
                    var comment = new CommentDetail()
                    
                {
                   comment1 = item.comment1,
                   Mark = item.Vote,
                  CreatAt = item.CreatAt,
                  firstName = User.firstName,
                  lastName = User.LastName,
                  ImagePath = ImageUser.Path,
                  SubComments = subsList
                    };
                listCommentClient.Add(comment);
            }
            ViewBag.UserId = posted.UserId;
            @ViewBag.postId = posted.id;
            ViewData["CommentList"] = listCommentClient;
            return View(detailPosted);
        }
        [HttpPost]
        public ActionResult Login(TravelLogin travel)
        {
            try
            {
              ServiceReference1.Traller usr =  client.Login(travel.email, travel.password);

                if (usr!=null)
                {
                    Session["UserId"] = usr.id;
                    Session["UserName"] = usr.LastName;
                    Session["UserRole"] = usr.RoleId;
                    return RedirectToAction("LogedIn");
                }
                else
                {
                    ModelState.AddModelError(" ", "Usename or password is wrong");
                    ViewBag.message = "Usename or password is wrong";
                }
                return View();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        
          
            
        }
        public ActionResult PostIndex()
        {
            return View(client.GetPosts());
        }
        public ActionResult Post()
        {
            if (Session["UserId"] != null)
            {
                int RoleID = Convert.ToInt32(Session["UserRole"].ToString());
                if (RoleID == 2)
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Login");
                }
                
            }
            else
            {
                return RedirectToAction("Login");
            }
               
        }
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login");
        }
        public ActionResult Search()
        {
            List<PostDetail> ls = new List<PostDetail>();
            var list = client.GetPosts();
            if (list.Length>0)
            {
                foreach (var item in list)
                {
                    var postedUser = client.GetTrallerById(item.UserId);

                    var UserImage = client.GetImageByUserId(item.UserId);
                    List<ImagePost> listImage = new List<ImagePost>();
                    var listImg = client.SearchImage(item.id);
                    foreach (var img in listImg)
                    {
                        var image = new ImagePost()
                        {
                            Id = img.Id,
                            Name = img.Name,
                            Path = img.Path
                        };
                        listImage.Add(image);
                    }
                    var postDetail = new PostDetail()
                    {
                        Content = item.Content,
                        CreatedAt = item.CreatedAt,
                        id = item.id,
                        Title = item.Title,
                        firstName = postedUser.firstName,
                        lastName = postedUser.LastName,
                        ImagePath = UserImage.Path,
                        ImageP = listImage[0].Path


                    };
                    ls.Add(postDetail);
                }
            }
            

            return View(ls);

           
        }
        [HttpPost]
        public ActionResult Search(string searching)
        {
            List<PostDetail> ls = new List<PostDetail>();
            var list = client.Search(searching);
            foreach (var item in list)
            {
                var postedUser = client.GetTrallerById(item.UserId);
                List<ImagePost> listImage = new List<ImagePost>();
                var listImg = client.SearchImage(item.id);
                foreach (var img in listImg)
                {
                    var image = new ImagePost()
                    {
                        Id = img.Id,
                        Name = img.Name,
                        Path = img.Path
                    };
                    listImage.Add(image);
                }
                var UserImage = client.GetImageByUserId(item.UserId);
                var postDetail = new PostDetail() {
                    Content = item.Content,
                    CreatedAt = item.CreatedAt,
                    id = item.id,
                    Title = item.Title,
                    firstName = postedUser.firstName,
                    lastName = postedUser.LastName,
                    ImagePath = UserImage.Path,
                    ImageP = listImage[0].Path
                    

                };
                ls.Add(postDetail);
            }

            return View(client.Search(searching));
        }
       
        [HttpPost]
        public ActionResult Post(Post post, ImageFile objImage)
        {
            int UserID = Convert.ToInt32(Session["UserId"].ToString());
            
            client.Post1(post.title, post.content, UserID);
            var currentPosted = client.CurrentPost(post.title);
            foreach (var file in objImage.files)
            {
                if (file != null && file.ContentLength > 0)
                {
                  //  User user = (User)Session["User"];
                    string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    string extension = Path.GetExtension(file.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string filePath = "~/Uploads/" + fileName;
                    string DatePosted = DateTime.Now.ToString("yymmssfff");
                    //     Random rnd = new Random();
                    //     int fileId = rnd.Next(1, 100);
                    var image = new ServiceReference1.ImagePost()
                    {
                        Name = fileName,
                        Path = filePath
                };
                    client.ImagePost(image, currentPosted.id);
                   

                    file.SaveAs(Path.Combine(Server.MapPath("/Uploads"), fileName));
                }
            }
            
            return RedirectToAction("Search");
        }
        public class ImageFile
        {
            public List<HttpPostedFileBase> files { get; set; }
        }
        public ActionResult LogedIn()
        {
            if (Session["UserId"] != null)
            {
                if (Session["PostId"]!=null)
                {
                    int pId = Convert.ToInt32(Session["PostId"].ToString());
                    return RedirectToAction("PostDetail", new { id = pId });
                }
                return RedirectToAction("Search");
            }
            else
            {
                return RedirectToAction("Login");
            }
           
        }
        // GET: Traveller/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Traveller/Create
        public ActionResult Registor()
        {
            return View();
        }

        // POST: Traveller/Create
        [HttpPost]
        public ActionResult Registor(Travel traveler, ImageFile objImage)
        {
            try
            {
                // TODO: Add insert logic here
                var travel = new ServiceReference1.Traveler()
                {
                    address = traveler.address,
                    dob = traveler.DOB,
                    email = traveler.email,
                    firstName = traveler.firstname,
                    lastName = traveler.lastName,
                    password = traveler.password,
                    phone = traveler.phone,
                    RoleId = traveler.RoleId
                };
                client.Registor(travel);
                var UserRegistor = client.GetTrallerByEmail(traveler.email);
                foreach (var file in objImage.files)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        //  User user = (User)Session["User"];
                        string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                        string extension = Path.GetExtension(file.FileName);
                        fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        string filePath = "~/Uploads/" + fileName;
                        string DatePosted = DateTime.Now.ToString("yymmssfff");
                        //     Random rnd = new Random();
                        //     int fileId = rnd.Next(1, 100);
                        var image = new ServiceReference1.ImagePost()
                        {
                            Name = fileName,
                            Path = filePath
                        };
                        client.ImageUserPost(image, UserRegistor.id);


                        file.SaveAs(Path.Combine(Server.MapPath("/Uploads"), fileName));
                    }
                }
                return RedirectToAction("Search");
            }
            catch
            {
                return View();
            }
        }

        // GET: Traveller/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Traveller/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Traveller/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Traveller/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
       
    }
}
