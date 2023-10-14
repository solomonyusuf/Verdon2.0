using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using NToastNotify;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text;
using XeerLearn.Data;
using XeerLearn.Models.Auth;
using XeerLearn.Controllers;
using Microsoft.EntityFrameworkCore;
using XeerLearn.Models.Utility;

namespace XeerLearn.Areas.Identity.Pages.Account
{
    public class BecomeTutorModel : PageModel
    {
        private readonly SignInManager<Models.Auth.XeerLearnUser> _signInManager;
        private readonly UserManager<Models.Auth.XeerLearnUser> _userManager;
        private readonly IUserStore<Models.Auth.XeerLearnUser> _userStore;
        private readonly IUserEmailStore<Models.Auth.XeerLearnUser> _emailStore;
        private readonly ILogger<BecomeStudentModel> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IToastNotification _toast;
        private readonly EmailController _mail;
     
     
        public BecomeTutorModel(
            UserManager<Models.Auth.XeerLearnUser> userManager,
            IUserStore<Models.Auth.XeerLearnUser> userStore,
            SignInManager<Models.Auth.XeerLearnUser> signInManager,
            ILogger<BecomeStudentModel> logger,
            IToastNotification toast,
            EmailController mail,
            ApplicationDbContext context
            )
        {
            _userManager = userManager;
            _userStore = userStore;
            _signInManager = signInManager;
            _logger = logger;
            _toast = toast;
            _mail = mail;
            _context = context;
        }

        [BindProperty]
        public string type { get; set; }
        
        [BindProperty]
        public XeerLearnUser user { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }
        
        [BindProperty]
        public InputModel LoginInput { get; set; }
        
        [BindProperty]
        public string AccessName { get; set; }
        
        [BindProperty]
        public string AccessKey { get; set; }

      
        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

       
        public class InputModel
        {
            
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }
            
            [Required]
            [Display(Name = "FirstName")]
            public string FirstName { get; set; }
            
            [Required]
            [Display(Name = "LastName")]
            public string LastName { get; set; }
            
            [Required]
            [Display(Name = "RememberMe")]
            public bool RememberMe { get; set; }
            
