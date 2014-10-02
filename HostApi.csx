#r "D:\Code\GitHub\YorkshireTec.com\src\YorkshireDigital.Api\bin\Microsoft.Owin.Hosting.dll"
#r "D:\Code\GitHub\YorkshireTec.com\src\\YorkshireDigital.Api\\bin\\YorkshireDigital.Api.dll"
#r "D:\Code\GitHub\YorkshireTec.com\src\YorkshireDigital.Web\bin\Microsoft.Owin.dll"
#r "D:\Code\GitHub\YorkshireTec.com\src\YorkshireDigital.Web\bin\Microsoft.Owin.Host.HttpListener.dll"
#r "D:\Code\GitHub\YorkshireTec.com\src\YorkshireDigital.Web\bin\Microsoft.Owin.FileSystems.dll"
#r "D:\Code\GitHub\YorkshireTec.com\src\YorkshireDigital.Web\bin\Microsoft.Owin.Hosting.dll"
#r "D:\Code\GitHub\YorkshireTec.com\src\YorkshireDigital.Web\bin\Microsoft.Owin.StaticFiles.dll"
#r "D:\Code\GitHub\YorkshireTec.com\src\YorkshireDigital.Web\bin\Owin.dll"
#r "D:\Code\GitHub\YorkshireTec.com\src\YorkshireDigital.Web\bin\YorkshireDigital.Web.dll"

using System.Diagnostics;
using System.Configuration;
using Microsoft.Owin.Hosting;

Require<Bau>()

.Task("default").DependsOn("browse")

.Task("browse")
	.DependsOn("host")
	.Do(() => Console.WriteLine("browse"))

.Task("host")
	.DependsOn("api", "web")
	.Do(() => {
		Process.Start("http://localhost:5566");
		Console.WriteLine("Press any key to exit");
		Console.Read();
	})

.Task("api").Do(() =>
{
	//ConfigurationManager.OpenExeConfiguration("src\\YorkshireDigital.Api\\bin\\YorkshireDigital.Api.exe");
	WebApp.Start<YorkshireDigital.Api.Startup>("http://+:61140");
})
.Task("web").Do(() =>
{
	//ConfigurationManager.OpenExeConfiguration("src\\YorkshireDigital.Api\\bin\\YorkshireDigital.Api.exe");
	WebApp.Start<YorkshireDigital.Web.Startup>("http://+:5566");
})

/*.Exec("api").Do(exec => {
    exec.Command = "src\\YorkshireDigital.Api\\bin\\YorkshireDigital.Api.exe";
    exec.WorkingDirectory = "src\\YorkshireDigital.Api\\bin";
})*/

/*.Exec("web").Do(exec => {
    exec.Command = "src\\YorkshireDigitial.Web.Start\\bin\\Release\\YorkshireDigitial.Web.Start.exe";
    exec.WorkingDirectory = "src\\YorkshireDigitial.Web.Start\\bin\\Release\\";
})*/

.MSBuild("build").Do(msb =>
{
	Console.WriteLine("Building");
    msb.MSBuildVersion = "net45";
    msb.Solution = "src/YorkshireTec.sln";
    msb.Targets = new[] { "Clean", "Build" };
    msb.Properties = new { Configuration = "Release" };
})
.Run();