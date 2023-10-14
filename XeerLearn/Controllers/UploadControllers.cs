using System;
using System.IO;
using System.Net.Http.Headers;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XeerLearn.Data;

namespace XeerLearn.Controllers
{
    [ApiController]
#pragma warning disable SYSLIB0003 
    [SecurityPermission(SecurityAction.Demand, ControlThread = true)]
#pragma warning restore SYSLIB0003 
    public class UploadControllers : ControllerBase
    {

        [Route("Blocked")]
        [Authorize]
        public async Task<string> ProfileUpload(IFormFile seed, string name)
        {
            try
            {
                var file = seed.OpenReadStream();
                var folderName = Path.Combine("wwwroot", "StaticFiles");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                #pragma warning disable SYSLIB0003  
                FileIOPermission permission = new FileIOPermission(FileIOPermissionAccess.AllAccess, pathToSave);
                #pragma warning restore SYSLIB0003  
                permission.Demand();
                    if (file.Length > 0)
                    {
                        var fileName = name; 
                        var fullPath = Path.Combine(pathToSave, fileName);
                        var dbPath = Path.Combine(folderName, fileName);

                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        return dbPath;
                    }
                
            }
            catch (Exception ex)
            {
                Console.Write(ex);
               
            }
            return null;
        }



    }

 }


       