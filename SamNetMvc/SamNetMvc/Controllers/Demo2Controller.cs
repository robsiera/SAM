using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using SamNetMvc.Helpers;

namespace SamNetMvc.Controllers
{
    public class Demo2Controller : ApiController
    {
        private readonly SamView view = new SamView();
        private readonly SamState state = new SamState();
        private readonly SamModel model;
        private string _returnRepresentation;

        public Demo2Controller()
        {
            model = CreateInitialModel();
            state.view = view;
            model.state = state;
        }

        #region Api Calls

        /// <summary>
        /// Initializes the specified payload.
        /// POST api/<controller>/init
        /// </summary>
        [HttpGet]
        public string Init([FromBody]string payload)
        {
            return view.init(model);
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
            model.present(data, representation => { SetReturnRepresentation(representation); });
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
        private static SamModel CreateInitialModel()
        {
            var model = new SamModel();
            model.posts.Add(new BlogPost
            {
                id = 1,
                title = "The SAM Pattern",
                description = "SAM is a new reactive/functional pattern that simplifies Front-End architectures by clearly separating the business logic from the view and, in particular, strictly decoupling back-end APIs from the Front-End. SAM is technology independent and as such can be used to build Web Apps or Native Apps"
            });
            model.posts.Add(new BlogPost
            {
                id = 2,
                title = "Why I no longer use MVC Frameworks",
                description = "The worst part of my job these days is designing APIs for front-end developers. "
            });
            model.itemId = 3;
            return model;
        }

        #endregion
    }

    #region Types

    public class PresenterModel : BlogPost
    {
        public BlogPost item { get; set; }
        public BlogPost lastEdited { get; set; }
        public int deletedItemId { get; set; } = 0;
    }

    internal class SamState
    {
        public SamView view { get; set; }

        // Derive the state representation as a function of the systen
        // control state
        public void representation(SamModel model, Action<string> next)
        {
            var representation = "oops... something went wrong, the system is in an invalid state";

            if (this.ready(model))
            {
                representation = this.view.ready(model);
            }

            this.view.display(representation, next);
        }

        // Derive the current state of the system
        public bool ready(SamModel model)
        {
            return true;
        }


        /// <summary>
        /// Next action predicate, derives whether the system is in a (control) state where a new (next) action needs to be invoked
        /// </summary>
        public void nextAction(SamModel model)
        {
            //nothing to do in this App
        }

        public void render(SamModel model, Action<string> next)
        {
            this.representation(model, next);
            this.nextAction(model);
        }
    }

    internal class SamView
    {
        public string init(SamModel model)
        {
            return this.ready(model);
        }

        public string ready(SamModel model)
        {
            if (model.lastEdited == null)
            {
                // model.lastEdited = model.lastEdited || {};
            }
            var titleValue = model?.lastEdited?.title.orDefault("Title");
            var descriptionValue = model?.lastEdited?.description.orDefault("Description");
            var id = model?.lastEdited?.id.ToString().orDefault("");
            var cancelButton = $@"<button id=""cancel"" onclick=""JavaScript:return actions.cancel({{}});\"">Cancel</button>{Environment.NewLine}";
            var valAttr = "value";
            var actionLabel = "Save";
            var idElement = $@", 'id':'{id}'";
            if (id.IsNullOrEmpty())
            {
                cancelButton = ""; valAttr = "placeholder"; idElement = ""; actionLabel = "Add";
            }
            var output = (
                $@"<br><br><div class=""blog-post"">{Environment.NewLine}" + model.posts.map((e) =>
                {
                    return (
                        $@"<br><br><h3 class=""blog-post-title"" onclick=""JavaScript:return actions.edit({{'title':'{e.title}', 'description':'{e.description}', 'id':'{e.id}'}});"">{e.title}</h3>{Environment.NewLine}"
                        + $@"<p class=""blog-post-meta"">{e.description}</p>"
                        + $@"<button onclick=""JavaScript:return actions.delete({{'id':'{e.id}'}});"">Delete</button>");

                }).@join($"{Environment.NewLine}") + $@"{Environment.NewLine}</div>{Environment.NewLine}
                <br><br>{Environment.NewLine}
                <div class=""mdl-cell mdl-cell--6-col"">{Environment.NewLine}
                <input id=""title"" type=""text"" class=""form-control""  {valAttr}=""{titleValue}""><br>{Environment.NewLine}
                <input id=""description"" type=""textarea"" class=""form-control"" {valAttr}=""{descriptionValue}""><br>{Environment.NewLine}
                <button id=""save"" onclick=""JavaScript:return actions.save({{'title':document.getElementById('title').value, 'description': document.getElementById('description').value{idElement}}});"">{actionLabel}</button>
                {Environment.NewLine}{cancelButton}{Environment.NewLine}</div>
                <br><br>{Environment.NewLine}");
            return output;
        }

        public void display(string representation, Action<string> next)
        {
            next(representation);
            //var stateRepresentation = document.getElementById("representation");
            //stateRepresentation.innerHTML = representation;
        }
    }

    internal class SamModel
    {
        public List<BlogPost> posts { get; } = new List<BlogPost>();

        public int itemId { get; set; }

        public BlogPost lastEdited { get; set; }

        public BlogPost lastDeleted { get; set; }

        public SamState state { get; set; }

        public void present(PresenterModel data, Action<string> next)
        {
            var model = this;
            if (data?.deletedItemId > 0)
            {
                var d = -1;
                var index = -1;
                foreach (var el in model.posts)
                {
                    index += 1;
                    if (el.id == data.deletedItemId)
                    {
                        d = index;
                    }
                }
                if (d >= 0)
                {
                    Debugger.Break();
                    //convertion of following js line needs to be confirmed
                    model.lastDeleted = model.posts[d];  //model.lastDeleted = model.posts.splice(d, 1)[0];
                }
            }

            if (data?.lastEdited != null)
            {
                model.lastEdited = data.lastEdited;
            }
            else
            {
                Debugger.Break();
                //convertion of following js line needs to be confirmed
                model.lastEdited = null;    //delete model.lastEdited;
            }

            if (data?.item != null)
            {
                if (data.item.id > 0)
                {
                    // has been edited
                    var index = 0;
                    foreach (var el in model.posts)
                    {
                        index += 1;
                        if (el.id == data.item.id)
                        {
                            model.posts[index] = data.item;
                        }
                    }
                }
                else
                {
                    // new item
                    data.item.id = model.itemId++;
                    model.posts.Add(data.item);
                }
            }

            state.render(model, next);
        }
    }

    public class BlogPost
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
    }

    #endregion
}