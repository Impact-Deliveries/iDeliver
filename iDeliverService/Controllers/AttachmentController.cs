using iDeliverDataAccess.Repositories;
using IDeliverObjects.DTO;
using IDeliverObjects.Objects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text.RegularExpressions;

namespace iDeliverService.Controllers
{
    [Route("api/attachment")]
    [ApiController]
    //[Authorize]
    public class AttachmentController : ControllerBase
    {
        private readonly IAttachmentRepository _repository;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _env;
        public AttachmentController(IAttachmentRepository repository, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            _repository = repository;
            _env = env;

        }

        // GET: api/Attachments
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Attachment>>> GetAttachments()
        {
            var result = await _repository.GetAll();

            return Ok(result);
        }

        // GET: api/Attachments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Attachment>> GetAttachment(long id)
        {
            var Attachment = await _repository.GetByID(id);

            if (Attachment == null)
            {
                return NotFound();
            }

            return Ok(Attachment);
        }

        // GET: api/Attachments/GetAttachmentByModule
        [HttpGet("GetAttachmentByModule")]
        public async Task<IActionResult> GetAttachmentByModule(long ModuleID, int ModuleType)
        {
            var Attachments = await _repository.Find(a => a.ModuleId == ModuleID && a.ModuleType == ModuleType);
            if (Attachments == null)
            {
                return NotFound();
            }
            return Ok(Attachments);
        }

        // Post: api/Attachments/UploadAttachments
        [HttpPost("UploadAttachments")]
        public async Task<ActionResult<Attachment>> UploadAttachments()//,Attachment attachment)
        {

            try
            {
                List<Attachment> attachments = new List<Attachment>();
                var files = HttpContext.Request.Form.Files;
                string creation_date = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
                if (files == null || files.ToList().Count < 0)
                    return BadRequest();
                AttachmentDTO data = JsonConvert.DeserializeObject<AttachmentDTO>(HttpContext.Request.Form["data"]);

                if (string.IsNullOrEmpty(data.Path) || data == null)
                    return NotFound();
                if (files.Count > 0)
                {
                    foreach (var file in files)
                    {
                        string filename = file.FileName.Replace("#", "").Replace("+", "");
                        string file_extension = Path.GetExtension(filename);

                        filename = Regex.Replace(filename.Trim(), "[^A-Za-z0-9_. ]+", "");

                        string replace_filename = filename.Replace("~/", "").Split(file_extension)[0] + "_" + creation_date + file_extension;

                        var path = _env.ContentRootPath + data.Path.Replace("~/", "\\").Replace("/", "\\"); 
                        //Path.Combine(_env.ContentRootPath, data.Path.Replace("~/", "\\").Replace("/", "\\")); //Server.MapPath(data.Path);
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                       // string physical_path = data.Path; //+ replace_filename;
                       string file_path = Path.Combine(path + replace_filename);
                        // files.SaveAs(file_path);

                        using (var stream = System.IO.File.Create(file_path))
                        {
                            await file.CopyToAsync(stream);
                        }
                        Attachment atta = new Attachment
                        {
                            CreationDate = DateTime.Now,
                            Path = data.Path,
                            FileName = replace_filename,
                            CreatorId = data.CreatorID,
                            IsDeleted = false,
                            ModuleId = data.ModuleID,
                            ModuleType = data.ModuleTypeID,
                            Extension = file_extension.Trim(),
                            GroupId = data.GroupID.ToString(),
                            AttachmentType = data.AttachmentType
                        };
                        _repository.Add(atta);
                        attachments.Add(atta);
                    }

                    return Ok(attachments);
                }
                else
                {
                    return BadRequest(HttpStatusCode.BadRequest);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        private bool AttachmentExists(long id)
        {
            return _repository.IsExists(w => w.Id == id);
        }
    }
}
