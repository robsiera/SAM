using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SamNetMvc.Components
{
    internal class Model
    {
        public List<BlogPost> Posts { get; } = new List<BlogPost>();

        public int ItemId { get; set; }

        public BlogPost LastEdited { get; set; }

        public BlogPost LastDeleted { get; set; }

        public State State { get; set; }


        public void Present(PresenterModel data, Action<string> next)
        {
            var model = this;
            if (data?.DeletedItemId > 0)
            {
                var d = -1;
                var index = -1;
                foreach (var el in model.Posts)
                {
                    index += 1;
                    if (el.Id == data.DeletedItemId)
                    {
                        d = index;
                    }
                }
                if (d >= 0)
                {
                    Debugger.Break();
                    //convertion of following js line needs to be confirmed
                    model.LastDeleted = model.Posts[d];  //model.lastDeleted = model.posts.splice(d, 1)[0];
                }
            }

            if (data?.LastEdited != null)
            {
                model.LastEdited = data.LastEdited;
            }
            else
            {
                Debugger.Break();
                //convertion of following js line needs to be confirmed
                model.LastEdited = null;    //delete model.lastEdited;
            }

            if (data?.Item != null)
            {
                if (data.Item.Id > 0)
                {
                    // has been edited
                    var index = -1;
                    foreach (var el in model.Posts)
                    {
                        index += 1;
                        if (el.Id == data.Item.Id)
                        {
                            model.Posts[index] = data.Item;
                            break;
                        }
                    }
                }
                else
                {
                    // new item
                    data.Item.Id = model.ItemId++;
                    model.Posts.Add(data.Item);
                }
            }

            State.Render(model, next);
        }
    }
}