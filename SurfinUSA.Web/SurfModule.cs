using EasyHttp.Http;
using Nancy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace SurfinUSA.Web
{
    public class SurfModule : NancyModule
    {
        public SurfModule()
        {
            Get["/"] = _ => Response.AsRedirect("/nexus4", Nancy.Responses.RedirectResponse.RedirectType.SeeOther);

            Get["/nexus4"] = _ =>
                Response.AsText(
                    String.Format(
                        new HttpClient()
                            .Get("http://play.google.com/store/devices/details?id=nexus_4_16gb&feature=microsite&hl=en")
                            .RawText
                            .Contains("We are out of inventory. Please check back soon.") ? "Fortfarande slut {0}" : "Nu finns telefonen inne {0}!", 
                        DateTime.Now.ToString("G")
                    )
                );

            Get["/surf"] = _ =>
                {
                    Uri path;

                    if (Uri.TryCreate(Request.Query.url, UriKind.Absolute, out path))
                    {
                        var client = new HttpClient();
                        client.StreamResponse = true;
                        client.Request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.64 Safari/537.11";

                        client.Get(path.ToString());

                        using (var responseStream = client.Response.ResponseStream)
                        {
                            var memoryStream = new MemoryStream();
                            responseStream.CopyTo(memoryStream);
                            memoryStream.Position = 0;

                            return Response.FromStream(memoryStream, "text/html");
                        }
                    }
                    else
                    {
                        return Response.AsText("ogiltig url");
                    }
                };
        }
    }
}