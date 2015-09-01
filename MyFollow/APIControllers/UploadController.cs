using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;


namespace MyFollow.APIControllers
{
    public class UploadController : ApiController
    {

        [HttpPost]
        [Route("api/Upload")]
        public async Task<object> UploadFile()
        {
            if (!Request.Content.IsMimeMultipartContent("form-data"))
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.UnsupportedMediaType));
            }
            NamedMultipartFormDataStreamProvider streamProvider = new NamedMultipartFormDataStreamProvider(
                HttpContext.Current.Server.MapPath("~/Images/"));

            var result=  await Request.Content.ReadAsMultipartAsync(streamProvider);
            return new
            {
                FileNames = streamProvider.FileData.Select(entry => entry.Headers.ContentDisposition.FileName),
            };
        }
    }
}