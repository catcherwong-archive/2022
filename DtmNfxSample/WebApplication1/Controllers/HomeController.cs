using Dtmcli;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private IDtmClient _client;
        private IDtmTransFactory _transFactory;

        public HomeController(IDtmClient client, IDtmTransFactory transFactory)
        {
            this._client = client;
            this._transFactory = transFactory;
        }

        public async Task<ActionResult> Index()
        {
            var gid = await _client.GenGid(default);

            return Content(gid);
        }

        public async Task<ActionResult> Index2()
        {
            var gid = await _client.GenGid(default);
            var saga = _transFactory.NewSaga(gid)
                .Add("http://loclahost:8080/api/TransOut", "http://loclahost:8080/api/TransOutRevert", new { amount = 30 })
                .Add("http://loclahost:8080/api/TransIn", "http://loclahost:8080/api/TransInRevert", new { amount = 30 })
                ;

            await saga.Submit(default);

            return Content(gid);
        }
    }
}