            [Required]
            [Display(Name = "StaffID")]
            public string StaffID { get; set; }

            
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

          
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        { 
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            try
            {
                if (type == "register")
                {
                    if (AccessKey != null && AccessName != null)
                    {
                        _toast.AddWarningToastMessage("Paste an access key or enter name for new access key, only one choice.");
                        return Redirect("/become-tutor");
                    }
                    if (AccessKey != null)
                    {
                        var acc = await _context.AccessKeys.FindAsync(Guid.Parse(AccessKey));
                        if(acc == null)
                        {
                            _toast.AddWarningToastMessage("AccessKey Invalid");
                            return Redirect("/become-tutor");
                        }
                    }

                    var image = string.Empty;
                    if (Request.Form.Any() == false)
                    {
                        _toast.AddWarningToastMessage("All fields are required");
                        return Redirect("/become-tutor");
                    }

                    if (Request.Form.Files.Any() == true)
                    {
                        var file = Request.Form.Files[0];
                        if (file.Length > 3000000)
                        {
                            _toast.AddWarningToastMessage("Maximum file size is 3MB");
                            return Redirect("/become-tutor");
                        }
                        if (file.ContentType.Contains("jpg") || file.ContentType.Contains("png") || file.ContentType.Contains("jpeg"))
                        {
                            var folderName = Path.Combine("wwwroot", "StaticFiles");
                            var returnName = Path.Combine("StaticFiles");
                            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                            if (file.Length > 0)
                            {
                                var fileName = file.FileName.Replace(" ", "");
                                var fullPath = Path.Combine(pathToSave, fileName);
                                var dbPath = Path.Combine(returnName, fileName);

                                using (var stream = new FileStream(fullPath, FileMode.Create))
                                {
                                    await file.CopyToAsync(stream);
                                }

                                image = dbPath;
                            }
                        }
                        else
                        {
                            _toast.AddWarningToastMessage("File format rejected.Accepted include (jpg, jpeg, png).Secure protocol activated.");
                            return Redirect("/become-tutor");
                        }
                    }
                    else
                    {
                        _toast.AddWarningToastMessage("Image Upload is required");
                        return LocalRedirect("/become-tutor");
                    }

                    var user = new Models.Auth.XeerLearnUser()
                    {
                        FirstName = Input.FirstName,
                        UserName = Input.Email,
                        LastName = Input.LastName,
                        Email = Input.Email,
                        StaffID = Input.StaffID,
                        StaffDocument = image
                    };

                    var result = await _userManager.CreateAsync(user, Input.Password);

                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User created a new account with password.");

                        await _userManager.AddToRoleAsync(user, "Tutor");
                        //------send mail here --------------
                        var userId = await _userManager.GetUserIdAsync(user);

                        if (AccessKey != null)
                        {
                            var acc = await _context.AccessKeys.FindAsync(Guid.Parse(AccessKey));                        
                            var u_acc = new UserAccess {
                                AccessKeyId = Guid.Parse(AccessKey),
                                XeerLearnUserId = userId,
                                Type = "Tutor",
                                ExpiryDate = acc.ExpiryDate
                            };
                            await _context.UserAccess.AddAsync(u_acc);
                            await _context.SaveChangesAsync();
                        }

                        if (AccessName != null)
                        {
                            var acc = new AccessKey {
                                ExpiryDate = DateTime.Now.AddMonths(3),
                                DateTime = DateTime.Now,
                                Students = 400,
                                Tutors = 40,
                                Name = AccessName
                                                             
                            };
                            var tracker = await _context.AccessKeys.AddAsync(acc);

                            var u_acc = new UserAccess();
                            u_acc.AccessKeyId = tracker.Entity.Id;
                            u_acc.ExpiryDate = tracker.Entity.ExpiryDate;
                            u_acc.XeerLearnUserId = user.Id;
                            await _context.UserAccess.AddAsync(u_acc);
                            await _context.SaveChangesAsync();
                        }

                        var baseUrl = string.Format("{0}://{1}{2}", Request.Scheme, Request.Host, Request.PathBase);
                        var model = new
                        {
                            Email = user.Email,
                            Subject = "XeerLearn Account Confirmation",
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Body = @$"
                                    <link rel=""stylesheet"" href=""https://cdn.jsdelivr.net/npm/bootstrap@4.3.1/dist/css/bootstrap.min.css"" integrity=""sha384-ggOyR0iXCbMQv3Xipma34MD+dH/1fQ784/j6cY/iJTQUOhcWr7x9JvoRxT2MZw1T"" crossorigin=""anonymous"">
        
                                     <h1 align=""""center""""style=""""color:purple;"""">XeerLearn</h1><br/>
                                        <br/>
                                <p align=""""center"""">Please confirm your account by clicking on the button below</p><br/>                         
                                <a align=""""center"""" class=""btn btn-lg btn-info"" href='{baseUrl}/confirm-account/{user.Id}'>Confim Account</a>

                                <script src=""https://code.jquery.com/jquery-3.3.1.slim.min.js"" integrity=""sha384-q8i/X+965DzO0rT7abK41JStQIAqVgRVzpbzo5smXKp4YfRvH+8abtTE1Pi6jizo"" crossorigin=""anonymous""></script>
                                <script src=""https://cdn.jsdelivr.net/npm/popper.js@1.14.7/dist/umd/popper.min.js"" integrity=""sha384-UO2eT0CpHqdSJQ6hJty5KVphtPhzWj9WO1clHTMGa3JDZwrnQq4sF86dIHNDz0W1"" crossorigin=""anonymous""></script>
                                <script src=""https://cdn.jsdelivr.net/npm/bootstrap@4.3.1/dist/js/bootstrap.min.js"" integrity=""sha384-JjSmVgyd0p3pXB1rRibZUAYoIIy6OrQ6VrjIEaFf/nJGzIxFDsf4x0xIM+B07jRM"" crossorigin=""anonymous""></script>
                                    "
                        };
                       // await _mail.SendEmailAsync(model);

                        //---------redirect to confirm account-------
                        _toast.AddSuccessToastMessage("Account created, check your mail box for verification");
                        return LocalRedirect("/become-tutor");
                    }
                    else
                    {
                        _toast.AddErrorToastMessage("Registration Unsucessful");
                        return LocalRedirect("/become-tutor");
                    }

                }

                if (type == "login")
                {
                    var result = await _signInManager.PasswordSignInAsync(LoginInput.Email, LoginInput.Password, LoginInput.RememberMe, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                         user = await _userManager.FindByEmailAsync(LoginInput.Email);
                        _logger.LogInformation("User logged in.");

                        _toast.AddSuccessToastMessage("Select level to tutor");

                        return Redirect($"/choose-teaching-level/{user.Id}");
                    }
                    if (result.IsLockedOut)
                    {
                        _logger.LogWarning("User account locked out.");
                        return RedirectToPage("./Lockout");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return Page();
                    }
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }

            return Redirect($"/choose-teaching-level/{user.Id}");
        }
  
    }
}
