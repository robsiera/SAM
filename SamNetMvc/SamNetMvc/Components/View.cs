using System;
using System.IO;
using System.Web.Hosting;
using HandlebarsDotNet;
using SamNetMvc.Helpers;

namespace SamNetMvc.Components
{
    internal class View
    {
        public static string Init(Model model)
        {
            return Ready(model);
        }

        public static string Ready(Model model)
        {
            if (model.LastEdited == null)
            {
                // model.lastEdited = model.lastEdited || {};
            }
            var titleValue = model?.LastEdited?.Title.orDefault("Title");
            var descriptionValue = model?.LastEdited?.Description.orDefault("Description");
            var id = model?.LastEdited?.Id.ToString().orDefault("");
            var cancelButton = $@"<button id=""cancel"" onclick=""JavaScript:return actions.cancel({{}});\"">Cancel</button>{Environment.NewLine}";
            var valAttr = "value";
            var actionLabel = "Save";
            var idElement = $@", 'id':'{id}'";
            if (id.IsNullOrEmpty())
            {
                cancelButton = ""; valAttr = "placeholder"; idElement = ""; actionLabel = "Add";
            }
            var output = (
                $@"<br><br><div class=""blog-post"">{Environment.NewLine}" + model.Posts.map((e) =>
                {
                    return RenderHbs("post", e);

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

        private static string RenderHbs(string templateSource, dynamic model)
        {
            var file = HostingEnvironment.MapPath($@"~/sam/demo2/templates/{templateSource}.hbs");
            var template = Handlebars.Compile(File.ReadAllText(file));
            var retval = template(model);
            return retval;
        }

        public static void Display(string representation, Action<string> next)
        {
            next(representation);
        }
    }
}