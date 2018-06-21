using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(QuizApplication.Startup))]
namespace QuizApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
