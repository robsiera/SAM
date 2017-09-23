using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using SamNetMvc.Components;

namespace SamNetMvc.Controllers
{
    public class Demo2Controller : ApiController
    {
        private readonly View view = new View();
        private readonly State state = new State();
        private readonly Model model;
        private string _returnRepresentation;

        public Demo2Controller()
        {
            model = CreateInitialModel();
            state.View = view;
            model.State = state;
        }

        #region Api Calls

        /// <summary>
        /// Initializes the specified payload.
        /// POST api/<controller>/init
        /// </summary>
        [HttpGet]
        public string Init([FromBody]string payload)
        {
            return View.Init(model);
        }

        /// <summary>
        /// Presents the specified payload to the Model.
        /// POST api/<controller>/Present
        /// </summary>
        /// <remarks>
        /// This Api is called with different signatures. See the Actions in /sam/demo2/blog.js
        /// </remarks>
        [HttpPost]
        public string Present([FromBody]PresenterModel presenterModel)
        {
            PresenterModel data = presenterModel;
            model.Present(data, representation => { SetReturnRepresentation(representation); });
            return GetReturnRepresentation();
        }

        #endregion

        #region Private methods

        private void SetReturnRepresentation(string representation)
        {
            _returnRepresentation = representation;
        }

        private string GetReturnRepresentation()
        {
            return _returnRepresentation;
        }

        /// <summary>
        /// Creates the initial model.
        /// </summary>
        /// <remarks>
        /// in this demo we don't use a database but an inMemory model. 
        /// we don't even keep it in a session, thus it gets overridden each time. Not every useful, but al least the demo allows you to follow the code flow.
        /// </remarks>
        private static Model CreateInitialModel()
        {
            var model = new Model();
            model.Posts.Add(new BlogPost
            {
                Id = 1,
                Title = "The SAM Pattern",
                Description = "SAM is a new reactive/functional pattern that simplifies Front-End architectures by clearly separating the business logic from the view and, in particular, strictly decoupling back-end APIs from the Front-End. SAM is technology independent and as such can be used to build Web Apps or Native Apps"
            });
            model.Posts.Add(new BlogPost
            {
                Id = 2,
                Title = "Why I no longer use MVC Frameworks",
                Description = "The worst part of my job these days is designing APIs for front-end developers. "
            });
            model.ItemId = 3;
            return model;
        }

        #endregion
    }

}