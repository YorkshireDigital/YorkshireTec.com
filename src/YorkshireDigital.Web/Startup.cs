using Microsoft.Owin;
using YorkshireDigital.Web;

[assembly: OwinStartup(typeof(Startup))]

namespace YorkshireDigital.Web
{
    using System.IO;
    using Microsoft.Owin.FileSystems;
    using Microsoft.Owin.StaticFiles;
    using Owin;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var fileSystem = new FileServerOptions
            {
                EnableDirectoryBrowsing = true,
                FileSystem = new PhysicalFileSystem(Path.GetFullPath(@"D:\Code\GitHub\YorkshireTec.com\src\YorkshireDigital.Web"))
            };

            app.UseFileServer(fileSystem);
        }
    }
}
