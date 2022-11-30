using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ProfileSample.DAL;
using ProfileSample.Models;

namespace ProfileSample.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var context = new ProfileSampleEntities();

            var model = await context.ImgSources.Take(20)
                .Select(x => new ImageModel
                {
                    Name = x.Name,
                    Data = x.Data
                }).ToListAsync();

            //var sources = context.ImgSources.Take(20).Select(x => x.Id);

            //var model = new List<ImageModel>();

            //foreach (var id in sources)
            //{
            //    var item = context.ImgSources.Find(id);

            //    var obj = new ImageModel()
            //    {
            //        Name = item.Name,
            //        Data = item.Data
            //    };

            //model.Add(obj);
            //} 

            return View(model);
        }

        public async Task<ActionResult> Convert()
        {
            var files = Directory.GetFiles(Server.MapPath("~/Content/Img"), "*.jpg");

            using (var context = new ProfileSampleEntities())
            {
                var imgSources = new List<ImgSource>();

                foreach (var file in files)
                {
                    using (var stream = new FileStream(file, FileMode.Open))
                    {
                        byte[] buff = new byte[stream.Length];

                        //stream.Read(buff, 0, (int) stream.Length);
                        await stream.ReadAsync(buff, 0, (int)stream.Length);

                        // var entity = new ImgSource()
                        imgSources.Add(new ImgSource()
                        {
                            Name = Path.GetFileName(file),
                            Data = buff,
                        });

                        //context.ImgSources.Add(entity);
                        //context.SaveChanges();
                    }
                }
                if (imgSources.Any())
                {
                    context.ImgSources.AddRange(imgSources);
                    await context.SaveChangesAsync();
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}