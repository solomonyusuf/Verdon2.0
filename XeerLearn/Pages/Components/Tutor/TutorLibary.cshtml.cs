using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using System.Data;
using XeerLearn.Controllers;
using XeerLearn.Data;
using XeerLearn.Models.Auth;
using XeerLearn.Models.Libary;
using XeerLearn.Models.Utility;

namespace XeerLearn.Pages.Components.Tutor
{
    [Authorize(Roles = "Tutor")]
    public class TutorLibaryModel : PageModel
    {
        [BindProperty]
        public Libary material { get; set; }
        public Models.Auth.XeerLearnUser user { get; set; }
        [BindProperty]
        public string type { get; set; }
        [BindProperty]
        public string Id { get; set; }
        public Guid Access { get; set; }
        public List<Libary> materials { get; set; }
        public readonly ApplicationDbContext _context;
        public readonly IToastNotification _toast;
        public readonly UploadControllers _upload;
        public readonly UserManager<Models.Auth.XeerLearnUser> _manager;
        public readonly IHttpContextAccessor _acc;

        public TutorLibaryModel(IHttpContextAccessor acc, UserManager<Models.Auth.XeerLearnUser> manager,UploadControllers upload,IToastNotification toast, ApplicationDbContext context)
        {
            _acc = acc;
            _manager = manager;
            _upload = upload;
            _toast = toast;
            _context = context;

        }
        public async Task OnGet()
        {
            try
            {

                user = await _manager.FindByEmailAsync(User.Identity.Name);
                var access = await _context.UserAccess.Where(x => x.AccessKeyId == Guid.Parse(_acc.HttpContext.Session.GetString("AccessKey"))).FirstAsync();
                materials = await _context.Libaries.OrderByDescending(x=> x.Id).Where(x=> x.XeerLearnUserId == user.Id).OrderByDescending(x => x.Id).ToListAsync();
                Access = access.AccessKeyId;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public async Task<IActionResult> OnPost()
        {
            try
            {
                if (type == "delete")
                {
                    var book = await _context.Libaries.FindAsync(Guid.Parse(Id));
                    _context.Libaries.Remove(book);
                    await _context.SaveChangesAsync();
                    
                    _toast.AddSuccessToastMessage("Material Removed");
                }
                if(type == "upload")
                {
                     user = await _manager.FindByEmailAsync(User.Identity.Name);
                    if (Request.Form.Files.Any() == true)
                    {   
                        if(Request.Form.Files.Count() > 10)
                        {
                            _toast.AddWarningToastMessage("Only 10 files can be uploaded at once");
                            return Redirect("/tutor-libary");
                        }
                        foreach(var item in Request.Form.Files)
                        {
                            var file = item;
                            if (file.Length > 3000000)
                            {
                                _toast.AddWarningToastMessage("Maximum file size is 3mb");
                                return Redirect("/tutor-libary");
                            }
                            if (file.ContentType.Contains("pdf") || file.ContentType.Contains("doc") || file.ContentType.Contains("docx"))
                            {
                                var entity = new Libary { 
                                    XeerLearnUserId = user.Id,
                                    Name = file.FileName.Replace(" ", "_"),
                                    Author = user.FirstName+" "+user.LastName
                                };
                                var folderName = Path.Combine("wwwroot", "StaticFiles");
                                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                                var returnName = Path.Combine("StaticFiles");

                                if (file.Length > 0)
                                {
                                    var fileName = file.FileName.Replace(" ", "");
                                    var fullPath = Path.Combine(pathToSave, fileName);
                                    var dbPath = Path.Combine(returnName, fileName);

                                    using (var stream = new FileStream(fullPath, FileMode.Create))
                                    {
                                        await file.CopyToAsync(stream);
                                    }

                                    entity.File = dbPath;
                                }
                                entity.AccessKeyId = Access;
                                await _context.Libaries.AddAsync(entity);
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                _toast.AddWarningToastMessage("File format rejected.Accepted include (pdf, docx, doc).Secure protocol activated.");
                                return Redirect("/tutor-libary");
                            }

                        }

                      
                            _toast.AddSuccessToastMessage("New Materials Uploaded");
                    }

                }
              

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _toast.AddErrorToastMessage("An error occured");

            }

            return Redirect("/tutor-libary");
        }

       
    }
}
