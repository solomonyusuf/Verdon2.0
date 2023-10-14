using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using System.Data;
using System.Net.Http.Headers;
using System.Security.Permissions;
using XeerLearn.Controllers;
using XeerLearn.Data;
using XeerLearn.Models.Auth;
using XeerLearn.Models.Utility;

namespace XeerLearn.Pages.Components.Student
{
#nullable disable
    [Authorize(Roles = "Student")]
    public class StudentAccountModel : PageModel
    {
        [BindProperty]
        public Models.Auth.XeerLearnUser user { get; set; }

        [BindProperty]
        public string number { get; set; }

        [BindProperty]
        public string type { get; set; }


        [BindProperty]
        public string image { get; set; }

        [BindProperty]
        public string new_password { get; set; }

        [BindProperty]
        public string confirm_password { get; set; }

        public readonly ApplicationDbContext _context;
        public readonly IToastNotification _toast;
        public readonly UploadControllers _upload;
        public readonly UserManager<Models.Auth.XeerLearnUser> _manager;

        public new Dictionary<string, string> country = new Dictionary<string, string>() {
   { "Afghanistan", "93" },
   { "Åland Islands", "358" },
   { "Albania", "355" },
   { "Algeria", "213" },
   { "American Samoa", "1" },
   { "Andorra", "376" },
   { "Angola", "244" },
   { "Anguilla", "1" },
   { "Antigua and Barbuda", "1" },
   { "Argentina", "54" },
   { "Armenia", "374" },
   { "Aruba", "297" },
   { "Australia", "61" },
   { "Austria", "43" },
   { "Azerbaijan", "994" },
   { "Bahamas", "1" },
   { "Bahrain", "973" },
   { "Bangladesh", "880" },
   { "Barbados", "1" },
   { "Belarus", "375" },
   { "Belgium", "32" },
   { "Belize", "501" },
   { "Benin", "229" },
   { "Bermuda", "1" },
   { "Bhutan", "975" },
   { "Bolivia", "591" },
   { "Bonaire, Sint Eustatius and Saba", "599" },
   { "Bosnia and Herzegovina", "387" },
   { "Botswana", "267" },
   { "Brazil", "55" },
   { "British Indian Ocean Territory", "246" },
   { "British Virgin Islands", "1" },
   { "Brunei", "673" },
   { "Bulgaria", "359" },
   { "Burkina Faso", "226" },
   { "Burundi", "257" },
   { "Cabo Verde", "238" },
   { "Cambodia", "855" },
   { "Cameroon", "237" },
   { "Canada", "1" },
   { "Caribbean", "0" },
   { "Cayman Islands", "1" },
   { "Central African Republic", "236" },
   { "Chad", "235" },
   { "Chile", "56" },
   { "China", "86" },
   { "Christmas Island", "61" },
   { "Cocos (Keeling) Islands", "61" },
   { "Colombia", "57" },
   { "Comoros", "269" },
   { "Congo", "242" },
   { "Congo (DRC)", "243" },
   { "Cook Islands", "682" },
   { "Costa Rica", "506" },
   { "Côte d’Ivoire", "225" },
   { "Croatia", "385" },
   { "Cuba", "53" },
   { "Curaçao", "599" },
   { "Cyprus", "357" },
   { "Czechia", "420" },
   { "Denmark", "45" },
   { "Djibouti", "253" },
   { "Dominica", "1" },
   { "Dominican Republic", "1" },
   { "Ecuador", "593" },
   { "Egypt", "20" },
   { "El Salvador", "503" },
   { "Equatorial Guinea", "240" },
   { "Eritrea", "291" },
   { "Estonia", "372" },
   { "Ethiopia", "251" },
   { "Europe", "0" },
   { "Falkland Islands", "500" },
   { "Faroe Islands", "298" },
   { "Fiji", "679" },
   { "Finland", "358" },
   { "France", "33" },
   { "French Guiana", "594" },
   { "French Polynesia", "689" },
   { "Gabon", "241" },
   { "Gambia", "220" },
   { "Georgia", "995" },
   { "Germany", "49" },
   { "Ghana", "233" },
   { "Gibraltar", "350" },
   { "Greece", "30" },
   { "Greenland", "299" },
   { "Grenada", "1" },
   { "Guadeloupe", "590" },
   { "Guam", "1" },
   { "Guatemala", "502" },
   { "Guernsey", "44" },
   { "Guinea", "224" },
   { "Guinea-Bissau", "245" },
   { "Guyana", "592" },
   { "Haiti", "509" },
   { "Honduras", "504" },
   { "Hong Kong SAR", "852" },
   { "Hungary", "36" },
   { "Iceland", "354" },
   { "India", "91" },
   { "Indonesia", "62" },
   { "Iran", "98" },
   { "Iraq", "964" },
   { "Ireland", "353" },
   { "Isle of Man", "44" },
   { "Israel", "972" },
   { "Italy", "39" },
   { "Jamaica", "1" },
   { "Japan", "81" },
   { "Jersey", "44" },
   { "Jordan", "962" },
   { "Kazakhstan", "7" },
   { "Kenya", "254" },
   { "Kiribati", "686" },
   { "Korea", "82" },
   { "Kosovo", "383" },
   { "Kuwait", "965" },
   { "Kyrgyzstan", "996" },
   { "Laos", "856" },
   { "Latin America", "0" },
   { "Latvia", "371" },
   { "Lebanon", "961" },
   { "Lesotho", "266" },
   { "Liberia", "231" },
   { "Libya", "218" },
   { "Liechtenstein", "423" },
   { "Lithuania", "370" },
   { "Luxembourg", "352" },
   { "Macao SAR", "853" },
   { "Macedonia, FYRO", "389" },
   { "Madagascar", "261" },
   { "Malawi", "265" },
   { "Malaysia", "60" },
   { "Maldives", "960" },
   { "Mali", "223" },
   { "Malta", "356" },
   { "Marshall Islands", "692" },
   { "Martinique", "596" },
   { "Mauritania", "222" },
   { "Mauritius", "230" },
   { "Mayotte", "262" },
   { "Mexico", "52" },
   { "Micronesia", "691" },
   { "Moldova", "373" },
   { "Monaco", "377" },
   { "Mongolia", "976" },
   { "Montenegro", "382" },
   { "Montserrat", "1" },
   { "Morocco", "212" },
   { "Mozambique", "258" },
   { "Myanmar", "95" },
   { "Namibia", "264" },
   { "Nauru", "674" },
   { "Nepal", "977" },
   { "Netherlands", "31" },
   { "New Caledonia", "687" },
   { "New Zealand", "64" },
   { "Nicaragua", "505" },
   { "Niger", "227" },
   { "Nigeria", "234" },
   { "Niue", "683" },
   { "Norfolk Island", "672" },
   { "North Korea", "850" },
   { "Northern Mariana Islands", "1" },
   { "Norway", "47" },
   { "Oman", "968" },
   { "Pakistan", "92" },
   { "Palau", "680" },
   { "Palestinian Authority", "970" },
   { "Panama", "507" },
   { "Papua New Guinea", "675" },
   { "Paraguay", "595" },
   { "Peru", "51" },
   { "Philippines", "63" },
   { "Pitcairn Islands", "0" },
   { "Poland", "48" },
   { "Portugal", "351" },
   { "Puerto Rico", "1" },
   { "Qatar", "974" },
   { "Réunion", "262" },
   { "Romania", "40" },
   { "Russia", "7" },
   { "Rwanda", "250" },
   { "Saint Barthélemy", "590" },
   { "Saint Kitts and Nevis", "1" },
   { "Saint Lucia", "1" },
   { "Saint Martin", "590" },
   { "Saint Pierre and Miquelon", "508" },
   { "Saint Vincent and the Grenadines", "1" },
   { "Samoa", "685" },
   { "San Marino", "378" },
   { "São Tomé and Príncipe", "239" },
   { "Saudi Arabia", "966" },
   { "Senegal", "221" },
   { "Serbia", "381" },
   { "Seychelles", "248" },
   { "Sierra Leone", "232" },
   { "Singapore", "65" },
   { "Sint Maarten", "1" },
   { "Slovakia", "421" },
   { "Slovenia", "386" },
   { "Solomon Islands", "677" },
   { "Somalia", "252" },
   { "South Africa", "27" },
   { "South Sudan", "211" },
   { "Spain", "34" },
   { "Sri Lanka", "94" },
   { "St Helena, Ascension, Tristan da Cunha", "290" },
   { "Sudan", "249" },
   { "Suriname", "597" },
   { "Svalbard and Jan Mayen", "47" },
   { "Swaziland", "268" },
   { "Sweden", "46" },
   { "Switzerland", "41" },
   { "Syria", "963" },
   { "Taiwan", "886" },
   { "Tajikistan", "992" },
   { "Tanzania", "255" },
   { "Thailand", "66" },
   { "Timor-Leste", "670" },
   { "Togo", "228" },
   { "Tokelau", "690" },
   { "Tonga", "676" },
   { "Trinidad and Tobago", "1" },
   { "Tunisia", "216" },
   { "Turkey", "90" },
   { "Turkmenistan", "993" },
   { "Turks and Caicos Islands", "1" },
   { "Tuvalu", "688" },
   { "U.S. Outlying Islands", "0" },
   { "U.S. Virgin Islands", "1" },
   { "Uganda", "256" },
   { "Ukraine", "380" },
   { "United Arab Emirates", "971" },
   { "United Kingdom", "44" },
   { "United States", "1" },
   { "Uruguay", "598" },
   { "Uzbekistan", "998" },
   { "Vanuatu", "678" },
   { "Vatican City", "39" },
   { "Venezuela", "58" },
   { "Vietnam", "84" },
   { "Wallis and Futuna", "681" },
   { "World", "0" },
   { "Yemen", "967" },
   { "Zambia", "260" },
   { "Zimbabwe", "263" },
};

        public StudentAccountModel(UploadControllers upload,UserManager<Models.Auth.XeerLearnUser> manager, IToastNotification toast, ApplicationDbContext context)
        {
            _upload = upload;
            _manager = manager;
            _toast = toast;
            _context = context;

        }
        public async Task OnGet()
        {
            try
            {
                user = await _manager.FindByEmailAsync(User.Identity.Name);
              
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            try 
            {
                if(type == "account")
                {
                    var USER = await _manager.FindByEmailAsync(User.Identity.Name);
                    USER.Image = USER.Image;
                    USER.Initials = user.Initials;
                    USER.FirstName = user.FirstName;
                    USER.MiddleName = user.MiddleName;
                    USER.LastName = user.LastName;
                    USER.PhoneNumber = user.PhoneNumber;
                    USER.Description = user.Description;
                    USER.Matric = user.Matric;
                    USER.Address = user.Address;
                    USER.Country = user.Country;
                    USER.State = user.State;
                    USER.Currency = user.Currency;
                    USER.BankCode = user.BankCode;
                    USER.BankName = user.BankName;
                    USER.AccountName = user.AccountName;
                    USER.AccountNumber = user.AccountNumber;

                    if (Request.Form.Files.Any() == true)
                    { 
                        var file = Request.Form.Files[0];
                        if (file.Length > 3000000)
                        {
                            _toast.AddWarningToastMessage("Maximum file size is 3MB");
                            return Redirect("/tutor-account");
                        }
                        if(file.ContentType.Contains("jpg")  || file.ContentType.Contains("png") || file.ContentType.Contains("jpeg"))
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

                                USER.Image = dbPath;
                            }
                        }
                        else
                        {
                            _toast.AddWarningToastMessage("File format rejected.Accepted include (jpg, jpeg, png).Secure protocol activated.");
                            return Redirect("/tutor-account");
                        }
                    }
                    _context.User.Update(USER);
                    _context.SaveChanges();
                    _toast.AddSuccessToastMessage("Account Updated");
                }
                if(type == "password")
                {
                    if(new_password == confirm_password)
                    {
                        var token = await _manager.GeneratePasswordResetTokenAsync(user);
                        await _manager.ResetPasswordAsync(user,token, new_password);
                        _toast.AddSuccessToastMessage("Password Updated");
                    }
                    else
                    {
                        _toast.AddWarningToastMessage("Passwords do not match");
                        return Redirect("/tutor-account");
                    }
                }
                if(type == "deactivate")
                {
                    await _manager.DeleteAsync(user);
                    _toast.AddWarningToastMessage("Account Deactivated and Non existent");
                    return Redirect("/");
                }
              
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return Redirect("/student-account");
        }
    }
}
